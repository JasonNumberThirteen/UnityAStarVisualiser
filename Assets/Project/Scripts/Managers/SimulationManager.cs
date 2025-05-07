using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class SimulationManager : MonoBehaviour
{
	public UnityEvent<bool> simulationEnabledStateWasChangedEvent;
	
	private bool simulationIsEnabled;
	private float simulationStepDelay;

	public bool SimulationIsEnabled() => simulationIsEnabled;
	public float GetSimulationStepDelay() => simulationStepDelay;

	public void SetSimulationEnabled(bool enabled)
	{
		simulationIsEnabled = enabled;

		simulationEnabledStateWasChangedEvent?.Invoke(simulationIsEnabled);		
	}

	public void SetSimulationStepDelay(float stepDelay)
	{
		simulationStepDelay = stepDelay;
	}
}