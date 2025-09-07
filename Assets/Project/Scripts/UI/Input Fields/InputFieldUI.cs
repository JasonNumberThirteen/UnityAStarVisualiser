using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldUI : MonoBehaviour
{
	public UnityEvent<string> editWasFinishedEvent;
	
	private TMP_InputField inputField;

	public string GetText() => inputField.text;

	public void SetText(string text)
	{
		inputField.text = text;
	}

	public void SetInteractable(bool interactable)
	{
		inputField.interactable = interactable;
	}

	public void SetCharacterLimit(int characterLimit)
	{
		inputField.characterLimit = characterLimit;
	}

	protected virtual void Awake()
	{
		inputField = GetComponent<TMP_InputField>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			inputField.onEndEdit.AddListener(OnEditWasFinished);
		}
		else
		{
			inputField.onEndEdit.RemoveListener(OnEditWasFinished);
		}
	}

	private void OnEditWasFinished(string text)
	{
		editWasFinishedEvent?.Invoke(text);
	}
}