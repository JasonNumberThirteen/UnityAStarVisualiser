using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class MapTileLegendItemPanelUI : MonoBehaviour
{
	private Graphic graphic;
	private LocalizeStringEvent localizeStringEvent;

	private static readonly string MAP_TILES_LEGEND_PANEL_LOCALIZATION_TABLE_REFERENCE_KEY = "Map Tiles Legend Panel";

	public void Setup(MapTileLegendData mapTileLegendData)
	{
		if(graphic != null)
		{
			graphic.color = mapTileLegendData.Color;
		}

		if(localizeStringEvent != null)
		{
			localizeStringEvent.StringReference = new LocalizedString(MAP_TILES_LEGEND_PANEL_LOCALIZATION_TABLE_REFERENCE_KEY, mapTileLegendData.Text);
		}
	}

	private void Awake()
	{
		graphic = GetComponentInChildren<Graphic>();
		localizeStringEvent = GetComponentInChildren<LocalizeStringEvent>();
	}
}