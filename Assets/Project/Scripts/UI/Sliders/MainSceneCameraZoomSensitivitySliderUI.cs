public class MainSceneCameraZoomSensitivityListenableSliderUI : ListenableSliderUI
{
	private MainSceneCameraZoomController mainSceneCameraZoomController;

	protected override void Awake()
	{
		mainSceneCameraZoomController = ObjectMethods.FindComponentOfType<MainSceneCameraZoomController>();

		base.Awake();
		OnValueWasChanged(slider.value);
	}

	protected override void OnValueWasChanged(float value)
	{
		if(mainSceneCameraZoomController != null)
		{
			mainSceneCameraZoomController.SetZoomPerScroll(value);
		}
	}
}