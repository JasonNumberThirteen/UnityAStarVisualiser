using System;
using UnityEngine;

[Serializable]
public class AndroidRectTransformUISizeAdjusterElement
{
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private bool adjustWidth;
	[SerializeField] private bool adjustHeight;

	public RectTransform GetRectTransform() => rectTransform;
	public bool WidthShouldBeAdjusted() => adjustWidth;
	public bool HeightShouldBeAdjusted() => adjustHeight;
}