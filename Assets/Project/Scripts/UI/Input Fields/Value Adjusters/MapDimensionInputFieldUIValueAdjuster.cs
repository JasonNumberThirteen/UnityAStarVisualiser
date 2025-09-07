using UnityEngine;

public class MapDimensionInputFieldUIValueAdjuster : InputFieldUIValueAdjuster<int>
{
	protected override void Awake()
	{
		base.Awake();
		inputFieldUI.SetCharacterLimit(maximumValue.GetNumberOfDigits());
	}

	protected override int GetValidatedValue(int value) => Mathf.Clamp(value, minimumValue, maximumValue);
	protected override bool ValuesAreIdentical(int valueA, int valueB) => valueA == valueB;
	protected override bool TextRepresentsValue(string text, out int value) => int.TryParse(text, out value);
}