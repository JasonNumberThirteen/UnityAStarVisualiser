public class BoolVisualiserEvent : VisualiserEvent
{
	private readonly bool @bool;

	public BoolVisualiserEvent(VisualiserEventType visualiserEventType, bool @bool) : base(visualiserEventType)
	{
		this.@bool = @bool;
	}

	public bool GetBoolValue() => @bool;
}