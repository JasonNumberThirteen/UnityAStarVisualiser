using UnityEngine;
using UnityEngine.Events;

public class VisualiserEventsManager : MonoBehaviour
{
	public UnityEvent<VisualiserEvent> eventWasSentEvent;

	public void SendEvent(VisualiserEvent visualiserEvent)
	{
		eventWasSentEvent?.Invoke(visualiserEvent);
	}
}