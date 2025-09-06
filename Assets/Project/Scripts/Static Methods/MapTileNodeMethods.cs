using System;

public static class MapTileNodeMethods
{
	public static void InvokeActionOnMapTileNodesBelongingToPath(MapTileNode mapTileNodeToStartFrom, Action<MapTileNode> action)
	{
		if(mapTileNodeToStartFrom == null || action == null)
		{
			return;
		}
		
		var currentMapTileNode = mapTileNodeToStartFrom;
		var currentMapTileNodeData = currentMapTileNode.GetMapTileNodeData();

		while (currentMapTileNodeData.Parent != null)
		{
			action(currentMapTileNode);

			currentMapTileNode = currentMapTileNodeData.Parent;
			currentMapTileNodeData = currentMapTileNode.GetMapTileNodeData();
		}
	}
}