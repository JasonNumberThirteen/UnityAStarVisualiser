using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidUIScaleAdjuster : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField, Range(0.01f, 1f)] private float scale = 0.8f;
	[SerializeField] private AndroidUIScaleAdjusterElement[] elements;
#endif

	private void Start()
	{
#if UNITY_ANDROID
		AdjustCanvasScale();
		AdjustElements();
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
	private void AdjustCanvasScale()
	{
		var mainCanvasUI = ObjectMethods.FindComponentOfType<MainCanvasUI>();

		if(mainCanvasUI == null)
		{
			return;
		}

		var mainCanvasScaler = mainCanvasUI.GetCanvasScaler();
		
		if(mainCanvasScaler != null)
		{
			mainCanvasScaler.referenceResolution *= scale;
		}
	}

	private void AdjustElements()
	{
		elements.ForEach(adjustableElement =>
		{
			AdjustElementSizeIfNeeded(adjustableElement);
			AdjustElementAnchoredPositionYIfNeeded(adjustableElement);
		});
	}

	private void AdjustElementSizeIfNeeded(AndroidUIScaleAdjusterElement element)
	{
		if(!element.HeightShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = element.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.ScaleBy(1, scale);
		}
	}
	
	private void AdjustElementAnchoredPositionYIfNeeded(AndroidUIScaleAdjusterElement element)
	{
		if(!element.AnchoredPositionYShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = element.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.anchoredPosition = rectTransform.anchoredPosition.ScaleBy(1, scale);
		}
	}
#endif
}