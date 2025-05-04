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

	public Vector2 GetMapSize() => new(mapWidth, mapHeight);

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