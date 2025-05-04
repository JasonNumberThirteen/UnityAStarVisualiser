using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTileNode : MonoBehaviour
{
	public UnityEvent<MapTileNodeType> mapTileNodeTypeWasChangedEvent;
	
	public int Weight {get; set;}
	public MapTileNode Parent {get; set;}

	private MapTileNodeType mapTileNodeType;
	private PathfindingManager pathfindingManager;

	private readonly List<MapTileNode> neighbors = new();

	public Vector2 GetPosition() => transform.position;
	public List<MapTileNode> GetNeighbors() => neighbors;
	public MapTileNodeType GetMapTileNodeType() => mapTileNodeType;

	public float GetCostToReachTo(MapTileNode destinationMapTileNode)
	{
		if(destinationMapTileNode == null)
		{
			return float.MaxValue;
		}

		var heuristicValue = pathfindingManager != null ? pathfindingManager.GetHeuristicValue(GetPosition(), destinationMapTileNode.GetPosition()) : 0f;

		return GetPathLengthFromStart() + heuristicValue;
	}

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
		
		Parent = null;
	}

	public void FindNeighbors(List<MapTileNode> mapTileNodes)
	{
		neighbors.Clear();

		var gridSize = MapTile.GRID_SIZE;
		var allDirections = new List<Vector2>
		{
			Vector2.up*gridSize,
			Vector2.down*gridSize,
			Vector2.left*gridSize,
			Vector2.right*gridSize
		};

		allDirections.ForEach(position => AddNeighborIfPossible(mapTileNodes, position));
	}

	private void Awake()
	{
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
	}

	private void AddNeighborIfPossible(List<MapTileNode> mapTileNodes, Vector2 direction)
	{
		var neighboringPosition = GetPosition() + direction;
		var neighboringNode = mapTileNodes.FirstOrDefault(mapTileNode => mapTileNode.GetPosition() == neighboringPosition);

		if(neighboringNode != null)
		{
			neighbors.Add(neighboringNode);
		}
	}

	private int GetPathLengthFromStart()
	{
		var length = 0;

		if(pathfindingManager != null)
		{
			pathfindingManager.OperateOnMapTileNodes(this, mapTileNode => length += mapTileNode.Weight);
		}

		return length;
	}
}