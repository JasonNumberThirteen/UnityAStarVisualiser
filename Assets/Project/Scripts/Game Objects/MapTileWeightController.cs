using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTileWeightController : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<int> weightWasChangedEvent;
	
	private MapTile mapTile;
	private bool tileIsBeingDragged;
	private bool hoveringTilesIsLocked;
	private bool panelUIHoverWasDetected;
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
		userInputController = FindFirstObjectByType<UserInputController>();
		hoveredMapTileManager = FindFirstObjectByType<HoveredMapTileManager>();
		selectedMapTileManager = FindFirstObjectByType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = FindFirstObjectByType<PanelUIHoverDetectionManager>();

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
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.AddListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.RemoveListener(OnWheelScrolled);
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
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.RemoveListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
	}

	private void OnWheelScrolled(Vector2 scrollVector)
	{
		if(tileIsBeingDragged || hoveringTilesIsLocked || panelUIHoverWasDetected)
		{
			return;
		}
		
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		
		if(mapTile != null && allowedMapTileTypes.Contains(mapTile.GetTileType()))
		{
			ModifyWeightOfMapTile(Mathf.RoundToInt(scrollVector.y));
		}
	}

	private void ModifyWeightOfMapTile(int weightValue)
	{
		if(mapTile == null || weightValue == 0)
		{
			return;
		}

		mapTile.ModifyWeightBy(weightValue);
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

	private void OnPanelUIHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}
}