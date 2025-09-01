using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class SimulationManager : MonoBehaviour
{
	public UnityEvent<bool> simulationEnabledStateWasChangedEvent;
	public UnityEvent<SimulationType> simulationTypeWasChangedEvent;

	private SimulationType simulationType;	
	private bool simulationIsEnabled;
	private float simulationStepDelay;
	private bool simulationIsPaused;
	private PathfindingManager pathfindingManager;

	public SimulationType GetSimulationType() => simulationType;
	public bool SimulationIsEnabled() => simulationIsEnabled;
	public float GetSimulationStepDelay() => simulationStepDelay;
	public bool SimulationIsPaused() => simulationIsPaused;

	public void SetSimulationType(SimulationType simulationType)
	{
		this.simulationType = simulationType;

		simulationTypeWasChangedEvent?.Invoke(this.simulationType);
	}

	public void SetSimulationEnabled(bool enabled)
	{
		simulationIsEnabled = enabled;

		simulationEnabledStateWasChangedEvent?.Invoke(simulationIsEnabled);		
	}

	public void SetSimulationStepDelay(float stepDelay)
	{
		simulationStepDelay = stepDelay;
	}

	public void InitiateStepForward()
	{
		simulationIsPaused = false;
	}

	public IEnumerator GetNextStepDelayDependingOnSimulationType()
	{
		switch (GetSimulationType())
		{
			case SimulationType.Timed:
				yield return new WaitForSeconds(GetSimulationStepDelay());
				break;
			
			case SimulationType.Stepwise:
				yield return new WaitUntil(() => !SimulationIsPaused());
				break;
		}
	}

	private void Awake()
	{
		pathfindingManager = ObjectMethods.FindComponentOfType<PathfindingManager>();

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
			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
				pathfindingManager.mapTileNodeWasVisitedEvent.AddListener(OnMapTileNodeWasVisited);
			}
		}
		else
		{
			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
				pathfindingManager.mapTileNodeWasVisitedEvent.RemoveListener(OnMapTileNodeWasVisited);
			}
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		simulationIsPaused = started && simulationType == SimulationType.Stepwise;
	}

	private void OnMapTileNodeWasVisited(MapTileNode mapTileNode)
	{
		if(simulationType == SimulationType.Stepwise)
		{
			simulationIsPaused = true;
		}
	}
}