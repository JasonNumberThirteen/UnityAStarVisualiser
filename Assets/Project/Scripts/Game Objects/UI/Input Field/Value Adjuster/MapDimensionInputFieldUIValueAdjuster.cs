using UnityEngine;

public class MapDimensionInputFieldUIValueAdjuster : InputFieldUIValueAdjuster<int>
{
	private MapBoundariesManager mapBoundariesManager;

	protected override void Awake()
	{
		mapBoundariesManager = ObjectMethods.FindComponentOfType<MapBoundariesManager>();
		
		base.Awake();
		SetInitialValues();
	}

	private void SetInitialValues()
	{
		if(mapBoundariesManager == null)
		{
			return;
		}
		
		SetMinimumValue(mapBoundariesManager.GetLowerBound());
		SetMaximumValue(mapBoundariesManager.GetUpperBound());
	}

	private void SetMinimumValue(int minimumValue)
	{
		this.minimumValue = minimumValue;

		if(this.minimumValue > maximumValue)
		{
			maximumValue = this.minimumValue;
		}

		AdjustValueToChangedRangeIfNeeded();
	}
	
	private void SetMaximumValue(int maximumValue)
	{
		this.maximumValue = maximumValue;

		if(this.maximumValue < minimumValue)
		{
			minimumValue = this.maximumValue;
		}

		AdjustValueToChangedRangeIfNeeded();
		inputFieldUI.SetCharacterLimit(this.maximumValue.GetNumberOfDigits());
	}

	private void AdjustValueToChangedRangeIfNeeded()
	{
		if(!TextRepresentsValue(inputFieldUI.GetText(), out var value))
		{
			return;
		}

		var validatedValue = GetValidatedValue(value);
			
		UpdateValueIfNeeded(validatedValue);
		inputFieldUI.SetText(validatedValue.ToString());
	}

	protected override int GetValidatedValue(int value) => Mathf.Clamp(value, minimumValue, maximumValue);
	protected override bool ValuesAreIdentical(int valueA, int valueB) => valueA == valueB;
	protected override bool TextRepresentsValue(string text, out int value) => int.TryParse(text, out value);
}