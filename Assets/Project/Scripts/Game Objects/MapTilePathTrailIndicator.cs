using DG.Tweening;
using UnityEngine;

public class MapTilePathTrailIndicator : MonoBehaviour
{
	[SerializeField, Min(0f)] private float movementPeriodOfOscillation = 3f;
	[SerializeField, Range(0f, 0.5f)] private float movementDistanceFromCenterOfTile = 0.125f;

	private Tween movementTween;
	
	public void Setup(MapTileNode currentMapTileNode, MapTileNode nextMapTileNode)
	{
		if(currentMapTileNode != null)
		{
			transform.SetParent(currentMapTileNode.transform);
		}

		if(nextMapTileNode != null)
		{
			transform.up = nextMapTileNode.transform.position - transform.position;
		}

		StartMoving();
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	private void StartMoving()
	{
		if(Mathf.Approximately(movementPeriodOfOscillation, 0f))
		{
			return;
		}
		
		var positionOffset = transform.up*movementDistanceFromCenterOfTile;
		var startPosition = transform.position - positionOffset;
		var endPosition = startPosition + 2*positionOffset;

		movementTween?.Kill();

		transform.position = startPosition;
		movementTween = transform.DOMove(endPosition, movementPeriodOfOscillation*0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	private void OnDestroy()
	{
		movementTween?.Kill();
	}
}