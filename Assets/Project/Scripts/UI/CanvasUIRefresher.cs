using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CanvasUIRefresher : MonoBehaviour
{
	public UnityEvent canvasWasRebuiltEvent;
	
	[SerializeField] private Canvas canvas;

	private bool canvasIsReady;

	private bool CanvasIsReady
	{
		set
		{
			var canvasWasReady = canvasIsReady;
			
			canvasIsReady = value;

			if(canvasWasReady != canvasIsReady && canvasIsReady)
			{
				canvasWasRebuiltEvent?.Invoke();
			}
		}
	}

	private void OnEnable()
	{
		Canvas.willRenderCanvases += OnWillRenderCanvases;
	}

	private void OnDisable()
	{
		Canvas.willRenderCanvases -= OnWillRenderCanvases;
	}

	private void OnWillRenderCanvases()
	{
		if(canvas != null && canvas.isActiveAndEnabled)
		{
			CanvasIsReady = !CanvasUpdateRegistry.IsRebuildingLayout() && !CanvasUpdateRegistry.IsRebuildingGraphics();
		}
	}
}