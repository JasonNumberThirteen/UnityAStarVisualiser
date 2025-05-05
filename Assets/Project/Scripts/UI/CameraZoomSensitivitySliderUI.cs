using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CameraZoomSensitivitySliderUI : MonoBehaviour
{
	private Slider slider;
	private CameraZoomController cameraZoomController;

	private void Awake()
	{
		slider = GetComponent<Slider>();
		cameraZoomController = FindFirstObjectByType<CameraZoomController>();

		RegisterToListeners(true);
		SetZoomPerScroll(slider.value);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			slider.onValueChanged.AddListener(SetZoomPerScroll);
		}
		else
		{
			slider.onValueChanged.RemoveListener(SetZoomPerScroll);
		}
	}

	private void SetZoomPerScroll(float value)
	{
		if(cameraZoomController != null)
		{
			cameraZoomController.SetZoomPerScroll(value);
		}
	}
}