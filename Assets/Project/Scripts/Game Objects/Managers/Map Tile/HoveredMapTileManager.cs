using UnityEngine;
using UnityEngine.Events;

public class HoveredMapTileManager : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<MapTile> hoveredMapTileWasChangedEvent;

#if !UNITY_ANDROID
	private bool mapTileCanBeDetectedAfterPathfinding = true;
#endif
	private bool mapTilesCanBeHovered = true;
	private MapTile mapTile;
	private MapPathManager mapPathManager;
#if !UNITY_ANDROID
	private MapTileRaycaster mapTileRaycaster;
#else
	private SimulationManager simulationManager;
#endif
	private VisualiserEventsManager visualiserEventsManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		if(!active)
		{
			SetMapTile(null);
		}

#if !UNITY_ANDROID
		mapTileCanBeDetectedAfterPathfinding = active;
#endif
	}

	public void SetMapEditingElementActive(bool active)
	{
		mapTilesCanBeHovered = active;
	}

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
#if !UNITY_ANDROID
		mapTileRaycaster = ObjectMethods.FindComponentOfType<MapTileRaycaster>();
#else
		simulationManager = ObjectMethods.FindComponentOfType<SimulationManager>();
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
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.AddListener(OnEventWasSent);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
			
			if(visualiserEventsManager != null)
			{
				visualiserEventsManager.eventWasSentEvent.RemoveListener(OnEventWasSent);
			}
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
#if UNITY_ANDROID
		if(simulationManager != null && simulationManager.SimulationIsEnabled() && started && mapTile != null)
		{
			SetMapTile(null);
		}
#else
		if(!started && mapTileCanBeDetectedAfterPathfinding && mapTileRaycaster != null && mapTileRaycaster.ComponentWasDetected(MouseMethods.GetMousePosition(), out var mapTile))
		{
			SetMapTile(mapTile);
		}
#endif
	}

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