using System.Collections.Generic;
using UnityEngine;

public static class VectorMethods
{
	public static Vector2Int GetNorthEastDirection() => Vector2Int.one;
	public static Vector2Int GetSouthEastDirection() => new(1, -1);
	public static Vector2Int GetSouthWestDirection() => -Vector2Int.one;
	public static Vector2Int GetNorthWestDirection() => new(-1, 1);
	
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
}