using UnityEngine;

[RequireComponent(typeof(DecimalNumberInputFieldUIValueAdjuster))]
public class HeuristicWeightInputFieldUI : MonoBehaviour
{
	private DecimalNumberInputFieldUIValueAdjuster decimalNumberInputFieldUIValueAdjuster;
	private HeuristicManager heuristicManager;

	private void Awake()
	{
		decimalNumberInputFieldUIValueAdjuster = GetComponent<DecimalNumberInputFieldUIValueAdjuster>();
		heuristicManager = ObjectMethods.FindComponentOfType<HeuristicManager>();

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
			decimalNumberInputFieldUIValueAdjuster.valueWasChangedEvent.AddListener(OnValueWasChanged);
		}
		else
		{
			decimalNumberInputFieldUIValueAdjuster.valueWasChangedEvent.RemoveListener(OnValueWasChanged);
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