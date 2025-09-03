using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapScreenshotCamera : MonoBehaviour
{
	private Camera thisCamera;
	private MapGenerationManager mapGenerationManager;
	private MainCameraZoomController mainCameraZoomController;

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
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		mainCameraZoomController = ObjectMethods.FindComponentOfType<MainCameraZoomController>();

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

		SetPositionToCenterOfMap();
	}

	private void SetPositionToCenterOfMap()
	{
		var centerOfMap = mapGenerationManager != null ? mapGenerationManager.GetCenterOfMap() : new Vector2();
		
		transform.position = centerOfMap.ToVector3(transform.position.z);
	}
}