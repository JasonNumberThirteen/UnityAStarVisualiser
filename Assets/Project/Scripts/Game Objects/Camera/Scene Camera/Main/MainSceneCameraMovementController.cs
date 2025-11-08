#if UNITY_ANDROID
using System.Linq;
using System.Collections.Generic;
#endif
using UnityEngine;

public class MainSceneCameraMovementController : MonoBehaviour, IPrimaryWindowElement
{
	private float movementSpeed;
	private bool inputIsActive = true;
#if UNITY_ANDROID
	private bool draggingIsActive = true;
	private bool draggingDirectionIsOpposite;
#endif
	private Vector2 movementDirection;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;
#if UNITY_ANDROID
	private SelectedMapTileManager selectedMapTileManager;
	private VisualiserEventsManager visualiserEventsManager;

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
#if UNITY_ANDROID
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
#endif

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

#if UNITY_ANDROID
			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
#endif
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

#if UNITY_ANDROID
			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
#endif
		}
	}

#if UNITY_STANDALONE || UNITY_WEBGL
	private void OnMovementKeyWasPressed(Vector2 movementVector)
	{
		movementDirection = movementVector;
	}
#elif UNITY_ANDROID
	private void OnTouchesWereUpdated(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		if(mainSceneCamera == null || !inputIsActive || touches.Count != 1)
		{
			return;
		}

		var touch = touches.First();

		ActivateDraggingOnTouchReleaseIfNeeded(touch);
		MoveByTouch(touch);
	}

	private void ActivateDraggingOnTouchReleaseIfNeeded(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		if(!draggingIsActive && !touch.isInProgress)
		{
			draggingIsActive = true;
		}
	}

	private void MoveByTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		if(!draggingIsActive || touch.phase != UnityEngine.InputSystem.TouchPhase.Moved)
		{
			return;
		}
		
		var delta = touch.delta;

		movementDirection = draggingDirectionIsOpposite ? delta : -delta;
		
		mainSceneCamera.MoveBy(movementSpeed*MOVEMENT_SPEED_ANDROID_TOUCH_DELTA_MULTIPLIER*movementDirection);
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		draggingDirectionIsOpposite = mapTile != null;
	}

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(draggingIsActive && visualiserEvent is PanelUIBoolVisualiserEvent panelUIBoolVisualiserEvent && panelUIBoolVisualiserEvent.GetBoolValue())
		{
			draggingIsActive = false;
		}
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