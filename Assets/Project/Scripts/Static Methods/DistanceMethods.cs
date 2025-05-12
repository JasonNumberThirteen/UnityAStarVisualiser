using UnityEngine;

public static class DistanceMethods
{
	public static bool OneDimensionalDistanceBetweenPositionsIsSingleAxis(float a, float b) => Mathf.Approximately(GetOneDimensionalDistance(a, b), 0f);
	public static float GetOneDimensionalDistance(float a, float b) => Mathf.Abs(a - b);
	
	public static bool DistanceBetweenPositionsIsSingleAxis(Vector2 positionA, Vector2 positionB)
	{
		var distanceInXIsSingleAxis = OneDimensionalDistanceBetweenPositionsIsSingleAxis(positionA.x, positionB.x);
		var distanceInYIsSingleAxis = OneDimensionalDistanceBetweenPositionsIsSingleAxis(positionA.y, positionB.y);
		
		return distanceInXIsSingleAxis || distanceInYIsSingleAxis;
	}
}