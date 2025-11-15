using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DefaultExecutionOrder(-200)]
[RequireComponent(typeof(Button))]
public class ButtonUI : MonoBehaviour
{
	private Button button;
	private TextMeshProUGUI buttonText;
	
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

	public void SetText(string text)
	{
		if(buttonText != null)
		{
			buttonText.text = text;
		}
	}

	public void SetInteractable(bool interactable)
	{
		button.interactable = interactable;
	}

	protected virtual void Awake()
	{
		button = GetComponent<Button>();
		buttonText = GetComponentInChildren<TextMeshProUGUI>();
	}
}