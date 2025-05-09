using System.Collections.Generic;
using UnityEngine;

public static class VectorMethods
{
	public static Vector2 GetNorthEastDirection() => Vector2.one;
	public static Vector2 GetSouthEastDirection() => new(1, -1);
	public static Vector2 GetSouthWestDirection() => -Vector2.one;
	public static Vector2 GetNorthWestDirection() => new(-1, 1);
	
	public static List<Vector2> GetDirectionsForFindingNeighbouringNodes(bool allowDiagonal)
	{
		var directions = new List<Vector2>()
		{
			Vector2.up,
			Vector2.down,
			Vector2.left,
			Vector2.right
		};

		if(allowDiagonal)
		{
			directions.AddRange(GetNorthEastDirection(), GetSouthEastDirection(), GetSouthWestDirection(), GetNorthWestDirection());
		}

		return directions;
	}
}