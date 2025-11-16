using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapTilesPathTrailManager : MonoBehaviour
{
	public UnityEvent<List<MapTilePathTrailIndicator>> indicatorsWereAddedEvent;
	public UnityEvent<List<MapTilePathTrailIndicator>> indicatorsWereRemovedEvent;
	
	[SerializeField] private MapTilePathTrailIndicator mapTilePathTrailIndicatorPrefab;

	private bool pathTrailIsEnabled;
	private MapPathManager mapPathManager;

	private readonly List<MapTilePathTrailIndicator> allMapTilePathTrailIndicators = new();
	private readonly List<MapTilePathTrailIndicator> modifiedMapTilePathTrailIndicators = new();

	public void SetPathTrailEnabled(bool enabled)
	{
		pathTrailIsEnabled = enabled;

		allMapTilePathTrailIndicators.ForEach(mapTilePathTrailIndicator => mapTilePathTrailIndicator.SetActive(pathTrailIsEnabled));
	}

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(true);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathWasFoundEvent.AddListener(OnPathWasFound);
				mapPathManager.resultsWereClearedEvent.AddListener(OnResultsWereCleared);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathWasFoundEvent.RemoveListener(OnPathWasFound);
				mapPathManager.resultsWereClearedEvent.RemoveListener(OnResultsWereCleared);
			}
		}
	}

	private void OnPathWasFound(List<MapTileNode> mapTileNodes)
	{
		modifiedMapTilePathTrailIndicators.Clear();
		mapTileNodes.ForEachReversed(CreateMapTilePathTrailIndicator);
		indicatorsWereAddedEvent?.Invoke(modifiedMapTilePathTrailIndicators);
	}

	private void OnResultsWereCleared()
	{
		modifiedMapTilePathTrailIndicators.Clear();
		allMapTilePathTrailIndicators.ForEachReversed(RemoveMapTilePathTrailIndicator);
		indicatorsWereRemovedEvent?.Invoke(modifiedMapTilePathTrailIndicators);
	}

	private void CreateMapTilePathTrailIndicator(MapTileNode currentMapTileNode, MapTileNode nextMapTileNode)
	{
		if(mapTilePathTrailIndicatorPrefab == null || currentMapTileNode == null || nextMapTileNode == null)
		{
			return;
		}
		
		var mapTilePathTrailIndicator = Instantiate(mapTilePathTrailIndicatorPrefab, currentMapTileNode.GetPosition(), Quaternion.identity);
		
		mapTilePathTrailIndicator.Setup(currentMapTileNode, nextMapTileNode);
		mapTilePathTrailIndicator.SetActive(pathTrailIsEnabled);
		AddIndicator(mapTilePathTrailIndicator);
	}

	private void RemoveMapTilePathTrailIndicator(MapTilePathTrailIndicator mapTilePathTrailIndicator)
	{
		RemoveIndicator(mapTilePathTrailIndicator);
		Destroy(mapTilePathTrailIndicator.gameObject);
	}

	private void AddIndicator(MapTilePathTrailIndicator mapTilePathTrailIndicator)
	{
		allMapTilePathTrailIndicators.Add(mapTilePathTrailIndicator);
		modifiedMapTilePathTrailIndicators.Add(mapTilePathTrailIndicator);
	}

	private void RemoveIndicator(MapTilePathTrailIndicator mapTilePathTrailIndicator)
	{
		allMapTilePathTrailIndicators.Remove(mapTilePathTrailIndicator);
		modifiedMapTilePathTrailIndicators.Add(mapTilePathTrailIndicator);
	}
}