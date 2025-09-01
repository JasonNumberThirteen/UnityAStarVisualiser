using UnityEngine;

public static class RectMethods
{
	public static Rect GetRectWithSize(Vector2Int size) => new(Vector2Int.zero, size);
}