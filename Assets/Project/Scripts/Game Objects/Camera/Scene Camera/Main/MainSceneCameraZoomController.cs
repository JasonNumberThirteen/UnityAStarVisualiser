using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSceneCameraZoomController : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<float> cameraSizeWasUpdatedEvent;
	
	private bool inputIsActive = true;
#if UNITY_ANDROID
	private bool zoomingIsActive = true;
#endif
	private bool mapTileIsHovered;
	private bool mapTileIsSelected;
	private bool panelUIHoverWasDetected;
	private float zoomPerScroll;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;
	private MapBoundariesManager mapBoundariesManager;
	private MapGenerationManager mapGenerationManager;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
#if UNITY_ANDROID
	private VisualiserEventsManager visualiserEventsManager;
#endif
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	private static readonly float ADDITIONAL_OFFSET_FROM_MAP_EDGES = 1f;
#if UNITY_ANDROID
	private static readonly float SIZE_MODIFY_STEP_ANDROID_TOUCH_DELTA_MULTIPLIER = 0.05f;
#endif

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
		panelUIHoverWasDetected = false;
	}

	public void SetZoomPerScroll(float zoomPerScroll)
	{
		this.zoomPerScroll = zoomPerScroll;
	}

	private void Awake()
	{
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();
		mapBoundariesManager = ObjectMethods.FindComponentOfType<MapBoundariesManager>();
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
#if UNITY_ANDROID
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
#endif
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

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
				userInputController.mouseWheelWasScrolledEvent.AddListener(OnMouseWheelWasScrolled);
#elif UNITY_ANDROID
				userInputController.touchesWereUpdatedEvent.AddListener(OnTouchesWereUpdated);
#endif
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

#if UNITY_ANDROID
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);	
			}
#endif

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(userInputController != null)
			{
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.mouseWheelWasScrolledEvent.RemoveListener(OnMouseWheelWasScrolled);
#elif UNITY_ANDROID
				userInputController.touchesWereUpdatedEvent.RemoveListener(OnTouchesWereUpdated);
#endif
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

#if UNITY_ANDROID
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);	
			}
#endif

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}

		RegisterToMapGenerationManagerListeners(register);
	}

	private void RegisterToMapGenerationManagerListeners(bool register)
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		if(register)
		{
			mapGenerationManager.mapWasGeneratedEvent.AddListener(UpdateMaximumSize);
			mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
		}
		else
		{
			mapGenerationManager.mapWasGeneratedEvent.RemoveListener(UpdateMaximumSize);
			mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
		}
	}

#if UNITY_STANDALONE || UNITY_WEBGL
	private void OnMouseWheelWasScrolled(Vector2 scrollVector)
	{
		if(!inputIsActive || (mapTileIsHovered && !mapTileIsSelected) || panelUIHoverWasDetected || mainSceneCamera == null || !mainSceneCamera.IsOrthographic())
		{
			return;
		}

		var sizeModifyStep = zoomPerScroll*scrollVector.y;
		var orthographicSize = Mathf.Clamp(mainSceneCamera.GetOrthographicSize() - sizeModifyStep, GetMinimumSize(), GetMaximumSize());

		mainSceneCamera.SetOrthographicSize(orthographicSize);
	}
#endif

#if UNITY_ANDROID
	private void OnTouchesWereUpdated(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		ActivateZoomingOnTouchReleaseIfNeeded(touches);
		
		if(!inputIsActive || mainSceneCamera == null || !mainSceneCamera.IsOrthographic() || touches.Count != 2)
		{
			return;
		}

		var firstTouch = touches.First();
		var secondTouch = touches[1];

		ModifyOrthographicSizeByTouchIfPossible(firstTouch, secondTouch);
	}

	private void ActivateZoomingOnTouchReleaseIfNeeded(List<UnityEngine.InputSystem.EnhancedTouch.Touch> touches)
	{
		if(!zoomingIsActive && touches.Count == 0)
		{
			zoomingIsActive = true;
		}
	}

	private void ModifyOrthographicSizeByTouchIfPossible(UnityEngine.InputSystem.EnhancedTouch.Touch firstTouch, UnityEngine.InputSystem.EnhancedTouch.Touch secondTouch)
	{
		if(!zoomingIsActive)
		{
			return;
		}
		
		var firstTouchPreviousScreenPosition = firstTouch.screenPosition - firstTouch.delta;
		var secondTouchPreviousScreenPosition = secondTouch.screenPosition - secondTouch.delta;
		var previousDistanceBetweenTouches = Vector2.Distance(firstTouchPreviousScreenPosition, secondTouchPreviousScreenPosition);
		var currentDistanceBetweenTouches = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);
		var zoomDelta = previousDistanceBetweenTouches - currentDistanceBetweenTouches;
		var sizeModifyStep = zoomPerScroll*zoomDelta*SIZE_MODIFY_STEP_ANDROID_TOUCH_DELTA_MULTIPLIER;
		var orthographicSize = Mathf.Clamp(mainSceneCamera.GetOrthographicSize() + sizeModifyStep, GetMinimumSize(), GetMaximumSize());

		mainSceneCamera.SetOrthographicSize(orthographicSize);
	}
#endif

	private void OnMapTilesWereAdded(List<MapTile> mapTiles)
	{
		UpdateMaximumSize();
	}

	private void OnMapTilesWereRemoved(List<MapTile> mapTiles)
	{
		UpdateMaximumSize();
	}

	private void UpdateMaximumSize()
	{
		if(mainSceneCamera == null)
		{
			return;
		}

		var orthographicSize = GetMaximumSize();

		mainSceneCamera.SetOrthographicSize(orthographicSize);
		cameraSizeWasUpdatedEvent?.Invoke(orthographicSize);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		mapTileIsHovered = mapTile != null;
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		mapTileIsSelected = mapTile != null;
	}

#if UNITY_ANDROID
	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(zoomingIsActive && visualiserEvent is PanelUIBoolVisualiserEvent panelUIBoolVisualiserEvent && panelUIBoolVisualiserEvent.GetBoolValue())
		{
			zoomingIsActive = false;
		}
	}
#endif

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}

	private float GetMaximumSize()
	{
		var maximumMapDimension = GetSizeBy(mapGenerationManager != null ? mapGenerationManager.GetMapDimensions().GetMaximumDimension() : 0f);
		
		return Mathf.Max(GetMinimumSize(), maximumMapDimension);
	}

	private float GetMinimumSize() => GetSizeBy(mapBoundariesManager != null ? mapBoundariesManager.GetLowerBound() : 0);
	private float GetSizeBy(float value) => value*0.5f + ADDITIONAL_OFFSET_FROM_MAP_EDGES;
}