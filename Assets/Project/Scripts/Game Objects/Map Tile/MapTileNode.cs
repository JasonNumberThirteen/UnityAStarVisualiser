using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTileNode : MonoBehaviour
{
	public UnityEvent<MapTileNodeType> mapTileNodeTypeWasChangedEvent;

	public int Weight {get; set;}

	private MapTileNodeType mapTileNodeType;
	private PathfindingManager pathfindingManager;

	private readonly MapTileNodeData mapTileNodeData = new();
	private readonly List<MapTileNode> neighbours = new();

	public Vector2 GetPosition() => transform.position;
	public List<MapTileNode> GetNeighbours() => neighbours;
	public MapTileNodeType GetMapTileNodeType() => mapTileNodeType;
	public MapTileNodeData GetMapTileNodeData() => mapTileNodeData;

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
}