using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour, IMapEditingElement
{
	[SerializeField, Min(0f)] private float animationTransitionDuration = 0.5f;

	private static readonly float ANIMATION_TRANSITION_MINIMUM_SCALE = 0.8f;
	
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private bool mapTileIsSelected;
	private bool indicatorCanBeShown = true;

	public void SetMapEditingElementActive(bool active)
	{
		indicatorCanBeShown = active;
	}

	private void Awake()
	{
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void OnEnable()
	{
		if(mapTile != null)
		{
			transform.position = mapTile.transform.position;
		}
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(!indicatorCanBeShown || visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		UpdateMapTileReference(mapTileBoolVisualiserEvent);
		UpdateIndicatorState(mapTileBoolVisualiserEvent);
	}

	private void UpdateMapTileReference(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTile = mapTileBoolVisualiserEvent.GetMapTile();
		var mapTileIsDefined = mapTile != null;
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();

		switch (eventType)
		{
			case VisualiserEventType.MapTileHoverStateWasChanged:
				this.mapTile = mapTileIsDefined && stateIsEnabled ? mapTile : null;
				break;
			
			case VisualiserEventType.MapTileSelectionStateWasChanged:
				this.mapTile = mapTileIsDefined && !stateIsEnabled ? mapTile : null;
				break;
		}
	}

	private void UpdateIndicatorState(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		
		if(eventType == VisualiserEventType.MapTileHoverStateWasChanged && mapTileIsSelected)
		{
			return;
		}
		
		mapTileIsSelected = eventType == VisualiserEventType.MapTileSelectionStateWasChanged && mapTileBoolVisualiserEvent.GetBoolValue();

		gameObject.SetActive(mapTile != null);
	}

	private void Update()
	{
		if(Mathf.Approximately(animationTransitionDuration, 0f))
		{
			return;
		}
		
		var scale = transform.localScale;
		var scalePercent = Mathf.PingPong(Time.time, animationTransitionDuration);
		var scaleInterpolation = Mathf.Lerp(ANIMATION_TRANSITION_MINIMUM_SCALE, 1f, scalePercent);

		scale.x = scale.y = scaleInterpolation;
		transform.localScale = scale;
	}
}