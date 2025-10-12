using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSplashScreenBackgroundColorSetter : MonoBehaviour
{
	private Camera thisCamera;

	private void Awake()
	{
		thisCamera = GetComponent<Camera>();
	}
	
	private void Start()
	{
		thisCamera.backgroundColor = ColorMethods.GetColorWithSetAlpha(PlayerSettings.SplashScreen.backgroundColor, 0);
	}
}