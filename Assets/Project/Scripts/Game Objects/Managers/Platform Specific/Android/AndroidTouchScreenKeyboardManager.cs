using UnityEngine;
using UnityEngine.Events;

#if UNITY_ANDROID
[DefaultExecutionOrder(-400)]
#endif
public class AndroidTouchScreenKeyboardManager : MonoBehaviour
{
	public UnityEvent<bool> keyboardVisibilityStateWasChangedEvent;

#if UNITY_ANDROID
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