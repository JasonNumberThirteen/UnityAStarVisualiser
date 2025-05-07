public class MainWindowPanelUI : PanelUI, IPrimaryWindowElement
{
	public void SetPrimaryWindowElementActive(bool active)
	{
		SetActive(active);
	}
}