using UnityEngine;

public class SimulationOptionsPanelUI : MonoBehaviour
{
	[SerializeField] private SliderUI simulationStepDelaySliderUI;
	
	private SimulationManager simulationManager;

	private void Awake()
	{
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();

		
	}
}