using UnityEngine;

public static class VectorExtensions
{
	public static Vector3 ToTiledPosition(this Vector3 vector, float tileSize)
	{
		var x = MathMethods.GetTiledCoordinate(vector.x, tileSize);
		var y = MathMethods.GetTiledCoordinate(vector.y, tileSize);

		return new Vector3(x, y, 0f);
	}
}