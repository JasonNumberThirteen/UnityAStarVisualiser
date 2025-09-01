using System.Collections.Generic;

public static class ListExtensions
{
	public static void AddRange<T>(this List<T> list, params T[] elements)
	{
		list.AddRange(elements);
	}
}