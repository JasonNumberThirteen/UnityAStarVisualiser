using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Timer))]
public class AndroidMapTileSelectionManager : MonoBehaviour
{
#if UNITY_ANDROID
	private bool panelUIHoverWasDetected;
	private Timer timer;
	private UserInputController userInputController;
	private AndroidMapTileRaycaster androidMapTileRaycaster;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;
	private UnityEngine.InputSystem.EnhancedTouch.Touch touch;
	private MapTileStateController mapTileStateController;
#endif
	
	private void Awake()
	{
#if UNITY_ANDROID
		timer = GetComponent<Timer>();
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();
		androidMapTileRaycaster = ObjectMethods.FindComponentOfType<AndroidMapTileRaycaster>();
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

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
			timer.timerStartedEvent.AddListener(OnTimerStarted);
			timer.timerFinishedEvent.AddListener(OnTimerFinished);
			
			if(userInputController != null)
			{
				userInputController.touchesWereUpdatedEvent.AddListener(OnTouchesWereUpdated);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			timer.timerStartedEvent.RemoveListener(OnTimerStarted);
			timer.timerFinishedEvent.RemoveListener(OnTimerFinished);
			
			if(userInputController != null)
			{
				userInputController.touchesWereUpdatedEvent.RemoveListener(OnTouchesWereUpdated);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}
	}

	private void OnTimerStarted()
	{
		if(androidMapTileRaycaster != null && (!androidMapTileRaycaster.MapTileWasTapped(touch.screenPosition, out var mapTileStateController) || !MapTileStateControllersAreEqual(mapTileStateController)))
		{
			UnassignMapTileCompletelyIfNeeded();
		}
	}

	private void OnTimerFinished()
	{
		if(androidMapTileRaycaster != null && androidMapTileRaycaster.MapTileWasTapped(touch.screenPosition, out var mapTileStateController))
		{
			HandleTappedMapTile(mapTileStateController);
		}
	}

	private void HandleTappedMapTile(MapTileStateController mapTileStateController)
	{
		EnsureReferenceToTappedMapTile(mapTileStateController);
		SetNextMapTileState();
	}

	private void EnsureReferenceToTappedMapTile(MapTileStateController mapTileStateController)
	{
		if(!MapTileStateControllersAreEqual(mapTileStateController))
		{
			this.mapTileStateController = mapTileStateController;
		}
	}

	private void SetNextMapTileState()
	{
		if(mapTileStateController == null)
		{
			return;
		}
		
		if(!mapTileStateController.IsHovered)
		{
			mapTileStateController.IsHovered = true;
		}
		else
		{
			mapTileStateController.IsSelected = true;
		}
	}

	private void UnassignMapTileCompletelyIfNeeded()
	{
		if(mapTileStateController == null)
		{
			return;
		}
		
		if(mapTileStateController.IsHovered)
		{
			mapTileStateController.IsHovered = false;
		}

		if(mapTileStateController.IsSelected)
		{
			mapTileStateController.IsSelected = false;
		}

		mapTileStateController = null;
	}

	private void OnTouchesWereUpdated(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		var numberOfTouches = touches.Count;

		if(!panelUIHoverWasDetected && numberOfTouches == 1)
		{
			HandleTouch(touches.First());
		}
		else if(MapTileShouldBeDeselected(numberOfTouches))
		{
			UnassignMapTileCompletelyIfNeeded();
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
			UnityEngine.InputSystem.TouchPhase.Canceled
		};

		return touch.MoveIsSufficientlyFast(Mathf.Epsilon) || touchPhases.Contains(touch.phase);
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}
	
	private bool TouchWasRegistered(UnityEngine.InputSystem.EnhancedTouch.Touch touch) => touch.phase == UnityEngine.InputSystem.TouchPhase.Began;
	private bool MapTileShouldBeDeselected(int numberOfTouches) => numberOfTouches == 0 && mapTileStateController != null && mapTileStateController.IsSelected;
	private bool MapTileStateControllersAreEqual(MapTileStateController mapTileStateController) => this.mapTileStateController == mapTileStateController;
#endif
}