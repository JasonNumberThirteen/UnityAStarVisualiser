using UnityEngine;

public class MainSceneCamera : SceneCamera
{
	private MapAreaManager mapAreaManager;

	public void Translate(Vector2 translation)
	{
		transform.Translate(translation);
		ClampPositionWithinMapArea();
	}
	
	protected override void Awake()
	{
		mapAreaManager = ObjectMethods.FindComponentOfType<MapAreaManager>();
		
		base.Awake();
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
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.AddListener(SetPositionToCenterOfMap);
			}

			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.AddListener(OnMapAreaWasChanged);
			}
		}
		else
		{
			if(mapGenerationManager != null)
			{
				mapGenerationManager.mapWasGeneratedEvent.RemoveListener(SetPositionToCenterOfMap);
			}

			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.RemoveListener(OnMapAreaWasChanged);
			}
		}
	}

	private void OnMapAreaWasChanged(Rect mapArea)
	{
		SetPosition(mapArea.center);
	}
	
	private void ClampPositionWithinMapArea()
	{
		var mapArea = mapAreaManager != null ? mapAreaManager.GetMapArea() : Rect.zero;
		var currentCameraPosition = (Vector2)GetPosition();
		
		SetPosition(currentCameraPosition.GetClampedWithin(mapArea));
	}
}