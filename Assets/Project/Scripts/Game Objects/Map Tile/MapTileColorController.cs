using UnityEngine;

[RequireComponent(typeof(MapTile), typeof(SpriteRenderer))]
public class MapTileColorController : MonoBehaviour
{
	private MapTile mapTile;
	private SpriteRenderer spriteRenderer;

	private static readonly Color32 PASSABLE_NODE_WITH_MAX_WEIGHT_COLOR = new(255, 127, 0, 255);

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		spriteRenderer = GetComponent<SpriteRenderer>();

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
			mapTile.tileTypeWasChangedEvent.AddListener(OnTileTypeWasChanged);
			mapTile.weightWasChangedEvent.AddListener(OnWeightWasChanged);
		}
		else
		{
			mapTile.tileTypeWasChangedEvent.RemoveListener(OnTileTypeWasChanged);
			mapTile.weightWasChangedEvent.RemoveListener(OnWeightWasChanged);
		}
	}

	private void OnTileTypeWasChanged(MapTileType mapTileType)
	{
		spriteRenderer.color = MapTileTypeMethods.GetColorByMapTileType(mapTileType);
	}

	private void OnWeightWasChanged(int weight)
	{
		var percent = (float)weight / MapTile.MAX_WEIGHT;
		
		spriteRenderer.color = Color.Lerp(MapTileTypeMethods.GetColorByMapTileType(MapTileType.Passable), PASSABLE_NODE_WITH_MAX_WEIGHT_COLOR, percent);
	}
}