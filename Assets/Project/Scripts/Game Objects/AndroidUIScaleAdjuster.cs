using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidUIScaleAdjuster : MonoBehaviour
{
	[SerializeField, Range(0.01f, 1f)] private float scale = 0.8f;
	[SerializeField] private AndroidUIScaleAdjusterElement[] uiScaleAdjusterElements;

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
		uiScaleAdjusterElements.ForEach(uiScaleAdjusterElement =>
		{
			AdjustUIScaleAdjusterElementSizeIfNeeded(uiScaleAdjusterElement);
			AdjustUIScaleAdjusterElementAnchoredPositionYIfNeeded(uiScaleAdjusterElement);
		});
	}

	private void AdjustUIScaleAdjusterElementSizeIfNeeded(AndroidUIScaleAdjusterElement uiScaleAdjusterElement)
	{
		if(!uiScaleAdjusterElement.HeightShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = uiScaleAdjusterElement.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.ScaleBy(1, scale);
		}
	}
	
	private void AdjustUIScaleAdjusterElementAnchoredPositionYIfNeeded(AndroidUIScaleAdjusterElement uiScaleAdjusterElement)
	{
		if(!uiScaleAdjusterElement.AnchoredPositionYShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = uiScaleAdjusterElement.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.anchoredPosition = rectTransform.anchoredPosition.ScaleBy(1, scale);
		}
	}
#endif
}