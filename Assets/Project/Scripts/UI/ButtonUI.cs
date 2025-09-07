using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(Button))]
public class ButtonUI : MonoBehaviour
{
	private Button button;
	
	public void RegisterToClickListener(UnityAction action, bool register)
	{
		if(register)
		{
			button.onClick.AddListener(action);
		}
		else
		{
			button.onClick.RemoveListener(action);
		}
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	public void SetInteractable(bool interactable)
	{
		button.interactable = interactable;
	}

	private void Awake()
	{
		button = GetComponent<Button>();
	}
}