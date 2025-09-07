using UnityEngine;
using UnityEngine.Events;

public class PanelUIHoverDetectionManager : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<bool> hoverDetectionStateWasChangedEvent;
	
	private bool hoverWasDetected;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		hoverWasDetected = false;
	}

	private void Awake()
	{
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();

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
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is PanelUIBoolVisualiserEvent panelUIBoolVisualiserEvent)
		{
			SetHoverDetectionState(panelUIBoolVisualiserEvent.GetBoolValue());
		}
	}

	private void SetHoverDetectionState(bool detected)
	{
		var previousState = hoverWasDetected;

		if(previousState == detected)
		{
			return;
		}

		hoverWasDetected = detected;
		
		hoverDetectionStateWasChangedEvent?.Invoke(hoverWasDetected);
	}
}