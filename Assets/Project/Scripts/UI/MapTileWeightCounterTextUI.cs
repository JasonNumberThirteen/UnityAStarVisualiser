using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MapTileWeightCounterTextUI : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	private TextMeshProUGUI textUI;
	private MapTile hoveredMapTile;
	private MapTile selectedMapTile;
	private MapTile currentMapTile;
	private bool textUIWasHidden;
	private bool panelUIHoverWasDetected;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private MapTileWeightController mapTileWeightController;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	private MapTile CurrentMapTile
	{
		set
		{
			currentMapTile = value;

			OnWeightWasChanged(currentMapTile != null ? currentMapTile.GetWeight() : 0);
		}
	}

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
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
		mapTileWeightController = ObjectMethods.FindComponentOfType<MapTileWeightController>();
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
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(mapTileWeightController != null)
			{
				mapTileWeightController.weightWasChangedEvent.AddListener(OnWeightWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.panelUIHoverDetectionStateWasChangedEvent.AddListener(OnPanelUIHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

			if(mapTileWeightController != null)
			{
				mapTileWeightController.weightWasChangedEvent.RemoveListener(OnWeightWasChanged);
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

	private void OnEnable()
	{
		if(currentMapTile != null)
		{
			transform.position = currentMapTile.transform.position;
		}
	}

	private void OnWeightWasChanged(int weight)
	{
		SetMapTileWeightText(weight.ToString());
		UpdateActiveState();
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		hoveredMapTile = mapTile;

		if(selectedMapTile == null)
		{
			CurrentMapTile = hoveredMapTile;
		}
	}

	private void SetMapTileWeightText(string text)
	{
		textUI.SetText(text);
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		selectedMapTile = mapTile;
		CurrentMapTile = selectedMapTile == null ? hoveredMapTile : null;
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
		gameObject.SetActive(active);
	}

	private bool TextUIShouldBeVisible(MapTile mapTile) => mapTile != null && mapTile.GetTileType() == MapTileType.Passable;
}