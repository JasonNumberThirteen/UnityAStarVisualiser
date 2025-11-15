using UnityEngine;

public class HeuristicManager : MonoBehaviour
{
	private HeuristicType heuristicType;
	private float heuristicWeight;

	private static readonly float TIE_BREAKING_HEURISTIC_MULTIPLIER = 0.001f;

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
		var heuristicMultiplier = 1 + TIE_BREAKING_HEURISTIC_MULTIPLIER;
		var heuristicValue = heuristicType switch
		{
			HeuristicType.ManhattanDistance => DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x) + DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y),
			HeuristicType.EuclideanDistance => Vector2.Distance(positionA, positionB),
			HeuristicType.ChebyshevDistance => Mathf.Max(DistanceMethods.GetOneDimensionalDistance(positionA.x, positionB.x), DistanceMethods.GetOneDimensionalDistance(positionA.y, positionB.y)),
			_ => 0f
		};

		return heuristicValue*heuristicWeight*heuristicMultiplier;
	}
}