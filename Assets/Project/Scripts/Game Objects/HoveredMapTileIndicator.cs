using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	[SerializeField, Min(0f)] private float animationTransitionDuration = 0.5f;

	private static readonly float ANIMATION_TRANSITION_MINIMUM_SCALE = 0.8f;
	
	private MapTile hoveredMapTile;
	private MapTile selectedMapTile;
	private MapTile currentMapTile;
	private bool indicatorWasHidden;
	private bool panelUIHoverWasDetected;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		indicatorWasHidden = !active;
		panelUIHoverWasDetected = false;

		UpdateActiveState();
	}

	public void SetMapEditingElementActive(bool active)
	{
		indicatorWasHidden = !active;
		
		UpdateActiveState();
	}

	private void Awake()
	{
		hoveredMapTileManager = FindFirstObjectByType<HoveredMapTileManager>();
		selectedMapTileManager = FindFirstObjectByType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = FindFirstObjectByType<PanelUIHoverDetectionManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void OnEnable()
	{
		if(currentMapTile != null)
		{
			transform.position = currentMapTile.transform.position;
		}
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.AddListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.RemoveListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		hoveredMapTile = mapTile;
		
		if(selectedMapTile != null)
		{
			return;
		}

		currentMapTile = hoveredMapTile;
		
		UpdateActiveState();
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		selectedMapTile = mapTile;
		currentMapTile = selectedMapTile == null ? hoveredMapTile : null;

		UpdateActiveState();
	}

	private void OnPanelUIHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
		
		UpdateActiveState();
	}

	private void UpdateActiveState()
	{
		gameObject.SetActive(!indicatorWasHidden && !panelUIHoverWasDetected && currentMapTile != null);
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