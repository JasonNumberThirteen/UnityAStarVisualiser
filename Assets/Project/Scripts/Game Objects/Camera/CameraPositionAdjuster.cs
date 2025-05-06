using UnityEngine;

public class CameraPositionAdjuster : MonoBehaviour
{
	private Camera mainCamera;
	private MapGenerationManager mapGenerationManager;

	private void Awake()
	{
		mainCamera = Camera.main;
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
		if(mapGenerationManager == null || mainCamera == null)
		{
			return;
		}

		var centerOfMap = mapGenerationManager.GetCenterOfMap();
		
		mainCamera.transform.position = new Vector3(centerOfMap.x, centerOfMap.y, mainCamera.transform.position.z);
	}
}