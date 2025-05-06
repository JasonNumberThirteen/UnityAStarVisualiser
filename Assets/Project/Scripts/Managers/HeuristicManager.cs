using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	private HeuristicType heuristicType;
	private float heuristicWeight;

	public void SetHeuristicType(HeuristicType heuristicType)
	{
		this.heuristicType = heuristicType;
	}

	public void SetHeuristicWeight(float weight)
	{
		heuristicWeight = weight;
	}

	public float GetHeuristicValue(Vector2 positionA, Vector2 positionB)
	{
		var heuristicValue = heuristicType switch
		{
			HeuristicType.ManhattanDistance => DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x) + DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y),
			HeuristicType.EuclideanDistance => Vector2.Distance(positionA, positionB),
			HeuristicType.ChebyshevDistance => Mathf.Max(DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x), DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y)),
			_ => 0f
		};

		return heuristicValue*heuristicWeight;
	}
}