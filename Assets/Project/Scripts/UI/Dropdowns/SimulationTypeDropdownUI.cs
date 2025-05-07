using TMPro;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class SimulationTypeDropdownUI : MonoBehaviour
{
	private TMP_Dropdown dropdown;
	private SimulationManager simulationManager;

	private void Awake()
	{
		dropdown = GetComponent<TMP_Dropdown>();
		simulationManager = FindFirstObjectByType<SimulationManager>();

		AddOptionsToDropdown();
		RegisterToListeners(true);
		SetInitialValueToDropdown();
	}

	private void AddOptionsToDropdown()
	{
		var simulationTypeNames = Enum.GetNames(typeof(SimulationType)).ToList();
		
		dropdown.AddOptions(simulationTypeNames);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			dropdown.onValueChanged.AddListener(OnValueChanged);
		}
		else
		{
			dropdown.onValueChanged.RemoveListener(OnValueChanged);
		}
	}

	private void OnValueChanged(int value)
	{
		if(simulationManager != null)
		{
			simulationManager.SetSimulationType((SimulationType)value);
		}
	}

	private void SetInitialValueToDropdown()
	{
		dropdown.value = Convert.ToInt32(SimulationType.Timed);
	}
}