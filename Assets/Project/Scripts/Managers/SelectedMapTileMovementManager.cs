using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectedMapTileMovementManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	[SerializeField] private LayerMask unacceptableGameObjects;
	
	private Camera mainCamera;
	private MapTile mapTile;
	private bool selectingTilesIsLocked;
	private bool panelUIHoverWasDetected;
	private Vector3 translationPositionOffset;
	private MapAreaManager mapAreaManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	private readonly float COLLISION_BOX_SIZE_OFFSET = 0.1f;

	public void SetPrimaryWindowElementActive(bool active)
	{
		selectingTilesIsLocked = !active;
		panelUIHoverWasDetected = false;
		mapTile = null;
	}

	public void SetMapEditingElementActive(bool active)
	{
		selectingTilesIsLocked = !active;
		mapTile = null;
	}

	private void Awake()
	{
		mainCamera = Camera.main;
		mapAreaManager = ObjectMethods.FindComponentOfType<MapAreaManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

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
			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.AddListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.RemoveListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		this.mapTile = selectingTilesIsLocked || panelUIHoverWasDetected ? null : mapTile;

		if(this.mapTile != null)
		{
			translationPositionOffset = this.mapTile.gameObject.transform.position - GetMousePositionToWorldPoint();
		}
	}

	private void OnPanelUIHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}

	private void Update()
	{
		if(mapTile == null)
		{
			return;
		}

		var mapTileRealPosition = GetMousePositionToWorldPoint() + translationPositionOffset;
		var mapTileTiledPosition = GetPositionWithinArea(mapTileRealPosition.ToTiledPosition());

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
		var collisionBoxSize = Vector2.one;
		var collisionBoxSizeOffset = Vector2.one*COLLISION_BOX_SIZE_OFFSET;
		var colliders = Physics2D.OverlapBoxAll(position, collisionBoxSize - collisionBoxSizeOffset, 0f, unacceptableGameObjects);

		return colliders.Any(collider => collider.gameObject != gameObject);
	}

	private Vector2 GetPositionWithinArea(Vector2 position)
	{
		if(mapAreaManager == null)
		{
			return position;
		}

		var mapArea = mapAreaManager.GetMapArea();
		var positionWithinArea = new Vector2
		{
			x = Mathf.Clamp(position.x, mapArea.x + 0.5f, mapArea.width - 0.5f),
			y = Mathf.Clamp(position.y, mapArea.y + 0.5f, mapArea.height - 0.5f)
		};

		return positionWithinArea;
	}
}