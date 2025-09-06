using UnityEngine;

public class SimulationSettingsPanelUI : PanelUI
{
	[SerializeField] private DropdownUI simulationTypeDropdownUI;
	[SerializeField] private PanelUI simulationStepDelaySliderUIPanelUI;
	[SerializeField] private ButtonUI simulationStepForwardButtonUI;
	[SerializeField] private ButtonUI interruptSimulationButtonUI;
	
	private MapPathManager mapPathManager;
	private SimulationManager simulationManager;
	private PathfindingManager pathfindingManager;

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
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
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
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
			pathfindingManager.InterruptPathfindingIfPossible();
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
			simulationTypeDropdownUI.SetInteractable(!interactable);
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