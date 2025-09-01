using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SimulationStepForwardButtonUI : MonoBehaviour
{
	private Button button;
	private SimulationManager simulationManager;

	private void Awake()
	{
		button = GetComponent<Button>();
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();

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
			button.onClick.AddListener(OnButtonWasClicked);
		}
		else
		{
			button.onClick.RemoveListener(OnButtonWasClicked);
		}
	}

	private void OnButtonWasClicked()
	{
		if(simulationManager != null)
		{
			simulationManager.InitiateStepForward();
		}
	}
}