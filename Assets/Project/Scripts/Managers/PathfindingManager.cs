using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
	private readonly List<MapTile> mapTilesInScene = new();
	private readonly List<MapTileNode> pathMapTileNodes = new();
	private readonly PriorityQueue<MapTileNode> priorityQueue = new();

	private MapTile startMapTile;
	private MapTile destinationMapTile;
	private bool pathWasFound;
	private MapGenerationManager mapGenerationManager;
	private HeuristicManager heuristicManager;
	private VisualiserEventsManager visualiserEventsManager;

	public float GetHeuristicValue(Vector2 positionA, Vector2 positionB) => heuristicManager != null ? heuristicManager.GetHeuristicValue(positionA, positionB) : 0f;
	
	public void FindPath()
	{
		ClearData();
		InitiatePathfinder();
		FindPathToDestination();
	}

	public void OperateOnMapTileNodes(MapTileNode mapTileNodeToStartFrom, Action<MapTileNode> action)
	{
		var currentMapTileNode = mapTileNodeToStartFrom;

		while (currentMapTileNode.Parent != null)
		{
			action?.Invoke(currentMapTileNode);

			currentMapTileNode = currentMapTileNode.Parent;
		}	
	}

	private void Awake()
	{
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		heuristicManager = FindFirstObjectByType<HeuristicManager>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

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
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.AddListener(OnMapGenerated);
			}

			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.RemoveListener(OnMapGenerated);
			}

			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnMapGenerated()
	{
		mapTilesInScene.Clear();
		mapTilesInScene.AddRange(FindObjectsByType<MapTile>(FindObjectsSortMode.None));
		
		startMapTile = mapTilesInScene.FirstOrDefault(mapTile => mapTile.GetTileType() == MapTileType.Start);
		destinationMapTile = mapTilesInScene.FirstOrDefault(mapTile => mapTile.GetTileType() == MapTileType.Destination);
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		var eventTypesClearingColoredTiles = new List<VisualiserEventType>()
		{
			VisualiserEventType.MapTileWeightWasChanged,
			VisualiserEventType.MapTileSelectionStateWasChanged
		};
		
		if(!eventTypesClearingColoredTiles.Contains(mapTileBoolVisualiserEvent.GetVisualiserEventType()))
		{
			return;
		}

		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.ResetData());
		pathMapTileNodes.Clear();
		mapTilesInScene.Where(mapTile => mapTile.GetMapTileNode().GetMapTileNodeType() == MapTileNodeType.Visited).ToList().ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
	}

	private void ClearData()
	{
		pathMapTileNodes.Clear();
		priorityQueue.Clear();
		CreateConnectionsBetweenMapTileNodes();
	}

	private void CreateConnectionsBetweenMapTileNodes()
	{
		var passableMapTileNodes = mapTilesInScene.Where(mapTile => mapTile.GetTileType() != MapTileType.Impassable).Select(mapTile => mapTile.GetMapTileNode()).ToList();
		
		mapTilesInScene.ForEach(mapTile => mapTile.GetMapTileNode().FindNeighbors(passableMapTileNodes));
	}

	private void InitiatePathfinder()
	{
		if(startMapTile == null || destinationMapTile == null)
		{
			return;
		}

		pathWasFound = false;

		mapTilesInScene.ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
		startMapTile.SetWeightTo(0);
		AddMapTileNodeToQueue(startMapTile.GetMapTileNode());
	}

	private void FindPathToDestination()
	{
		while (!pathWasFound && priorityQueue.Count > 0)
		{
			VisitMapTileNodeIfNeeded(priorityQueue.Dequeue());
		}
	}

	private void VisitMapTileNodeIfNeeded(MapTileNode mapTileNode)
	{
		if(mapTileNode == null || mapTileNode.GetMapTileNodeType() == MapTileNodeType.Visited || destinationMapTile == null)
		{
			return;
		}

		mapTileNode.SetTileNodeType(MapTileNodeType.Visited);
		OperateOnMapTileNode(mapTileNode);
	}

	private void OperateOnMapTileNode(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		if(mapTileNode == destinationMapTile.GetMapTileNode())
		{
			FinishSearchingOn(mapTileNode);
		}
		else
		{
			AddNeighborsOf(mapTileNode);
		}
	}

	private void FinishSearchingOn(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		pathWasFound = true;

		OperateOnMapTileNodes(mapTileNode, mapTileNode => pathMapTileNodes.Add(mapTileNode));
		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.SetTileNodeType(MapTileNodeType.BelongingToPath));
	}

	private void AddNeighborsOf(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		var neighbors = mapTileNode.GetNeighbors().Where(neighbor => neighbor.GetMapTileNodeType() != MapTileNodeType.Visited).ToList();

		neighbors?.ForEach(neighbor => SetupAndAddNeighbor(neighbor, mapTileNode));
	}

	private void SetupAndAddNeighbor(MapTileNode neighboringMapTile, MapTileNode parentMapTile)
	{
		neighboringMapTile.Parent = parentMapTile;

		AddMapTileNodeToQueue(neighboringMapTile);
	}

	private void AddMapTileNodeToQueue(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		var mapTileNodeCost = mapTileNode.GetCostToReachTo(destinationMapTile.GetMapTileNode());
		var mapTileNodeWithCost = new PriorityQueueElement<MapTileNode>(mapTileNode, mapTileNodeCost);

		priorityQueue.Enqueue(mapTileNodeWithCost);
	}
}