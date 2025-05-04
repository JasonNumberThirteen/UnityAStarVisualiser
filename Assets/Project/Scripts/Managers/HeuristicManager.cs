using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	private readonly HeuristicType heuristicType = HeuristicType.ManhattanDistance;

	private float heuristicWeight;

	public void SetHeuristicWeight(float weight)
	{
		heuristicWeight = weight;
	}

	public float GetHeuristicValue(Vector2 positionA, Vector2 positionB)
	{
		var heuristicValue = heuristicType switch
		{
			HeuristicType.ManhattanDistance => DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x) + DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y),
			_ => 0f
		};

		return heuristicValue*heuristicWeight;
	}
}