using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextUI : MonoBehaviour
{
	protected TextMeshProUGUI text;

	public void SetText(string text)
	{
		this.text.SetText(text);
	}

	protected virtual void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
}