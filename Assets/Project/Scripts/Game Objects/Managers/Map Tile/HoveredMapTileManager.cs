using UnityEngine;
using UnityEngine.Events;

public class HoveredMapTileManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<MapTile> hoveredMapTileWasChangedEvent;

	private bool mapTilesCanBeHovered = true;
	private MapTile mapTile;
#if !UNITY_ANDROID
	private MapPathManager mapPathManager;
	private MapTileRaycaster mapTileRaycaster;
#endif
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		if(!active)
		{
			SetMapTile(null);
		}
	}

	public void SetMapEditingElementActive(bool active)
	{
		mapTilesCanBeHovered = active;
	}

	private void Awake()
	{
#if !UNITY_ANDROID
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
		mapTileRaycaster = ObjectMethods.FindComponentOfType<MapTileRaycaster>();
#endif
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();

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
#if !UNITY_ANDROID
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
#endif
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
#if !UNITY_ANDROID
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
#endif
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

#if !UNITY_ANDROID
	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		if(!started && mapTileRaycaster != null && mapTileRaycaster.ComponentWasDetected(MouseMethods.GetMousePosition(), out var mapTile))
		{
			SetMapTile(mapTile);
		}
	}
#endif

	private void OnEventWasSent(VisualiserEvent visualiserEvent)
	{
		if(mapTilesCanBeHovered && visualiserEvent is MapTileBoolVisualiserEvent mapTileBoolVisualiserEvent && mapTileBoolVisualiserEvent.GetVisualiserEventType() == VisualiserEventType.MapTileHoverStateWasChanged)
		{
			SetMapTile(mapTileBoolVisualiserEvent.GetBoolValue() ? mapTileBoolVisualiserEvent.GetMapTile() : null);
		}
	}

	private void SetMapTile(MapTile mapTile)
	{
		this.mapTile = mapTile;

		hoveredMapTileWasChangedEvent?.Invoke(this.mapTile);
	}
}