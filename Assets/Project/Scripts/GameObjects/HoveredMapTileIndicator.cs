using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour
{
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private bool mapTileIsSelected;

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
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		if(mapTileBoolVisualiserEvent.GetVisualiserEventType() != VisualiserEventType.MapTileHoverStateWasChanged || !mapTileIsSelected)
		{
			UpdateIndicatorState(mapTileBoolVisualiserEvent);
		}
	}

	private void UpdateIndicatorState(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTile = mapTileBoolVisualiserEvent.GetMapTile();
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();
		var mapTileHoverStateIsSetAsHovered = eventType == VisualiserEventType.MapTileHoverStateWasChanged && stateIsEnabled;
		var mapTileSelectionStateIsSetAsNotSelected = eventType == VisualiserEventType.MapTileSelectionStateWasChanged && !stateIsEnabled;
		var indicatorShouldBeShown = mapTile != null && (mapTileHoverStateIsSetAsHovered || mapTileSelectionStateIsSetAsNotSelected);

		this.mapTile = mapTile;
		mapTileIsSelected = eventType == VisualiserEventType.MapTileSelectionStateWasChanged && stateIsEnabled;

		gameObject.SetActive(indicatorShouldBeShown);
	}
}