using UnityEngine;

public static class MathMethods
{
	public static float GetTiledCoordinate(float coordinate, float tileSize) => Mathf.Round(coordinate / tileSize)*tileSize;
}