using UnityEngine;

[RequireComponent(typeof(MapTile))]
public class MapTileStateController : MonoBehaviour
{
	private bool isHovered;
	private bool isSelected;
	private MapTile mapTile;
	private VisualiserEventsManager visualiserEventsManager;

	public bool IsHovered
	{
		get
		{
			return isHovered;
		}
		set
		{
			isHovered = value;

			SendEvent(VisualiserEventType.MapTileHoverStateWasChanged, isHovered);
		}
	}

	public bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			isSelected = value;

			SendEvent(VisualiserEventType.MapTileSelectionStateWasChanged, isSelected);
		}
	}

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
	}

	private void SendEvent(VisualiserEventType visualiserEventType, bool enabled)
	{
		if(visualiserEventsManager != null)
		{
			visualiserEventsManager.SendEvent(new MapTileBoolVisualiserEvent(mapTile, visualiserEventType, enabled));
		}
	}
}