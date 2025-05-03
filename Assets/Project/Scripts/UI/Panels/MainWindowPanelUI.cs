using UnityEngine;
using UnityEngine.UI;

public class MainWindowPanelUI : PanelUI
{
	[SerializeField] private Button exitButton;

	private void Awake()
	{
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
			if(exitButton != null)
			{
				exitButton.onClick.AddListener(OnExitButtonClicked);
			}
		}
		else
		{
			if(exitButton != null)
			{
				exitButton.onClick.RemoveListener(OnExitButtonClicked);
			}
		}
	}

	private void OnExitButtonClicked()
	{
		Application.Quit();
	}
}