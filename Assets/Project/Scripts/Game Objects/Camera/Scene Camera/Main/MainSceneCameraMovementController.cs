using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneCameraMovementController : MonoBehaviour, IPrimaryWindowElement
{
	private float movementSpeed;
	private bool inputIsActive = true;
	private Vector2 movementDirection;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;

#if UNITY_ANDROID
	private static readonly float MOVEMENT_SPEED_ANDROID_TOUCH_DELTA_MULTIPLIER = 0.00125f;
#endif

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
#elif UNITY_ANDROID
				userInputController.touchesWereUpdatedEvent.AddListener(OnTouchesWereUpdated);
#endif
			}
		}
		else
		{
			if(userInputController != null)
			{
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.movementKeyWasPressedEvent.RemoveListener(OnMovementKeyWasPressed);
#elif UNITY_ANDROID
				userInputController.touchesWereUpdatedEvent.RemoveListener(OnTouchesWereUpdated);
#endif
			}
		}
	}

#if UNITY_STANDALONE || UNITY_WEBGL
	private void OnMovementKeyWasPressed(Vector2 movementVector)
	{
		movementDirection = movementVector;
	}
#endif

#if UNITY_ANDROID
	private void OnTouchesWereUpdated(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		if(mainSceneCamera == null || !inputIsActive || touches.Count != 1)
		{
			return;
		}

		var touch = touches.First();

		if(touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
		{
			MoveByTouch(touch);
		}
	}

	private void MoveByTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		if(touch.phase != UnityEngine.InputSystem.TouchPhase.Moved)
		{
			return;
		}
		
		movementDirection = -touch.delta;
		
		mainSceneCamera.MoveBy(movementSpeed*MOVEMENT_SPEED_ANDROID_TOUCH_DELTA_MULTIPLIER*movementDirection);
	}
#endif

#if UNITY_STANDALONE || UNITY_WEBGL
	private void LateUpdate()
	{
		if(mainSceneCamera != null && inputIsActive && !movementDirection.IsZero())
		{
			mainSceneCamera.Translate(movementSpeed*Time.deltaTime*movementDirection);
		}
	}
#endif
}