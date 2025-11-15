using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class MainWindowButtonsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private ButtonUI findPathButtonUI;
	[SerializeField] private ButtonUI clearResultsButtonUI;
	[SerializeField] private ButtonUI resetTilesButtonUI;
	[SerializeField] private ButtonUI changeMapDimensionsButtonUI;
	[SerializeField] private ButtonUI takeMapScreenshotButtonUI;
	[SerializeField] private ButtonUI fullscreenWindowedModeButtonUI;
	[SerializeField] private ButtonUI switchLanguageButtonUI;
	[SerializeField] private ButtonUI informationButtonUI;
	[SerializeField] private ButtonUI exitButtonUI;

	private MapPathManager mapPathManager;
	private PathfindingManager pathfindingManager;
	private MapGenerationManager mapGenerationManager;
	private ChangeMapDimensionsPopupPanelUI changeMapDimensionsPopupPanelUI;
	private InformationPopupPanelUI informationPopupPanelUI;
	private ExitPopupPanelUI exitPopupPanelUI;
	private MapScreenshotTaker mapScreenshotTaker;

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
	}

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
		pathfindingManager = ObjectMethods.FindComponentOfType<PathfindingManager>();
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		changeMapDimensionsPopupPanelUI = ObjectMethods.FindComponentOfType<ChangeMapDimensionsPopupPanelUI>();
		informationPopupPanelUI = ObjectMethods.FindComponentOfType<InformationPopupPanelUI>();
		exitPopupPanelUI = ObjectMethods.FindComponentOfType<ExitPopupPanelUI>();
		mapScreenshotTaker = ObjectMethods.FindComponentOfType<MapScreenshotTaker>();
		
		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(findPathButtonUI != null)
		{
			findPathButtonUI.RegisterToClickListener(OnFindPathButtonUIWasClicked, register);
		}

		if(clearResultsButtonUI != null)
		{
			clearResultsButtonUI.RegisterToClickListener(OnClearResultsButtonUIWasClicked, register);
		}

		if(resetTilesButtonUI != null)
		{
			resetTilesButtonUI.RegisterToClickListener(OnResetTilesButtonUIWasClicked, register);
		}

		if(changeMapDimensionsButtonUI != null)
		{
			changeMapDimensionsButtonUI.RegisterToClickListener(OnChangeMapDimensionsButtonUIWasClicked, register);
		}

		if(takeMapScreenshotButtonUI != null)
		{
			takeMapScreenshotButtonUI.RegisterToClickListener(OnTakeMapScreenshotButtonUIWasClicked, register);
		}

		if(switchLanguageButtonUI != null)
		{
			switchLanguageButtonUI.RegisterToClickListener(OnSwitchLanguageButtonUIWasClicked, register);
		}

		if(fullscreenWindowedModeButtonUI != null)
		{
			fullscreenWindowedModeButtonUI.RegisterToClickListener(OnFullscreenWindowedModeButtonUIWasClicked, register);
		}

		if(informationButtonUI != null)
		{
			informationButtonUI.RegisterToClickListener(OnInformationButtonUIWasClicked, register);
		}
		
		if(exitButtonUI != null)
		{
			exitButtonUI.RegisterToClickListener(OnExitButtonUIWasClicked, register);
		}
		
		if(register)
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
		}
	}

	private void OnFindPathButtonUIWasClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.InitiatePathfindingProcessIfPossible();
		}
	}

	private void OnClearResultsButtonUIWasClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.ClearResults();
		}
	}

	private void OnResetTilesButtonUIWasClicked()
	{
		if(mapGenerationManager != null)
		{
			mapGenerationManager.ResetMapTiles();
		}
	}

	private void OnChangeMapDimensionsButtonUIWasClicked()
	{
		if(changeMapDimensionsPopupPanelUI != null)
		{
			changeMapDimensionsPopupPanelUI.SetActive(true);
		}
	}

	private void OnTakeMapScreenshotButtonUIWasClicked()
	{
		if(takeMapScreenshotButtonUI != null)
		{
			mapScreenshotTaker.TakeMapScreenshot();
		}
	}

	private void OnFullscreenWindowedModeButtonUIWasClicked()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	private void OnSwitchLanguageButtonUIWasClicked()
	{
		if(switchLanguageButtonUI == null)
		{
			return;
		}

		var selectedLocale = LocalizationSettings.SelectedLocale;
		var availableLocales = LocalizationSettings.AvailableLocales.Locales;
		var selectedLocaleIndex = availableLocales.IndexOf(selectedLocale);
		var nextLocaleIndex = (selectedLocaleIndex + 1) % availableLocales.Count;

		LocalizationSettings.SelectedLocale = availableLocales[nextLocaleIndex];
	}

	private void OnInformationButtonUIWasClicked()
	{
		if(informationPopupPanelUI != null)
		{
			informationPopupPanelUI.SetActive(true);
		}
	}

	private void OnExitButtonUIWasClicked()
	{
		if(exitPopupPanelUI != null)
		{
			exitPopupPanelUI.SetActive(true);
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		var buttonUIs = new List<ButtonUI>(){findPathButtonUI, clearResultsButtonUI, resetTilesButtonUI, changeMapDimensionsButtonUI};

		buttonUIs.ForEach(buttonUI => SetButtonUIInteractable(buttonUI, !started));
	}

	private void SetButtonUIInteractable(ButtonUI buttonUI, bool interactable)
	{
		if(buttonUI != null)
		{
			buttonUI.SetInteractable(interactable);
		}
	}
}