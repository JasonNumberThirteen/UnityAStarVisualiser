using UnityEngine;

[RequireComponent(typeof(MapTile))]
public class MapTileMouseEventsSender : MonoBehaviour, IPrimaryWindowElement
{
	private bool inputIsActive = true;
	private bool isHovered;
	private bool isSelected;
	private bool panelUIHoverWasDetected;
	private MapTile mapTile;
	private VisualiserEventsManager visualiserEventsManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	private bool IsHovered
	{
		set
		{
			isHovered = value;

			SendEvent(VisualiserEventType.MapTileHoverStateWasChanged, isHovered);
		}
	}

	private bool IsSelected
	{
		set
		{
			isSelected = value;

			SendEvent(VisualiserEventType.MapTileSelectionStateWasChanged, isSelected);
		}
	}

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

	private void Awake()
	{
		mapTile = GetComponent<MapTile>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}

	private void OnMouseEnter()
	{
		IsHovered = true;
	}

	private void OnMouseExit()
	{
		IsHovered = false;
	}

	private void OnMouseDown()
	{
		if(!panelUIHoverWasDetected)
		{
			IsSelected = true;
		}
	}

	private void OnMouseUp()
	{
		if(isSelected)
		{
			IsSelected = false;
		}
	}

	private void SendEvent(VisualiserEventType visualiserEventType, bool enabled)
	{
		if(inputIsActive && visualiserEventsManager != null)
		{
			visualiserEventsManager.SendEvent(new MapTileBoolVisualiserEvent(mapTile, visualiserEventType, enabled));
		}
	}
}