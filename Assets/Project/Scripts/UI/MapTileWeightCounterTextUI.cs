using TMPro;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MapTileWeightCounterTextUI : MonoBehaviour, IMapEditingElement
{
	private TextMeshProUGUI textUI;
	private MapTile mapTile;
	private MapTile selectedMapTile;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;

	public void SetMapEditingElementActive(bool active)
	{
		SetActive(false);
	}

	private void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		mapTile = GetComponentInParent<MapTile>();
		hoveredMapTileManager = FindFirstObjectByType<HoveredMapTileManager>();
		selectedMapTileManager = FindFirstObjectByType<SelectedMapTileManager>();

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
			if(mapTile != null)
			{
				mapTile.weightWasChangedEvent.AddListener(OnWeightWasChanged);
			}
			
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}
		}
		else
		{
			if(mapTile != null)
			{
				mapTile.weightWasChangedEvent.RemoveListener(OnWeightWasChanged);
			}
			
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}
		}
	}

	private void Start()
	{
		SetActive(false);
	}

	private void OnWeightWasChanged(int weight)
	{
		textUI.text = weight.ToString();

		SetActive(weight >= 0);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		if(selectedMapTile == null)
		{
			SetActive(TextUIShouldBeVisible(mapTile));
		}
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		SetActive(mapTile == null && TextUIShouldBeVisible(selectedMapTile));

		selectedMapTile = mapTile;
	}

	private void SetActive(bool active)
	{
		textUI.enabled = active;
	}

	private bool TextUIShouldBeVisible(MapTile mapTile)
	{
		var mapTileIsDefined = mapTile != null;
		var mapTileIsTheSame = mapTileIsDefined && mapTile == this.mapTile;
		var mapTileWeightIsPositive = mapTileIsDefined && mapTile.GetWeight() >= 0;
		var allowedTileTypes = new List<MapTileType>
		{
			MapTileType.Passable,
			MapTileType.Impassable
		};

		return mapTileIsTheSame && mapTileWeightIsPositive && allowedTileTypes.Contains(mapTile.GetTileType());
	}
}