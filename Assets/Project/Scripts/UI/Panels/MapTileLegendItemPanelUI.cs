using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapTileLegendItemPanelUI : MonoBehaviour
{
	private Graphic graphic;
	private TextMeshProUGUI text;

	public void Setup(MapTileLegendData mapTileLegendData)
	{
		if(graphic != null)
		{
			graphic.color = mapTileLegendData.Color;
		}

		if(text != null)
		{
			text.text = mapTileLegendData.Text;
		}
	}

	private void Awake()
	{
		graphic = GetComponentInChildren<Graphic>();
		text = GetComponentInChildren<TextMeshProUGUI>();
	}
}