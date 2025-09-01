using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainCameraZoomController : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<float> cameraSizeWasUpdatedEvent;
	
	private float zoomPerScroll;
	private bool zoomCanBeModified = true;
	private bool mapTileIsSelected;
	private bool inputIsActive = true;
	private Camera mainCamera;
	private UserInputController userInputController;
	private MapGenerationManager mapGenerationManager;
	private VisualiserEventsManager visualiserEventsManager;

	private readonly float ADDITIONAL_OFFSET_FROM_MAP_EDGES = 1f;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	public void SetZoomPerScroll(float zoomPerScroll)
	{
		this.zoomPerScroll = zoomPerScroll;
	}

	private void Awake()
	{
		mainCamera = Camera.main;
		userInputController = FindFirstObjectByType<UserInputController>();
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

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
				userInputController.mouseWheelWasScrolledEvent.AddListener(OnMouseWheelWasScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.AddListener(UpdateMaximumSize);
				mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.mouseWheelWasScrolledEvent.RemoveListener(OnMouseWheelWasScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.RemoveListener(UpdateMaximumSize);
				mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

	private void OnMouseWheelWasScrolled(Vector2 scrollVector)
	{
		if(!inputIsActive || !zoomCanBeModified || mainCamera == null || !mainCamera.orthographic)
		{
			return;
		}

		var sizeModifyStep = zoomPerScroll*scrollVector.y;

		mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - sizeModifyStep, GetMinimumSize(), GetMaximumSize());
	}

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
		if(mainCamera == null)
		{
			return;
		}

		mainCamera.orthographicSize = GetMaximumSize();

		cameraSizeWasUpdatedEvent?.Invoke(mainCamera.orthographicSize);
	}

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();

		switch (mapTileBoolVisualiserEvent.GetVisualiserEventType())
		{
			case VisualiserEventType.MapTileHoverStateWasChanged:
				if(!mapTileIsSelected)
				{
					zoomCanBeModified = !stateIsEnabled;
				}
				break;
			
			case VisualiserEventType.MapTileSelectionStateWasChanged:
				mapTileIsSelected = stateIsEnabled;
				break;
		}
	}

	private float GetMaximumSize()
	{
		var maximumMapDimension = GetSizeBy(mapGenerationManager != null ? mapGenerationManager.GetMaximumMapDimension() : 0f);
		
		return Mathf.Max(GetMinimumSize(), maximumMapDimension);
	}

	private float GetMinimumSize() => GetSizeBy(MapGenerationManager.MAP_DIMENSION_LOWER_BOUND);
	private float GetSizeBy(float value) => value*0.5f + ADDITIONAL_OFFSET_FROM_MAP_EDGES;
}