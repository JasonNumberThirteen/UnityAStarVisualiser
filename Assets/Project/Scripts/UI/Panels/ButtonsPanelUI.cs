using System.Collections.Generic;
using UnityEngine;

public class ButtonsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private ButtonUI findPathButtonUI;
	[SerializeField] private ButtonUI clearResultsButtonUI;
	[SerializeField] private ButtonUI resetTilesButtonUI;
	[SerializeField] private ButtonUI changeMapDimensionsButtonUI;
	[SerializeField] private ButtonUI takeMapScreenshotButtonUI;
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
			findPathButtonUI.RegisterToClickListener(OnFindPathButtonUIClicked, register);
		}

		if(clearResultsButtonUI != null)
		{
			clearResultsButtonUI.RegisterToClickListener(OnClearResultsButtonUIClicked, register);
		}

		if(resetTilesButtonUI != null)
		{
			resetTilesButtonUI.RegisterToClickListener(OnResetTilesButtonUIClicked, register);
		}

		if(changeMapDimensionsButtonUI != null)
		{
			changeMapDimensionsButtonUI.RegisterToClickListener(OnChangeMapDimensionsButtonUIClicked, register);
		}

		if(takeMapScreenshotButtonUI != null)
		{
			takeMapScreenshotButtonUI.RegisterToClickListener(OnTakeMapScreenshotButtonUIClicked, register);
		}

		if(informationButtonUI != null)
		{
			informationButtonUI.RegisterToClickListener(OnInformationButtonUIClicked, register);
		}
		
		if(exitButtonUI != null)
		{
			exitButtonUI.RegisterToClickListener(OnExitButtonUIClicked, register);
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

	private void OnFindPathButtonUIClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.InitiatePathfindingProcessIfPossible();
		}
	}

	private void OnClearResultsButtonUIClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.ClearResults();
		}
	}

	private void OnResetTilesButtonUIClicked()
	{
		if(mapGenerationManager != null)
		{
			mapGenerationManager.ResetMapTiles();
		}
	}

	private void OnChangeMapDimensionsButtonUIClicked()
	{
		if(changeMapDimensionsPopupPanelUI != null)
		{
			changeMapDimensionsPopupPanelUI.SetActive(true);
		}
	}

	private void OnTakeMapScreenshotButtonUIClicked()
	{
		if(takeMapScreenshotButtonUI != null)
		{
			mapScreenshotTaker.TakeMapScreenshot();
		}
	}

	private void OnInformationButtonUIClicked()
	{
		if(informationPopupPanelUI != null)
		{
			informationPopupPanelUI.SetActive(true);
		}
	}

	private void OnExitButtonUIClicked()
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