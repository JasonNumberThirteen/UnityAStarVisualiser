using UnityEngine;

public static class RectExtensions
{
	public static Vector2 GetMagnitude(this Rect rect)
	{
		var width = DistanceMethods.GetOneDimensionalDistance(rect.x, rect.width);
		var height = DistanceMethods.GetOneDimensionalDistance(rect.y, rect.height);

		return new(width, height);
	}
}