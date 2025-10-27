using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Timer))]
public class AndroidMapTileDetectionManager : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private LayerMask detectableGameObjects;

	private UnityEngine.InputSystem.EnhancedTouch.Touch touch;
	private Timer timer;
	private UserInputController userInputController;
	private MapTileStateController mapTileStateController;
#endif
	
	private void Awake()
	{
#if UNITY_ANDROID
		timer = GetComponent<Timer>();
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();

		RemoveRaycasterFromMainCameraIfPossible();
		RegisterToListeners(true);
#else
		Destroy(this);
#endif
	}

#if UNITY_ANDROID
	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RemoveRaycasterFromMainCameraIfPossible()
	{
		var mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();

		if(mainSceneCamera.TryGetComponent<Physics2DRaycaster>(out var physics2DRaycaster))
		{
			Destroy(physics2DRaycaster);
		}
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			timer.timerStartedEvent.AddListener(DisableHoveringOfMapTileIfNeeded);
			timer.timerFinishedEvent.AddListener(OnTimerWasFinished);
			
			if(userInputController != null)
			{
				userInputController.touchesWereUpdatedEvent.AddListener(OnTouchesWereUpdated);
			}
		}
		else
		{
			timer.timerStartedEvent.RemoveListener(DisableHoveringOfMapTileIfNeeded);
			timer.timerFinishedEvent.RemoveListener(OnTimerWasFinished);
			
			if(userInputController != null)
			{
				userInputController.touchesWereUpdatedEvent.RemoveListener(OnTouchesWereUpdated);
			}
		}
	}

	private void DisableHoveringOfMapTileIfNeeded()
	{
		if(mapTileStateController != null)
		{
			mapTileStateController.IsHovered = false;
		}
	}

	private void OnTimerWasFinished()
	{
		CheckIfMapTileWasTapped(touch);
	}

	private void CheckIfMapTileWasTapped(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		var touchPosition = Camera.main.ScreenToWorldPoint(touch.screenPosition);
		var raycastHit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, detectableGameObjects);

		if(raycastHit.collider != null && raycastHit.collider.TryGetComponent<MapTileStateController>(out var mapTileStateController))
		{
			HoverTappedMapTile(mapTileStateController);
		}
	}

	private void HoverTappedMapTile(MapTileStateController mapTileStateController)
	{
		if(this.mapTileStateController != mapTileStateController)
		{
			this.mapTileStateController = mapTileStateController;
		}
		
		this.mapTileStateController.IsHovered = true;
	}

	private void OnTouchesWereUpdated(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		if(touches.Count == 1)
		{
			HandleTouch(touches.First());
		}
	}

	private void HandleTouch(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		if(TouchWasRegistered(touch))
		{
			HandleTap(touch);
		}
		else if(StationaryTouchWasInterrupted(touch))
		{
			timer.InterruptTimerIfPossible();
		}
	}

	private void HandleTap(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		this.touch = touch;
			
		timer.StartTimer();
	}

	private bool StationaryTouchWasInterrupted(UnityEngine.InputSystem.EnhancedTouch.Touch touch)
	{
		var touchPhases = new List<UnityEngine.InputSystem.TouchPhase>()
		{
			UnityEngine.InputSystem.TouchPhase.Ended,
			UnityEngine.InputSystem.TouchPhase.Moved,
			UnityEngine.InputSystem.TouchPhase.Canceled
		};

		return touchPhases.Contains(touch.phase);
	}

	private bool TouchWasRegistered(UnityEngine.InputSystem.EnhancedTouch.Touch touch) => touch.phase == UnityEngine.InputSystem.TouchPhase.Began;
#endif
}