using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapScreenshotCamera : MonoBehaviour
{
	private Camera thisCamera;
	private MainCameraZoomController mainCameraZoomController;
	private MapGenerationManager mapGenerationManager;

	public void SetTargetTexture(RenderTexture renderTexture)
	{
		thisCamera.targetTexture = renderTexture;
	}

	public void Render()
	{
		thisCamera.Render();
	}

	private void Awake()
	{
		thisCamera = GetComponent<Camera>();
		mainCameraZoomController = FindFirstObjectByType<MainCameraZoomController>();
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();

		RegisterToListeners(true);
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(mainCameraZoomController != null)
			{
				mainCameraZoomController.cameraSizeWasUpdatedEvent.AddListener(OnCameraSizeWasUpdated);
			}
		}
		else
		{
			if(mainCameraZoomController != null)
			{
				mainCameraZoomController.cameraSizeWasUpdatedEvent.RemoveListener(OnCameraSizeWasUpdated);
			}
		}
	}

	private void OnCameraSizeWasUpdated(float size)
	{
		thisCamera.orthographicSize = size;

		if(mapGenerationManager == null)
		{
			return;
		}

		var centerOfMap = mapGenerationManager.GetCenterOfMap();
		
		transform.position = new Vector3(centerOfMap.x, centerOfMap.y, transform.position.z);
	}
}