using UnityEngine;

public static class FloatExtensions
{
	public static Vector3 ToUniformVector2(this float @float) => new(@float, @float, 1f);
}