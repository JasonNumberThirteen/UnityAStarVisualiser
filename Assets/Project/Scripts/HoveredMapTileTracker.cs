using UnityEngine;
using UnityEngine.Events;

public class HoveredMapTileTracker : MonoBehaviour
{
	public UnityEvent<MapTile> hoveredMapTileWasChangedEvent;

	private MapTile hoveredMapTile;

	public void SetHoveredMapTile(MapTile mapTile)
	{
		if(hoveredMapTile == mapTile)
		{
			return;
		}

		hoveredMapTile = mapTile;

		hoveredMapTileWasChangedEvent?.Invoke(hoveredMapTile);
	}
}