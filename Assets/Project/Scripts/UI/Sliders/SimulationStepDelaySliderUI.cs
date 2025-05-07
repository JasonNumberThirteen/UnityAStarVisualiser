using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SimulationStepDelaySliderUI : MonoBehaviour
{
	private Slider slider;
	private SimulationManager simulationManager;

	private void Awake()
	{
		slider = GetComponent<Slider>();
		simulationManager = FindFirstObjectByType<SimulationManager>();

		RegisterToListeners(true);
		SetSimulationStepDelay(slider.value);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			slider.onValueChanged.AddListener(SetSimulationStepDelay);
		}
		else
		{
			slider.onValueChanged.RemoveListener(SetSimulationStepDelay);
		}
	}

	private void SetSimulationStepDelay(float value)
	{
		if(simulationManager != null)
		{
			simulationManager.SetSimulationStepDelay(value);
		}
	}
}