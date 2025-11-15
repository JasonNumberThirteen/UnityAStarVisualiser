using UnityEngine;

public class HeuristicSettingsPanelUI : PanelUI
{
	[SerializeField] private DropdownUI heuristicTypeDropdownUI;
	[SerializeField] private InputFieldUI heuristicWeightInputFieldUI;

	private MapPathManager mapPathManager;

	private void Awake()
	{
		mapPathManager = ObjectMethods.FindComponentOfType<MapPathManager>();

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
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.AddListener(OnPathfindingProcessStateWasChanged);
			}
		}
		else
		{
			if(mapPathManager != null)
			{
				mapPathManager.pathfindingProcessStateWasChangedEvent.RemoveListener(OnPathfindingProcessStateWasChanged);
			}
		}
	}

	private void OnPathfindingProcessStateWasChanged(bool started)
	{
		if(heuristicTypeDropdownUI != null)
		{
			heuristicTypeDropdownUI.SetInteractable(!started);
		}

		if(heuristicWeightInputFieldUI != null)
		{
			heuristicWeightInputFieldUI.SetInteractable(!started);
		}
	}
}