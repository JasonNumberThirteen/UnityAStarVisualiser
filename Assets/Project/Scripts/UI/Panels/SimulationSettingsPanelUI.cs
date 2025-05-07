using UnityEngine;
using UnityEngine.UI;

public class SimulationSettingsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Slider simulationStepDelaySliderUI;
	[SerializeField] private Button interruptSimulationButtonUI;
	
	private SimulationManager simulationManager;
	private PathfindingManager pathfindingManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
	}

	private void Awake()
	{
		simulationManager = FindFirstObjectByType<SimulationManager>();
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();

		RegisterToListeners(true);
		SetInterruptSimulationButtonUIInteractable(false);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(interruptSimulationButtonUI != null)
			{
				interruptSimulationButtonUI.onClick.AddListener(OnInterruptSimulationButtonUIClicked);
			}
			
			if(simulationManager != null)
			{
				simulationManager.simulationEnabledStateWasChangedEvent.AddListener(OnSimulationEnabledStateWasChanged);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(SetInterruptSimulationButtonUIInteractable);
			}
		}
		else
		{
			if(interruptSimulationButtonUI != null)
			{
				interruptSimulationButtonUI.onClick.RemoveListener(OnInterruptSimulationButtonUIClicked);
			}
			
			if(simulationManager != null)
			{
				simulationManager.simulationEnabledStateWasChangedEvent.RemoveListener(OnSimulationEnabledStateWasChanged);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(SetInterruptSimulationButtonUIInteractable);
			}
		}
	}

	private void OnInterruptSimulationButtonUIClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.InterruptPathfindingIfNeeded();
		}
	}

	private void OnSimulationEnabledStateWasChanged(bool enabled)
	{
		SetActive(enabled);
	}

	private void SetInterruptSimulationButtonUIInteractable(bool interactable)
	{
		if(interruptSimulationButtonUI != null)
		{
			interruptSimulationButtonUI.interactable = interactable;
		}
	}
}