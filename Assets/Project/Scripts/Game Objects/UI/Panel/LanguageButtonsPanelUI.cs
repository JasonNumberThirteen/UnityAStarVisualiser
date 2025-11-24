using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageButtonsPanelUI : PanelUI
{
	[SerializeField] private LanguageButtonUI languageButtonUI;

#if !UNITY_WEBGL
	private void Awake()
	{
		CreateLanguageButtons();
	}
#else
	private async void Start()
	{
		await LocalizationSettings.InitializationOperation.Task;

		CreateLanguageButtons();
	}
#endif

	private void CreateLanguageButtons()
	{
		LocalizationSettings.AvailableLocales.Locales.GetReversedList().ForEach(CreateLanguageButtonWithLocale);
	}

	private void CreateLanguageButtonWithLocale(Locale locale)
	{
		if(this.languageButtonUI == null || locale == null)
		{
			return;
		}
		
		var languageButtonUI = Instantiate(this.languageButtonUI, gameObject.transform);
		
		languageButtonUI.SetLocale(locale);
		languageButtonUI.SetText(GetLocaleNameToDisplay(locale));
	}

	private string GetLocaleNameToDisplay(Locale locale) => LocalizationMethods.GetLanguageLocaleNameIfPossible(locale).ToUpper();
}