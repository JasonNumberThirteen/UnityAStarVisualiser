using UnityEngine;
using UnityEngine.Events;

public class HoveredMapTileManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<MapTile> hoveredMapTileWasChangedEvent;

	private bool mapTilesCanBeHovered = true;
	private MapTile mapTile;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		if(!active)
		{
			SetMapTile(null);
		}
	}

	public void SetMapEditingElementActive(bool active)
	{
		mapTilesCanBeHovered = active;
	}

	private void Awake()
	{
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();

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
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(mapTilesCanBeHovered && visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent && mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileHoverStateWasChanged)
		{
			SetMapTile(mapTileBoolVisualiserEvent.GetBoolValue() ? mapTileBoolVisualiserEvent.GetMapTile() : null);
		}
	}

	private void SetMapTile(MapTile mapTile)
	{
		this.mapTile = mapTile;

		hoveredMapTileWasChangedEvent?.Invoke(this.mapTile);
	}
}