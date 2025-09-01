using System.Collections.Generic;
using UnityEngine;

public class PathTrailManager : MonoBehaviour
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
		if(mapTilePathTrailIndicatorPrefab == null)
		{
			return;
		}

		for (var i = mapTileNodes.Count - 1; i >= 1; --i)
		{
			var currentMapTileNode = mapTileNodes[i];
			var mapTilePathTrailIndicator = Instantiate(mapTilePathTrailIndicatorPrefab, currentMapTileNode.transform.position, Quaternion.identity);
			
			mapTilePathTrailIndicator.Setup(currentMapTileNode, mapTileNodes[i - 1]);
			mapTilePathTrailIndicator.SetActive(pathTrailIsEnabled);
			mapTilePathTrailIndicators.Add(mapTilePathTrailIndicator);
		}
	}

	private void OnResultsWereCleared()
	{
		for (var i = mapTilePathTrailIndicators.Count - 1; i >= 0; --i)
		{
			var mapTilePathTrailIndicator = mapTilePathTrailIndicators[i];

			mapTilePathTrailIndicators.Remove(mapTilePathTrailIndicator);
			Destroy(mapTilePathTrailIndicator.gameObject);
		}
	}
}