using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageButtonUI : ButtonUI
{
	private Locale locale;
	private ApplicationSceneManager applicationSceneManager;

	public void SetLocale(Locale locale)
	{
		this.locale = locale;
	}

	protected override void Awake()
	{
		applicationSceneManager = ObjectMethods.FindComponentOfType<ApplicationSceneManager>();
		
		base.Awake();
		RegisterToClickListener(OnButtonWasClicked, true);
	}

	private void OnDestroy()
	{
		RegisterToClickListener(OnButtonWasClicked, false);
	}

	private void OnButtonWasClicked()
	{
		if(locale != null)
		{
			LocalizationSettings.SelectedLocale = locale;
		}

		if(applicationSceneManager != null)
		{
			applicationSceneManager.LoadSceneByName(ApplicationSceneManager.VISUALISER_SCENE_NAME);
		}
	}
}