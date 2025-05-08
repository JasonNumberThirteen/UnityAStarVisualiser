using UnityEngine;
using UnityEngine.UI;

public class InformationPopupPanelUI : PopupPanelUI
{
	[SerializeField] private Button closeButtonUI;

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
			if(closeButtonUI != null)
			{
				closeButtonUI.onClick.AddListener(OnCloseButtonUIClicked);
			}
		}
		else
		{
			if(closeButtonUI != null)
			{
				closeButtonUI.onClick.RemoveListener(OnCloseButtonUIClicked);
			}
		}
	}

	private void OnCloseButtonUIClicked()
	{
		SetActive(false);
	}
}