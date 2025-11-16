using UnityEngine;

public readonly struct MapTileLegendData
{
	public Color Color {get;}
	public string Text {get;}

	public MapTileLegendData(Color color, string text)
	{
		(Color, Text) = (color, text);
	}
}