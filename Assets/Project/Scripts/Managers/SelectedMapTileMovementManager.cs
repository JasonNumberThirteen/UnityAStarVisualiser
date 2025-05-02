using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedMapTileMovementManager : MonoBehaviour
{
	private Camera mainCamera;
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private Vector3 translationPositionOffset;

	private readonly float GRID_SIZE = 1f;

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
		if(visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			UpdateMapTileReference(mapTileBoolVisualiserEvent);
		}
	}

	private void UpdateMapTileReference(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		if(MapTileShouldBeSelected(mapTileBoolVisualiserEvent))
		{
			mapTile = mapTileBoolVisualiserEvent.GetMapTile();
			translationPositionOffset = mapTile.gameObject.transform.position - GetMousePositionToWorldPoint();
		}
		else if(MapTileShouldBeDeselected(mapTileBoolVisualiserEvent))
		{
			mapTile = null;
		}
	}

	private bool MapTileShouldBeSelected(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTileSelectionStateIsSetAsSelected = mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileSelectionStateWasChanged && mapTileBoolVisualiserEvent.GetBoolValue();
		
		return mapTile == null && mapTileSelectionStateIsSetAsSelected;
	}

	private bool MapTileShouldBeDeselected(MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
	{
		var mapTileSelectionStateIsSetAsNotSelected = mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileSelectionStateWasChanged && !mapTileBoolVisualiserEvent.GetBoolValue();
		
		return mapTile != null && mapTileSelectionStateIsSetAsNotSelected;
	}

	private void Update()
	{
		if(mapTile == null)
		{
			return;
		}

		var mapTileRealPosition = GetMousePositionToWorldPoint() + translationPositionOffset;

		mapTile.gameObject.transform.position = mapTileRealPosition.ToTiledPosition(GRID_SIZE);
	}

	private Vector3 GetMousePositionToWorldPoint()
	{
		var mousePositionToWorldPoint = mainCamera != null ? mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : Vector3.zero;

		return new Vector3(mousePositionToWorldPoint.x, mousePositionToWorldPoint.y, 0f);
	}
}