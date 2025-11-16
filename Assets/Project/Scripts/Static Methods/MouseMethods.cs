#if !UNITY_ANDROID
using UnityEngine;
using UnityEngine.InputSystem;
#endif

public static class MouseMethods
{
#if !UNITY_ANDROID
	public static Vector2 GetMousePosition() => Mouse.current.position.ReadValue();
#endif
}