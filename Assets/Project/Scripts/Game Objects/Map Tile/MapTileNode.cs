using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTileNode : MonoBehaviour
{
	public UnityEvent<MapTileNodeType> mapTileNodeTypeWasChangedEvent;

	public int Weight {get; set;}

	private readonly MapTileNodeData mapTileNodeData = new();
	private readonly List<MapTileNode> neighbours = new();

	private MapTileNodeType mapTileNodeType;
	private PathfindingManager pathfindingManager;

	public Vector2 GetPosition() => transform.position;
	public MapTileNodeData GetMapTileNodeData() => mapTileNodeData;
	public List<MapTileNode> GetNeighbours() => neighbours;
	public MapTileNodeType GetMapTileNodeType() => mapTileNodeType;
	
	public void SetTileNodeType(MapTileNodeType mapTileNodeType)
	{
		var previousTileNodeType = this.mapTileNodeType;

		if(previousTileNodeType == mapTileNodeType)
		{
			return;
		}
		
		this.mapTileNodeType = mapTileNodeType;

		mapTileNodeTypeWasChangedEvent?.Invoke(this.mapTileNodeType);
	}

	public void ResetData()
	{
		SetTileNodeType(MapTileNodeType.Unvisited);
		mapTileNodeData.SetValues(null, 0, 0);
	}

	public void FindNeighbours(List<MapTileNode> mapTileNodes)
	{
		var diagonalMovementIsEnabled = pathfindingManager != null && pathfindingManager.DiagonalMovementIsEnabled();
		var directions = VectorMethods.GetCardinalDirections(diagonalMovementIsEnabled);

		neighbours.Clear();
		directions.ForEach(direction => AddNeighbourIfPossible(mapTileNodes, direction));
	}

	public float GetTotalCostToReach(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return 0f;
		}
		
		var thisMapTileNodeRealValue = GetMapTileNodeData().RealValue;
		var distanceToOtherMapTileNode = DistanceMethods.DistanceBetweenPositionsIsSingleAxis(GetPosition(), mapTileNode.GetPosition()) ? 1 : Mathf.Sqrt(2);
		var otherMapTileNodeRealValue = distanceToOtherMapTileNode*mapTileNode.Weight;
		
		return thisMapTileNodeRealValue + otherMapTileNodeRealValue;
	}

	private void Awake()
	{
		pathfindingManager = ObjectMethods.FindComponentOfType<PathfindingManager>();
	}

	private void AddNeighbourIfPossible(List<MapTileNode> mapTileNodes, Vector2Int direction)
	{
		var neighbouringPosition = GetPosition() + direction;
		var neighbouringNode = mapTileNodes.FirstOrDefault(mapTileNode => mapTileNode.GetPosition() == neighbouringPosition);

		if(neighbouringNode != null)
		{
			neighbours.Add(neighbouringNode);
		}
	}
}