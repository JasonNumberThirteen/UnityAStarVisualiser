using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedMapTileMovementManager : MonoBehaviour, IMapEditingElement
{
	[SerializeField] private LayerMask unacceptableGameObjects;
	
	private Camera mainCamera;
	private VisualiserEventsManager visualiserEventsManager;
	private MapTile mapTile;
	private Vector3 translationPositionOffset;
	private bool tilesCanBeSelected = true;

	private readonly float COLLISION_BOX_SIZE_OFFSET = 0.1f;

	public void SetMapEditingElementActive(bool active)
	{
		tilesCanBeSelected = active;
	}

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
		if(tilesCanBeSelected && visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
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
		var mapTileTiledPosition = mapTileRealPosition.ToTiledPosition(MapTile.GRID_SIZE);

		if(!DetectedAnyUnacceptableCollider(mapTileTiledPosition))
		{
			mapTile.gameObject.transform.position = mapTileTiledPosition;
		}
	}

	private Vector3 GetMousePositionToWorldPoint()
	{
		var mousePositionToWorldPoint = mainCamera != null ? mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) : Vector3.zero;

		return new Vector3(mousePositionToWorldPoint.x, mousePositionToWorldPoint.y, 0f);
	}

	private bool DetectedAnyUnacceptableCollider(Vector2 position)
	{
		var collisionBoxSize = Vector2.one*MapTile.GRID_SIZE;
		var collisionBoxSizeOffset = Vector2.one*COLLISION_BOX_SIZE_OFFSET;
		var colliders = Physics2D.OverlapBoxAll(position, collisionBoxSize - collisionBoxSizeOffset, 0f, unacceptableGameObjects);

		return colliders.Any(collider => collider.gameObject != gameObject);
	}
}