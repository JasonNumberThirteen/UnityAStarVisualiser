using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class FadeScreenPanelUI : PanelUI
{
	[SerializeField, Min(0f)] private float fadeDuration = 0.5f;

	private Graphic graphic;
	private bool isFading;

	public void FadeIn(TweenCallback onFadeWasCompleted = null)
	{
		Fade(1, () => InitiateFading(0), onFadeWasCompleted);
	}

	public void FadeOut(TweenCallback onFadeWasCompleted = null)
	{
		Fade(0, () => InitiateFading(1), () =>
		{
			onFadeWasCompleted?.Invoke();
			SetGraphicRaycastTarget(false);
		});
	}

	private void Awake()
	{
		graphic = GetComponent<Graphic>();

		FadeOut();
	}

	private void InitiateFading(float initialAlpha)
	{
		SetGraphicAlpha(initialAlpha);
		SetGraphicRaycastTarget(true);
	}
	
	private void SetGraphicAlpha(float alpha)
	{
		graphic.color = ColorMethods.GetColorWithSetAlpha(graphic.color, alpha);
	}

	private void SetGraphicRaycastTarget(bool raycastTarget)
	{
		graphic.raycastTarget = raycastTarget;
	}

	private void Fade(float targetAlpha, Action onFadeWasStarted = null, TweenCallback onFadeWasCompleted = null)
	{
		if(isFading)
		{
			return;
		}
		
		isFading = true;

		onFadeWasStarted?.Invoke();
		
		graphic.DOFade(targetAlpha, fadeDuration).SetEase(Ease.Linear).OnComplete(() =>
		{
			onFadeWasCompleted?.Invoke();

			isFading = false;
		});
	}
}