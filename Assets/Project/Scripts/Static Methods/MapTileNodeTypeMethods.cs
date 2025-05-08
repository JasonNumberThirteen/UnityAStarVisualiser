using System.Collections.Generic;
using UnityEngine;

public class MapTileNodeTypeMethods : MonoBehaviour
{
	private static readonly Dictionary<MapTileNodeType, Color32> TILE_COLOR_BY_NODE_TYPE = new()
	{
		{MapTileNodeType.Visited, new Color32(124, 252, 0, 255)},
		{MapTileNodeType.BelongingToPath, new Color32(255, 222, 33, 255)}
	};

	private static readonly Color DEFAULT_COLOR = Color.white;

	public static Color32 GetColorByMapTileNodeType(MapTileNodeType mapTileNodeType) => TILE_COLOR_BY_NODE_TYPE.TryGetValue(mapTileNodeType, out var color) ? color : DEFAULT_COLOR;
}