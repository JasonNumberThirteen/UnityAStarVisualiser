using System.Collections.Generic;
using UnityEngine;

public class MapTileWeightController : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	private MapTile mapTile;
	private bool mapTileIsSelected;
	private bool inputIsActive = true;
	private bool tilesCanBeHovered = true;
	private UserInputController userInputController;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	public void SetMapEditingElementActive(bool active)
	{
		tilesCanBeHovered = active;
	}

	private void Awake()
	{
		userInputController = FindFirstObjectByType<UserInputController>();
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
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnWheelScrolled(Vector2 scrollVector)
	{
		if(!inputIsActive)
		{
			return;
		}
		
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		
		if(mapTileIsSelected || mapTile == null || !allowedMapTileTypes.Contains(mapTile.GetTileType()))
		{
			return;
		}

		ModifyWeightOfMapTile(Mathf.RoundToInt(scrollVector.y));
	}

	private void ModifyWeightOfMapTile(int weightValue)
	{
		if(mapTile != null && weightValue != 0)
		{
			mapTile.ModifyWeightBy(weightValue);
		}
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(!tilesCanBeHovered || visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		if(mapTileBoolVisualiserEvent.GetVisualiserEventType() != VisualiserEventType.MapTileHoverStateWasChanged || !mapTileIsSelected)
		{
			UpdateMapTileReference(mapTileBoolVisualiserEvent);
		}
	}

	private void UpdateMapTileReference(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		if(MapTileShouldBeSelected(mapTileBoolVisualiserEvent))
		{
			mapTile = mapTileBoolVisualiserEvent.GetMapTile();
		}
		else if(MapTileShouldBeDeselected(mapTileBoolVisualiserEvent))
		{
			mapTile = null;
		}

		mapTileIsSelected = mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileSelectionStateWasChanged && mapTileBoolVisualiserEvent.GetBoolValue();
	}

	private bool MapTileShouldBeSelected(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTileHoverStateIsSetAsSelected = mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileHoverStateWasChanged && mapTileBoolVisualiserEvent.GetBoolValue();
		
		return mapTile == null && mapTileHoverStateIsSetAsSelected;
	}

	private bool MapTileShouldBeDeselected(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTileHoverStateIsSetAsNotSelected = mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileHoverStateWasChanged && !mapTileBoolVisualiserEvent.GetBoolValue();
		
		return mapTile != null && mapTileHoverStateIsSetAsNotSelected;
	}
}