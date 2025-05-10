using UnityEngine;
using UnityEngine.Events;

public class SelectedMapTileManager : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<MapTile> selectedMapTileWasChangedEvent;

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
		if(visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent && mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileSelectionStateWasChanged)
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