using UnityEngine;
using UnityEngine.UI;

public class MainWindowPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Button findPathButton;
	[SerializeField] private Button clearResultsButton;
	[SerializeField] private Button resetTilesButton;
	[SerializeField] private Button changeMapDimensionsButton;
	[SerializeField] private Button takeMapScreenshotButton;
	[SerializeField] private Button exitButton;

	private PathfindingManager pathfindingManager;
	private MapGenerationManager mapGenerationManager;
	private ChangeMapDimensionsPanelUI changeMapDimensionsPanelUI;
	private MapScreenshotTaker mapScreenshotTaker;

	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
	}

	private void Awake()
	{
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		changeMapDimensionsPanelUI = FindFirstObjectByType<ChangeMapDimensionsPanelUI>(FindObjectsInactive.Include);
		mapScreenshotTaker = FindFirstObjectByType<MapScreenshotTaker>();
		
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
			if(findPathButton != null)
			{
				findPathButton.onClick.AddListener(OnFindPathButtonClicked);
			}

			if(clearResultsButton != null)
			{
				clearResultsButton.onClick.AddListener(OnClearResultsButtonClicked);
			}

			if(resetTilesButton != null)
			{
				resetTilesButton.onClick.AddListener(OnResetTilesButtonClicked);
			}

			if(changeMapDimensionsButton != null)
			{
				changeMapDimensionsButton.onClick.AddListener(OnChangeMapDimensionsButtonClicked);
			}

			if(takeMapScreenshotButton != null)
			{
				takeMapScreenshotButton.onClick.AddListener(OnTakeMapScreenshotButtonClicked);
			}
			
			if(exitButton != null)
			{
				exitButton.onClick.AddListener(OnExitButtonClicked);
			}
		}
		else
		{
			if(findPathButton != null)
			{
				findPathButton.onClick.RemoveListener(OnFindPathButtonClicked);
			}

			if(clearResultsButton != null)
			{
				clearResultsButton.onClick.RemoveListener(OnClearResultsButtonClicked);
			}

			if(resetTilesButton != null)
			{
				resetTilesButton.onClick.RemoveListener(OnResetTilesButtonClicked);
			}

			if(changeMapDimensionsButton != null)
			{
				changeMapDimensionsButton.onClick.RemoveListener(OnChangeMapDimensionsButtonClicked);
			}

			if(takeMapScreenshotButton != null)
			{
				takeMapScreenshotButton.onClick.RemoveListener(OnTakeMapScreenshotButtonClicked);
			}

			if(exitButton != null)
			{
				exitButton.onClick.RemoveListener(OnExitButtonClicked);
			}
		}
	}

	private void OnFindPathButtonClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.FindPath();
		}
	}

	private void OnClearResultsButtonClicked()
	{
		if(pathfindingManager != null)
		{
			pathfindingManager.ClearResults();
		}
	}

	private void OnResetTilesButtonClicked()
	{
		if(mapGenerationManager != null)
		{
			mapGenerationManager.ResetTiles();
		}
	}

	private void OnChangeMapDimensionsButtonClicked()
	{
		if(changeMapDimensionsPanelUI != null)
		{
			changeMapDimensionsPanelUI.SetActive(true);
		}
	}

	private void OnTakeMapScreenshotButtonClicked()
	{
		if(takeMapScreenshotButton != null)
		{
			mapScreenshotTaker.TakeMapScreenshot();
		}
	}

	private void OnExitButtonClicked()
	{
		Application.Quit();
	}
}