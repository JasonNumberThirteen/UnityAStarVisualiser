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
	private UnityEngine.InputSystem.EnhancedTouch.Touch touch;
#endif
	private Vector2 movementDirection;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;
#if UNITY_ANDROID
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

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
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();
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

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
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
			
			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
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

		touch = touches.First();

		ActivateDraggingOnTouchReleaseIfNeeded();
		MoveByTouch();
	}

	private void ActivateDraggingOnTouchReleaseIfNeeded()
	{
		if(!draggingIsActive && !touch.isInProgress)
		{
			draggingIsActive = true;
		}
	}

	private void MoveByTouch()
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

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		var panelWasTapped = touch.Equals(default) || !touch.inProgress;
		
		if(draggingIsActive && detected && panelWasTapped)
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