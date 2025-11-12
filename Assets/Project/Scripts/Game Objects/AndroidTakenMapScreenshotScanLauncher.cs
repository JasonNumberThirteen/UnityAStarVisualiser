using UnityEngine;

public class AndroidTakenMapScreenshotScanLauncher : MonoBehaviour
{
#if UNITY_ANDROID
	private MapScreenshotTaker mapScreenshotTaker;
#endif

	private void Awake()
	{
#if UNITY_ANDROID
		mapScreenshotTaker = ObjectMethods.FindComponentOfType<MapScreenshotTaker>();

		RegisterToListeners(true);
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(mapScreenshotTaker != null)
			{
				mapScreenshotTaker.screenshotWasTakenEvent.AddListener(OnScreenshotWasTaken);
			}
		}
		else
		{
			if(mapScreenshotTaker != null)
			{
				mapScreenshotTaker.screenshotWasTakenEvent.RemoveListener(OnScreenshotWasTaken);
			}
		}
	}

	private void OnScreenshotWasTaken(string directoryPathFolder)
	{
		using var unityPlayer = new AndroidJavaClass(AndroidJNINames.UNITY_PLAYER_CLASS_NAME);
		using var mediaScanner = new AndroidJavaClass(AndroidJNINames.MEDIA_SCANNER_CONNECTION_CLASS_NAME);
		using var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>(AndroidJNINames.CURRENT_ACTIVITY_FIELD_NAME);

		mediaScanner.CallStatic(AndroidJNINames.SCAN_FILE_METHOD_NAME, currentActivity, new string[] {directoryPathFolder}, null, null);
	}
#endif
}