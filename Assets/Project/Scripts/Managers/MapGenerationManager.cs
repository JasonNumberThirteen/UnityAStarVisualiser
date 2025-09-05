using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class MapGenerationManager : MonoBehaviour
{
	public UnityEvent mapWasGeneratedEvent;
	public UnityEvent<List<MapTile>> mapTilesWereAddedEvent;
	public UnityEvent<List<MapTile>> mapTilesWereRemovedEvent;
	
	[SerializeField] private Transform goParentTransform;

	private readonly List<MapTile> mapTiles = new();

	private Vector2Int mapDimensions;
	private MapTilesPooler mapTilesPooler;

	public Vector2 GetCenterOfMap() => (GetMapSize() - Vector2.one)*0.5f;
	public Vector2 GetMapSize() => GetMapDimensions();
	public Vector2 GetMapDimensions() => mapDimensions;
	public int GetMaximumMapDimension() => Mathf.Max(mapDimensions.x, mapDimensions.y);

	public void ResetTiles()
	{
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};
		var mapTiles = ObjectMethods.FindComponentsOfType<MapTile>(false).Where(mapTile => allowedMapTileTypes.Contains(mapTile.GetTileType())).ToList();

		mapTiles.ForEach(mapTile => mapTile.ResetTile());
	}

	public void ChangeMapDimensionsIfNeeded(Vector2Int newMapSize)
	{
		mapDimensions = newMapSize;
		
		RemoveTilesFromShrinkingIfNeeded(mapDimensions);
		AddTilesFromExtendingIfNeeded(mapDimensions);
		EnsureExistanceOfMapTileOfType(MapTileType.Start, Vector2.zero);
		EnsureExistanceOfMapTileOfType(MapTileType.Destination, GetMapSize() - Vector2.one);
		mapWasGeneratedEvent?.Invoke();
	}

	private void RemoveTilesFromShrinkingIfNeeded(Vector2Int newMapSize)
	{
		var mapTilesToRemove = GetMapTilesToRemove(newMapSize);

		if(mapTilesPooler == null || mapTilesToRemove.Count() == 0)
		{
			return;
		}

		mapTilesToRemove.ForEach(mapTile => mapTilesPooler.ReturnMapTileToPooler(mapTile, mapTile => mapTiles.Remove(mapTile)));
		mapTilesWereRemovedEvent?.Invoke(mapTilesToRemove);
	}

	private void AddTilesFromExtendingIfNeeded(Vector2Int newMapSize)
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

	private List<MapTile> GetMapTilesToRemove(Vector2Int newMapSize)
	{
		var mapTilesToRemove = new List<MapTile>();
		var rectangleArea = RectMethods.GetRectWithSize(newMapSize);

		mapTilesToRemove.AddRange(mapTiles.Where(mapTile => !rectangleArea.Contains(mapTile.transform.position)).ToList());

		return mapTilesToRemove;
	}

	private List<MapTile> GetMapTilesToAdd(Vector2Int newMapSize)
	{
		if(mapTilesPooler == null)
		{
			return new List<MapTile>();
		}
		
		var mapTilesToAdd = new List<MapTile>();
		var alreadyTakenPositions = new List<Vector2Int>(mapTiles.Select(mapTile => Vector2Int.RoundToInt(mapTile.GetPosition())));
		var allTilesPositions = Enumerable.Range(0, newMapSize.x*newMapSize.y).Select(i => new Vector2Int(i % newMapSize.x, i / newMapSize.x));
		var missingTilesPositions = allTilesPositions.Where(position => !alreadyTakenPositions.Contains(position)).ToList();

		missingTilesPositions.ForEach(position => mapTilesPooler.GetFirstAvailableMapTile(goParentTransform, mapTile =>
		{
			mapTile.transform.position = (Vector2)position;

			mapTilesToAdd.Add(mapTile);
		}));

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
		mapTilesPooler = ObjectMethods.FindComponentOfType<MapTilesPooler>();
		
		GenerateInitialMap(10);
	}

	private void GenerateInitialMap(int size)
	{
		var mapBoundariesManager = ObjectMethods.FindComponentOfType<MapBoundariesManager>();
		var mapDimensionLowerBound = mapBoundariesManager != null ? mapBoundariesManager.GetLowerBound() : 0;
		var mapDimensionUpperBound = mapBoundariesManager != null ? mapBoundariesManager.GetUpperBound() : 0;
		var initialMapSize = Mathf.Clamp(size, mapDimensionLowerBound, mapDimensionUpperBound);
		
		ChangeMapDimensionsIfNeeded(Vector2Int.one*initialMapSize);
	}
}