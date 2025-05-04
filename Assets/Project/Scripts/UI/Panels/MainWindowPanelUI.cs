using UnityEngine;
using UnityEngine.UI;

public class MainWindowPanelUI : PanelUI
{
	[SerializeField] private Button findPathButton;
	[SerializeField] private Button clearResultsButton;
	[SerializeField] private Button resetTilesButton;
	[SerializeField] private Button exitButton;

	private PathfindingManager pathfindingManager;
	private MapGenerationManager mapGenerationManager;

	private void Awake()
	{
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		
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

	private void OnExitButtonClicked()
	{
		Application.Quit();
	}
}