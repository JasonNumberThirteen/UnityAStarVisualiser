using UnityEngine;

[DefaultExecutionOrder(-300)]
public class MapBoundariesManager : MonoBehaviour
{
	private int lowerBound;
	private int upperBound;

	public int GetLowerBound() => lowerBound;
	public int GetUpperBound() => upperBound;

	private void Awake()
	{
		var mapDimensionInputFieldUIValueAdjuster = ObjectMethods.FindComponentOfType<MapDimensionInputFieldUIValueAdjuster>();

		if(mapDimensionInputFieldUIValueAdjuster == null)
		{
			return;
		}

		lowerBound = mapDimensionInputFieldUIValueAdjuster.GetMinimumValue();
		upperBound = mapDimensionInputFieldUIValueAdjuster.GetMaximumValue();
	}
}