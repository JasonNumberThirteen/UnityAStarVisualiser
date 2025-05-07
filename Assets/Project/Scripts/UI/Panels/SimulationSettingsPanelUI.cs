using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulationSettingsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private TMP_Dropdown simulationTypeDropdownUI;
	[SerializeField] private PanelUI simulationStepDelaySliderUIPanelUI;
	[SerializeField] private Button simulationStepForwardButtonUI;
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
				simulationManager.simulationEnabledStateWasChangedEvent.AddListener(SetActive);
				simulationManager.simulationTypeWasChangedEvent.AddListener(SetSelectableUIsInteractableDependingOnSimulationType);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
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
				simulationManager.simulationEnabledStateWasChangedEvent.RemoveListener(SetActive);
				simulationManager.simulationTypeWasChangedEvent.RemoveListener(SetSelectableUIsInteractableDependingOnSimulationType);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(SetSelectableUIsInteractableDependingOnPathfindingProcessState);
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

	private void SetSelectableUIsInteractableDependingOnPathfindingProcessState(bool interactable)
	{
		if(simulationTypeDropdownUI != null)
		{
			simulationTypeDropdownUI.interactable = !interactable;
		}

		if(simulationStepForwardButtonUI != null)
		{
			simulationStepForwardButtonUI.interactable = interactable;
		}
		
		if(interruptSimulationButtonUI != null)
		{
			interruptSimulationButtonUI.interactable = interactable;
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