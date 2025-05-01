using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour
{
	private HoveredMapTileTracker hoveredMapTileTracker;

	private void Awake()
	{
		hoveredMapTileTracker = FindFirstObjectByType<HoveredMapTileTracker>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(hoveredMapTileTracker != null)
			{
				hoveredMapTileTracker.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}
		}
		else
		{
			if(hoveredMapTileTracker != null)
			{
				hoveredMapTileTracker.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}
		}
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		var mapTileIsDefined = mapTile != null;

		gameObject.SetActive(mapTileIsDefined);

		if(mapTileIsDefined)
		{
			transform.position = mapTile.transform.position;
		}
	}
}