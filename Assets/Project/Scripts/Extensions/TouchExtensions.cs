#if UNITY_ANDROID
using UnityEngine;
#endif

public static class TouchExtensions
{
#if UNITY_ANDROID
	public static bool MoveIsSufficientlyFast(this UnityEngine.InputSystem.EnhancedTouch.Touch touch, float speedThreshold)
	{
		var delta = touch.delta;
		var movementSpeedX = Mathf.Abs(delta.x);
		var movementSpeedY = Mathf.Abs(delta.y);

		return movementSpeedX >= speedThreshold || movementSpeedY >= speedThreshold;
	}
#endif
}