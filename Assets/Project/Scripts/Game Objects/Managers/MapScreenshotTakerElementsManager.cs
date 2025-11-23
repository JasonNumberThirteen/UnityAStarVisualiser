using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapScreenshotTakerElementsManager : MonoBehaviour
{
	private MapScreenshotTaker mapScreenshotTaker;
	private MapTilesPathTrailManager mapTilesPathTrailManager;

	private readonly List<IMapScreenshotTakerElement> mapScreenshotTakerElements = new();

	private void Awake()
	{
		mapScreenshotTaker = ObjectMethods.FindComponentOfType<MapScreenshotTaker>();
		mapTilesPathTrailManager = ObjectMethods.FindComponentOfType<MapTilesPathTrailManager>();

		mapScreenshotTakerElements.AddRange(ObjectMethods.FindInterfacesOfType<IMapScreenshotTakerElement>());
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
			if(mapScreenshotTaker != null)
			{
				mapScreenshotTaker.takingScreenshotStateWasChangedEvent.AddListener(AdjustAllElementsForTakingMapScreenshot);
			}
			
			if(mapTilesPathTrailManager != null)
			{
				mapTilesPathTrailManager.indicatorsWereAddedEvent.AddListener(OnIndicatorsWereAdded);
				mapTilesPathTrailManager.indicatorsWereRemovedEvent.AddListener(OnIndicatorsWereRemoved);
			}
		}
		else
		{
			if(mapScreenshotTaker != null)
			{
				mapScreenshotTaker.takingScreenshotStateWasChangedEvent.RemoveListener(AdjustAllElementsForTakingMapScreenshot);
			}
			
			if(mapTilesPathTrailManager != null)
			{
				mapTilesPathTrailManager.indicatorsWereAddedEvent.RemoveListener(OnIndicatorsWereAdded);
				mapTilesPathTrailManager.indicatorsWereRemovedEvent.RemoveListener(OnIndicatorsWereRemoved);
			}
		}
	}

	private void AdjustAllElementsForTakingMapScreenshot(bool started)
	{
		mapScreenshotTakerElements.ForEach(mapScreenshotTakerElement => mapScreenshotTakerElement.AdjustForTakingMapScreenshot(started));
	}

	private void OnIndicatorsWereAdded(List<MapTilePathTrailIndicator> mapTilePathTrailIndicators)
	{
		mapScreenshotTakerElements.AddRange(GetElementsFrom(mapTilePathTrailIndicators));
	}

	private void OnIndicatorsWereRemoved(List<MapTilePathTrailIndicator> mapTilePathTrailIndicators)
	{
		var elementsFromMapTilePathTrailIndicators = GetElementsFrom(mapTilePathTrailIndicators);

		mapScreenshotTakerElements.RemoveAll(elementsFromMapTilePathTrailIndicators.Contains);
	}

	private IEnumerable<IMapScreenshotTakerElement> GetElementsFrom<T>(List<T> components) => components.OfType<IMapScreenshotTakerElement>();
}