using UnityEngine;
using UnityEngine.Events;

public class PanelUIHoverDetectionManager : MonoBehaviour, IPrimaryWindowElement
{
	public UnityEvent<bool> panelUIHoverDetectionStateWasChangedEvent;
	
	private bool hoverWasDetected;
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		hoverWasDetected = false;
	}

	private void Awake()
	{
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

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
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
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
		
		panelUIHoverDetectionStateWasChangedEvent?.Invoke(hoverWasDetected);
	}
}