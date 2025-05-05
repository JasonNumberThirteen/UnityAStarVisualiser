using TMPro;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
[RequireComponent(typeof(TMP_InputField))]
public class DecimalNumberInputFieldUIValueAdjuster : MonoBehaviour
{
	public UnityEvent<float> valueWasChangedEvent;
	
	[SerializeField] private float minimumNumber = 0;
	[SerializeField] private float maximumNumber = 100;

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
		var textRepresentsDecimalNumber = float.TryParse(text, out var decimalNumber);
		var decimalNumberValue = textRepresentsDecimalNumber ? Mathf.Clamp(decimalNumber, minimumNumber, maximumNumber) : 0f;

		if(GetCachedString().Equals(text))
		{
			return;
		}
			
		stringBuilder.Clear();
		stringBuilder.Append(decimalNumberValue);
		valueWasChangedEvent?.Invoke(decimalNumberValue);
	}

	private string GetCachedString() => stringBuilder.ToString();
}