using UnityEngine;

[RequireComponent(typeof(MapTile), typeof(SpriteRenderer))]
public class MapTileColorController : MonoBehaviour
{
	private MapTile mapTile;
	private SpriteRenderer spriteRenderer;

	private readonly Color32 PASSABLE_NODE_COLOR = new(255, 255, 255, 255);
	private readonly Color32 START_NODE_COLOR = new(151, 208, 119, 255);
	private readonly Color32 DESTINATION_NODE_COLOR = new(255, 153, 153, 255);

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		if(mapTile != null && spriteRenderer != null)
		{
			spriteRenderer.color = GetColorByTileType();
		}
	}

	private Color GetColorByTileType()
	{
		return mapTile.GetTileType() switch
		{
			MapTileType.Passable => PASSABLE_NODE_COLOR,
			MapTileType.Start => START_NODE_COLOR,
			MapTileType.Destination => DESTINATION_NODE_COLOR,
			_ => Color.white
		};
	}
}