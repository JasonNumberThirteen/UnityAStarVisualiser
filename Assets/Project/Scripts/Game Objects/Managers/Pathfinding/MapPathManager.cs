using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapPathManager : MonoBehaviour
{
	public UnityEvent resultsWereClearedEvent;
	public UnityEvent<MapTileNode> mapTileNodeWasVisitedEvent;
	public UnityEvent<List<MapTileNode>> pathWasFoundEvent;
	public UnityEvent<bool> pathfindingProcessStateWasChangedEvent;
	
	private readonly List<MapTileNode> pathMapTileNodes = new();
	private readonly MapTileNodesQueue mapTileNodesQueue = new();

	private bool pathWasFound;
	private bool pathfindingWasStarted;
	private MapTile startMapTile;
	private MapTile destinationMapTile;
	private Coroutine pathfindingCoroutine;
	private HeuristicManager heuristicManager;
	private SimulationManager simulationManager;
	private MapTileNodesManager mapTileNodesManager;
	private MapGenerationManager mapGenerationManager;
	private VisualiserEventsManager visualiserEventsManager;

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

			SetCoroutineEnabled(pathfindingWasStarted);
			pathfindingProcessStateWasChangedEvent?.Invoke(pathfindingWasStarted);
		}
	}
	
	public void InitiatePathfindingProcess()
	{
		ClearResults();
		GetSpecialMapTiles();
		StartFindingPath();
	}

	public void ClearResults()
	{
		if(pathfindingWasStarted)
		{
			return;
		}

		ResetMapTileNodesData();
		pathMapTileNodes.Clear();
		mapTileNodesQueue.Clear();
		resultsWereClearedEvent?.Invoke();
	}

	public void InterruptPathfinding()
	{
		PathfindingWasStarted = false;
	}

	private void GetSpecialMapTiles()
	{
		if(mapGenerationManager == null)
		{
			return;
		}
		
		startMapTile = mapGenerationManager.GetMapTileOfType(MapTileType.Start);
		destinationMapTile = mapGenerationManager.GetMapTileOfType(MapTileType.Destination);
	}

	private void StartFindingPath()
	{
		if(startMapTile == null || destinationMapTile == null)
		{
			return;
		}

		CreateConnectionsBetweenMapTileNodes();
		mapTileNodesQueue.Add(startMapTile.GetMapTileNode());

		PathWasFound = false;
	}

	private void CreateConnectionsBetweenMapTileNodes()
	{
		if(mapTileNodesManager != null)
		{
			mapTileNodesManager.CreateConnectionsBetweenMapTileNodes();
		}
	}

	private void ResetMapTileNodesData()
	{
		if(mapTileNodesManager != null && mapGenerationManager != null)
		{
			mapTileNodesManager.ResetMapTileNodesData(mapGenerationManager.GetMapTiles());
		}
	}

	private void SetCoroutineEnabled(bool enabled)
	{
		if(enabled && pathfindingCoroutine == null)
		{
			pathfindingCoroutine = StartCoroutine(FindPathToDestination());
		}
		else if(!enabled && pathfindingCoroutine != null)
		{
			StopCoroutine(pathfindingCoroutine);
			
			pathfindingCoroutine = null;
		}
	}

	private void Awake()
	{
		heuristicManager = ObjectMethods.FindComponentOfType<HeuristicManager>();
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();
		mapTileNodesManager = ObjectMethods.FindComponentOfType<MapTileNodesManager>();
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
		
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
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
		
		RegisterToMapGenerationListeners(register);
	}

	private void RegisterToMapGenerationListeners(bool register)
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		if(register)
		{
			mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
		}
		else
		{
			mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
		}
	}

	private void OnMapTilesWereAdded(List<MapTile> mapTiles)
	{
		ClearResults();
	}

	private void OnMapTilesWereRemoved(List<MapTile> mapTiles)
	{
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

	private IEnumerator FindPathToDestination()
	{
		var simulationIsEnabled = simulationManager != null && simulationManager.SimulationIsEnabled();
		
		while (!pathWasFound && !mapTileNodesQueue.IsEmpty())
		{
			if(VisitMapTileNode(mapTileNodesQueue.GetNext()) && simulationIsEnabled)
			{
				yield return simulationManager.GetNextStepDelayDependingOnSimulationType();
			}
		}

		PathfindingWasStarted = false;
	}

	private bool VisitMapTileNode(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return false;
		}
		
		mapTileNode.SetTileNodeType(MapTileNodeType.Visited);
		mapTileNodesQueue.Remove(mapTileNode);
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
			FinishFindingPathOn(mapTileNode);
		}
		else
		{
			OperateOnNeighboursOf(mapTileNode);
		}
	}

	private void FinishFindingPathOn(MapTileNode mapTileNode)
	{
		if(mapTileNode == null)
		{
			return;
		}

		PathWasFound = true;

		MapTileNodeMethods.InvokeActionOnMapTileNodesBelongingToPath(mapTileNode, mapTileNode => pathMapTileNodes.Add(mapTileNode));
		pathMapTileNodes.ForEach(mapTileNode => mapTileNode.SetTileNodeType(MapTileNodeType.BelongingToPath));
		pathWasFoundEvent?.Invoke(pathMapTileNodes);
	}

	private void OperateOnNeighboursOf(MapTileNode parentMapTileNode)
	{
		if(parentMapTileNode == null)
		{
			return;
		}
		
		var neighbours = parentMapTileNode.GetNeighbours().Where(neighbour => neighbour.GetMapTileNodeType() != MapTileNodeType.Visited);

		neighbours.ForEach(neighbour => OperateOnNeighbourIfNeeded(parentMapTileNode, neighbour));
	}

	private void OperateOnNeighbourIfNeeded(MapTileNode parentMapTileNode, MapTileNode neighbouringMapTileNode)
	{
		if(parentMapTileNode == null || neighbouringMapTileNode == null)
		{
			return;
		}
		
		var totalCostToReachNeighbourFromParent = parentMapTileNode.GetTotalCostToReach(neighbouringMapTileNode);
		var neighbourCanBeProcessed = !mapTileNodesQueue.Contains(neighbouringMapTileNode) || totalCostToReachNeighbourFromParent < neighbouringMapTileNode.GetMapTileNodeData().RealValue;

		if(neighbourCanBeProcessed)
		{
			ProcessNeighbouringNode(parentMapTileNode, neighbouringMapTileNode, totalCostToReachNeighbourFromParent);
		}
	}

	private void ProcessNeighbouringNode(MapTileNode parentMapTileNode, MapTileNode neighbouringMapTileNode, float totalCostToReachNeighbourFromParent)
	{
		if(parentMapTileNode == null || neighbouringMapTileNode == null)
		{
			return;
		}
		
		neighbouringMapTileNode.GetMapTileNodeData().SetValues(parentMapTileNode, totalCostToReachNeighbourFromParent, GetHeuristicValueOf(neighbouringMapTileNode));
		mapTileNodesQueue.Add(neighbouringMapTileNode, !mapTileNodesQueue.Contains(neighbouringMapTileNode));
	}

	private float GetHeuristicValueOf(MapTileNode mapTileNode)
	{
		if(heuristicManager == null || destinationMapTile == null || mapTileNode == null)
		{
			return 0f;
		}
		
		return heuristicManager.GetHeuristicValue(mapTileNode.GetPosition(), destinationMapTile.GetMapTileNode().GetPosition());
	}
}