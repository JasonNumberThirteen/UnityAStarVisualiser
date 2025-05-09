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

	private readonly List<MapTileNode> neighbours = new();

	public Vector2 GetPosition() => transform.position;
	public List<MapTileNode> GetNeighbours() => neighbours;
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

	public void FindNeighbours(List<MapTileNode> mapTileNodes)
	{
		var allowDiagonalDirections = pathfindingManager != null && pathfindingManager.DiagonalMovementIsEnabled();
		var directions = VectorMethods.GetDirectionsForFindingNeighbouringNodes(allowDiagonalDirections);

		neighbours.Clear();
		directions.ForEach(direction => AddNeighbourIfPossible(mapTileNodes, direction));
	}

	private void Awake()
	{
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
	}

	private void AddNeighbourIfPossible(List<MapTileNode> mapTileNodes, Vector2 direction)
	{
		var neighbouringPosition = GetPosition() + direction;
		var neighbouringNode = mapTileNodes.FirstOrDefault(mapTileNode => mapTileNode.GetPosition() == neighbouringPosition);

		if(neighbouringNode != null)
		{
			neighbours.Add(neighbouringNode);
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