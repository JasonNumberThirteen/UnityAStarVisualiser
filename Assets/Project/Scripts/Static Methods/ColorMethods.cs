using UnityEngine;

public static class ColorMethods
{
	public static Color GetColorWithSetAlpha(Color color, float alpha) => new(color.r, color.g, color.b, alpha);
}