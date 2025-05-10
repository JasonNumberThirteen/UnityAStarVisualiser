using UnityEngine;
using UnityEngine.Events;

public class MapAreaManager : MonoBehaviour
{
	public UnityEvent<Rect> mapAreaWasChangedEvent;
	
	[SerializeField, Min(0)] private int additionalOffsetFromMapEdgesInTiles = 1;
	
	private Rect mapArea;

	private MapGenerationManager mapGenerationManager;

	public Rect GetMapArea() => mapArea;

	private void Awake()
	{
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.AddListener(OnMapGenerated);
			}
		}
		else
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.RemoveListener(OnMapGenerated);
			}
		}
	}

	private void OnMapGenerated()
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		var centerOfMap = mapGenerationManager.GetCenterOfMap();
		var halfOfMapSize = mapGenerationManager.GetMapSize()*0.5f;
		var additionalOffset = Vector2Int.one*additionalOffsetFromMapEdgesInTiles;

		mapArea = new Rect(centerOfMap - halfOfMapSize - additionalOffset, centerOfMap + halfOfMapSize + additionalOffset);

		mapAreaWasChangedEvent?.Invoke(mapArea);
	}
}