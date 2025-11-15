using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapTilesManager : MonoBehaviour
{
	[SerializeField] private Transform goParentTransform;
	
	private MapTilesPooler mapTilesPooler;

	public List<MapTile> GetMapTiles(int numberOfTiles, Action<MapTile> onMapTileWasGot)
	{
		if(mapTilesPooler == null)
		{
			return new List<MapTile>();
		}

		var mapTiles = new List<MapTile>();

		Enumerable.Range(1, numberOfTiles).ForEach(i => mapTiles.Add(mapTilesPooler.GetFirstAvailableMapTile(goParentTransform, onMapTileWasGot)));
		
		return mapTiles;
	}

	public void RemoveMapTiles(List<MapTile> mapTiles, Action<MapTile> onMapTileWasRemoved)
	{
		if(mapTilesPooler != null)
		{
			mapTiles?.ForEach(mapTile => mapTilesPooler.ReturnMapTileToPooler(mapTile, onMapTileWasRemoved));
		}
	}

	private void Awake()
	{
		mapTilesPooler = ObjectMethods.FindComponentOfType<MapTilesPooler>();
	}
}