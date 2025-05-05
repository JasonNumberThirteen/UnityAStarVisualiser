using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class MapGenerationManager : MonoBehaviour
{
	public UnityEvent mapGeneratedEvent;
	
	[SerializeField, Min(3)] private int mapWidth;
	[SerializeField, Min(3)] private int mapHeight;
	[SerializeField] private MapTile mapTilePrefab;
	[SerializeField] private Transform goParentTransform;

	private readonly List<MapTile> mapTiles = new();

	public Vector2 GetMapDimensions() => new(mapWidth, mapHeight);
	public Vector2 GetMapSize() => GetMapDimensions()*MapTile.GRID_SIZE;
	public List<MapTile> GetMapTiles() => mapTiles;

	public void ResetTiles()
	{
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		var mapTiles = FindObjectsByType<MapTile>(FindObjectsSortMode.None).Where(mapTile => allowedMapTileTypes.Contains(mapTile.GetTileType())).ToList();

		mapTiles.ForEach(mapTile => mapTile.ResetTile());
	}

	public List<MapTile> GetMapCornersTiles()
	{
		var mapSize = GetMapSize();
		var mapTiles = GetMapTiles();
		
		return mapTiles.Where(mapTile =>
		{
			var mapTilePosition = mapTile.GetPosition();
			var mapTileIsPlacedInTopLeftCorner = mapTilePosition.x == 0 && mapTilePosition.y == 0;
			var mapTileIsPlacedInTopRightCorner = mapTilePosition.x == mapSize.x - 1 && mapTilePosition.y == 0;
			var mapTileIsPlacedInBottomLeftCorner = mapTilePosition.x == 0 && mapTilePosition.y == mapSize.y - 1;
			var mapTileIsPlacedInBottomRightCorner = mapTilePosition.x == mapSize.x - 1 && mapTilePosition.y == mapSize.y - 1;

			return mapTileIsPlacedInTopLeftCorner || mapTileIsPlacedInTopRightCorner || mapTileIsPlacedInBottomLeftCorner || mapTileIsPlacedInBottomRightCorner;
		}).ToList();
	}

	private void Awake()
	{
		for (var y = 0; y < mapHeight; ++y)
		{
			for (var x = 0; x < mapWidth; ++x)
			{
				SpawnMapTile(new Vector2(x, y));
			}
		}

		mapGeneratedEvent?.Invoke();
	}

	private void SpawnMapTile(Vector2 positionInTiles)
	{
		var instance = Instantiate(mapTilePrefab, positionInTiles*MapTile.GRID_SIZE, Quaternion.identity, goParentTransform);

		instance.SetTileType(GetMapTileType(positionInTiles));
		mapTiles.Add(instance);
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