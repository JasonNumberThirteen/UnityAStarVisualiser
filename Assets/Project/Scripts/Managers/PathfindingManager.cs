using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathfindingManager : MonoBehaviour
{
	public UnityEvent<bool> pathfindingProcessStateWasChangedEvent;
	public UnityEvent<MapTileNode> mapTileNodeWasVisitedEvent;
	public UnityEvent<List<MapTileNode>> pathWasFoundEvent;
	public UnityEvent resultsWereClearedEvent;
	
	private readonly List<MapTile> mapTilesInScene = new();
	private readonly List<MapTileNode> pathMapTileNodes = new();
	private readonly List<IMapEditingElement> mapEditingElements = new();
	private readonly PriorityQueue<MapTileNode> priorityQueue = new();

	private MapTile startMapTile;
	private MapTile destinationMapTile;
	private bool pathWasFound;
	private bool pathfindingWasStarted;
	private bool diagonalMovementIsEnabled;
	private MapGenerationManager mapGenerationManager;
	private HeuristicManager heuristicManager;
	private VisualiserEventsManager visualiserEventsManager;
	private SimulationManager simulationManager;
	private Coroutine pathfindingCoroutine;

	private bool PathWasFound
	{
		set
		{
			pathWasFound = value;
			PathfindingWasStarted = !pathWasFound;
		}
	}

	private bool PathfindingWasStarted
	{
		set
		{
			pathfindingWasStarted = value;

			if(pathfindingWasStarted && pathfindingCoroutine == null)
			{
				pathfindingCoroutine = StartCoroutine(FindPathToDestination());
			}
			else if(!pathfindingWasStarted && pathfindingCoroutine != null)
			{
				StopCoroutine(pathfindingCoroutine);
				
				pathfindingCoroutine = null;
			}

			mapEditingElements.ForEach(mapEditingElement => mapEditingElement.SetMapEditingElementActive(!pathfindingWasStarted));
			pathfindingProcessStateWasChangedEvent?.Invoke(pathfindingWasStarted);
		}
	}

	public float GetHeuristicValue(Vector2 positionA, Vector2 positionB) => heuristicManager != null ? heuristicManager.GetHeuristicValue(positionA, positionB) : 0f;
	public bool DiagonalMovementIsEnabled() => diagonalMovementIsEnabled;
	
	public void FindPath()
	{
		if(pathfindingWasStarted)
		{
			return;
		}
		
		ClearData();
		InitiatePathfinder();
	}

	public void ClearResults()
	{
		if(pathfindingWasStarted)
		{
			return;
		}
		
		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.ResetData());
		pathMapTileNodes.Clear();
		mapTilesInScene.Where(mapTile => mapTile.GetMapTileNode().GetMapTileNodeType() == MapTileNodeType.Visited).ToList().ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
		resultsWereClearedEvent?.Invoke();
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

	public void SetDiagonalMovementEnabled(bool enabled)
	{
		diagonalMovementIsEnabled = enabled;
	}

	public void InterruptPathfindingIfNeeded()
	{
		if(pathfindingWasStarted)
		{
			PathfindingWasStarted = false;
		}
	}

	private void Awake()
	{
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		heuristicManager = FindFirstObjectByType<HeuristicManager>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();
		simulationManager = FindFirstObjectByType<SimulationManager>();

		mapEditingElements.AddRange(FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IMapEditingElement>());
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
				mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
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
				mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
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
	}

	private void OnMapTilesWereAdded(List<MapTile> mapTiles)
	{
		var mapEditingElementsToAdd = mapTiles.Select(mapTile => mapTile.GetComponentInChildren<IMapEditingElement>());
		
		mapEditingElements.AddRange(mapEditingElementsToAdd);
		mapTilesInScene.AddRange(mapTiles);
		ClearResults();
	}

	private void OnMapTilesWereRemoved(List<MapTile> mapTiles)
	{
		var mapEditingElementsToRemove = mapTiles.Select(mapTile => mapTile.GetComponentInChildren<IMapEditingElement>());

		mapEditingElements.RemoveAll(mapEditingElementsToRemove.Contains);
		mapTilesInScene.RemoveAll(mapTiles.Contains);
		ClearResults();
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
		
		if(eventTypesClearingColoredTiles.Contains(mapTileBoolVisualiserEvent.GetVisualiserEventType()))
		{
			ClearResults();
		}
	}

	private void ClearData()
	{
		startMapTile = mapTilesInScene.FirstOrDefault(mapTile => mapTile.GetTileType() == MapTileType.Start);
		destinationMapTile = mapTilesInScene.FirstOrDefault(mapTile => mapTile.GetTileType() == MapTileType.Destination);
		
		ClearResults();
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

		mapTilesInScene.ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
		startMapTile.SetWeightTo(0);
		AddMapTileNodeToQueue(startMapTile.GetMapTileNode());

		PathWasFound = false;
	}

	private IEnumerator FindPathToDestination()
	{
		var simulationIsEnabled = simulationManager != null && simulationManager.SimulationIsEnabled();
		
		while (!pathWasFound && priorityQueue.Count > 0)
		{
			if(VisitMapTileNodeIfNeeded(priorityQueue.Dequeue()) && simulationIsEnabled)
			{
				yield return simulationManager.GetNextStepDelayDependingOnSimulationType();
			}
		}

		PathfindingWasStarted = false;
	}

	private bool VisitMapTileNodeIfNeeded(MapTileNode mapTileNode)
	{
		if(mapTileNode == null || mapTileNode.GetMapTileNodeType() == MapTileNodeType.Visited || destinationMapTile == null)
		{
			return false;
		}

		mapTileNode.SetTileNodeType(MapTileNodeType.Visited);
		OperateOnMapTileNode(mapTileNode);
		mapTileNodeWasVisitedEvent?.Invoke(mapTileNode);

		return true;
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

		PathWasFound = true;

		OperateOnMapTileNodes(mapTileNode, mapTileNode => pathMapTileNodes.Add(mapTileNode));
		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.SetTileNodeType(MapTileNodeType.BelongingToPath));
		pathWasFoundEvent?.Invoke(pathMapTileNodes);
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