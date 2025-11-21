using System;
#if !UNITY_ANDROID
using System.IO;
#endif
using UnityEngine;
using UnityEngine.Events;

public class MapScreenshotTaker : MonoBehaviour
{
	public UnityEvent<bool> takingScreenshotStateWasChangedEvent;
	
	private MapScreenshotSceneCamera mapScreenshotSceneCamera;

	private static readonly string SCREENSHOT_NAME = "Screenshot";
	private static readonly string SCREENSHOT_EXTENSION = ".png";
	private static readonly string SCREENSHOTS_FOLDER_NAME = "Screenshots";
	private static readonly int RENDER_TEXTURE_DEPTH = 24;
	private static readonly TextureFormat RENDER_TEXTURE_FORMAT = TextureFormat.RGB24;

	public void TakeMapScreenshot()
	{
		takingScreenshotStateWasChangedEvent?.Invoke(true);
		InitiateTakingScreenshot();
		takingScreenshotStateWasChangedEvent?.Invoke(false);
	}

	private void Awake()
	{
		mapScreenshotSceneCamera = ObjectMethods.FindComponentOfType<MapScreenshotSceneCamera>();
	}

	private void InitiateTakingScreenshot()
	{
#if !UNITY_ANDROID
		EnsureExistanceOfDirectory(GetDirectoryPath());
#endif

		TakeScreenshotInDirectory();
	}

#if !UNITY_ANDROID
	private void EnsureExistanceOfDirectory(string path)
	{
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}
#endif

	private void TakeScreenshotInDirectory()
	{
		var directoryPath = GetDirectoryPath();
		var screenshotPath = GetScreenshotPath();
		var screenshotTexture = GetTextureForScreenshot();

#if !UNITY_ANDROID
		File.WriteAllBytes($"{directoryPath}/{screenshotPath}", screenshotTexture.EncodeToPNG());
#else
		NativeGallery.SaveImageToGallery(screenshotTexture, directoryPath, screenshotPath);
#endif
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
#if !UNITY_ANDROID
		return Path.GetDirectoryName(Application.dataPath);
#else
		return SCREENSHOTS_FOLDER_NAME;
#endif
	}

	private string GetDirectoryName()
	{
#if !UNITY_ANDROID
		return SCREENSHOTS_FOLDER_NAME;
#else
		return "AStar Visualiser";
#endif
	}

	private string GetDirectoryPath() => $"{GetDirectoryPathFolder()}/{GetDirectoryName()}";
	private string GetScreenshotPath() => $"{SCREENSHOT_NAME}_{DateTime.Now:yyyyMMdd_HHmmss}_AStarVisualiser{SCREENSHOT_EXTENSION}";
}