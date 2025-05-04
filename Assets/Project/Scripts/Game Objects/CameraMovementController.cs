using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
	[SerializeField, Min(0f)] private float movementSpeed = 10f;
	
	private Camera mainCamera;
	private Vector2 movementDirection;
	private UserInputController userInputController;

	private void Awake()
	{
		mainCamera = Camera.main;
		userInputController = FindFirstObjectByType<UserInputController>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(userInputController != null)
			{
				userInputController.movementKeyPressedEvent.AddListener(OnMovementKeyPressed);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.movementKeyPressedEvent.RemoveListener(OnMovementKeyPressed);
			}
		}
	}

	private void OnMovementKeyPressed(Vector2 movementVector)
	{
		movementDirection = movementVector;
	}

	private void LateUpdate()
	{
		if(mainCamera != null && movementDirection != Vector2.zero)
		{
			mainCamera.gameObject.transform.Translate(movementSpeed*Time.deltaTime*movementDirection);
		}
	}
}