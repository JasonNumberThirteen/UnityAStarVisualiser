using UnityEngine;
using UnityEngine.Events;

public class HoveredMapTileManager : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<MapTile> hoveredMapTileWasChangedEvent;

	private MapTile mapTile;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		if(!active)
		{
			SetMapTile(null);
		}
	}

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
		if(visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent && mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileHoverStateWasChanged)
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