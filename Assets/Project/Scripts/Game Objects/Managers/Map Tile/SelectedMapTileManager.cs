using UnityEngine;
using UnityEngine.Events;

public class SelectedMapTileManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<MapTile> selectedMapTileWasChangedEvent;

	private bool mapTilesCanBeSelected = true;
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
		mapTilesCanBeSelected = active;
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
		if(mapTilesCanBeSelected && visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent && mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileSelectionStateWasChanged)
		{
			SetMapTile(mapTileBoolVisualiserEvent.GetBoolValue() ? mapTileBoolVisualiserEvent.GetMapTile() : null);
		}
	}

	private void SetMapTile(MapTile mapTile)
	{
		this.mapTile = mapTile;

		selectedMapTileWasChangedEvent?.Invoke(this.mapTile);
	}
}