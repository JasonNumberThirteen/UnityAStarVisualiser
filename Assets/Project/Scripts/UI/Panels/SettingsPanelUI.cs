using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : PanelUI
{
	[SerializeField] private Toggle showMapTilesLegendToggle;
	[SerializeField] private Toggle showInstructionsToggle;
	[SerializeField] private Toggle enableDiagonalMovementToggle;

	private MapTilesLegendPanelUI mapTilesLegendPanelUI;
	private InstructionsPanelUI instructionsPanelUI;
	private PathfindingManager pathfindingManager;

	private void Awake()
	{
		mapTilesLegendPanelUI = FindFirstObjectByType<MapTilesLegendPanelUI>();
		instructionsPanelUI = FindFirstObjectByType<InstructionsPanelUI>();
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();

		SetPanelUIActiveDependingOnToggle(mapTilesLegendPanelUI, showMapTilesLegendToggle);
		SetPanelUIActiveDependingOnToggle(instructionsPanelUI, showInstructionsToggle);
		SetDiagonalMovementEnabled(enableDiagonalMovementToggle != null && enableDiagonalMovementToggle.isOn);
		RegisterToListeners(true);
	}

	private void SetPanelUIActiveDependingOnToggle(PanelUI panelUI, Toggle toggle)
	{
		if(panelUI != null && toggle != null)
		{
			panelUI.SetActive(toggle.isOn);
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
			if(showMapTilesLegendToggle != null)
			{
				showMapTilesLegendToggle.onValueChanged.AddListener(OnShowMapTilesLegendToggleValueChanged);
			}

			if(showInstructionsToggle != null)
			{
				showInstructionsToggle.onValueChanged.AddListener(OnShowInstructionsToggleValueChanged);
			}

			if(enableDiagonalMovementToggle != null)
			{
				enableDiagonalMovementToggle.onValueChanged.AddListener(OnEnableDiagonalMovementToggleValueChanged);
			}
		}
		else
		{
			if(showMapTilesLegendToggle != null)
			{
				showMapTilesLegendToggle.onValueChanged.RemoveListener(OnShowMapTilesLegendToggleValueChanged);
			}

			if(showInstructionsToggle != null)
			{
				showInstructionsToggle.onValueChanged.RemoveListener(OnShowInstructionsToggleValueChanged);
			}

			if(enableDiagonalMovementToggle != null)
			{
				enableDiagonalMovementToggle.onValueChanged.RemoveListener(OnEnableDiagonalMovementToggleValueChanged);
			}
		}
	}

	private void OnShowMapTilesLegendToggleValueChanged(bool enabled)
	{
		SetPanelUIActive(mapTilesLegendPanelUI, enabled);
	}

	private void OnShowInstructionsToggleValueChanged(bool enabled)
	{
		SetPanelUIActive(instructionsPanelUI, enabled);
	}

	private void OnEnableDiagonalMovementToggleValueChanged(bool enabled)
	{
		SetDiagonalMovementEnabled(enabled);
	}

	private void SetDiagonalMovementEnabled(bool enabled)
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.SetDiagonalMovementEnabled(enabled);
		}
	}

	private void SetPanelUIActive(PanelUI panelUI, bool active)
	{
		if(panelUI != null)
		{
			panelUI.SetActive(active);
		}
	} 
}