using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour, IMapEditingElement
{
	[SerializeField, Min(0f)] private float animationTransitionDuration = 0.5f;

	private static readonly float ANIMATION_TRANSITION_MINIMUM_SCALE = 0.8f;
	
	private MapTile currentMapTile;
	private MapTile previousMapTile;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;

	public void SetMapEditingElementActive(bool active)
	{
		gameObject.SetActive(active);
	}

	private void Awake()
	{
		hoveredMapTileManager = FindFirstObjectByType<HoveredMapTileManager>();
		selectedMapTileManager = FindFirstObjectByType<SelectedMapTileManager>();

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
		}
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		if(previousMapTile != null)
		{
			return;
		}
		
		currentMapTile = mapTile;
		
		gameObject.SetActive(currentMapTile != null);
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		if(mapTile != null)
		{
			previousMapTile = mapTile;
			currentMapTile = null;
		}
		else
		{
			currentMapTile = previousMapTile;
			previousMapTile = null;
		}

		gameObject.SetActive(currentMapTile != null);
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