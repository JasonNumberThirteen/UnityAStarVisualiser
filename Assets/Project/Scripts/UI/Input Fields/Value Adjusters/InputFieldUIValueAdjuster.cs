using System.Text;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(InputFieldUI))]
public abstract class InputFieldUIValueAdjuster<T> : MonoBehaviour where T : struct
{
	public UnityEvent<T> valueWasChangedEvent;
	
	[SerializeField] protected T minimumValue;
	[SerializeField] protected T maximumValue;

	protected InputFieldUI inputFieldUI;

	private T cachedValue;

	private readonly StringBuilder stringBuilder = new();

	protected abstract T GetValidatedValue(T value);
	protected abstract bool ValuesAreIdentical(T valueA, T valueB);
	protected abstract bool TextRepresentsValue(string text, out T value);

	protected virtual void Awake()
	{
		inputFieldUI = GetComponent<InputFieldUI>();

		RegisterToListeners(true);
		ValidateText(inputFieldUI.GetText());
	}

	protected virtual void OnDestroy()
	{
		RegisterToListeners(false);
	}

	protected void UpdateValueIfNeeded(T value)
	{
		if(ValuesAreIdentical(cachedValue, value))
		{
			return;
		}

		cachedValue = value;

		SetText(cachedValue.ToString());
		valueWasChangedEvent?.Invoke(cachedValue);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			inputFieldUI.editWasFinishedEvent.AddListener(OnEditWasFinished);
		}
		else
		{
			inputFieldUI.editWasFinishedEvent.RemoveListener(OnEditWasFinished);
		}
	}

	private void OnEditWasFinished(string text)
	{
		if(text.Length >= 1)
		{
			ValidateText(text);
		}

		inputFieldUI.SetText(stringBuilder.ToString());
	}

	private void ValidateText(string text)
	{
		if(TextRepresentsValue(text, out var value))
		{
			UpdateValueIfNeeded(GetValidatedValue(value));
		}
	}

	private void SetText(string text)
	{
		stringBuilder.Clear();
		stringBuilder.Append(text);
	}
}