using TMPro;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MapTileWeightCounterTextUI : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	private TextMeshProUGUI textUI;
	private MapTile mapTile;
	private MapTile hoveredMapTile;
	private MapTile selectedMapTile;
	private MapTile currentMapTile;
	private bool textUIWasHidden;
	private bool panelUIHoverWasDetected;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		textUIWasHidden = !active;
		panelUIHoverWasDetected = false;
		
		UpdateActiveState();
	}

	public void SetMapEditingElementActive(bool active)
	{
		textUIWasHidden = !active;
		
		UpdateActiveState();
	}

	private void Awake()
	{
		textUI = GetComponent<TextMeshProUGUI>();
		mapTile = GetComponentInParent<MapTile>();
		hoveredMapTileManager = FindFirstObjectByType<HoveredMapTileManager>();
		selectedMapTileManager = FindFirstObjectByType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = FindFirstObjectByType<PanelUIHoverDetectionManager>();

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

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.AddListener(OnPanelUIHoverDetectionStateWasChanged);
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

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.RemoveListener(OnPanelUIHoverDetectionStateWasChanged);
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
		hoveredMapTile = mapTile;
		
		if(selectedMapTile != null)
		{
			return;
		}

		currentMapTile = hoveredMapTile;

		UpdateActiveState();
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		selectedMapTile = mapTile;
		currentMapTile = selectedMapTile == null ? hoveredMapTile : null;
		
		UpdateActiveState();
	}

	private void OnPanelUIHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;

		UpdateActiveState();
	}

	private void UpdateActiveState()
	{
		SetActive(!textUIWasHidden && !panelUIHoverWasDetected && TextUIShouldBeVisible(currentMapTile));
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