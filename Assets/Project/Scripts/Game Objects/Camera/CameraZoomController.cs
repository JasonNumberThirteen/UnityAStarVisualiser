using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
	[SerializeField, Min(0f)] private float zoomPerScroll = 0.1f;
	[SerializeField, Min(0f)] private float initialSize = 2f;
	[SerializeField, Min(0f)] private float minimumSize = 1f;
	[SerializeField, Min(0f)] private float maximumSize = 10f;
	
	private bool zoomCanBeModified = true;
	private bool mapTileIsSelected;
	private Camera mainCamera;
	private UserInputController userInputController;
	private VisualiserEventsManager visualiserEventsManager;

	private void Awake()
	{
		mainCamera = Camera.main;
		userInputController = FindFirstObjectByType<UserInputController>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

		RegisterToListeners(true);
		SetSizeTo(initialSize);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.AddListener(OnWheelScrolled);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.RemoveListener(OnWheelScrolled);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnWheelScrolled(Vector2 scrollVector)
	{
		if(zoomCanBeModified && mainCamera != null && mainCamera.orthographic)
		{
			SetSizeTo(mainCamera.orthographicSize - zoomPerScroll*scrollVector.y);
		}
	}

	private void SetSizeTo(float size)
	{
		mainCamera.orthographicSize = Mathf.Clamp(size, minimumSize, maximumSize);
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();

		switch (mapTileBoolVisualiserEvent.GetVisualiserEventType())
		{
			case VisualiserEventType.MapTileHoverStateWasChanged:
				if(!mapTileIsSelected)
				{
					zoomCanBeModified = !stateIsEnabled;
				}
				break;
			
			case VisualiserEventType.MapTileSelectionStateWasChanged:
				mapTileIsSelected = stateIsEnabled;
				break;
		}
	}
}