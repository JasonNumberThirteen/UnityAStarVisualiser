public class PanelUIBoolVisualiserEvent : BoolVisualiserEvent
{
	private readonly PanelUI panelUI;

	public PanelUIBoolVisualiserEvent(PanelUI panelUI, VisualiserEventType visualiserEventType, bool stateIsEnabled) : base(visualiserEventType, stateIsEnabled)
	{
		this.panelUI = panelUI;
	}

	public PanelUI GetPanelUI() => panelUI;
}