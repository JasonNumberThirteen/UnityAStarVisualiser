using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasScaler))]
public class CanvasUI : MonoBehaviour
{
	protected Canvas canvas;
	protected CanvasScaler canvasScaler;

	public Canvas GetCanvas() => canvas;
	public CanvasScaler GetCanvasScaler() => canvasScaler;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		canvasScaler = GetComponent<CanvasScaler>();
	}
}