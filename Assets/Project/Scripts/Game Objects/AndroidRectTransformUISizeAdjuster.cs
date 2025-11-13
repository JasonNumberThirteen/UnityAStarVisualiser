using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidRectTransformUISizeAdjuster : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private AndroidRectTransformUISizeAdjusterElement[] rectTransformUISizeAdjusterElements;
	[SerializeField, Min(32f)] private float size;
#endif

	private void Awake()
	{
#if UNITY_ANDROID
		rectTransformUISizeAdjusterElements.ForEach(AdjustRectTransformUISizeAdjusterElementIfNeeded);
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
	private void AdjustRectTransformUISizeAdjusterElementIfNeeded(AndroidRectTransformUISizeAdjusterElement rectTransformUISizeAdjusterElement)
	{
		if(rectTransformUISizeAdjusterElement != null)
		{
			AdjustRectTransformSizeIfPossible(rectTransformUISizeAdjusterElement);
		}
	}

	private void AdjustRectTransformSizeIfPossible(AndroidRectTransformUISizeAdjusterElement rectTransformUISizeAdjusterElement)
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