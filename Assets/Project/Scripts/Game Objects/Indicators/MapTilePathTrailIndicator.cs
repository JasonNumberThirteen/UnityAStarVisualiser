using DG.Tweening;
using UnityEngine;

public class MapTilePathTrailIndicator : MonoBehaviour, IMapScreenshotTakerElement
{
	[SerializeField, Min(0f)] private float movementPeriodOfOscillation = 3f;
	[SerializeField, Range(0f, 0.5f)] private float movementDistanceFromCenterOfTile = 0.125f;

	private Tween movementTween;
	private Vector2 lastLocalPosition;

	public void AdjustForTakingMapScreenshot(bool started)
	{
		if(started)
		{
			movementTween?.Pause();

			lastLocalPosition = transform.localPosition;
			transform.localPosition = Vector2.zero;
		}
		else
		{
			transform.localPosition = lastLocalPosition;

			movementTween?.Play();
		}
	}
	
	public void Setup(MapTileNode currentMapTileNode, MapTileNode nextMapTileNode)
	{
		SetParentAs(currentMapTileNode);
		SetFacingTo(nextMapTileNode);
		movementTween?.Kill();
		StartMovingIfPossible();
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	private void SetParentAs(MapTileNode mapTileNode)
	{
		if(mapTileNode != null)
		{
			transform.SetParent(mapTileNode.transform);
		}
	}

	private void SetFacingTo(MapTileNode mapTileNode)
	{
		if(mapTileNode != null)
		{
			transform.up = mapTileNode.transform.position - transform.position;
		}
	}

	private void StartMovingIfPossible()
	{
		if(!Mathf.Approximately(movementPeriodOfOscillation, 0f))
		{
			StartMoving();
		}
	}

	private void StartMoving()
	{
		var positionOffset = transform.up*movementDistanceFromCenterOfTile;
		var startPosition = transform.position - positionOffset;
		var endPosition = startPosition + 2*positionOffset;

		transform.position = startPosition;
		movementTween = transform.DOMove(endPosition, movementPeriodOfOscillation*0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	private void OnDestroy()
	{
		movementTween?.Kill();
	}
}