using System;
using UnityEngine;

public static class IntExtensions
{
	public static int GetNumberOfDigits(this int @int) => Mathf.FloorToInt(Mathf.Log10(Mathf.Abs(@int)) + 1);
	public static T ToEnumValue<T>(this int @int) where T : Enum => (T)Enum.ToObject(typeof(T), @int);
}