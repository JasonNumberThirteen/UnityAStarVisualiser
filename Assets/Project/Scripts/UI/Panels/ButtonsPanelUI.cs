using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Button findPathButtonUI;
	[SerializeField] private Button clearResultsButtonUI;
	[SerializeField] private Button resetTilesButtonUI;
	[SerializeField] private Button changeMapDimensionsButtonUI;
	[SerializeField] private Button takeMapScreenshotButtonUI;
	[SerializeField] private Button informationButtonUI;
	[SerializeField] private Button exitButtonUI;

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
		if(register)
		{
			if(findPathButtonUI != null)
			{
				findPathButtonUI.onClick.AddListener(OnFindPathButtonUIClicked);
			}

			if(clearResultsButtonUI != null)
			{
				clearResultsButtonUI.onClick.AddListener(OnClearResultsButtonUIClicked);
			}

			if(resetTilesButtonUI != null)
			{
				resetTilesButtonUI.onClick.AddListener(OnResetTilesButtonUIClicked);
			}

			if(changeMapDimensionsButtonUI != null)
			{
				changeMapDimensionsButtonUI.onClick.AddListener(OnChangeMapDimensionsButtonUIClicked);
			}

			if(takeMapScreenshotButtonUI != null)
			{
				takeMapScreenshotButtonUI.onClick.AddListener(OnTakeMapScreenshotButtonUIClicked);
			}

			if(informationButtonUI != null)
			{
				informationButtonUI.onClick.AddListener(OnInformationButtonUIClicked);
			}
			
			if(exitButtonUI != null)
			{
				exitButtonUI.onClick.AddListener(OnExitButtonUIClicked);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
		}
		else
		{
			if(findPathButtonUI != null)
			{
				findPathButtonUI.onClick.RemoveListener(OnFindPathButtonUIClicked);
			}

			if(clearResultsButtonUI != null)
			{
				clearResultsButtonUI.onClick.RemoveListener(OnClearResultsButtonUIClicked);
			}

			if(resetTilesButtonUI != null)
			{
				resetTilesButtonUI.onClick.RemoveListener(OnResetTilesButtonUIClicked);
			}

			if(changeMapDimensionsButtonUI != null)
			{
				changeMapDimensionsButtonUI.onClick.RemoveListener(OnChangeMapDimensionsButtonUIClicked);
			}

			if(takeMapScreenshotButtonUI != null)
			{
				takeMapScreenshotButtonUI.onClick.RemoveListener(OnTakeMapScreenshotButtonUIClicked);
			}

			if(informationButtonUI != null)
			{
				informationButtonUI.onClick.RemoveListener(OnInformationButtonUIClicked);
			}

			if(exitButtonUI != null)
			{
				exitButtonUI.onClick.RemoveListener(OnExitButtonUIClicked);
			}

			if(pathfindingManager != null)
			{
				pathfindingManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
		}
	}

	private void OnFindPathButtonUIClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.FindPath();
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
			mapGenerationManager.ResetTiles();
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
		var buttonUIs = new List<Button>()
		{
			findPathButtonUI,
			clearResultsButtonUI,
			resetTilesButtonUI,
			changeMapDimensionsButtonUI
		};

		buttonUIs.ForEach(buttonUI =>
		{
			if(buttonUI != null)
			{
				buttonUI.interactable = !started;
			}
		});
	}
}