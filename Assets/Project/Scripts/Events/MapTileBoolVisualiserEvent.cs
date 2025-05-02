public class MapTileBoolVisualiserEvent : BoolVisualiserEvent
{
	private readonly MapTile mapTile;

	public MapTileBoolVisualiserEvent(MapTile mapTile, VisualiserEventType visualiserEventType, bool stateIsEnabled) : base(visualiserEventType, stateIsEnabled)
	{
		this.mapTile = mapTile;
	}

	public MapTile GetMapTile() => mapTile;
}