using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectUIPositionAdjuster : MonoBehaviour
{
	[SerializeField] private Direction direction;
	[SerializeField][Range(0f, 1f)] private float initialNormalizedPosition = 1f;
	
	private ScrollRect scrollRect;
	private float lastNormalizedPosition;

	private static readonly float LAST_NORMALIZED_POSITION_UPDATE_ON_ENABLE_DELAY = 0.1f;

	private void Awake()
	{
		scrollRect = GetComponent<ScrollRect>();
		lastNormalizedPosition = initialNormalizedPosition;
	}

	private void OnEnable()
	{
		CoroutineMethods.ExecuteAfterDelayInSeconds(this, SetLastNormalizedPosition, LAST_NORMALIZED_POSITION_UPDATE_ON_ENABLE_DELAY);
	}

	private void OnDisable()
	{
		lastNormalizedPosition = GetNormalizedPosition();
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