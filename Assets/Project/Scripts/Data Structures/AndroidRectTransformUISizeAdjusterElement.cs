#if UNITY_ANDROID
using System;
using UnityEngine;
#endif

#if UNITY_ANDROID
[Serializable]
#endif
public class AndroidRectTransformUISizeAdjusterElement
{
#if UNITY_ANDROID
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private bool adjustWidth;
	[SerializeField] private bool adjustHeight;

	public RectTransform GetRectTransform() => rectTransform;
	public bool WidthShouldBeAdjusted() => adjustWidth;
	public bool HeightShouldBeAdjusted() => adjustHeight;
#endif
}