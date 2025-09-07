using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTileWeightController : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<int> weightWasChangedEvent;
	
	private bool tileIsBeingDragged;
	private bool hoveringTilesIsLocked;
	private bool panelUIHoverWasDetected;
	private MapTile mapTile;
	private UserInputController userInputController;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		hoveringTilesIsLocked = !active;
		panelUIHoverWasDetected = false;
		mapTile = null;
	}

	public void SetMapEditingElementActive(bool active)
	{
		hoveringTilesIsLocked = !active;
	}

	private void Awake()
	{
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();
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
	}

	private void OnMouseWheelWasScrolled(Vector2 scrollVector)
	{
		if(!tileIsBeingDragged && !hoveringTilesIsLocked && !panelUIHoverWasDetected)
		{
			ModifyWeightOfMapTileIfPossible(Mathf.RoundToInt(scrollVector.y));
		}
	}

	private void ModifyWeightOfMapTileIfPossible(int weight)
	{
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		
		if(mapTile != null && allowedMapTileTypes.Contains(mapTile.GetTileType()))
		{
			ModifyWeightOfMapTile(weight);
		}
	}

	private void ModifyWeightOfMapTile(int weight)
	{
		if(mapTile == null || weight == 0)
		{
			return;
		}

		mapTile.ModifyWeightBy(weight);
		weightWasChangedEvent?.Invoke(mapTile.GetWeight());
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		this.mapTile = mapTile;
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		tileIsBeingDragged = mapTile != null;
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}
}