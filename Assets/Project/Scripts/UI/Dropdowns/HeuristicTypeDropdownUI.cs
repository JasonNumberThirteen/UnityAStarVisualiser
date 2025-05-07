using TMPro;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class HeuristicTypeDropdownUI : MonoBehaviour
{
	private TMP_Dropdown dropdown;
	private HeuristicManager heuristicManager;

	private void Awake()
	{
		dropdown = GetComponent<TMP_Dropdown>();
		heuristicManager = FindFirstObjectByType<HeuristicManager>();

		AddOptionsToDropdown();
		RegisterToListeners(true);
		SetInitialValueToDropdown();
	}

	private void AddOptionsToDropdown()
	{
		var heuristicTypeNames = Enum.GetNames(typeof(HeuristicType));
		var adjustedHeuristicTypeNames = new List<string>();

		foreach (var heuristicTypeName in heuristicTypeNames)
		{
			adjustedHeuristicTypeNames.Add(Regex.Replace(heuristicTypeName, "(\\B[A-Z])", " $1"));
		}
		
		dropdown.AddOptions(adjustedHeuristicTypeNames);
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
		if(heuristicManager != null)
		{
			heuristicManager.SetHeuristicType((HeuristicType)value);
		}
	}

	private void SetInitialValueToDropdown()
	{
		dropdown.value = Convert.ToInt32(HeuristicType.ManhattanDistance);
	}
}