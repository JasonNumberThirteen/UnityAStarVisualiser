using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	private readonly HeuristicType heuristicType = HeuristicType.ManhattanDistance;

	public float GetHeuristicValue(Vector2 positionA, Vector2 positionB)
	{
		return heuristicType switch
		{
			HeuristicType.ManhattanDistance => DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x) + DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y),
			_ => 0f
		};
	}
}