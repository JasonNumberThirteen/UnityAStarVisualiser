using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapTileNodesManager : MonoBehaviour
{
	private MapGenerationManager mapGenerationManager;
	
	public void ResetMapTileNodesData(List<MapTile> mapTiles)
	{
		mapTiles?.ForEach(mapTile => mapTile.GetMapTileNode().ResetData());
	}

	public void CreateConnectionsBetweenMapTileNodes()
	{
		var mapTiles = mapGenerationManager != null ? mapGenerationManager.GetMapTiles() : new List<MapTile>();
		var passableMapTileNodes = mapTiles.Where(mapTile => mapTile.GetTileType() != MapTileType.Impassable).Select(mapTile => mapTile.GetMapTileNode());
		
		mapTiles.ForEach(mapTile => mapTile.GetMapTileNode().FindNeighbours(passableMapTileNodes));
	}

	private void Awake()
	{
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
	}
}