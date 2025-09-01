using UnityEngine;

public class MainCameraPositionAdjuster : MonoBehaviour
{
	private Camera mainCamera;
	private MapGenerationManager mapGenerationManager;

	private void Awake()
	{
		mainCamera = Camera.main;
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();

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
				mapGenerationManager.mapWasGeneratedEvent.AddListener(OnMapWasGenerated);
			}
		}
		else
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.RemoveListener(OnMapWasGenerated);
			}
		}
	}

	private void OnMapWasGenerated()
	{
		if(mapGenerationManager == null || mainCamera == null)
		{
			return;
		}

		var centerOfMap = mapGenerationManager.GetCenterOfMap();
		
		mainCamera.transform.position = new Vector3(centerOfMap.x, centerOfMap.y, mainCamera.transform.position.z);
	}
}