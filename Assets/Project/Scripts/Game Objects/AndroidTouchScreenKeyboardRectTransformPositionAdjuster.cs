using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
[RequireComponent(typeof(RectTransform))]
#endif
public class AndroidTouchScreenKeyboardRectTransformPositionAdjuster : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private CanvasScaler canvasScaler;
	
	private RectTransform rectTransform;
	private Vector2 initialPosition;
	private AndroidTouchScreenKeyboardManager androidTouchScreenKeyboardManager;
#endif

	private void Awake()
	{
#if UNITY_ANDROID
		rectTransform = GetComponent<RectTransform>();
		initialPosition = rectTransform.anchoredPosition;
		androidTouchScreenKeyboardManager = ObjectMethods.FindComponentOfType<AndroidTouchScreenKeyboardManager>();

		RegisterToListeners(true);
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(androidTouchScreenKeyboardManager != null)
			{
				androidTouchScreenKeyboardManager.keyboardVisibilityStateWasChangedEvent.AddListener(OnKeyboardVisibilityStateWasChanged);
			}
		}
		else
		{
			if(androidTouchScreenKeyboardManager != null)
			{
				androidTouchScreenKeyboardManager.keyboardVisibilityStateWasChangedEvent.RemoveListener(OnKeyboardVisibilityStateWasChanged);
			}
		}
	}

	private void OnKeyboardVisibilityStateWasChanged(bool visible)
	{
		var y = visible ? GetOffsetDependingOnAnchor() : initialPosition.y;
		
		rectTransform.anchoredPosition = new Vector2(initialPosition.x, y);
	}

	private float GetOffsetDependingOnAnchor()
	{
		var canvasHeight = canvasScaler != null ? canvasScaler.referenceResolution.y : 0f;
		var rectTransformHeight = rectTransform.sizeDelta.y;
		
		return rectTransform.pivot.y switch
		{
			1 => 0f,
			0.5f => (canvasHeight - rectTransformHeight)*0.5f,
			0f => canvasHeight - rectTransformHeight,
			_ => 0f
		};
	}
#endif
}