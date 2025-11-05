using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MapTileNode))]
public class MapTile : MonoBehaviour
{
	public static readonly int MIN_WEIGHT = -1;
	public static readonly int MAX_WEIGHT = 10;

	public UnityEvent<MapTileType> tileTypeWasChangedEvent;
	public UnityEvent<int> weightWasChangedEvent;
	
	private int weight;
	private MapTileType tileType;
	private MapTileNode mapTileNode;
	private VisualiserEventsManager visualiserEventsManager;

	public bool IsActive() => gameObject.activeInHierarchy;
	public Vector3 GetPosition() => transform.position;
	public int GetWeight() => weight;
	public MapTileType GetTileType() => tileType;
	public MapTileNode GetMapTileNode() => mapTileNode;

	public void Setup(Vector2Int position)
	{
		SetPosition(position);
		SetTileType(MapTileType.Passable);
	}

	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}

	public void SetPosition(Vector2Int position)
	{
		transform.position = position.ToVector3(transform.position.z);
	}

	public void ResetTile()
	{
		SetWeightTo(0);
		SetTileType(MapTileType.Passable);
	}

	public void ModifyWeightBy(int weight)
	{
		SetWeightTo(this.weight + weight);
	}

	public void SetWeightTo(int weight)
	{
		var previousWeight = this.weight;
		
		this.weight = Mathf.Clamp(weight, MIN_WEIGHT, MAX_WEIGHT);

		if(previousWeight != this.weight)
		{
			OnWeightWasChanged();
		}
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

	public bool BelongsToPassableTypes()
	{
		var allowedMapTileTypes = new List<MapTileType>()
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};

		return allowedMapTileTypes.Contains(GetTileType());
	}

	private void Awake()
	{
		mapTileNode = GetComponent<MapTileNode>();
		visualiserEventsManager = ObjectMethods.FindComponentOfType<VisualiserEventsManager>();
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