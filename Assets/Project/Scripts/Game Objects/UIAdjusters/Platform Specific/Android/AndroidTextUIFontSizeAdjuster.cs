using TMPro;
using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidTextUIFontSizeAdjuster : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI[] texts;
	[SerializeField, Min(8f)] private float fontSize;

	private void Awake()
	{
#if UNITY_ANDROID
		texts.ForEach(AdjustTextFontSize);
#else
		Destroy(this);
#endif
	}

#if UNITY_ANDROID
	private void AdjustTextFontSize(TextMeshProUGUI text)
	{
		if(text != null)
		{
			text.fontSize = fontSize;
		}
	}
#endif
}