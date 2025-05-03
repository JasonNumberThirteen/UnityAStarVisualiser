using System.Collections.Generic;
using UnityEngine;

public static class MapTileTypeMethods
{
	private static readonly Dictionary<MapTileType, Color32> TILE_COLOR_BY_TYPE = new()
	{
		{MapTileType.Passable, Color.white},
		{MapTileType.Impassable, new(51, 51, 51, 255)},
		{MapTileType.Start, new(151, 208, 119, 255)},
		{MapTileType.Destination, new(255, 153, 153, 255)}
	};

	private static readonly Color DEFAULT_COLOR = Color.white;

	public static IReadOnlyDictionary<MapTileType, Color32> GetKeyValuePairs() => TILE_COLOR_BY_TYPE;
	public static Color32 GetColorByMapTileType(MapTileType mapTileType) => TILE_COLOR_BY_TYPE.TryGetValue(mapTileType, out var color) ? color : DEFAULT_COLOR;
}