using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectUIPositionAdjuster : MonoBehaviour
{
	[SerializeField] private Direction direction;
	
	private ScrollRect scrollRect;
	private float lastNormalizedPosition;

	private static readonly float LAST_NORMALIZED_POSITION_UPDATE_ON_ENABLE_DELAY = 0.1f;
	private static readonly float LAST_NORMALIZED_POSITION_SETTING_ON_START_DELAY = 0.5f;

	private void Awake()
	{
		scrollRect = GetComponent<ScrollRect>();

		CoroutineMethods.ExecuteAfterDelayInSeconds(this, UpdateLastNormalizedPosition, LAST_NORMALIZED_POSITION_SETTING_ON_START_DELAY);
	}

	private void OnEnable()
	{
		CoroutineMethods.ExecuteAfterDelayInSeconds(this, SetLastNormalizedPosition, LAST_NORMALIZED_POSITION_UPDATE_ON_ENABLE_DELAY);
	}

	private void OnDisable()
	{
		UpdateLastNormalizedPosition();
	}

	private void SetLastNormalizedPosition()
	{
		switch (direction)
		{
			case Direction.Horizontal:
				scrollRect.horizontalNormalizedPosition = lastNormalizedPosition;
				break;
			
			case Direction.Vertical:
				scrollRect.verticalNormalizedPosition = lastNormalizedPosition;
				break;
		}
	}

	private void UpdateLastNormalizedPosition()
	{
		lastNormalizedPosition = GetNormalizedPosition();
	}

	private float GetNormalizedPosition()
	{
		return direction switch
		{
			Direction.Horizontal => scrollRect.horizontalNormalizedPosition,
			Direction.Vertical => scrollRect.verticalNormalizedPosition,
			_ => 0f
		};
	}
}