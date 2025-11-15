using UnityEngine;

public class InformationTextUI : TextUI
{
	protected override void Awake()
	{
		base.Awake();
		SetText(GetText());
	}

	private string GetText() => StringMethods.GetMultilineString("\"A* Visualiser\"", Application.version, "2025");
}