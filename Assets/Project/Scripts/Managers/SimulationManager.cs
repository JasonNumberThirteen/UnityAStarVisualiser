using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class SimulationManager : MonoBehaviour
{
	public UnityEvent<bool> simulationEnabledStateWasChangedEvent;
	public UnityEvent<SimulationType> simulationTypeWasChangedEvent;

	private bool simulationIsEnabled;
	private bool simulationIsPaused;
	private float simulationStepDelay;
	private SimulationType simulationType;
	private MapPathManager mapPathManager;

	public bool SimulationIsEnabled() => simulationIsEnabled;
	public bool SimulationIsPaused() => simulationIsPaused;
	public float GetSimulationStepDelay() => simulationStepDelay;
	public SimulationType GetSimulationType() => simulationType;

	public void SetSimulationEnabled(bool enabled)
	{
		simulationIsEnabled = enabled;

		simulationEnabledStateWasChangedEvent?.Invoke(simulationIsEnabled);		
	}

	public void SetSimulationPaused(bool paused)
	{
		simulationIsPaused = paused;
	}

	public void SetSimulationStepDelay(float stepDelay)
	{
		simulationStepDelay = stepDelay;
	}

	public void SetSimulationType(SimulationType simulationType)
	{
		this.simulationType = simulationType;

		simulationTypeWasChangedEvent?.Invoke(this.simulationType);
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
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();

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
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
				mapPathManager.mapTileNodeWasVisitedEvent.AddListener(OnMapTileNodeWasVisited);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
				mapPathManager.mapTileNodeWasVisitedEvent.RemoveListener(OnMapTileNodeWasVisited);
			}
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		SetSimulationPaused(started && SimulationTypeIsSetTo(SimulationType.Stepwise));
	}

	private void OnMapTileNodeWasVisited(MapTileNode mapTileNode)
	{
		if(SimulationTypeIsSetTo(SimulationType.Stepwise))
		{
			SetSimulationPaused(true);
		}
	}

	private bool SimulationTypeIsSetTo(SimulationType simulationType) => this.simulationType == simulationType;
}