using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private ToggleUI showMapTilesLegendToggleUI;
	[SerializeField] private ToggleUI showInstructionsToggleUI;
	[SerializeField] private ToggleUI showMapAreaToggleUI;
	[SerializeField] private ToggleUI showPathTrailToggleUI;
	[SerializeField] private ToggleUI enableDiagonalMovementToggleUI;
	[SerializeField] private ToggleUI enableSimulationModeToggleUI;

	private MapTilesLegendPanelUI mapTilesLegendPanelUI;
	private InstructionsPanelUI instructionsPanelUI;
	private MapAreaIndicator mapAreaIndicator;
	private SimulationSettingsPanelUI simulationSettingsPanelUI;
	private MapPathManager mapPathManager;
	private PathfindingManager pathfindingManager;
	private SimulationManager simulationManager;
	private MapTilesPathTrailManager mapTilesPathTrailManager;
#if UNITY_ANDROID
	private AndroidTouchScreenKeyboardManager androidTouchScreenKeyboardManager;
#endif

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
		UpdateUIElementsDependantOnToggleUIStates();
	}

	private void Awake()
	{
		mapTilesLegendPanelUI = ObjectMethods.FindComponentOfType<MapTilesLegendPanelUI>();
		instructionsPanelUI = ObjectMethods.FindComponentOfType<InstructionsPanelUI>();
		mapAreaIndicator = ObjectMethods.FindComponentOfType<MapAreaIndicator>();
		simulationSettingsPanelUI = ObjectMethods.FindComponentOfType<SimulationSettingsPanelUI>();
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
		pathfindingManager = ObjectMethods.FindComponentOfType<PathfindingManager>();
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();
		mapTilesPathTrailManager = ObjectMethods.FindComponentOfType<MapTilesPathTrailManager>();
#if UNITY_ANDROID
		androidTouchScreenKeyboardManager = ObjectMethods.FindComponentOfType<AndroidTouchScreenKeyboardManager>();
#endif

		UpdateUIElementsDependantOnToggleUIStates();
		SetMapAreaIndicatorActive(showMapAreaToggleUI != null && showMapAreaToggleUI.IsOn());
		SetPathTrailEnabled(showPathTrailToggleUI != null && showPathTrailToggleUI.IsOn());
		SetDiagonalMovementEnabled(enableDiagonalMovementToggleUI != null && enableDiagonalMovementToggleUI.IsOn());
		SetSimulationEnabled(enableSimulationModeToggleUI != null && enableSimulationModeToggleUI.IsOn());
		RegisterToListeners(true);
	}

	private void UpdateUIElementsDependantOnToggleUIStates()
	{
		SetPanelUIActiveDependingOnToggle(mapTilesLegendPanelUI, showMapTilesLegendToggleUI);
		SetPanelUIActiveDependingOnToggle(instructionsPanelUI, showInstructionsToggleUI);
		SetPanelUIActiveDependingOnToggle(simulationSettingsPanelUI, enableSimulationModeToggleUI);
	}

	private void SetPanelUIActiveDependingOnToggle(PanelUI panelUI, ToggleUI toggleUI)
	{
		if(panelUI != null && toggleUI != null)
		{
			panelUI.SetActive(gameObject.activeSelf && toggleUI.IsOn());
		}
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(showMapTilesLegendToggleUI != null)
		{
			showMapTilesLegendToggleUI.RegisterToValueChangeListener(OnShowMapTilesLegendToggleUIValueChanged, register);
		}

		if(showInstructionsToggleUI != null)
		{
			showInstructionsToggleUI.RegisterToValueChangeListener(OnShowInstructionsToggleUIValueChanged, register);
		}

		if(showMapAreaToggleUI != null)
		{
			showMapAreaToggleUI.RegisterToValueChangeListener(SetMapAreaIndicatorActive, register);
		}

		if(showPathTrailToggleUI != null)
		{
			showPathTrailToggleUI.RegisterToValueChangeListener(SetPathTrailEnabled, register);
		}

		if(enableDiagonalMovementToggleUI != null)
		{
			enableDiagonalMovementToggleUI.RegisterToValueChangeListener(SetDiagonalMovementEnabled, register);
		}

		if(enableSimulationModeToggleUI != null)
		{
			enableSimulationModeToggleUI.RegisterToValueChangeListener(SetSimulationEnabled, register);
		}
		
		if(register)
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}

#if UNITY_ANDROID
			if(androidTouchScreenKeyboardManager != null)
			{
				androidTouchScreenKeyboardManager.keyboardVisibilityStateWasChangedEvent.AddListener(OnKeyboardVisibilityStateWasChanged);
			}
#endif
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}

#if UNITY_ANDROID
			if(androidTouchScreenKeyboardManager != null)
			{
				androidTouchScreenKeyboardManager.keyboardVisibilityStateWasChangedEvent.RemoveListener(OnKeyboardVisibilityStateWasChanged);
			}
#endif
		}
	}

	private void OnShowMapTilesLegendToggleUIValueChanged(bool enabled)
	{
		SetPanelUIActive(mapTilesLegendPanelUI, enabled);
	}

	private void OnShowInstructionsToggleUIValueChanged(bool enabled)
	{
		SetPanelUIActive(instructionsPanelUI, enabled);
	}

	private void SetPanelUIActive(PanelUI panelUI, bool active)
	{
		if(panelUI != null)
		{
			panelUI.SetActive(active);
		}
	}

	private void SetMapAreaIndicatorActive(bool enabled)
	{
		if(mapAreaIndicator != null)
		{
			mapAreaIndicator.SetActive(enabled);
		}
	}

	private void SetPathTrailEnabled(bool enabled)
	{
		if(mapTilesPathTrailManager != null)
		{
			mapTilesPathTrailManager.SetPathTrailEnabled(enabled);
		}
	}

	private void SetDiagonalMovementEnabled(bool enabled)
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.SetDiagonalMovementEnabled(enabled);
		}
	}

	private void SetSimulationEnabled(bool enabled)
	{
		if(simulationManager != null)
		{
			simulationManager.SetSimulationEnabled(enabled);
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		var toggleUIs = new List<ToggleUI>(){enableDiagonalMovementToggleUI, enableSimulationModeToggleUI};

		toggleUIs.ForEach(toggleUI => SetToggleUIInteractable(toggleUI, !started));
	}

	private void SetToggleUIInteractable(ToggleUI toggleUI, bool interactable)
	{
		if(toggleUI != null)
		{
			toggleUI.SetInteractable(interactable);
		}
	}

#if UNITY_ANDROID
	private void OnKeyboardVisibilityStateWasChanged(bool visible)
	{
		if(gameObject.activeSelf)
		{
			SetSimulationEnabled(!visible && enableSimulationModeToggleUI != null && enableSimulationModeToggleUI.IsOn());
		}
	}
#endif
}