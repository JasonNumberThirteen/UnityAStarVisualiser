using UnityEngine;
using UnityEngine.UI;

public class ButtonsPanelUI : PanelUI, IPrimaryWindowElement
{
	[SerializeField] private Button findPathButtonUI;
	[SerializeField] private Button clearResultsButtonUI;
	[SerializeField] private Button resetTilesButtonUI;
	[SerializeField] private Button changeMapDimensionsButtonUI;
	[SerializeField] private Button takeMapScreenshotButtonUI;
	[SerializeField] private Button exitButtonUI;

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
			
			if(exitButtonUI != null)
			{
				exitButtonUI.onClick.AddListener(OnExitButtonUIClicked);
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

			if(exitButtonUI != null)
			{
				exitButtonUI.onClick.RemoveListener(OnExitButtonUIClicked);
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
		if(changeMapDimensionsPanelUI != null)
		{
			changeMapDimensionsPanelUI.SetActive(true);
		}
	}

	private void OnTakeMapScreenshotButtonUIClicked()
	{
		if(takeMapScreenshotButtonUI != null)
		{
			mapScreenshotTaker.TakeMapScreenshot();
		}
	}

	private void OnExitButtonUIClicked()
	{
		Application.Quit();
	}
}