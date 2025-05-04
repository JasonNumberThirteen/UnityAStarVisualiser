using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform), typeof(TMP_InputField))]
public class InputFieldUIResiser : MonoBehaviour
{
	private RectTransform rectTransform;
	private TMP_InputField inputField;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		inputField = GetComponent<TMP_InputField>();

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
			inputField.onValueChanged.AddListener(OnValueChanged);
		}
		else
		{
			inputField.onValueChanged.RemoveListener(OnValueChanged);
		}
	}

	private void OnValueChanged(string text)
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inputField.preferredWidth);
	}
}