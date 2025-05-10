using UnityEngine;

public class MapTilePathTrailIndicator : MonoBehaviour
{
	[SerializeField, Min(0f)] private float movementPeriodOfOscillation = 3f;
	[SerializeField, Range(0f, 0.5f)] private float movementDistanceFromCenterOfTile = 0.125f;
	
	private Vector3 initialPosition;
	
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

		initialPosition = transform.position;
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	private void Update()
	{
		if(Mathf.Approximately(movementPeriodOfOscillation, 0f))
		{
			return;
		}
		
		var frequency = 2*Mathf.PI / movementPeriodOfOscillation;
		var positionOffset = Mathf.Sin(Time.time*frequency)*movementDistanceFromCenterOfTile;

		transform.position = initialPosition + transform.up*positionOffset;
	}
}