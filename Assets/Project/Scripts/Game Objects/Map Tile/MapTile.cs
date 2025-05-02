using UnityEngine;

public class MapTile : MonoBehaviour
{
	public static readonly float GRID_SIZE = 1f;
	
	private MapTileType tileType;

	public MapTileType GetTileType() => tileType;

	public void SetTileType(MapTileType tileType)
	{
		this.tileType = tileType;
	}
}