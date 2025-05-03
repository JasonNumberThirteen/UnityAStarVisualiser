using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : PanelUI
{
	[SerializeField] private Toggle showMapTilesLegendToggle;
	[SerializeField] private Toggle showInstructionsToggle;

	private MapTilesLegendPanelUI mapTilesLegendPanelUI;
	private InstructionsPanelUI instructionsPanelUI;

	private void Awake()
	{
		mapTilesLegendPanelUI = FindFirstObjectByType<MapTilesLegendPanelUI>();
		instructionsPanelUI = FindFirstObjectByType<InstructionsPanelUI>();

		SetPanelUIActiveDependingOnToggle(mapTilesLegendPanelUI, showMapTilesLegendToggle);
		SetPanelUIActiveDependingOnToggle(instructionsPanelUI, showInstructionsToggle);
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

	private void SetPanelUIActive(PanelUI panelUI, bool active)
	{
		if(panelUI != null)
		{
			panelUI.SetActive(active);
		}
	} 
}