using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Toggle showMapTilesLegendToggleUI;
	[SerializeField] private Toggle showInstructionsToggleUI;
	[SerializeField] private Toggle showPathTrailToggleUI;
	[SerializeField] private Toggle enableDiagonalMovementToggleUI;
	[SerializeField] private Toggle enableSimulationModeToggleUI;

	private MapTilesLegendPanelUI mapTilesLegendPanelUI;
	private InstructionsPanelUI instructionsPanelUI;
	private SimulationSettingsPanelUI simulationSettingsPanelUI;
	private PathfindingManager pathfindingManager;
	private SimulationManager simulationManager;
	private PathTrailManager pathTrailManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
		UpdateUIElementsDependantOnToggleUIStates();
	}

	private void Awake()
	{
		mapTilesLegendPanelUI = FindFirstObjectByType<MapTilesLegendPanelUI>();
		instructionsPanelUI = FindFirstObjectByType<InstructionsPanelUI>();
		simulationSettingsPanelUI = FindFirstObjectByType<SimulationSettingsPanelUI>();
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
		simulationManager = FindFirstObjectByType<SimulationManager>();
		pathTrailManager = FindFirstObjectByType<PathTrailManager>();

		UpdateUIElementsDependantOnToggleUIStates();
		SetPathTrailEnabled(showPathTrailToggleUI != null && showPathTrailToggleUI.isOn);
		SetDiagonalMovementEnabled(enableDiagonalMovementToggleUI != null && enableDiagonalMovementToggleUI.isOn);
		SetSimulationEnabled(enableSimulationModeToggleUI != null && enableSimulationModeToggleUI.isOn);
		RegisterToListeners(true);
	}

	private void UpdateUIElementsDependantOnToggleUIStates()
	{
		SetPanelUIActiveDependingOnToggle(mapTilesLegendPanelUI, showMapTilesLegendToggleUI);
		SetPanelUIActiveDependingOnToggle(instructionsPanelUI, showInstructionsToggleUI);
		SetPanelUIActiveDependingOnToggle(simulationSettingsPanelUI, enableSimulationModeToggleUI);
	}

	private void SetPanelUIActiveDependingOnToggle(PanelUI panelUI, Toggle toggle)
	{
		if(panelUI != null && toggle != null)
		{
			panelUI.SetActive(gameObject.activeSelf && toggle.isOn);
		}
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(showMapTilesLegendToggleUI != null)
			{
				showMapTilesLegendToggleUI.onValueChanged.AddListener(OnShowMapTilesLegendToggleUIValueChanged);
			}

			if(showInstructionsToggleUI != null)
			{
				showInstructionsToggleUI.onValueChanged.AddListener(OnShowInstructionsToggleUIValueChanged);
			}

			if(showPathTrailToggleUI != null)
			{
				showPathTrailToggleUI.onValueChanged.AddListener(SetPathTrailEnabled);
			}

			if(enableDiagonalMovementToggleUI != null)
			{
				enableDiagonalMovementToggleUI.onValueChanged.AddListener(SetDiagonalMovementEnabled);
			}

			if(enableSimulationModeToggleUI != null)
			{
				enableSimulationModeToggleUI.onValueChanged.AddListener(SetSimulationEnabled);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
		}
		else
		{
			if(showMapTilesLegendToggleUI != null)
			{
				showMapTilesLegendToggleUI.onValueChanged.RemoveListener(OnShowMapTilesLegendToggleUIValueChanged);
			}

			if(showInstructionsToggleUI != null)
			{
				showInstructionsToggleUI.onValueChanged.RemoveListener(OnShowInstructionsToggleUIValueChanged);
			}

			if(showPathTrailToggleUI != null)
			{
				showPathTrailToggleUI.onValueChanged.RemoveListener(SetPathTrailEnabled);
			}

			if(enableDiagonalMovementToggleUI != null)
			{
				enableDiagonalMovementToggleUI.onValueChanged.RemoveListener(SetDiagonalMovementEnabled);
			}

			if(enableSimulationModeToggleUI != null)
			{
				enableSimulationModeToggleUI.onValueChanged.RemoveListener(SetSimulationEnabled);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
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

	private void SetPathTrailEnabled(bool enabled)
	{
		if(pathTrailManager != null)
		{
			pathTrailManager.SetPathTrailEnabled(enabled);
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

	private void SetPanelUIActive(PanelUI panelUI, bool active)
	{
		if(panelUI != null)
		{
			panelUI.SetActive(active);
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		var selectableUIs = new List<Selectable>()
		{
			enableDiagonalMovementToggleUI,
			enableSimulationModeToggleUI
		};

		selectableUIs.ForEach(selectableUI =>
		{
			if(selectableUI != null)
			{
				selectableUI.interactable = !started;
			}
		});
	}
}