using UnityEngine;

public static class VectorExtensions
{
	public static Vector3 ToTiledPosition(this Vector3 vector)
	{
		var x = MathMethods.GetTiledCoordinate(vector.x, 1);
		var y = MathMethods.GetTiledCoordinate(vector.y, 1);

		return new Vector3(x, y, 0f);
	}
}