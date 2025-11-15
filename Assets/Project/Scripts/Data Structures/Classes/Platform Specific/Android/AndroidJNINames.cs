public static class AndroidJNINames
{
#if UNITY_ANDROID
	public static readonly string UNITY_PLAYER_CLASS_NAME = "com.unity3d.player.UnityPlayer";
	public static readonly string OS_ENVIRONMENT_CLASS_NAME = "android.os.Environment";
	public static readonly string MEDIA_SCANNER_CONNECTION_CLASS_NAME = "android.media.MediaScannerConnection";

	public static readonly string SCAN_FILE_METHOD_NAME = "scanFile";
	public static readonly string GET_ABSOLUTE_PATH_METHOD_NAME = "getAbsolutePath";
	public static readonly string GET_EXTERNAL_STORAGE_PUBLIC_DIRECTORY_METHOD_NAME = "getExternalStoragePublicDirectory";

	public static readonly string DIRECTORY_DCIM_FIELD_NAME = "DIRECTORY_DCIM";
	public static readonly string CURRENT_ACTIVITY_FIELD_NAME = "currentActivity";
#endif
}