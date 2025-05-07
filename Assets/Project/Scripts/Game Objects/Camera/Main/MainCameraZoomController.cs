using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainCameraZoomController : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<float> cameraSizeWasUpdatedEvent;
	
	private float zoomPerScroll;
	private float sizeUpperBoundToMapSize;
	private bool zoomCanBeModified = true;
	private bool mapTileIsSelected;
	private bool inputIsActive = true;
	private Camera mainCamera;
	private UserInputController userInputController;
	private MapGenerationManager mapGenerationManager;
	private VisualiserEventsManager visualiserEventsManager;

	private readonly float MINIMUM_SIZE = 3f;
	private readonly float MAXIMUM_SIZE = 26f;
	private readonly float SIZE_GROWTH_PER_ITERATION = 1f;

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
		sizeUpperBoundToMapSize = mainCamera.orthographicSize;
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
				userInputController.wheelScrolledEvent.AddListener(OnWheelScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.AddListener(UpdateMaximumSize);
				mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.RemoveListener(OnWheelScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.RemoveListener(UpdateMaximumSize);
				mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnWheelScrolled(Vector2 scrollVector)
	{
		if(inputIsActive && zoomCanBeModified && mainCamera != null && mainCamera.orthographic)
		{
			mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - zoomPerScroll*scrollVector.y, MINIMUM_SIZE, sizeUpperBoundToMapSize);
		}
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
		if(mapGenerationManager == null)
		{
			return;
		}

		mainCamera.orthographicSize = MINIMUM_SIZE;

		while (!EntireMapIsVisibleWithinCamera() && mainCamera.orthographicSize < MAXIMUM_SIZE)
		{
			mainCamera.orthographicSize += SIZE_GROWTH_PER_ITERATION;
		}

		sizeUpperBoundToMapSize = mainCamera.orthographicSize;

		cameraSizeWasUpdatedEvent?.Invoke(mainCamera.orthographicSize);
	}

	private bool EntireMapIsVisibleWithinCamera()
	{
		if(mapGenerationManager == null)
		{
			return false;
		}
		
		var cameraHeight = mainCamera.orthographicSize*2f;
		var cameraWidth = cameraHeight*mainCamera.aspect;
		var cameraSize = new Vector2(cameraWidth, cameraHeight);
		var mapCornersTiles = mapGenerationManager.GetMapCornersTiles();
		var mapSizeWithOffset = mapGenerationManager.GetMapSize() - Vector2.one*MapTile.GRID_SIZE;
		var offsetToCenter = (mapSizeWithOffset - cameraSize)*0.5f;
		var areaInTheCenterOfMap = new Rect(offsetToCenter, cameraSize);

		return mapCornersTiles.All(mapTile =>
		{
			var mapTileBounds = mapTile.GetMapTileRenderer().GetBounds();
			var mapTileBoundsRectangle = new Rect(mapTileBounds.min, mapTileBounds.size);

			return areaInTheCenterOfMap.Contains(mapTileBoundsRectangle.min) && areaInTheCenterOfMap.Contains(mapTileBoundsRectangle.max);
		});
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
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
}