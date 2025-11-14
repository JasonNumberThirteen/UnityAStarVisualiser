using UnityEngine;

public class MapBoundariesManager : MonoBehaviour
{
	[SerializeField, Range(3, 50)] private int lowerBound = 3;
	[SerializeField, Range(3, 50)] private int upperBound = 50;

	public int GetLowerBound() => lowerBound;
	public int GetUpperBound() => upperBound;

	private void Awake()
	{
		var mapDimensionInputFieldUIValueAdjusters = ObjectMethods.FindComponentsOfType<MapDimensionInputFieldUIValueAdjuster>();

		mapDimensionInputFieldUIValueAdjusters.ForEach(SetAdjusterValueBounds);
	}

	private void SetAdjusterValueBounds(MapDimensionInputFieldUIValueAdjuster mapDimensionInputFieldUIValueAdjuster)
	{
		if(mapDimensionInputFieldUIValueAdjuster == null)
		{
			return;
		}

		mapDimensionInputFieldUIValueAdjuster.SetMinimumValue(lowerBound);
		mapDimensionInputFieldUIValueAdjuster.SetMaximumValue(upperBound);
	}
}