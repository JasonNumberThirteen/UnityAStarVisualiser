using System.Collections.Generic;
using UnityEngine;

public class MapTilesPathTrailManager : MonoBehaviour
{
	[SerializeField] private MapTilePathTrailIndicator mapTilePathTrailIndicatorPrefab;

	private bool pathTrailIsEnabled;
	private PathfindingManager pathfindingManager;

	private readonly List<MapTilePathTrailIndicator> mapTilePathTrailIndicators = new();

	public void SetPathTrailEnabled(bool enabled)
	{
		pathTrailIsEnabled = enabled;

		mapTilePathTrailIndicators.ForEach(mapTilePathTrailIndicator => mapTilePathTrailIndicator.SetActive(pathTrailIsEnabled));
	}

	private void Awake()
	{
		pathfindingManager = ObjectMethods.FindComponentOfType<PathfindingManager>();

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
			if(pathfindingManager != null)
			{
				pathfindingManager.pathWasFoundEvent.AddListener(OnPathWasFound);
				pathfindingManager.resultsWereClearedEvent.AddListener(OnResultsWereCleared);
			}
		}
		else
		{
			if(pathfindingManager != null)
			{
				pathfindingManager.pathWasFoundEvent.RemoveListener(OnPathWasFound);
				pathfindingManager.resultsWereClearedEvent.RemoveListener(OnResultsWereCleared);
			}
		}
	}

	private void OnPathWasFound(List<MapTileNode> mapTileNodes)
	{
		mapTileNodes.ForEachReversed(CreateMapTilePathTrailIndicator);
	}

	private void OnResultsWereCleared()
	{
		mapTilePathTrailIndicators.ForEachReversed(RemoveMapTilePathTrailIndicator);
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
		mapTilePathTrailIndicators.Add(mapTilePathTrailIndicator);
	}

	private void RemoveMapTilePathTrailIndicator(MapTilePathTrailIndicator mapTilePathTrailIndicator)
	{
		mapTilePathTrailIndicators.Remove(mapTilePathTrailIndicator);
		Destroy(mapTilePathTrailIndicator.gameObject);
	}
}