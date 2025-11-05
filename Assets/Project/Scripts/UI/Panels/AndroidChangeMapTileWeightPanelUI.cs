using UnityEngine;

public class AndroidChangeMapTileWeightPanelUI : PanelUI
{
#if UNITY_ANDROID
	[SerializeField] private SliderUI sliderUI;
	
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
#endif
	
	private void Awake()
	{
#if UNITY_ANDROID
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();

		AdjustSliderRange();
		RegisterToListeners(true);
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void AdjustSliderRange()
	{
		if(sliderUI == null)
		{
			return;
		}

		sliderUI.SetMinValue(MapTile.MIN_WEIGHT);
		sliderUI.SetMaxValue(MapTile.MAX_WEIGHT);
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
		}
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		var hoveredMapTileBelongsToPassableTypes = mapTile != null && mapTile.BelongsToPassableTypes();
		
		if(sliderUI != null && hoveredMapTileBelongsToPassableTypes)
		{
			sliderUI.SetValue(mapTile.GetWeight());
		}
		
		SetActive(hoveredMapTileBelongsToPassableTypes);
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		SetActive(false);
	}

	private void Start()
	{
		SetActive(false);
	}
#endif
}