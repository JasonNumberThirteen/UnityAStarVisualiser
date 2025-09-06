using System.Linq;
using UnityEngine;

public class SelectedMapTileMovementManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	[SerializeField] private LayerMask unacceptableGameObjects;
	
	private bool selectingTilesIsLocked;
	private bool panelUIHoverWasDetected;
	private MapTile mapTile;
	private Vector3 translationPositionOffset;
	private MapAreaManager mapAreaManager;
	private MainSceneCamera mainSceneCamera;
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
		mapAreaManager = ObjectMethods.FindComponentOfType<MapAreaManager>();
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
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
			translationPositionOffset = this.mapTile.GetPosition() - GetMousePositionToWorldPoint();
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

		var mapArea = mapAreaManager != null ? mapAreaManager.GetMapArea() : Rect.zero;
		var mapTileRealPosition = GetMousePositionToWorldPoint() + translationPositionOffset;
		var mapTileTiledPosition = VectorMethods.GetPositionWithinRect(mapTileRealPosition.ToTiledPosition(), mapArea, 0.5f);

		if(!AnyUnacceptableColliderWasDetected(mapTileTiledPosition))
		{
			mapTile.SetPosition(Vector2Int.RoundToInt(mapTileTiledPosition));
		}
	}

	private Vector3 GetMousePositionToWorldPoint()
	{
		var position = mainSceneCamera != null ? (Vector2)mainSceneCamera.GetScreenToWorldPointFromMousePosition() : Vector2.zero;

		return position.ToVector3();
	}

	private bool AnyUnacceptableColliderWasDetected(Vector2 position)
	{
		var size = VectorMethods.GetUniformVector2(1f - COLLISION_BOX_SIZE_OFFSET);
		var colliders = Physics2D.OverlapBoxAll(position, size, 0f, unacceptableGameObjects);

		return colliders.Any(collider => collider.gameObject != gameObject);
	}
}