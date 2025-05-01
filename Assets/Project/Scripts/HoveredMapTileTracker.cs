using UnityEngine;

public class HoveredMapTileTracker : MonoBehaviour
{
	private MapTile hoveredMapTile;

	public void SetHoveredMapTile(MapTile mapTile)
	{
		if(hoveredMapTile != mapTile)
		{
			hoveredMapTile = mapTile;
		}
	}
}