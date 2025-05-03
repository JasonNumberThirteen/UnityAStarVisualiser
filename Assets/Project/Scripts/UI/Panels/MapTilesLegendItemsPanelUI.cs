using UnityEngine;

public class MapTilesLegendItemsPanelUI : MonoBehaviour
{
	[SerializeField] private MapTileLegendItemPanelUI mapTileLegendItemPanelUIPrefab;
	
	private void Awake()
	{
		if(mapTileLegendItemPanelUIPrefab == null)
		{
			return;
		}

		var colorsByMapTileType = MapTileTypeMethods.GetKeyValuePairs();

		foreach (var pair in colorsByMapTileType)
		{
			Instantiate(mapTileLegendItemPanelUIPrefab, transform).Setup(new MapTileLegendData(pair.Value, pair.Key.ToString()));
		}
	}
}