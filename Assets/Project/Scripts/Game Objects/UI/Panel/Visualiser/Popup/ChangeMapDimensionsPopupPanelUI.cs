using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapDimensionsPopupPanelUI : PopupPanelUI
{
	[SerializeField] private TMP_InputField widthMapDimensionInputFieldUI;
	[SerializeField] private TMP_InputField heightMapDimensionInputFieldUI;
	[SerializeField] private ButtonUI changeDimensionsButtonUI;
	[SerializeField] private ButtonUI cancelButtonUI;

	private MapGenerationManager mapGenerationManager;

	protected override void Awake()
	{
		base.Awake();
		
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
		
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
		if(changeDimensionsButtonUI != null)
		{
			changeDimensionsButtonUI.RegisterToClickListener(OnChangeDimensionsButtonUIClicked, register);
		}

		if(cancelButtonUI != null)
		{
			cancelButtonUI.RegisterToClickListener(OnCancelButtonUIClicked, register);
		}

		RegisterToMapGenerationListeners(register);
	}

	private void RegisterToMapGenerationListeners(bool register)
	{
		if(mapGenerationManager == null)
		{
			return;
		}
		
		if(register)
		{
			mapGenerationManager.mapTilesWereAddedEvent.AddListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.AddListener(OnMapTilesWereRemoved);
		}
		else
		{
			mapGenerationManager.mapTilesWereAddedEvent.RemoveListener(OnMapTilesWereAdded);
			mapGenerationManager.mapTilesWereRemovedEvent.RemoveListener(OnMapTilesWereRemoved);
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

		mapGenerationManager.ChangeMapDimensionsIfNeeded(new Vector2Int(mapWidth, mapHeight));
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
		var mapDimensions = mapGenerationManager != null ? mapGenerationManager.GetMapDimensions() : Vector2.zero;
		
		if(widthMapDimensionInputFieldUI != null)
		{
			widthMapDimensionInputFieldUI.text = mapDimensions.x.ToString();
		}

		if(heightMapDimensionInputFieldUI != null)
		{
			heightMapDimensionInputFieldUI.text = mapDimensions.y.ToString();
		}
	}
}