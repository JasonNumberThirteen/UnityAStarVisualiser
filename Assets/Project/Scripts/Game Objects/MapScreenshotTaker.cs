using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapScreenshotTaker : MonoBehaviour
{
	public UnityEvent<string> screenshotWasTakenEvent;
	
	private MapScreenshotSceneCamera mapScreenshotSceneCamera;

	private static readonly string SCREENSHOT_NAME = "Screenshot";
	private static readonly string SCREENSHOT_EXTENSION = ".png";
#if UNITY_ANDROID
	private static readonly string ANDROID_SCREENSHOTS_FOLDER_NAME = "Screenshots";
#endif
	private static readonly int RENDER_TEXTURE_DEPTH = 24;
	private static readonly TextureFormat RENDER_TEXTURE_FORMAT = TextureFormat.RGB24;
	
	public void TakeMapScreenshot()
	{
		var directoryPathFolder = GetDirectoryPathFolder();
		var directoryPath = $"{directoryPathFolder}/{GetDirectoryName()}";
		
		EnsureExistanceOfDirectory(directoryPath);
		TakeScreenshotInDirectory(directoryPath, directoryPathFolder);
	}

	private void Awake()
	{
		mapScreenshotSceneCamera = ObjectMethods.FindComponentOfType<MapScreenshotSceneCamera>();
	}

	private void EnsureExistanceOfDirectory(string path)
	{
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	private void TakeScreenshotInDirectory(string path, string pathFolder)
	{
		var numberOfExistingScreenshots = new DirectoryInfo(path).GetFiles().Where(file => file.Extension.Equals(SCREENSHOT_EXTENSION)).Count();
		var screenshotNumber = numberOfExistingScreenshots + 1;
		var screenshotPath = $"{path}/{SCREENSHOT_NAME}{screenshotNumber}{SCREENSHOT_EXTENSION}";
		var screenshotTexture = GetTextureForScreenshot();

		File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());
		screenshotWasTakenEvent?.Invoke(pathFolder);
	}

	private Texture2D GetTextureForScreenshot()
	{
		var (width, height) = (Screen.width, Screen.height);
		var renderTexture = new RenderTexture(width, height, RENDER_TEXTURE_DEPTH);
		var texture2D = new Texture2D(width, height, RENDER_TEXTURE_FORMAT, false);
		var screenRectangle = RectMethods.GetRectWithSize(new Vector2Int(width, height));

		SetRenderTextureResult(renderTexture);
		texture2D.ReadPixels(screenRectangle, 0, 0);
		texture2D.Apply();
		SetRenderTextureResult(null);
		Destroy(renderTexture);

		return texture2D;
	}

	private void SetRenderTextureResult(RenderTexture renderTexture)
	{
		if(mapScreenshotSceneCamera == null)
		{
			return;
		}
		
		mapScreenshotSceneCamera.SetTargetTexture(renderTexture);
		
		if(renderTexture != null)
		{
			mapScreenshotSceneCamera.Render();
		}

		RenderTexture.active = renderTexture;
	}

	private string GetDirectoryPathFolder()
	{
#if UNITY_ANDROID
		using var environment = new AndroidJavaClass(AndroidJNINames.OS_ENVIRONMENT_CLASS_NAME);
		var directoryDCIM = environment.GetStatic<string>(AndroidJNINames.DIRECTORY_DCIM_FIELD_NAME);
		using var externalStoragePublicDirectory = environment.CallStatic<AndroidJavaObject>(AndroidJNINames.GET_EXTERNAL_STORAGE_PUBLIC_DIRECTORY_METHOD_NAME, directoryDCIM);
		var absolutePath = externalStoragePublicDirectory.Call<string>(AndroidJNINames.GET_ABSOLUTE_PATH_METHOD_NAME);
		
		return Path.Combine(absolutePath, ANDROID_SCREENSHOTS_FOLDER_NAME);
#else
		return Path.GetDirectoryName(Application.dataPath);
#endif
	}

	private string GetDirectoryName()
	{
#if UNITY_ANDROID
		return "AStar Visualiser";
#else
		return "Screenshots";
#endif
	}
}