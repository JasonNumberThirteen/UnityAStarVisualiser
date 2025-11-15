using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : MonoBehaviour
{
	private readonly List<IMapEditingElement> mapEditingElements = new();

	private bool pathfindingWasStarted;
	private bool diagonalMovementIsEnabled;
	private MapPathManager mapPathManager;

	public bool DiagonalMovementIsEnabled() => diagonalMovementIsEnabled;

	public void SetDiagonalMovementEnabled(bool enabled)
	{
		diagonalMovementIsEnabled = enabled;
	}
	
	public void InitiatePathfindingProcessIfPossible()
	{
		if(!pathfindingWasStarted && mapPathManager != null)
		{
			mapPathManager.InitiatePathfindingProcess();
		}
	}

	public void ClearResults()
	{
		if(mapPathManager != null)
		{
			mapPathManager.ClearResults();
		}
	}

	public void InterruptPathfindingIfPossible()
	{
		if(!pathfindingWasStarted)
		{
			return;
		}

		pathfindingWasStarted = false;

		if(mapPathManager != null)
		{
			mapPathManager.InterruptPathfinding();
		}
	}

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
		
		mapEditingElements.AddRange(ObjectMethods.FindInterfacesOfType<IMapEditingElement>());
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
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessWasStarted);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessWasStarted);
			}
		}
	}

	private void OnPathfindingProcessWasStarted(bool started)
	{
		pathfindingWasStarted = started;

		mapEditingElements.ForEach(mapEditingElement => mapEditingElement.SetMapEditingElementActive(!pathfindingWasStarted));
	}
}