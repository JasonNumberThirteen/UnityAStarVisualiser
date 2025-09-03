public class SimulationStepDelayListenableSliderUI : ListenableSliderUI
{
	private SimulationManager simulationManager;

	protected override void Awake()
	{
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();
		
		base.Awake();
		OnValueWasChanged(slider.value);
	}

	protected override void OnValueWasChanged(float value)
	{
		if(simulationManager != null)
		{
			simulationManager.SetSimulationStepDelay(value);
		}
	}
}