using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapTilesPooler : MonoBehaviour
{
	[SerializeField] private MapTile mapTilePrefab;
	
	private readonly List<MapTile> mapTiles = new();

	public MapTile GetFirstAvailableMapTile(Transform parentTransform, Action<MapTile> onMapTileWasFound = null)
	{
		var mapTile = mapTiles.FirstOrDefault(mapTile => !mapTile.IsActive());

		if(mapTile != null)
		{
			SetupMapTile(mapTile, true, parentTransform, onMapTileWasFound);
		}

		return mapTile;
	}

	public void ReturnMapTileToPooler(MapTile mapTile, Action<MapTile> onMapTileWasReturned = null)
	{
		if(mapTile != null)
		{
			SetupMapTile(mapTile, false, transform, onMapTileWasReturned);
		}
	}

	private void SetupMapTile(MapTile mapTile, bool active, Transform parentTransform, Action<MapTile> action)
	{
		if(mapTile == null)
		{
			return;
		}

		mapTile.SetActive(active);
		mapTile.transform.SetParent(parentTransform);
		action?.Invoke(mapTile);
	}

	private void Awake()
	{
		if(mapTilePrefab == null)
		{
			return;
		}
		
		var mapDimensionUpperBound = MapGenerationManager.MAP_DIMENSION_UPPER_BOUND;
		var numberOfTiles = mapDimensionUpperBound*mapDimensionUpperBound;

		for (var i = 0; i < numberOfTiles; ++i)
		{
			var instance = Instantiate(mapTilePrefab);

			ReturnMapTileToPooler(instance);
			mapTiles.Add(instance);
		}
	}
}