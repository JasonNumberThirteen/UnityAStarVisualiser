using UnityEngine;

public class MapScreenshotSceneCamera : SceneCamera
{
	private MainSceneCameraZoomController mainSceneCameraZoomController;

	public void SetTargetTexture(RenderTexture renderTexture)
	{
		thisCamera.targetTexture = renderTexture;
	}

	public void Render()
	{
		thisCamera.Render();
	}

	protected override void Awake()
	{
		mainSceneCameraZoomController = ObjectMethods.FindComponentOfType<MainSceneCameraZoomController>();
		
		base.Awake();
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
			if(mainSceneCameraZoomController != null)
			{
				mainSceneCameraZoomController.cameraSizeWasUpdatedEvent.AddListener(OnCameraSizeWasUpdated);
			}
		}
		else
		{
			if(mainSceneCameraZoomController != null)
			{
				mainSceneCameraZoomController.cameraSizeWasUpdatedEvent.RemoveListener(OnCameraSizeWasUpdated);
			}
		}
	}

	private void OnCameraSizeWasUpdated(float size)
	{
		SetOrthographicSize(size);
		SetPositionToCenterOfMap();
	}
}