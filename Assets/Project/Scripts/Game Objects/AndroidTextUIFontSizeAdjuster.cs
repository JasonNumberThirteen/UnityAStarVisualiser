using TMPro;
using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidTextUIFontSizeAdjuster : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private TextMeshProUGUI[] texts;
	[SerializeField, Min(8f)] private float fontSize;
#endif

	private void Awake()
	{
#if UNITY_ANDROID
		texts.ForEach(AdjustTextUIFontSizeIfNeeded);
#else
		Destroy(this);
#endif
	}

#if UNITY_ANDROID
	private void AdjustTextUIFontSizeIfNeeded(TextMeshProUGUI text)
	{
		if(text != null)
		{
			text.fontSize = fontSize;
		}
	}
#endif
}