using TMPro;
using UnityEngine;

public class SimulationSettingsPanelUI : PanelUI
{
	[SerializeField] private TMP_Dropdown simulationTypeDropdownUI;
	[SerializeField] private PanelUI simulationStepDelaySliderUIPanelUI;
	[SerializeField] private ButtonUI simulationStepForwardButtonUI;
	[SerializeField] private ButtonUI interruptSimulationButtonUI;
	
	private SimulationManager simulationManager;
	private PathfindingManager pathfindingManager;

	private void Awake()
	{
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();
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
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
			}
		}
		else
		{
			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
			}
		}

		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.RegisterToClickListener(OnSimulationStepForwardButtonUIClicked, register);
		}

		if(interruptSimulationButtonUI != null)
		{
			interruptSimulationButtonUI.RegisterToClickListener(OnInterruptSimulationButtonUIClicked, register);
		}

		RegisterToSimulationManagerListeners(register);
	}

	private void RegisterToSimulationManagerListeners(bool register)
	{
		if(simulationManager == null)
		{
			return;
		}

		if(register)
		{
			simulationManager.simulationEnabledStateWasChangedEvent.AddListener(SetActive);
			simulationManager.simulationTypeWasChangedEvent.AddListener(SetSelectableUIsInteractableDependingOnSimulationType);
		}
		else
		{
			simulationManager.simulationEnabledStateWasChangedEvent.RemoveListener(SetActive);
			simulationManager.simulationTypeWasChangedEvent.RemoveListener(SetSelectableUIsInteractableDependingOnSimulationType);
		}
	}

	private void OnSimulationStepForwardButtonUIClicked()
	{
		if(simulationManager != null)
		{
			simulationManager.SetSimulationPaused(false);
		}
	}

	private void OnInterruptSimulationButtonUIClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.InterruptPathfindingIfNeeded();
		}
	}

	private void OnEnable()
	{
		SetSelectableUIsInteractableDependingOnPathfindingProcessState(false);
		
		if(simulationManager != null)
		{
			SetSelectableUIsInteractableDependingOnSimulationType(simulationManager.GetSimulationType());
		}
	}

	private void SetSelectableUIsInteractableDependingOnPathfindingProcessState(bool interactable)
	{
		if(simulationTypeDropdownUI != null)
		{
			simulationTypeDropdownUI.interactable = !interactable;
		}

		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.SetInteractable(interactable);
		}
		
		if(interruptSimulationButtonUI != null)
		{
			interruptSimulationButtonUI.SetInteractable(interactable);
		}
	}

	private void SetSelectableUIsInteractableDependingOnSimulationType(SimulationType simulationType)
	{
		if(simulationStepDelaySliderUIPanelUI != null)
		{
			simulationStepDelaySliderUIPanelUI.SetActive(simulationType == SimulationType.Timed);
		}
		
		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.gameObject.SetActive(simulationType == SimulationType.Stepwise);
		}
	}
}