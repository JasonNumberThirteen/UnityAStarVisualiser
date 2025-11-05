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

	private readonly List<MapTile> mapTiles = new();

	private Vector2Int mapDimensions;
	private MapTilesManager mapTilesManager;

	public List<MapTile> GetMapTiles() => mapTiles;
	public Vector2 GetMapDimensions() => mapDimensions;
	public Vector2 GetCenterOfMap() => GetPositionOfBottomRightCorner()*0.5f;
	public MapTile GetMapTileOfType(MapTileType mapTileType) => mapTiles.FirstOrDefault(mapTile => mapTile.GetTileType() == mapTileType);
	
	public void ResetMapTiles()
	{
		var mapTiles = ObjectMethods.FindComponentsOfType<MapTile>(false).Where(mapTile => mapTile.BelongsToPassableTypes());

		mapTiles.ForEach(mapTile => mapTile.ResetTile());
	}

	public void ChangeMapDimensionsIfNeeded(Vector2Int newMapSize)
	{
		mapDimensions = newMapSize;
		
		RemoveMapTilesFromShrinkingIfNeeded(mapDimensions);
		AddMapTilesFromExtendingIfNeeded(mapDimensions);
		EnsureExistanceOfMapTileOfType(MapTileType.Start, Vector2.zero);
		EnsureExistanceOfMapTileOfType(MapTileType.Destination, GetPositionOfBottomRightCorner());
		mapWasGeneratedEvent?.Invoke();
	}

	private void RemoveMapTilesFromShrinkingIfNeeded(Vector2Int newMapSize)
	{
		var mapTilesToRemove = GetMapTilesToRemove(newMapSize);

		if(mapTilesManager == null || mapTilesToRemove.Count == 0)
		{
			return;
		}

		mapTilesManager.RemoveMapTiles(mapTilesToRemove, mapTile => mapTiles.Remove(mapTile));
		mapTilesWereRemovedEvent?.Invoke(mapTilesToRemove);
	}

	private void AddMapTilesFromExtendingIfNeeded(Vector2Int newMapSize)
	{
		var mapTilesToAdd = new List<MapTile>();
		var positionsForMapTiles = GetPositionsForMapTilesToAdd(newMapSize);

		if(mapTilesManager == null || positionsForMapTiles.Count() == 0)
		{
			return;
		}

		var newMapTiles = mapTilesManager.GetMapTiles(positionsForMapTiles.Count(), mapTile => mapTilesToAdd.Add(mapTile));
		var positionsToMapTiles = newMapTiles.Zip(positionsForMapTiles, (mapTile, position) => new MapTilePosition(mapTile, position));

		positionsToMapTiles.ForEach(mapTilePosition => mapTilePosition.MapTile.Setup(mapTilePosition.Position));
		mapTiles.AddRange(mapTilesToAdd);
		mapTilesWereAddedEvent?.Invoke(mapTilesToAdd);
	}

	private IEnumerable<Vector2Int> GetPositionsForMapTilesToAdd(Vector2Int newMapSize)
	{
		var alreadyTakenPositions = GetAlreadyTakenPositionsByMapTiles();

		return VectorMethods.GetTiledPositionsWithin(newMapSize).Where(position => !alreadyTakenPositions.Contains(position));
	}

	private List<MapTile> GetMapTilesToRemove(Vector2Int newMapSize)
	{
		var rectangleArea = RectMethods.GetRectWithSize(newMapSize);

		return mapTiles.Where(mapTile => !rectangleArea.Contains(mapTile.GetPosition())).ToList();
	}

	private void EnsureExistanceOfMapTileOfType(MapTileType mapTileType, Vector2 position)
	{
		if(mapTiles.Any(mapTile => mapTile.GetTileType() == mapTileType))
		{
			return;
		}

		var mapTile = mapTiles.FirstOrDefault(mapTile => (Vector2)mapTile.GetPosition() == position);

		if(mapTile != null)
		{
			mapTile.SetTileType(mapTileType);
		}
	}

	private void Awake()
	{
		mapTilesManager = ObjectMethods.FindComponentOfType<MapTilesManager>();
		
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

	private Vector2 GetPositionOfBottomRightCorner() => GetMapDimensions() - Vector2.one;
	private List<Vector2Int> GetAlreadyTakenPositionsByMapTiles() => new(mapTiles.Select(mapTile => Vector2Int.RoundToInt(mapTile.GetPosition())));
}