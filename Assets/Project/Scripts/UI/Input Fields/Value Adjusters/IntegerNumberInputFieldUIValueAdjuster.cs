using TMPro;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
[RequireComponent(typeof(TMP_InputField))]
public class IntegerNumberInputFieldUIValueAdjuster : MonoBehaviour
{
	public UnityEvent<int> valueWasChangedEvent;
	
	[SerializeField] private int minimumNumber = 0;
	[SerializeField] private int maximumNumber = 100;

	private readonly StringBuilder stringBuilder = new();

	private TMP_InputField inputField;

	private void Awake()
	{
		inputField = GetComponent<TMP_InputField>();

		RegisterToListeners(true);
		ValidateText(inputField.text);
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
		if(text.Length >= 1)
		{
			ValidateText(text);
		}

		inputField.text = GetCachedString();
	}

	private void ValidateText(string text)
	{
		var textRepresentsIntegerNumber = int.TryParse(text, out var integerNumber);
		var integerNumberValue = textRepresentsIntegerNumber ? Mathf.Clamp(integerNumber, minimumNumber, maximumNumber) : 0;

		if(GetCachedString().Equals(text))
		{
			return;
		}
			
		stringBuilder.Clear();
		stringBuilder.Append(integerNumberValue);
		valueWasChangedEvent?.Invoke(integerNumberValue);
	}

	private string GetCachedString() => stringBuilder.ToString();
}