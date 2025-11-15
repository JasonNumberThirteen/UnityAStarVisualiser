public class MapTileNodeData
{
	public float TotalCost => RealValue + HeuristicValue;

	public MapTileNode Parent {get; private set;}
	public float RealValue {get; private set;}
	public float HeuristicValue {get; private set;}

	public void SetValues(MapTileNode parentMapTileNode, float realValue, float heuristicValue)
	{
		Parent = parentMapTileNode;
		RealValue = realValue;
		HeuristicValue = heuristicValue;
	}
}