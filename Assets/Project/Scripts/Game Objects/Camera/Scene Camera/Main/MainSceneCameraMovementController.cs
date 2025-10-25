using UnityEngine;

public class MainSceneCameraMovementController : MonoBehaviour, IPrimaryWindowElement
{
	private float movementSpeed;
	private bool inputIsActive = true;
	private Vector2 movementDirection;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	public void SetMovementSpeed(float movementSpeed)
	{
		this.movementSpeed = movementSpeed;
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
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.movementKeyWasPressedEvent.AddListener(OnMovementKeyWasPressed);
#endif
			}
		}
		else
		{
			if(userInputController != null)
			{
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.movementKeyWasPressedEvent.RemoveListener(OnMovementKeyWasPressed);
#endif
			}
		}
	}

#if UNITY_STANDALONE || UNITY_WEBGL
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
#endif
}