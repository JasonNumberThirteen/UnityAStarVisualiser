using UnityEngine;

[RequireComponent(typeof(MapTile))]
public class MapTileMouseHoverDetector : MonoBehaviour
{
	private MapTile mapTile;
	private HoveredMapTileTracker hoveredMapTileTracker;

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		hoveredMapTileTracker = FindFirstObjectByType<HoveredMapTileTracker>();
	}

	private void OnMouseEnter()
	{
		if(hoveredMapTileTracker != null)
		{
			hoveredMapTileTracker.SetHoveredMapTile(mapTile);
		}
	}

	private void OnMouseExit()
	{
		if(hoveredMapTileTracker != null)
		{
			hoveredMapTileTracker.SetHoveredMapTile(null);
		}
	}
}