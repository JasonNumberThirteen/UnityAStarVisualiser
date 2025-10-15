using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBackgroundColorSetter : MonoBehaviour
{
	private Camera thisCamera;

	private static readonly Color32 BACKGROUND_COLOR = new(239, 239, 239, 255);

	private void Awake()
	{
		thisCamera = GetComponent<Camera>();
	}
	
	private void Start()
	{
		thisCamera.backgroundColor = ColorMethods.GetColorWithSetAlpha(BACKGROUND_COLOR, 0);
	}
}