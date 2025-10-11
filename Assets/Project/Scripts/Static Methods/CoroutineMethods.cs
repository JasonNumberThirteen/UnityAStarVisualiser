using System;
using System.Collections;
using UnityEngine;

public static class CoroutineMethods
{
	public static void ExecuteAfterDelayInSeconds(MonoBehaviour monoBehaviour, Action action, float delay)
	{
		if(monoBehaviour != null && action != null)
		{
			monoBehaviour.StartCoroutine(ExecuteAfterDelayInSecondsCoroutine(action, delay));
		}
	}

	private static IEnumerator ExecuteAfterDelayInSecondsCoroutine(Action action, float delay)
	{
		yield return new WaitForSeconds(delay);

		action?.Invoke();
	}
}