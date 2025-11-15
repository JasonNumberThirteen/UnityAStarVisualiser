using UnityEngine;

public class MapDimensionInputFieldUIValueAdjuster : InputFieldUIValueAdjuster<int>
{
	public void SetMinimumValue(int minimumValue)
	{
		this.minimumValue = minimumValue;

		if(this.minimumValue > maximumValue)
		{
			maximumValue = this.minimumValue;
		}

		AdjustValueToChangedRangeIfNeeded();
	}
	
	public void SetMaximumValue(int maximumValue)
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