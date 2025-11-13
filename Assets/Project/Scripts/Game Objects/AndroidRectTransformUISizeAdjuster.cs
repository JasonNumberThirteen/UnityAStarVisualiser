using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidRectTransformUISizeAdjuster : MonoBehaviour
{
	[SerializeField] private AndroidRectTransformUISizeAdjusterElement[] rectTransformUISizeAdjusterElements;
	[SerializeField, Min(32f)] private float size;

	private void Awake()
	{
#if UNITY_ANDROID
		rectTransformUISizeAdjusterElements.ForEach(AdjustElement);
#else
		Destroy(this);
#endif
	}

#if UNITY_ANDROID
	private void AdjustElement(AndroidRectTransformUISizeAdjusterElement rectTransformUISizeAdjusterElement)
	{
		if(rectTransformUISizeAdjusterElement != null)
		{
			AdjustElementSize(rectTransformUISizeAdjusterElement);
		}
	}

	private void AdjustElementSize(AndroidRectTransformUISizeAdjusterElement rectTransformUISizeAdjusterElement)
	{
		var rectTransform = rectTransformUISizeAdjusterElement.GetRectTransform();
		
		if(rectTransform == null)
		{
			return;
		}
		
		var sizeDelta = rectTransform.sizeDelta;
		var width = rectTransformUISizeAdjusterElement.WidthShouldBeAdjusted() ? size : sizeDelta.x;
		var height = rectTransformUISizeAdjusterElement.HeightShouldBeAdjusted() ? size : sizeDelta.y;

		rectTransform.sizeDelta = new(width, height);
	}
#endif
}