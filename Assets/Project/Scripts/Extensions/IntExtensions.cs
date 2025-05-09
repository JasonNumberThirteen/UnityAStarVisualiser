using UnityEngine;

public static class IntExtensions
{
	public static int GetNumberOfDigits(this int @int) => Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(@int)) + 1);
}