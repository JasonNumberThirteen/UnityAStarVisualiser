using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseMethods
{
	public static Vector2 GetMousePosition() => Mouse.current.position.ReadValue();
}