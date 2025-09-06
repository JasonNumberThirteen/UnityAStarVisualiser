using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		if(action == null)
		{
			return;
		}

		foreach (var element in enumerable)
		{
			action(element);
		}
	}
}