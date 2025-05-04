using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MapTileWeightController : MonoBehaviour
{
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private bool mapTileIsSelected;

	private void Awake()
	{
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
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
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

	private void OnScrollWheel(InputValue inputValue)
	{
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		
		if(mapTileIsSelected || mapTile == null || !allowedMapTileTypes.Contains(mapTile.GetTileType()))
		{
			return;
		}

		var mouseScroll = inputValue.Get<Vector2>();

		ModifyWeightOfMapTile(Mathf.RoundToInt(mouseScroll.y));
	}

	private void ModifyWeightOfMapTile(int weightValue)
	{
		if(mapTile != null && weightValue != 0)
		{
			mapTile.ModifyWeightBy(weightValue);
		}
	}
}