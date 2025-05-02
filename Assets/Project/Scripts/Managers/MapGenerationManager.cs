using UnityEngine;

public class MapGenerationManager : MonoBehaviour
{
	[SerializeField, Min(3)] private int mapWidth;
	[SerializeField, Min(3)] private int mapHeight;
	[SerializeField] private MapTile mapTilePrefab;
	[SerializeField] private Transform goParentTransform;

	private void Awake()
	{
		for (var y = 0; y < mapHeight; ++y)
		{
			for (var x = 0; x < mapWidth; ++x)
			{
				SpawnMapTile(new Vector2(x, y));
			}
		}
	}

	private void SpawnMapTile(Vector2 positionInTiles)
	{
		var instance = Instantiate(mapTilePrefab, positionInTiles*MapTile.GRID_SIZE, Quaternion.identity, goParentTransform);

		instance.SetTileType(GetMapTileType(positionInTiles));
	}

	private MapTileType GetMapTileType(Vector2 positionInTiles)
	{
		if(positionInTiles.x == 0 && positionInTiles.y == 0)
		{
			return MapTileType.Start;
		}
		else if(positionInTiles.x == mapWidth - 1 && positionInTiles.y == mapHeight - 1)
		{
			return MapTileType.Destination;
		}

		return MapTileType.Passable;
	}
}