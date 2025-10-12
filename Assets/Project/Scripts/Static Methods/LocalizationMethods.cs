using System.Collections.Generic;
using UnityEngine.Localization;

public static class LocalizationMethods
{
	private static readonly Dictionary<string, string> LANGUAGE_LOCALE_FORMATTERS_NAMES = new()
	{
		{"pl", "Polski"},
		{"en", "English"}
	};

	public static string GetLanguageLocaleNameIfPossible(Locale locale) => locale != null && LANGUAGE_LOCALE_FORMATTERS_NAMES.TryGetValue(locale.Formatter.ToString(), out var formatterName) ? formatterName : string.Empty;
}