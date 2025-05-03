using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MapTileNode))]
public class MapTile : MonoBehaviour
{
	public static readonly float GRID_SIZE = 1f;
	public static readonly int MIN_WEIGHT = -1;
	public static readonly int MAX_WEIGHT = 10;

	public UnityEvent<MapTileType> tileTypeWasChangedEvent;
	public UnityEvent<int> weightWasChangedEvent;
	
	private MapTileType tileType;
	private int weight;
	private MapTileNode mapTileNode;

	public MapTileType GetTileType() => tileType;
	public int GetWeight() => weight;
	public MapTileNode GetMapTileNode() => mapTileNode;

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
		SetWeightTo(this.weight + weight);
	}

	public void SetWeightTo(int weight)
	{
		var previousWeight = this.weight;
		
		this.weight = Mathf.Clamp(weight, MIN_WEIGHT, MAX_WEIGHT);

		if(previousWeight == this.weight)
		{
			return;
		}

		weightWasChangedEvent?.Invoke(this.weight);
		SetTileType(this.weight < 0 ? MapTileType.Impassable : MapTileType.Passable);

		mapTileNode.Weight = this.weight;
	}

	private void Awake()
	{
		mapTileNode = GetComponent<MapTileNode>();
	}
}