using UnityEngine;
#if UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine.Events;
#endif
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInputController : MonoBehaviour
{
#if UNITY_STANDALONE || UNITY_WEBGL
	public UnityEvent<Vector2> movementKeyWasPressedEvent;
	public UnityEvent<Vector2> mouseWheelWasScrolledEvent;

	private void OnNavigate(InputValue inputValue)
	{
		movementKeyWasPressedEvent?.Invoke(inputValue.Get<Vector2>());
	}

	private void OnScrollWheel(InputValue inputValue)
	{
		mouseWheelWasScrolledEvent?.Invoke(inputValue.Get<Vector2>());
	}
#endif
}