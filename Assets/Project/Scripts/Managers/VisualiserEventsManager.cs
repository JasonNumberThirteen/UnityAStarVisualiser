using UnityEngine;
using UnityEngine.Events;

public class VisualiserEventsManager : MonoBehaviour
{
	public UnityEvent<VisualiserEvent> eventReceivedEvent;

	public void SendEvent(VisualiserEvent visualiserEvent)
	{
		eventReceivedEvent?.Invoke(visualiserEvent);
	}
}