using System;

public static class EnumExtensions
{
	public static int ToInt(this Enum @enum) => Convert.ToInt32(@enum);
}