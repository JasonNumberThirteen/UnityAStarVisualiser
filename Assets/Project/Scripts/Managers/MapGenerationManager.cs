using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class MapGenerationManager : MonoBehaviour
{
	public UnityEvent mapGeneratedEvent;
	public UnityEvent<List<MapTile>> mapTilesWereAddedEvent;
	public UnityEvent<List<MapTile>> mapTilesWereRemovedEvent;
	
	[SerializeField] private MapTile mapTilePrefab;
	[SerializeField] private Transform goParentTransform;

	private readonly List<MapTile> mapTiles = new();

	private int mapWidth = 3;
	private int mapHeight = 3;

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

	public void ChangeMapDimensionsIfNeeded(Vector2 newMapSize)
	{
		mapWidth = (int)newMapSize.x;
		mapHeight = (int)newMapSize.y;
		
		RemoveTilesFromShrinkingIfNeeded(newMapSize);
		AddTilesFromExtendingIfNeeded(newMapSize);
		EnsureExistanceOfMapTileOfType(MapTileType.Start, Vector2.zero);
		EnsureExistanceOfMapTileOfType(MapTileType.Destination, GetMapSize() - Vector2.one);
	}

	private void RemoveTilesFromShrinkingIfNeeded(Vector2 newMapSize)
	{
		var mapTilesToRemove = GetMapTilesToRemove(newMapSize);

		if(mapTilesToRemove.Count() == 0)
		{
			return;
		}

		mapTilesToRemove.ForEach(mapTile =>
		{
			mapTiles.Remove(mapTile);
			Destroy(mapTile.gameObject);
		});

		mapTilesWereRemovedEvent?.Invoke(mapTilesToRemove);
	}

	private void AddTilesFromExtendingIfNeeded(Vector2 newMapSize)
	{
		var mapTilesToAdd = GetMapTilesToAdd(newMapSize);

		if(mapTilesToAdd.Count() == 0)
		{
			return;
		}

		mapTilesToAdd.ForEach(mapTile =>
		{
			mapTile.SetTileType(MapTileType.Passable);
			mapTiles.Add(mapTile);
		});

		mapTilesWereAddedEvent?.Invoke(mapTilesToAdd);
	}

	private List<MapTile> GetMapTilesToRemove(Vector2 newMapSize)
	{
		var mapTilesToRemove = new List<MapTile>();

		mapTilesToRemove.AddRange(mapTiles.Where(mapTile =>
		{
			var mapTileType = mapTile.GetTileType();
			var mapTileIsOutsideOfMapWidth = mapTile.transform.position.x < 0 || mapTile.transform.position.x > newMapSize.x - 1;
			var mapTileIsOutsideOfMapHeight = mapTile.transform.position.y < 0 || mapTile.transform.position.y > newMapSize.y - 1;
			
			return mapTileIsOutsideOfMapWidth || mapTileIsOutsideOfMapHeight;
		}).ToList());

		return mapTilesToRemove;
	}

	private List<MapTile> GetMapTilesToAdd(Vector2 newMapSize)
	{
		var mapTilesToAdd = new List<MapTile>();
		var currentMapSize = GetMapSize();
		var mapWidthShouldBeExtended = currentMapSize.x < newMapSize.x;
		var mapHeightShouldBeExtended = currentMapSize.y < newMapSize.y;

		for (var y = 0; y < newMapSize.y; ++y)
		{
			for (var x = 0; x < newMapSize.x; ++x)
			{
				var position = new Vector2(x, y);
				
				if(!mapTiles.Any(mapTile => (Vector2)mapTile.transform.position == position))
				{
					mapTilesToAdd.Add(Instantiate(mapTilePrefab, position*MapTile.GRID_SIZE, Quaternion.identity, goParentTransform));
				}
			}
		}

		return mapTilesToAdd;
	}

	private void EnsureExistanceOfMapTileOfType(MapTileType mapTileType, Vector2 position)
	{
		if(mapTiles.Any(mapTile => mapTile.GetTileType() == mapTileType))
		{
			return;
		}

		var mapTile = mapTiles.FirstOrDefault(mapTile => (Vector2)mapTile.transform.position == position);

		if(mapTile != null)
		{
			mapTile.SetTileType(mapTileType);
		}
	}

	private void Awake()
	{
		ChangeMapDimensionsIfNeeded(new Vector2(mapWidth, mapHeight));
		mapGeneratedEvent?.Invoke();
	}
}