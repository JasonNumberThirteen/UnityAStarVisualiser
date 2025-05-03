using UnityEngine;
using UnityEngine.UI;

public class MainWindowPanelUI : PanelUI
{
	[SerializeField] private Button findPathButton;
	[SerializeField] private Button exitButton;

	private PathfindingManager pathfindingManager;

	private void Awake()
	{
		pathfindingManager = FindFirstObjectByType<PathfindingManager>();
		
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

	private void OnExitButtonClicked()
	{
		Application.Quit();
	}
}