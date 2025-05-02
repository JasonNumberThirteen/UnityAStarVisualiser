public class VisualiserEvent
{
	private readonly VisualiserEventType visualiserEventType;

	public VisualiserEvent(VisualiserEventType visualiserEventType)
	{
		this.visualiserEventType = visualiserEventType;
	}

	public VisualiserEventType GetVisualiserEventType() => visualiserEventType;
}