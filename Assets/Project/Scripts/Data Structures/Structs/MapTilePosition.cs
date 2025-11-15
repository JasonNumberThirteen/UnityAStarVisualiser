using UnityEngine;

public readonly struct MapTilePosition
{
	public MapTile MapTile {get;}
	public Vector2Int Position {get;}

	public MapTilePosition(MapTile mapTile, Vector2Int position)
	{
		(MapTile, Position) = (mapTile, position);
	}
}