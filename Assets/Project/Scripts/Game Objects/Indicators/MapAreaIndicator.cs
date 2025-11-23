using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapAreaIndicator : MonoBehaviour, IMapScreenshotTakerElement
{
	[SerializeField][Range(0f, 1f)] private float minimumAlphaWhileFading;
	[SerializeField][Min(0f)] private float fadeDuration = 5f;
	
	private SpriteRenderer spriteRenderer;
	private MapAreaManager mapAreaManager;
	private Tween fadeTween;
	private float lastAlpha;

	private static readonly float ALPHA_WHILE_TAKING_MAP_SCREENSHOT = 1f;

	public void AdjustForTakingMapScreenshot(bool started)
	{
		if(started)
		{
			fadeTween?.Pause();

			lastAlpha = spriteRenderer.color.a;

			SetAlpha(ALPHA_WHILE_TAKING_MAP_SCREENSHOT);
		}
		else
		{
			SetAlpha(lastAlpha);
			fadeTween?.Play();
		}
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		mapAreaManager = ObjectMethods.FindComponentOfType<MapAreaManager>();

		RegisterToListeners(true);
		StartFading();
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
		fadeTween?.Kill();
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.AddListener(OnMapAreaWasChanged);
			}
		}
		else
		{
			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.RemoveListener(OnMapAreaWasChanged);
			}
		}
	}

	private void OnMapAreaWasChanged(Rect mapArea)
	{
		var offset = mapArea.min.GetAbsoluteVector();

		transform.position = mapArea.center + offset*0.5f;
		spriteRenderer.size = mapArea.size + offset;
	}

	private void StartFading()
	{
		if(fadeDuration > 0)
		{
			fadeTween = spriteRenderer.DOFade(minimumAlphaWhileFading, fadeDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		}
	}

	private void SetAlpha(float alpha)
	{
		spriteRenderer.color = ColorMethods.GetColorWithSetAlpha(spriteRenderer.color, alpha);
	}
}