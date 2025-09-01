using UnityEngine;

public class MainCameraMovementController : MonoBehaviour, IPrimaryWindowElement
{
	[SerializeField, Min(0f)] private float movementSpeed = 10f;
	
	private bool inputIsActive = true;
	private Camera mainCamera;
	private Vector2 movementDirection;
	private MapAreaManager mapAreaManager;
	private UserInputController userInputController;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;

		if(!inputIsActive && movementDirection != Vector2.zero)
		{
			movementDirection = Vector2.zero;
		}
	}

	private void Awake()
	{
		mainCamera = Camera.main;
		mapAreaManager = FindFirstObjectByType<MapAreaManager>();
		userInputController = FindFirstObjectByType<UserInputController>();

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
			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.AddListener(OnMapAreaWasChanged);
			}
			
			if(userInputController != null)
			{
				userInputController.movementKeyWasPressedEvent.AddListener(OnMovementKeyWasPressed);
			}
		}
		else
		{
			if(mapAreaManager != null)
			{
				mapAreaManager.mapAreaWasChangedEvent.RemoveListener(OnMapAreaWasChanged);
			}
			
			if(userInputController != null)
			{
				userInputController.movementKeyWasPressedEvent.RemoveListener(OnMovementKeyWasPressed);
			}
		}
	}

	private void OnMapAreaWasChanged(Rect mapArea)
	{
		var cameraPosition = mainCamera.gameObject.transform.position;

		cameraPosition.x = mapArea.center.x;
		cameraPosition.y = mapArea.center.y;
		mainCamera.gameObject.transform.position = cameraPosition;
	}

	private void OnMovementKeyWasPressed(Vector2 movementVector)
	{
		if(inputIsActive)
		{
			movementDirection = movementVector;
		}
	}

	private void LateUpdate()
	{
		if(mainCamera == null || movementDirection == Vector2.zero)
		{
			return;
		}

		mainCamera.gameObject.transform.Translate(movementSpeed*Time.deltaTime*movementDirection);
		ClampCameraPositionIfNeeded();
	}

	private void ClampCameraPositionIfNeeded()
	{
		if(mapAreaManager == null)
		{
			return;
		}

		var mapArea = mapAreaManager.GetMapArea();
		var clampedCameraPosition = mainCamera.transform.position;

		clampedCameraPosition.x = Mathf.Clamp(clampedCameraPosition.x, mapArea.x, mapArea.width);
		clampedCameraPosition.y = Mathf.Clamp(clampedCameraPosition.y, mapArea.y, mapArea.height);
		mainCamera.transform.position = clampedCameraPosition;
	}
}