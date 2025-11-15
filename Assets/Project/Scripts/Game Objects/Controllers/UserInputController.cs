#if UNITY_ANDROID
using System.Linq;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine.InputSystem;
#elif UNITY_ANDROID
using UnityEngine.InputSystem.EnhancedTouch;
#endif

#if UNITY_STANDALONE || UNITY_WEBGL
[RequireComponent(typeof(PlayerInput))]
#endif
public class UserInputController : MonoBehaviour
{
	public UnityEvent<Vector2> movementKeyWasPressedEvent;
	public UnityEvent<Vector2> mouseWheelWasScrolledEvent;
	public UnityEvent<List<UnityEngine.InputSystem.EnhancedTouch.Touch>> touchesWereUpdatedEvent;

#if UNITY_STANDALONE || UNITY_WEBGL
	private void OnNavigate(InputValue inputValue)
	{
		movementKeyWasPressedEvent?.Invoke(inputValue.Get<Vector2>());
	}

	private void OnScrollWheel(InputValue inputValue)
	{
		mouseWheelWasScrolledEvent?.Invoke(inputValue.Get<Vector2>());
	}
#elif UNITY_ANDROID
	private void Awake()
	{
		EnhancedTouchSupport.Enable();
	}

	private void OnDestroy()
	{
		EnhancedTouchSupport.Disable();
	}

	private void Update()
	{
		touchesWereUpdatedEvent?.Invoke(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.ToList());
	}
#endif
}