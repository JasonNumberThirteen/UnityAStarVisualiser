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

	private float cachedValue;
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
	}

	private void ValidateText(string text)
	{
		var textRepresentsDecimalNumber = float.TryParse(text, out var decimalNumber);

		if(textRepresentsDecimalNumber)
		{
			UpdateValueIfNeeded(Mathf.Clamp(decimalNumber, minimumNumber, maximumNumber));
		}
	}

	private void UpdateValueIfNeeded(float value)
	{
		if(Mathf.Approximately(cachedValue, value))
		{
			return;
		}

		cachedValue = value;

		SetText(cachedValue.ToString());
		valueWasChangedEvent?.Invoke(cachedValue);
	}

	private void SetText(string text)
	{
		stringBuilder.Clear();
		stringBuilder.Append(text);

		inputField.text = stringBuilder.ToString();
	}
}