using System.Collections.Generic;
using UnityEngine;

public class MapTileNodeTypeMethods : MonoBehaviour
{
	private static readonly Dictionary<MapTileNodeType, Color32> TILE_COLOR_BY_NODE_TYPE = new()
	{
		{MapTileNodeType.Visited, Color.green},
		{MapTileNodeType.BelongingToPath, Color.yellow}
	};

	private static readonly Color DEFAULT_COLOR = Color.white;

	public static Color32 GetColorByMapTileNodeType(MapTileNodeType mapTileNodeType) => TILE_COLOR_BY_NODE_TYPE.TryGetValue(mapTileNodeType, out var color) ? color : DEFAULT_COLOR;
}