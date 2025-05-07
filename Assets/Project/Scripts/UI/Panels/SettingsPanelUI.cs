using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Toggle showMapTilesLegendToggleUI;
	[SerializeField] private Toggle showInstructionsToggleUI;
	[SerializeField] private Toggle enableDiagonalMovementToggleUI;

	private MapTilesLegendPanelUI mapTilesLegendPanelUI;
	private InstructionsPanelUI instructionsPanelUI;
	private PathfindingManager pathfindingManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);

		if(active)
		{
			UpdateUIElementsDependantOnToggleUIStates();
		}
	}

	private void Awake()
	{
		mapTilesLegendPanelUI = FindFirstObjectByType<MapTilesLegendPanelUI>();
		instructionsPanelUI = FindFirstObjectByType<InstructionsPanelUI>();
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();

		UpdateUIElementsDependantOnToggleUIStates();
		SetDiagonalMovementEnabled(enableDiagonalMovementToggleUI != null && enableDiagonalMovementToggleUI.isOn);
		RegisterToListeners(true);
	}

	private void UpdateUIElementsDependantOnToggleUIStates()
	{
		SetPanelUIActiveDependingOnToggle(mapTilesLegendPanelUI, showMapTilesLegendToggleUI);
		SetPanelUIActiveDependingOnToggle(instructionsPanelUI, showInstructionsToggleUI);
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
			if(showMapTilesLegendToggleUI != null)
			{
				showMapTilesLegendToggleUI.onValueChanged.AddListener(OnShowMapTilesLegendToggleUIValueChanged);
			}

			if(showInstructionsToggleUI != null)
			{
				showInstructionsToggleUI.onValueChanged.AddListener(OnShowInstructionsToggleUIValueChanged);
			}

			if(enableDiagonalMovementToggleUI != null)
			{
				enableDiagonalMovementToggleUI.onValueChanged.AddListener(OnEnableDiagonalMovementToggleUIValueChanged);
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

			if(enableDiagonalMovementToggleUI != null)
			{
				enableDiagonalMovementToggleUI.onValueChanged.RemoveListener(OnEnableDiagonalMovementToggleUIValueChanged);
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

	private void OnEnableDiagonalMovementToggleUIValueChanged(bool enabled)
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