using UnityEngine;

[RequireComponent(typeof(MapTile), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class MapTileSelectionIndicator : MonoBehaviour
{
	private MapTile mapTile;
	private BoxCollider2D boxCollider2D;
	private SpriteRenderer spriteRenderer;
	private SelectedMapTileManager selectedMapTileManager;

	private static readonly int SELECTED_MAP_TILE_SORTING_LAYER = 1;
	private static readonly float SELECTED_MAP_TILE_COLLIDER_SCALE = 0.5f;
	private static readonly float SELECTED_MAP_TILE_GAME_OBJECT_SCALE = 1.25f;

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();

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
		}
		else
		{
			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}
		}
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		var thisMapTileWasSelected = mapTile == this.mapTile;
		var gameObjectScale = thisMapTileWasSelected ? SELECTED_MAP_TILE_GAME_OBJECT_SCALE : 1f;
		var colliderScale = thisMapTileWasSelected ? SELECTED_MAP_TILE_COLLIDER_SCALE : 1f;
		var sortingOrder = thisMapTileWasSelected ? SELECTED_MAP_TILE_SORTING_LAYER : 0;
		
		transform.localScale = (Vector2.one*gameObjectScale).ToVector3(1);
		boxCollider2D.size = Vector2.one*colliderScale;
		spriteRenderer.sortingOrder = sortingOrder;
	}
}