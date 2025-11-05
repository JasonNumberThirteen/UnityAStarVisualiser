#if UNITY_ANDROID
using System;
using System.Collections.Generic;
#endif
using UnityEngine;
using UnityEngine.Events;

public class MapTileWeightController : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	public UnityEvent<int> weightWasChangedEvent;
	
	private bool tileIsBeingDragged;
	private bool hoveringTilesIsLocked;
	private bool panelUIHoverWasDetected;
	private MapTile mapTile;
	private UserInputController userInputController;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;
#if UNITY_ANDROID
	private AndroidChangeMapTileWeightSliderUI androidChangeMapTileWeightSliderUI;
#endif

	public void SetPrimaryWindowElementActive(bool active)
	{
		hoveringTilesIsLocked = !active;
		panelUIHoverWasDetected = false;
		mapTile = null;
	}

	public void SetMapEditingElementActive(bool active)
	{
		hoveringTilesIsLocked = !active;
	}

	private void Awake()
	{
		userInputController = ObjectMethods.FindComponentOfType<UserInputController>();
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();
#if UNITY_ANDROID
		androidChangeMapTileWeightSliderUI = ObjectMethods.FindComponentOfType<AndroidChangeMapTileWeightSliderUI>();
#endif

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
#if UNITY_ANDROID
		if(androidChangeMapTileWeightSliderUI != null)
		{
			androidChangeMapTileWeightSliderUI.RegisterToValueChangeListener(OnValueWasChanged, register);
		}
#endif
		
		if(register)
		{
			if(userInputController != null)
			{
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.mouseWheelWasScrolledEvent.AddListener(OnMouseWheelWasScrolled);
#endif
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(userInputController != null)
			{
#if UNITY_STANDALONE || UNITY_WEBGL
				userInputController.mouseWheelWasScrolledEvent.RemoveListener(OnMouseWheelWasScrolled);
#endif
			}

			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}
	}

#if UNITY_ANDROID
	private void OnValueWasChanged(float value)
	{
		if(!HoveredMapTileBelongsToPassableTypes())
		{
			return;
		}

		mapTile.SetWeightTo(Mathf.RoundToInt(value));
		weightWasChangedEvent?.Invoke(mapTile.GetWeight());
	}
#endif

#if UNITY_STANDALONE || UNITY_WEBGL
	private void OnMouseWheelWasScrolled(Vector2 scrollVector)
	{
		if(!tileIsBeingDragged && !hoveringTilesIsLocked && !panelUIHoverWasDetected && HoveredMapTileBelongsToPassableTypes())
		{
			ModifyWeightOfMapTile(Mathf.RoundToInt(scrollVector.y));
		}
	}

	private void ModifyWeightOfMapTile(int weight)
	{
		if(mapTile == null || weight == 0)
		{
			return;
		}

		mapTile.ModifyWeightBy(weight);
		weightWasChangedEvent?.Invoke(mapTile.GetWeight());
	}
#endif

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		this.mapTile = mapTile;
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		tileIsBeingDragged = mapTile != null;
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
	}

	private bool HoveredMapTileBelongsToPassableTypes() => mapTile != null && mapTile.BelongsToPassableTypes();
}