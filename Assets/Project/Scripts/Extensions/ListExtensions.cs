using System;
using System.Linq;
using System.Collections.Generic;

public static class ListExtensions
{
	public static void AddRange<T>(this List<T> list, params T[] elements)
	{
		list.AddRange(elements);
	}

	public static void ForEachReversed<T>(this List<T> list, Action<T> action)
	{
		if(action == null)
		{
			return;
		}
		
		for (var i = list.Count - 1; i >= 0; --i)
		{
			action(list[i]);
		}
	}

	public static void ForEachReversed<T>(this List<T> list, Action<T, T> action)
	{
		if(action == null)
		{
			return;
		}
		
		for (var i = list.Count - 1; i >= 0; --i)
		{
			action(list[i], i >= 1 ? list[i - 1] : default);
		}
	}
}