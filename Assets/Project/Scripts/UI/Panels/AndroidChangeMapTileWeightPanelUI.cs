#if UNITY_ANDROID
using UnityEngine;
#endif

public class AndroidChangeMapTileWeightPanelUI : PanelUI
{
#if UNITY_ANDROID
	[SerializeField] private SliderUI sliderUI;
	
	private MapTile mapTile;
	private MapPathManager mapPathManager;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
#endif
	
	private void Awake()
	{
#if UNITY_ANDROID
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();
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
			if(mapPathManager != null)
			{
				mapPathManager.resultsWereClearedEvent.AddListener(UpdateSliderUIValueIfPossible);
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
			if(mapPathManager != null)
			{
				mapPathManager.resultsWereClearedEvent.RemoveListener(UpdateSliderUIValueIfPossible);
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

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		this.mapTile = mapTile;
		
		UpdateSliderUIValueIfPossible();
		UpdateActiveState();
	}

	private void UpdateSliderUIValueIfPossible()
	{
		if(sliderUI != null && HoveredMapTileBelongsToPassableTypes())
		{
			sliderUI.SetValue(mapTile.GetWeight());
		}
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		this.mapTile = null;
		
		UpdateActiveState();
	}

	private void Start()
	{
		UpdateActiveState();
	}

	private void UpdateActiveState()
	{
		SetActive(HoveredMapTileBelongsToPassableTypes());
	}

	private bool HoveredMapTileBelongsToPassableTypes() => mapTile != null && mapTile.BelongsToPassableTypes();
#endif
}