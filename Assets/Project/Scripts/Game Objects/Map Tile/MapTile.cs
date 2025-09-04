using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MapTileNode))]
public class MapTile : MonoBehaviour
{
	public static readonly int MIN_WEIGHT = -1;
	public static readonly int MAX_WEIGHT = 10;

	public UnityEvent<MapTileType> tileTypeWasChangedEvent;
	public UnityEvent<int> weightWasChangedEvent;
	
	private MapTileType tileType;
	private int weight;
	private MapTileNode mapTileNode;
	private VisualiserEventsManager visualiserEventsManager;

	public bool IsActive() => gameObject.activeInHierarchy;
	public Vector3 GetPosition() => transform.position;
	public MapTileType GetTileType() => tileType;
	public int GetWeight() => weight;
	public MapTileNode GetMapTileNode() => mapTileNode;

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	public void ResetTile()
	{
		SetWeightTo(0);
		SetTileType(MapTileType.Passable);
	}

	public void SetTileType(MapTileType tileType)
	{
		var previousTileType = this.tileType;

		if(previousTileType == tileType)
		{
			return;
		}
		
		this.tileType = tileType;

		tileTypeWasChangedEvent?.Invoke(this.tileType);
	}

	public void ModifyWeightBy(int weight)
	{
		SetWeightTo(this.weight + weight);
	}

	private void Awake()
	{
		mapTileNode = GetComponent<MapTileNode>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
	}

	private void SetWeightTo(int weight)
	{
		var previousWeight = this.weight;
		
		this.weight = Mathf.Clamp(weight, MIN_WEIGHT, MAX_WEIGHT);

		if(previousWeight != this.weight)
		{
			OnWeightWasChanged();
		}
	}

	private void OnWeightWasChanged()
	{
		mapTileNode.Weight = weight;
		
		weightWasChangedEvent?.Invoke(weight);
		SetTileType(weight < 0 ? MapTileType.Impassable : MapTileType.Passable);
		SendWeightWasChangedEvent();
	}

	private void SendWeightWasChangedEvent()
	{
		if(visualiserEventsManager != null)
		{
			visualiserEventsManager.SendEvent(new MapTileBoolVisualiserEvent(this, VisualiserEventType.MapTileWeightWasChanged, true));
		}
	}
}