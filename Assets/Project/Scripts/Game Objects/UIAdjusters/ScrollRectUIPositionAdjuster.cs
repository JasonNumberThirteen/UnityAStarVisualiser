using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectUIPositionAdjuster : MonoBehaviour
{
	[SerializeField] private Direction direction;
	[SerializeField][Range(0f, 1f)] private float initialNormalizedPosition = 1f;
	
	private ScrollRect scrollRect;
	private float lastNormalizedPosition;
	private CanvasUIRefresher canvasUIRefresher;

	private void Awake()
	{
		scrollRect = GetComponent<ScrollRect>();
		lastNormalizedPosition = initialNormalizedPosition;
		canvasUIRefresher = ObjectMethods.FindComponentOfType<CanvasUIRefresher>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(canvasUIRefresher != null)
			{
				canvasUIRefresher.canvasWasRebuiltEvent.AddListener(SetLastNormalizedPosition);
			}
		}
		else
		{
			if(canvasUIRefresher != null)
			{
				canvasUIRefresher.canvasWasRebuiltEvent.RemoveListener(SetLastNormalizedPosition);
			}
		}
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

	private void OnDisable()
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