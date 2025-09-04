using UnityEngine;

public static class VectorExtensions
{
	public static bool IsZero(this Vector2 vector) => vector == Vector2.zero;
	public static Vector3 ToVector3(this Vector2 vector, float z = 0f) => new(vector.x, vector.y, z);

	public static Vector2 GetClampedWithin(this Vector2 vector, Rect rect)
	{
		var x = Mathf.Clamp(vector.x, rect.x, rect.width);
		var y = Mathf.Clamp(vector.y, rect.y, rect.height);

		return new Vector2(x, y);
	}
	
	public static Vector3 ToTiledPosition(this Vector3 vector)
	{
		var x = MathMethods.GetTiledCoordinate(vector.x, 1);
		var y = MathMethods.GetTiledCoordinate(vector.y, 1);

		return new Vector3(x, y, 0f);
	}
}