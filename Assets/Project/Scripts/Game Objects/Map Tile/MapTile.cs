using UnityEngine;
using UnityEngine.Events;

public class MapTile : MonoBehaviour
{
	public static readonly float GRID_SIZE = 1f;

	public UnityEvent<MapTileType> tileTypeWasChangedEvent;
	
	private MapTileType tileType;
	private int weight;

	private static readonly int MIN_WEIGHT = -1;
	private static readonly int MAX_WEIGHT = 10;

	public MapTileType GetTileType() => tileType;

	public void SetTileType(MapTileType tileType)
	{
		var previousTileType = this.tileType;

		if(previousTileType == tileType)
		{
			return;
		}
		
		this.tileType = tileType;

		tileTypeWasChangedEvent?.Invoke(this.tileType);
	}

	public void ModifyWeightBy(int weight)
	{
		this.weight = Mathf.Clamp(this.weight + weight, MIN_WEIGHT, MAX_WEIGHT);

		SetTileType(this.weight < 0 ? MapTileType.Impassable : MapTileType.Passable);
	}
}