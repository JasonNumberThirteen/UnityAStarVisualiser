using UnityEngine;

public class ExitPopupPanelUI : PopupPanelUI
{
	[SerializeField] private ButtonUI yesButtonUI;
	[SerializeField] private ButtonUI noButtonUI;

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
		if(yesButtonUI != null)
		{
			yesButtonUI.RegisterToClickListener(Application.Quit, register);
		}

		if(noButtonUI != null)
		{
			noButtonUI.RegisterToClickListener(OnNoButtonUIClicked, register);
		}
	}

	private void OnNoButtonUIClicked()
	{
		SetActive(false);
	}
}