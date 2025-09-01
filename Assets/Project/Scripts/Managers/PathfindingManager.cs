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
	private readonly List<MapTileNode> mapTileNodesWaitingForVisit = new();
	private readonly List<IMapEditingElement> mapEditingElements = new();

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
				mapGenerationManager.mapWasGeneratedEvent.AddListener(OnMapWasGenerated);
				mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
			}

			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.RemoveListener(OnMapWasGenerated);
				mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
			}

			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

	private void OnMapWasGenerated()
	{
		mapTilesInScene.Clear();
		mapTilesInScene.AddRange(FindObjectsByType<MapTile>(FindObjectsSortMode.None));
	}

	private void OnMapTilesWereAdded(List<MapTile> mapTiles)
	{
		mapTilesInScene.AddRange(mapTiles);
		ClearResults();
	}

	private void OnMapTilesWereRemoved(List<MapTile> mapTiles)
	{
		mapTilesInScene.RemoveAll(mapTiles.Contains);
		ClearResults();
	}

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
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
		mapTileNodesWaitingForVisit.Clear();
		CreateConnectionsBetweenMapTileNodes();
	}

	private void CreateConnectionsBetweenMapTileNodes()
	{
		var passableMapTileNodes = mapTilesInScene.Where(mapTile => mapTile.GetTileType() != MapTileType.Impassable).Select(mapTile => mapTile.GetMapTileNode()).ToList();
		
		mapTilesInScene.ForEach(mapTile => mapTile.GetMapTileNode().FindNeighbours(passableMapTileNodes));
	}

	private void InitiatePathfinder()
	{
		if(startMapTile == null || destinationMapTile == null)
		{
			return;
		}

		mapTilesInScene.ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
		InitiateStartMapTile();

		PathWasFound = false;
	}

	private void InitiateStartMapTile()
	{
		if(startMapTile != null)
		{
			mapTileNodesWaitingForVisit.Add(startMapTile.GetMapTileNode());
		}
	}

	private IEnumerator FindPathToDestination()
	{
		var simulationIsEnabled = simulationManager != null && simulationManager.SimulationIsEnabled();
		
		while (!pathWasFound && mapTileNodesWaitingForVisit.Count > 0)
		{
			var currentMapTileNode = mapTileNodesWaitingForVisit.OrderBy(mapTileNode => mapTileNode.GetMapTileNodeData().TotalCost).FirstOrDefault();

			if(VisitMapTileNodeIfPossible(currentMapTileNode) && simulationIsEnabled)
			{
				yield return simulationManager.GetNextStepDelayDependingOnSimulationType();
			}
		}

		PathfindingWasStarted = false;
	}

	private bool VisitMapTileNodeIfPossible(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return false;
		}
		
		mapTileNode.SetTileNodeType(MapTileNodeType.Visited);
		mapTileNodesWaitingForVisit.Remove(mapTileNode);
		mapTileNodeWasVisitedEvent?.Invoke(mapTileNode);
		OperateOnMapTileNode(mapTileNode);

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
			OperateOnNeighboursOf(mapTileNode);
		}
	}

	private void FinishSearchingOn(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		PathWasFound = true;

		DefinePathMapTileNodes(mapTileNode);
		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.SetTileNodeType(MapTileNodeType.BelongingToPath));
		pathWasFoundEvent?.Invoke(pathMapTileNodes);
	}

	private void DefinePathMapTileNodes(MapTileNode mapTileNodeToStartFrom)
	{
		var currentMapTileNode = mapTileNodeToStartFrom;
		var currentMapTileNodeData = currentMapTileNode.GetMapTileNodeData();

		while (currentMapTileNodeData.Parent != null)
		{
			pathMapTileNodes.Add(currentMapTileNode);

			currentMapTileNode = currentMapTileNodeData.Parent;
			currentMapTileNodeData = currentMapTileNode.GetMapTileNodeData();
		}	
	}

	private void OperateOnNeighboursOf(MapTileNode parentMapTileNode)
	{
		if(parentMapTileNode == null)
		{
			return;
		}
		
		var neighbours = parentMapTileNode.GetNeighbours().Where(neighbour => neighbour.GetMapTileNodeType() != MapTileNodeType.Visited).ToList();

		neighbours.ForEach(neighbour => OperateOnNeighbourIfNeeded(parentMapTileNode, neighbour));
	}

	private void OperateOnNeighbourIfNeeded(MapTileNode parentMapTileNode, MapTileNode neighbouringMapTileNode)
	{
		if(parentMapTileNode == null || neighbouringMapTileNode == null)
		{
			return;
		}
		
		var totalCostToReachNeighbourFromParent = GetTotalCostToReachNeighbourFromParent(parentMapTileNode, neighbouringMapTileNode);
		var neighbourIsAlreadyWaitingForVisit = mapTileNodesWaitingForVisit.Contains(neighbouringMapTileNode);
		var neighbourMapTileNodeData = neighbouringMapTileNode.GetMapTileNodeData();

		if(neighbourIsAlreadyWaitingForVisit && totalCostToReachNeighbourFromParent >= neighbourMapTileNodeData.RealValue)
		{
			return;
		}

		neighbourMapTileNodeData.SetValues(parentMapTileNode, totalCostToReachNeighbourFromParent, heuristicManager != null ? heuristicManager.GetHeuristicValue(neighbouringMapTileNode.GetPosition(), destinationMapTile.GetMapTileNode().GetPosition()) : 0f);

		if(!neighbourIsAlreadyWaitingForVisit)
		{
			mapTileNodesWaitingForVisit.Add(neighbouringMapTileNode);
		}
	}

	private float GetTotalCostToReachNeighbourFromParent(MapTileNode parentMapTileNode, MapTileNode neighbouringMapTileNode)
	{
		var distanceToNeighbour = DistanceMethods.DistanceBetweenPositionsIsSingleAxis(parentMapTileNode.GetPosition(), neighbouringMapTileNode.GetPosition()) ? 1 : Mathf.Sqrt(2);
		var neighbourRealValue = distanceToNeighbour*neighbouringMapTileNode.Weight;
		
		return parentMapTileNode.GetMapTileNodeData().RealValue + neighbourRealValue;
	}
}