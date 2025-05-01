using UnityEngine;

public class MapTile : MonoBehaviour
{
	[SerializeField] private MapTileType tileType;

	public MapTileType GetTileType() => tileType;
}