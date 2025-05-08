using UnityEngine;
using UnityEngine.UI;

public class ExitPopupPanelUI : PopupPanelUI
{
	[SerializeField] private Button yesButtonUI;
	[SerializeField] private Button noButtonUI;

	protected override void Awake()
	{
		base.Awake();
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
			if(yesButtonUI != null)
			{
				yesButtonUI.onClick.AddListener(Application.Quit);
			}

			if(noButtonUI != null)
			{
				noButtonUI.onClick.AddListener(OnNoButtonUIClicked);
			}
		}
		else
		{
			if(yesButtonUI != null)
			{
				yesButtonUI.onClick.RemoveListener(Application.Quit);
			}

			if(noButtonUI != null)
			{
				noButtonUI.onClick.RemoveListener(OnNoButtonUIClicked);
			}
		}
	}

	private void OnNoButtonUIClicked()
	{
		SetActive(false);
	}
}