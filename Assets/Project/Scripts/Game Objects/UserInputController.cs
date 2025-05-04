using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInputController : MonoBehaviour
{
	public UnityEvent<Vector2> movementKeyPressedEvent;
	public UnityEvent<Vector2> wheelScrolledEvent;

	private void OnNavigate(InputValue inputValue)
	{
		movementKeyPressedEvent?.Invoke(inputValue.Get<Vector2>());
	}

	private void OnScrollWheel(InputValue inputValue)
	{
		wheelScrolledEvent?.Invoke(inputValue.Get<Vector2>());
	}
}