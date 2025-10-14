using UnityEngine;

public class CameraSettingsPanelUI : PanelUI
{
	[SerializeField] private SliderUI cameraMovementSpeedSliderUI;
	[SerializeField] private SliderUI cameraZoomSensitivitySliderUI;

	private MainSceneCameraMovementController mainSceneCameraMovementController;
	private MainSceneCameraZoomController mainSceneCameraZoomController;

	private void Awake()
	{
		mainSceneCameraMovementController = ObjectMethods.FindComponentOfType<MainSceneCameraMovementController>();
		mainSceneCameraZoomController = ObjectMethods.FindComponentOfType<MainSceneCameraZoomController>();
		
		SetInitialValues();
		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(cameraMovementSpeedSliderUI != null)
		{
			cameraMovementSpeedSliderUI.RegisterToValueChangeListener(SetMainCameraMovementSpeed, register);
		}
		
		if(cameraZoomSensitivitySliderUI != null)
		{
			cameraZoomSensitivitySliderUI.RegisterToValueChangeListener(SetMainCameraZoom, register);
		}
	}

	private void SetInitialValues()
	{
		if(cameraMovementSpeedSliderUI != null)
		{
			SetMainCameraMovementSpeed(cameraMovementSpeedSliderUI.GetValue());
		}

		if(cameraMovementSpeedSliderUI != null)
		{
			SetMainCameraZoom(cameraMovementSpeedSliderUI.GetValue());
		}
	}

	private void SetMainCameraMovementSpeed(float movementSpeed)
	{
		if(mainSceneCameraMovementController != null)
		{
			mainSceneCameraMovementController.SetMovementSpeed(movementSpeed);
		}
	}

	private void SetMainCameraZoom(float zoom)
	{
		if(mainSceneCameraZoomController != null)
		{
			mainSceneCameraZoomController.SetZoomPerScroll(zoom);
		}
	}
}