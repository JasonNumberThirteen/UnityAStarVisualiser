using System;
using UnityEngine;

[Serializable]
public class AndroidCanvasUIScaleAdjusterElement
{
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private bool adjustHeight;
	[SerializeField] private bool adjustAnchoredPositionY;

	public RectTransform GetRectTransform() => rectTransform;
	public bool HeightShouldBeAdjusted() => adjustHeight;
	public bool AnchoredPositionYShouldBeAdjusted() => adjustAnchoredPositionY;
}