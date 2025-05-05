using UnityEngine;

[RequireComponent(typeof(MapTile))]
public class MapTileMouseEventsSender : MonoBehaviour, IPrimaryWindowElement
{
	private bool inputIsActive = true;
	private MapTile mapTile;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();
	}

	private void OnMouseEnter()
	{
		SendEvent(VisualiserEventType.MapTileHoverStateWasChanged, true);
	}

	private void OnMouseExit()
	{
		SendEvent(VisualiserEventType.MapTileHoverStateWasChanged, false);
	}

	private void OnMouseDown()
	{
		SendEvent(VisualiserEventType.MapTileSelectionStateWasChanged, true);
	}

	private void OnMouseUp()
	{
		SendEvent(VisualiserEventType.MapTileSelectionStateWasChanged, false);
	}

	private void SendEvent(VisualiserEventType visualiserEventType, bool enabled)
	{
		if(inputIsActive && visualiserEventsManager != null)
		{
			visualiserEventsManager.SendEvent(new MapTileBoolVisualiserEvent(mapTile, visualiserEventType, enabled));
		}
	}
}