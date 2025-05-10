using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PanelUI))]
public class PanelUIHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private PanelUI panelUI;
	private VisualiserEventsManager visualiserEventsManager;
	
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		SendHoverEvent(true);
	}

	public void OnPointerExit(PointerEventData pointerEventData)
	{
		SendHoverEvent(false);
	}

	private void Awake()
	{
		panelUI = GetComponent<PanelUI>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();
	}

	private void SendHoverEvent(bool @bool)
	{
		if(visualiserEventsManager != null)
		{
			visualiserEventsManager.SendEvent(new PanelUIBoolVisualiserEvent(panelUI, VisualiserEventType.PanelUIHoverStateWasChanged, @bool));
		}
	}
}