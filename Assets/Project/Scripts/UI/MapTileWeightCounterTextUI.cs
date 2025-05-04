using TMPro;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MapTileWeightCounterTextUI : MonoBehaviour
{
	private TextMeshProUGUI textUI;
	private MapTile mapTile;
	private bool mapTileIsHovered;
	private bool mapTileIsSelected;
	private VisualiserEventsManager visualiserEventsManager;

	private void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		mapTile = GetComponentInParent<MapTile>();
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
			if(mapTile != null)
			{
				mapTile.weightWasChangedEvent.AddListener(OnWeightWasChanged);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(mapTile != null)
			{
				mapTile.weightWasChangedEvent.RemoveListener(OnWeightWasChanged);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void Start()
	{
		textUI.enabled = false;
	}

	private void OnWeightWasChanged(int weight)
	{
		textUI.text = weight.ToString();
		textUI.enabled = weight >= 0;
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		UpdateHoverState(mapTileBoolVisualiserEvent);
		UpdateTextState(mapTileBoolVisualiserEvent);
	}

	private void UpdateHoverState(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();
		var mapTileIsTheSame = mapTileBoolVisualiserEvent.GetMapTile() == mapTile;
		
		switch (eventType)
		{
			case VisualiserEventType.MapTileHoverStateWasChanged:
				mapTileIsHovered = mapTileIsTheSame && stateIsEnabled;
				break;
			
			case VisualiserEventType.MapTileSelectionStateWasChanged:
				mapTileIsHovered = mapTileIsTheSame && !stateIsEnabled;
				break;
		}
	}

	private void UpdateTextState(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		
		if(mapTileIsSelected && eventType == VisualiserEventType.MapTileHoverStateWasChanged)
		{
			return;
		}
		
		var mapTile = mapTileBoolVisualiserEvent.GetMapTile();
		var mapTileWeightIsPositive = mapTile != null && mapTile.GetWeight() >= 0;
		var allowedTileTypes = new List<MapTileType>
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};

		textUI.enabled = mapTileIsHovered && mapTileWeightIsPositive && allowedTileTypes.Contains(mapTile.GetTileType());
		mapTileIsSelected = eventType == VisualiserEventType.MapTileSelectionStateWasChanged && mapTileBoolVisualiserEvent.GetBoolValue();
	}
}