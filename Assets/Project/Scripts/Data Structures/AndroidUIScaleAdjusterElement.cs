#if UNITY_ANDROID
using System;
using UnityEngine;
#endif

#if UNITY_ANDROID
[Serializable]
#endif
public class AndroidUIScaleAdjusterElement
{
#if UNITY_ANDROID
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private bool adjustHeight;
	[SerializeField] private bool adjustAnchoredPositionY;

	public RectTransform GetRectTransform() => rectTransform;
	public bool HeightShouldBeAdjusted() => adjustHeight;
	public bool AnchoredPositionYShouldBeAdjusted() => adjustAnchoredPositionY;
#endif
}