using System.Linq;
using System.Collections.Generic;

public class MapTileNodesQueue
{
	private readonly List<MapTileNode> queue = new();

	public bool IsEmpty() => queue.Count == 0;
	public bool Contains(MapTileNode mapTileNode) => queue.Contains(mapTileNode);
	public MapTileNode GetNext() => queue.OrderBy(mapTileNode => mapTileNode.GetMapTileNodeData().TotalCost).FirstOrDefault();

	public void Add(MapTileNode mapTileNode, bool canBeAdded = true)
	{
		if(mapTileNode != null && canBeAdded)
		{
			queue.Add(mapTileNode);
		}
	}

	public void Clear()
	{
		queue.Clear();
	}

	public void Remove(MapTileNode mapTileNode)
	{
		queue.Remove(mapTileNode);
	}
}