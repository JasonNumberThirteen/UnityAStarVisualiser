using System.Linq;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
	[SerializeField, Min(0.01f)] private float zoomPerScroll = 0.1f;
	[SerializeField, Min(0f)] private float initialSize = 2f;
	[SerializeField, Min(0f)] private float minimumSize = 1f;

	private float maximumSize = float.MaxValue;
	private bool zoomCanBeModified = true;
	private bool mapTileIsSelected;
	private Camera mainCamera;
	private UserInputController userInputController;
	private MapGenerationManager mapGenerationManager;
	private VisualiserEventsManager visualiserEventsManager;

	private void Awake()
	{
		mainCamera = Camera.main;
		userInputController = FindFirstObjectByType<UserInputController>();
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		visualiserEventsManager = FindFirstObjectByType<VisualiserEventsManager>();

		RegisterToListeners(true);
		SetSizeTo(initialSize);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.AddListener(OnWheelScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.AddListener(OnMapGenerated);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.AddListener(OnEventReceived);
			}
		}
		else
		{
			if(userInputController != null)
			{
				userInputController.wheelScrolledEvent.RemoveListener(OnWheelScrolled);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapGeneratedEvent.RemoveListener(OnMapGenerated);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventReceivedEvent.RemoveListener(OnEventReceived);
			}
		}
	}

	private void OnWheelScrolled(Vector2 scrollVector)
	{
		if(zoomCanBeModified && mainCamera != null && mainCamera.orthographic)
		{
			SetSizeTo(mainCamera.orthographicSize - zoomPerScroll*scrollVector.y);
		}
	}

	private void OnMapGenerated()
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		var currentSize = initialSize;

		while (!EntireMapIsVisibleWithinCamera() && currentSize < maximumSize)
		{
			currentSize += zoomPerScroll;

			SetSizeTo(currentSize);
		}

		maximumSize = currentSize;
	}

	private bool EntireMapIsVisibleWithinCamera()
	{
		if(mapGenerationManager == null)
		{
			return false;
		}
		
		var cameraHeight = mainCamera.orthographicSize*2f;
		var cameraWidth = cameraHeight*mainCamera.aspect;
		var cameraPosition = mainCamera.transform.position;
		var cameraSight = new Rect(cameraPosition.x - cameraWidth*0.5f, cameraPosition.y - cameraHeight*0.5f, cameraWidth, cameraHeight);
		var mapCornersTiles = mapGenerationManager.GetMapCornersTiles();

		return mapCornersTiles.All(mapTile =>
		{
			var mapTileBounds = mapTile.GetMapTileRenderer().GetBounds();
			var mapTileBoundsRectangle = new Rect(mapTileBounds.min.x, mapTileBounds.min.y, mapTileBounds.size.x, mapTileBounds.size.y);

			return cameraSight.Contains(mapTileBoundsRectangle.min) && cameraSight.Contains(mapTileBoundsRectangle.max);
		});
	}

	private void SetSizeTo(float size)
	{
		mainCamera.orthographicSize = Mathf.Clamp(size, minimumSize, maximumSize);
	}

	private void OnEventReceived(VisualiserEvent visualiserEvent)
	{
		if(visualiserEvent is not MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent)
		{
			return;
		}

		var stateIsEnabled = mapTileBoolVisualiserEvent.GetBoolValue();

		switch (mapTileBoolVisualiserEvent.GetVisualiserEventType())
		{
			case VisualiserEventType.MapTileHoverStateWasChanged:
				if(!mapTileIsSelected)
				{
					zoomCanBeModified = !stateIsEnabled;
				}
				break;
			
			case VisualiserEventType.MapTileSelectionStateWasChanged:
				mapTileIsSelected = stateIsEnabled;
				break;
		}
	}
}