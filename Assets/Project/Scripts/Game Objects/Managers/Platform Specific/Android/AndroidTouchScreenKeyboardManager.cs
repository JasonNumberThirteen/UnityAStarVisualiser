using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Events;

[DefaultExecutionOrder(-400)]
#endif
public class AndroidTouchScreenKeyboardManager : MonoBehaviour
{
#if UNITY_ANDROID
	public UnityEvent<bool> keyboardVisibilityStateWasChangedEvent;

	private bool keyboardIsVisible;

	private bool KeyboardIsVisible
	{
		set
		{
			var keyboardWasVisible = keyboardIsVisible;

			keyboardIsVisible = value;

			if(keyboardWasVisible != keyboardIsVisible)
			{
				keyboardVisibilityStateWasChangedEvent?.Invoke(keyboardIsVisible);
			}
		}
	}
#endif

#if !UNITY_ANDROID
	private void Awake()
	{
		Destroy(gameObject);
	}
#else
	private void Update()
	{
		if(TouchScreenKeyboard.isSupported)
		{
			HandleKeyboardVisibilityState();
		}
	}

	private void HandleKeyboardVisibilityState()
	{
		var keyboardIsCurrentlyVisible = TouchScreenKeyboard.visible;

		if(keyboardIsCurrentlyVisible && !keyboardIsVisible)
		{
			KeyboardIsVisible = true;
		}
		else if(!keyboardIsCurrentlyVisible && keyboardIsVisible)
		{
			KeyboardIsVisible = false;
		}
	}
#endif
}