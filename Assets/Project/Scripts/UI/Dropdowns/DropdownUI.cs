using TMPro;
using UnityEngine;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownUI : MonoBehaviour
{
	protected TMP_Dropdown dropdown;

	public void SetInteractable(bool interactable)
	{
		dropdown.interactable = interactable;
	}

	protected virtual void Awake()
	{
		dropdown = GetComponent<TMP_Dropdown>();
	}
}