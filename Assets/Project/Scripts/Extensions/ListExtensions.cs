using System.Linq;
using System.Collections.Generic;

public static class ListExtensions
{
	public static T PopFirst<T>(this List<T> list)
	{
		if(list.Count == 0)
		{
			return default;
		}

		var firstElement = list.First();

		list.RemoveAt(0);

		return firstElement;
	}
}