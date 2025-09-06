using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class VectorMethods
{
	public static Vector2Int GetNorthEastDirection() => Vector2Int.RoundToInt(GetUniformVector2());
	public static Vector2Int GetSouthEastDirection() => new(1, -1);
	public static Vector2Int GetSouthWestDirection() => Vector2Int.RoundToInt(GetUniformVector2(-1));
	public static Vector2Int GetNorthWestDirection() => new(-1, 1);
	public static Vector2 GetUniformVector2(float value = 1f) => Vector2.one*value;
	
	public static List<Vector2Int> GetCardinalDirections(bool allowDiagonal)
	{
		var directions = new List<Vector2Int>()
		{
			Vector2Int.up,
			Vector2Int.down,
			Vector2Int.left,
			Vector2Int.right
		};

		if(allowDiagonal)
		{
			directions.AddRange(GetNorthEastDirection(), GetSouthEastDirection(), GetSouthWestDirection(), GetNorthWestDirection());
		}

		return directions;
	}

	public static IEnumerable<Vector2Int> GetTiledPositionsWithin(Vector2Int size)
	{
		var width = size.x;
		
		return Enumerable.Range(0, width*size.y).Select(i => new Vector2Int(i % width, i / width));
	}

	public static Vector2 GetPositionWithinRect(Vector2 position, Rect rect, float innerOffsetFromRect = 0f)
	{
		var innerOffset = GetUniformVector2(innerOffsetFromRect);
		var areaWithOffset = new Rect(rect.position + innerOffset, rect.size - innerOffset);

		return position.GetClampedWithin(areaWithOffset);
	}
}