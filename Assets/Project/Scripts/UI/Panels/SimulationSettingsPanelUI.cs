using UnityEngine;

public class SimulationSettingsPanelUI : PanelUI
{
	[SerializeField] private DropdownUI simulationTypeDropdownUI;
	[SerializeField] private PanelUI simulationStepDelaySliderUIPanelUI;
	[SerializeField] private ButtonUI simulationStepForwardButtonUI;
	[SerializeField] private ButtonUI interruptSimulationButtonUI;
	
	private bool pathfindingWasStarted;
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
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
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
			simulationManager.simulationTypeWasChangedEvent.AddListener(SetUIElementsActiveDependingOnSimulationType);
		}
		else
		{
			simulationManager.simulationEnabledStateWasChangedEvent.RemoveListener(SetActive);
			simulationManager.simulationTypeWasChangedEvent.RemoveListener(SetUIElementsActiveDependingOnSimulationType);
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		pathfindingWasStarted = started;

		SetUIElementsInteractableDependingOnPathfindingProcessState();
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
		SetUIElementsInteractableDependingOnPathfindingProcessState();
		
		if(simulationManager != null)
		{
			SetUIElementsActiveDependingOnSimulationType(simulationManager.GetSimulationType());
		}
	}

	private void SetUIElementsInteractableDependingOnPathfindingProcessState()
	{
		if(simulationTypeDropdownUI != null)
		{
			simulationTypeDropdownUI.SetInteractable(!pathfindingWasStarted);
		}

		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.SetInteractable(pathfindingWasStarted);
		}
		
		if(interruptSimulationButtonUI != null)
		{
			interruptSimulationButtonUI.SetInteractable(pathfindingWasStarted);
		}
	}

	private void SetUIElementsActiveDependingOnSimulationType(SimulationType simulationType)
	{
		if(simulationStepDelaySliderUIPanelUI != null)
		{
			simulationStepDelaySliderUIPanelUI.SetActive(simulationType == SimulationType.Timed);
		}
		
		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.SetActive(simulationType == SimulationType.Stepwise);
		}
	}
}