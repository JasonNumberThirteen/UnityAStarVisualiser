using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UserInputController : MonoBehaviour
{
	public UnityEvent<Vector2> movementKeyPressedEvent;

	private void OnNavigate(InputValue inputValue)
	{
		movementKeyPressedEvent?.Invoke(inputValue.Get<Vector2>());
	}
}