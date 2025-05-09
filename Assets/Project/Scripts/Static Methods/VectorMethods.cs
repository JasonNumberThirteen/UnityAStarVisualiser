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
		var gridSize = MapTile.GRID_SIZE;
		var directions = new List<Vector2>()
		{
			Vector2.up*gridSize,
			Vector2.down*gridSize,
			Vector2.left*gridSize,
			Vector2.right*gridSize
		};

		if(allowDiagonal)
		{
			directions.AddRange(GetNorthEastDirection()*gridSize, GetSouthEastDirection()*gridSize, GetSouthWestDirection()*gridSize, GetNorthWestDirection()*gridSize);
		}

		return directions;
	}
}