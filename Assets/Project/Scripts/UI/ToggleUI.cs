using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(Toggle))]
public class ToggleUI : MonoBehaviour
{
	private Toggle toggle;

	public bool IsOn() => toggle.isOn;

	public void RegisterToValueChangeListener(UnityAction<bool> action, bool register)
	{
		if(register)
		{
			toggle.onValueChanged.AddListener(action);	
		}
		else
		{
			toggle.onValueChanged.RemoveListener(action);
		}
	}

	public void SetInteractable(bool interactable)
	{
		toggle.interactable = interactable;
	}

	private void Awake()
	{
		toggle = GetComponent<Toggle>();
	}
}