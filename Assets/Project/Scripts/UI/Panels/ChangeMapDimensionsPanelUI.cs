using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMapDimensionsPanelUI : PanelUI
{
	[SerializeField] private TMP_InputField mapWidthInputFieldUI;
	[SerializeField] private TMP_InputField mapHeightInputFieldUI;
	
	[SerializeField] private Button changeDimensionsButtonUI;
	[SerializeField] private Button cancelButtonUI;
	
	private readonly List<IPrimaryWindowElement> primaryWindowElements = new();

	private MapGenerationManager mapGenerationManager;

	private void Awake()
	{
		mapGenerationManager = FindFirstObjectByType<MapGenerationManager>();
		
		primaryWindowElements.AddRange(FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IPrimaryWindowElement>());
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
		
		if(mapGenerationManager == null || mapWidthInputFieldUI == null || mapHeightInputFieldUI == null)
		{
			return;
		}

		var mapWidth = int.TryParse(mapWidthInputFieldUI.text, out var width) ? width : 0;
		var mapHeight = int.TryParse(mapHeightInputFieldUI.text, out var height) ? height : 0;

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

	private void OnEnable()
	{
		primaryWindowElements.ForEach(primaryWindowElement => primaryWindowElement.SetPrimaryWindowElementActive(false));
		AssignMapSizeToInputFieldUIs();
	}

	private void OnDisable()
	{
		primaryWindowElements.ForEach(primaryWindowElement => primaryWindowElement.SetPrimaryWindowElementActive(true));
	}

	private void AssignMapSizeToInputFieldUIs()
	{
		if(mapGenerationManager == null)
		{
			return;
		}

		var mapSize = mapGenerationManager.GetMapSize();
		
		if(mapWidthInputFieldUI != null)
		{
			mapWidthInputFieldUI.text = mapSize.x.ToString();
		}

		if(mapWidthInputFieldUI != null)
		{
			mapHeightInputFieldUI.text = mapSize.y.ToString();
		}
	}
}