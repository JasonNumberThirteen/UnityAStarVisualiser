using UnityEngine;

public class HeuristicWeightInputFieldUIValueAdjuster : InputFieldUIValueAdjuster<float>
{
	private HeuristicManager heuristicManager;

	protected override float GetValidatedValue(float value) => Mathf.Clamp(value, minimumValue, maximumValue);
	protected override bool ValuesAreIdentical(float valueA, float valueB) => Mathf.Approximately(valueA, valueB);
	protected override bool TextRepresentsValue(string text, out float value) => float.TryParse(text, out value);

	protected override void Awake()
	{
		heuristicManager = ObjectMethods.FindComponentOfType<HeuristicManager>();

		RegisterToListeners(true);
		base.Awake();
	}

	protected override void OnDestroy()
	{
		RegisterToListeners(false);
		base.Awake();
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			valueWasChangedEvent.AddListener(OnValueWasChanged);
		}
		else
		{
			valueWasChangedEvent.RemoveListener(OnValueWasChanged);
		}
	}

	private void OnValueWasChanged(float value)
	{
		if(heuristicManager != null)
		{
			heuristicManager.SetHeuristicWeight(value);
		}
	}
}