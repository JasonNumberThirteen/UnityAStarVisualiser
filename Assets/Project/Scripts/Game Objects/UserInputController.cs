using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInputController : MonoBehaviour
{
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
}