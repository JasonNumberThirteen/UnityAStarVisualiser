using System.IO;
using System.Linq;
using UnityEngine;

public class MapScreenshotTaker : MonoBehaviour
{
	private MapScreenshotCamera mapScreenshotCamera;
	
	private static readonly string DIRECTORY_NAME = "Screenshots";
	private static readonly string SCREENSHOT_NAME = "Screenshot";
	private static readonly string SCREENSHOT_EXTENSION = ".png";
	private static readonly int RENDER_TEXTURE_DEPTH = 24;
	private static readonly TextureFormat RENDER_TEXTURE_FORMAT = TextureFormat.RGB24;
	
	public void TakeMapScreenshot()
	{
		var directoryPath = $"{Application.dataPath}/{DIRECTORY_NAME}";
		
		EnsureExistanceOfDirectory(directoryPath);
		TakeScreenshotInDirectory(directoryPath);
	}

	private void Awake()
	{
		mapScreenshotCamera = ObjectMethods.FindComponentOfType<MapScreenshotCamera>();
	}

	private void EnsureExistanceOfDirectory(string path)
	{
		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	private void TakeScreenshotInDirectory(string path)
	{
		var numberOfExistingScreenshots = new DirectoryInfo(path).GetFiles().Where(file => file.Extension.Equals(SCREENSHOT_EXTENSION)).Count();
		var screenshotNumber = numberOfExistingScreenshots + 1;
		var screenshotPath = $"{path}/{SCREENSHOT_NAME}{screenshotNumber}{SCREENSHOT_EXTENSION}";
		var screenshotTexture = GetTextureForScreenshot();

		File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());
	}

	private Texture2D GetTextureForScreenshot()
	{
		var (width, height) = (Screen.width, Screen.height);
		var renderTexture = new RenderTexture(width, height, RENDER_TEXTURE_DEPTH);
		var texture2D = new Texture2D(width, height, RENDER_TEXTURE_FORMAT, false);
		var screenRectangle = new Rect(Vector2Int.zero, new Vector2Int(width, height));

		SetRenderTextureResult(renderTexture);
		texture2D.ReadPixels(screenRectangle, 0, 0);
		texture2D.Apply();
		SetRenderTextureResult(null);
		Destroy(renderTexture);

		return texture2D;
	}

	private void SetRenderTextureResult(RenderTexture renderTexture)
	{
		if(mapScreenshotCamera == null)
		{
			return;
		}
		
		mapScreenshotCamera.SetTargetTexture(renderTexture);
		
		if(renderTexture != null)
		{
			mapScreenshotCamera.Render();
		}

		RenderTexture.active = renderTexture;
	}
}