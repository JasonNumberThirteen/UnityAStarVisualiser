using UnityEngine;

public class InformationPopupPanelUI : PopupPanelUI
{
	[SerializeField] private ButtonUI closeButtonUI;

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
		if(closeButtonUI != null)
		{
			closeButtonUI.RegisterToClickListener(OnCloseButtonUIClicked, register);
		}
	}

	private void OnCloseButtonUIClicked()
	{
		SetActive(false);
	}
}