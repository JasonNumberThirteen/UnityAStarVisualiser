using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainSceneCameraZoomController : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<float> cameraSizeWasUpdatedEvent;
	
	private bool inputIsActive = true;
	private bool mapTileIsSelected;
	private bool zoomCanBeModified = true;
	private float zoomPerScroll;
	private MainSceneCamera mainSceneCamera;
	private UserInputController userInputController;
	private MapBoundariesManager mapBoundariesManager;
	private MapGenerationManager mapGenerationManager;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	private static readonly float ADDITIONAL_OFFSET_FROM_MAP_EDGES = 1f;

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
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();
		mapBoundariesManager = ObjectMethods.FindComponentOfType<MapBoundariesManager>();
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
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
				userInputController.mouseWheelWasScrolledEvent.AddListener(OnMouseWheelWasScrolled);
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.mouseWheelWasScrolledEvent.RemoveListener(OnMouseWheelWasScrolled);
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

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

	private void OnMouseWheelWasScrolled(Vector2 scrollVector)
	{
		if(!inputIsActive || !zoomCanBeModified || mainSceneCamera == null || !mainSceneCamera.IsOrthographic())
		{
			return;
		}

		var sizeModifyStep = zoomPerScroll*scrollVector.y;
		var orthographicSize = Mathf.Clamp(mainSceneCamera.GetOrthographicSize() - sizeModifyStep, GetMinimumSize(), GetMaximumSize());

		mainSceneCamera.SetOrthographicSize(orthographicSize);
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
		if(!mapTileIsSelected)
		{
			zoomCanBeModified = mapTile == null;
		}
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		mapTileIsSelected = mapTile != null;
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		zoomCanBeModified = !detected;
	}

	private float GetMaximumSize()
	{
		var maximumMapDimension = GetSizeBy(mapGenerationManager != null ? mapGenerationManager.GetMapDimensions().GetMaximumDimension() : 0f);
		
		return Mathf.Max(GetMinimumSize(), maximumMapDimension);
	}

	private float GetMinimumSize() => GetSizeBy(mapBoundariesManager != null ? mapBoundariesManager.GetLowerBound() : 0);
	private float GetSizeBy(float value) => value*0.5f + ADDITIONAL_OFFSET_FROM_MAP_EDGES;
}