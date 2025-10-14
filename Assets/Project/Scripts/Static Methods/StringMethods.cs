public static class StringMethods
{
	public static string GetMultilineString(params string[] lines) => string.Join("\n", lines);
}