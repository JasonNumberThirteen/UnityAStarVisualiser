using UnityEngine;

public class MainSceneCameraMovementController : MonoBehaviour, IPrimaryWindowElement
{
	[SerializeField, Min(0f)] private float movementSpeed = 10f;
	
	private bool inputIsActive = true;
	private Vector2 movementDirection;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	private void Awake()
	{
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();

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
				userInputController.movementKeyWasPressedEvent.AddListener(OnMovementKeyWasPressed);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.movementKeyWasPressedEvent.RemoveListener(OnMovementKeyWasPressed);
			}
		}
	}

	private void OnMovementKeyWasPressed(Vector2 movementVector)
	{
		movementDirection = movementVector;
	}

	private void LateUpdate()
	{
		if(mainSceneCamera != null && inputIsActive && !movementDirection.IsZero())
		{
			mainSceneCamera.Translate(movementSpeed*Time.deltaTime*movementDirection);
		}
	}
}