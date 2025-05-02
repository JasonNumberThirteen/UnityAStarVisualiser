using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedMapTileMovementManager : MonoBehaviour
{
	private Camera mainCamera;
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private Vector3 translationPositionOffset;

	private void Awake()
	{
		mainCamera = Camera.main;
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
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		var mapTile = mapTileBoolVisualiserEvent.GetMapTile();
		var eventType = mapTileBoolVisualiserEvent.GetVisualiserEventType();
		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();
		
		if(this.mapTile == null && eventType == VisualiserEventType.MapTileSelectionStateWasChanged && stateIsEnabled)
		{
			this.mapTile = mapTile;
		}
		else if(this.mapTile != null && eventType == VisualiserEventType.MapTileSelectionStateWasChanged && !stateIsEnabled)
		{
			this.mapTile = null;
		}
		
		if(this.mapTile == null)
		{
			return;
		}

		var mousePosition = GetMousePosition();

		translationPositionOffset = this.mapTile.gameObject.transform.position - new Vector3(mousePosition.x, mousePosition.y, this.mapTile.gameObject.transform.position.z);
	}

	private void Update()
	{
		if(mapTile == null)
		{
			return;
		}

		var mousePosition = GetMousePosition();

		mapTile.gameObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, mapTile.gameObject.transform.position.z) + translationPositionOffset;
	}

	private Vector3 GetMousePosition() => mainCamera != null ? mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : Vector3.zero;
}