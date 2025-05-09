using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMapDimensionsPopupPanelUI : PopupPanelUI
{
	[SerializeField] private TMP_InputField widthMapDimensionInputFieldUI;
	[SerializeField] private TMP_InputField heightMapDimensionInputFieldUI;
	
	[SerializeField] private Button changeDimensionsButtonUI;
	[SerializeField] private Button cancelButtonUI;

	private MapGenerationManager mapGenerationManager;

	protected override void Awake()
	{
		base.Awake();
		
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		
		RegisterToListeners(true);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		AssignMapSizeToInputFieldUIs();
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(changeDimensionsButtonUI != null)
			{
				changeDimensionsButtonUI.onClick.AddListener(OnChangeDimensionsButtonUIClicked);
			}

			if(cancelButtonUI != null)
			{
				cancelButtonUI.onClick.AddListener(OnCancelButtonUIClicked);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
			}
		}
		else
		{
			if(changeDimensionsButtonUI != null)
			{
				changeDimensionsButtonUI.onClick.RemoveListener(OnChangeDimensionsButtonUIClicked);
			}

			if(cancelButtonUI != null)
			{
				cancelButtonUI.onClick.RemoveListener(OnCancelButtonUIClicked);
			}

			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
				mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
			}
		}
	}

	private void OnChangeDimensionsButtonUIClicked()
	{
		SetActive(false);
		
		if(mapGenerationManager == null || widthMapDimensionInputFieldUI == null || heightMapDimensionInputFieldUI == null)
		{
			return;
		}

		var mapWidth = int.TryParse(widthMapDimensionInputFieldUI.text, out var width) ? width : 0;
		var mapHeight = int.TryParse(heightMapDimensionInputFieldUI.text, out var height) ? height : 0;

		mapGenerationManager.ChangeMapDimensionsIfNeeded(new Vector2(mapWidth, mapHeight));
	}

	private void OnCancelButtonUIClicked()
	{
		SetActive(false);
	}

	private void OnMapTilesWereAdded(List<MapTile> mapTiles)
	{
		var mapTilesToAdd = mapTiles.Select(mapTile => mapTile.GetComponent<IPrimaryWindowElement>());
		
		primaryWindowElements.AddRange(mapTilesToAdd);
	}

	private void OnMapTilesWereRemoved(List<MapTile> mapTiles)
	{
		var mapTilesToRemove = mapTiles.Select(mapTile => mapTile.GetComponent<IPrimaryWindowElement>());

		primaryWindowElements.RemoveAll(mapTilesToRemove.Contains);
	}

	private void AssignMapSizeToInputFieldUIs()
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		var mapSize = mapGenerationManager.GetMapSize();
		
		if(widthMapDimensionInputFieldUI != null)
		{
			widthMapDimensionInputFieldUI.text = mapSize.x.ToString();
		}

		if(widthMapDimensionInputFieldUI != null)
		{
			heightMapDimensionInputFieldUI.text = mapSize.y.ToString();
		}
	}
}