using UnityEngine;

#if UNITY_ANDROID
[DefaultExecutionOrder(100)]
#endif
public class AndroidCanvasUIScaleAdjuster : MonoBehaviour
{
	[SerializeField, Range(0.01f, 1f)] private float scale = 0.8f;
	[SerializeField] private AndroidCanvasUIScaleAdjusterElement[] canvasUIScaleAdjusterElements;

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
		canvasUIScaleAdjusterElements.ForEach(canvasUIScaleAdjusterElement =>
		{
			AdjustCanvasUIScaleAdjusterElementSizeIfNeeded(canvasUIScaleAdjusterElement);
			AdjustCanvasUIScaleAdjusterElementAnchoredPositionYIfNeeded(canvasUIScaleAdjusterElement);
		});
	}

	private void AdjustCanvasUIScaleAdjusterElementSizeIfNeeded(AndroidCanvasUIScaleAdjusterElement canvasUIScaleAdjusterElement)
	{
		if(!canvasUIScaleAdjusterElement.HeightShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = canvasUIScaleAdjusterElement.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.ScaleBy(1, scale);
		}
	}
	
	private void AdjustCanvasUIScaleAdjusterElementAnchoredPositionYIfNeeded(AndroidCanvasUIScaleAdjusterElement canvasUIScaleAdjusterElement)
	{
		if(!canvasUIScaleAdjusterElement.AnchoredPositionYShouldBeAdjusted())
		{
			return;
		}

		var rectTransform = canvasUIScaleAdjusterElement.GetRectTransform();

		if(rectTransform != null)
		{
			rectTransform.anchoredPosition = rectTransform.anchoredPosition.ScaleBy(1, scale);
		}
	}
#endif
}