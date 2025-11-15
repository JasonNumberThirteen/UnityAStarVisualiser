using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MainWindowPanelUI : PanelUI, IPrimaryWindowElement
{
	private RectTransform rectTransform;
	private CanvasUIRefresher canvasUIRefresher;
	
	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
	}

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasUIRefresher = ObjectMethods.FindComponentOfType<CanvasUIRefresher>();

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
			if(canvasUIRefresher != null)
			{
				canvasUIRefresher.canvasWasRebuiltEvent.AddListener(OnCanvasWasRebuilt);
			}
		}
		else
		{
			if(canvasUIRefresher != null)
			{
				canvasUIRefresher.canvasWasRebuiltEvent.RemoveListener(OnCanvasWasRebuilt);
			}
		}
	}

	private void OnCanvasWasRebuilt()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
	}
}