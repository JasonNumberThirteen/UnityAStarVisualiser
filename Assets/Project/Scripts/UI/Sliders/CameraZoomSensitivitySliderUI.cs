public class CameraZoomSensitivityListenableSliderUI : ListenableSliderUI
{
	private MainCameraZoomController mainCameraZoomController;

	protected override void Awake()
	{
		mainCameraZoomController = ObjectMethods.FindComponentOfType<MainCameraZoomController>();

		base.Awake();
		OnValueWasChanged(slider.value);
	}

	protected override void OnValueWasChanged(float value)
	{
		if(mainCameraZoomController != null)
		{
			mainCameraZoomController.SetZoomPerScroll(value);
		}
	}
}