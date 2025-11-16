using UnityEngine;

public abstract class Raycaster<T> : MonoBehaviour
{
	[SerializeField] private LayerMask detectableGameObjects;

	private MainSceneCamera mainSceneCamera;

	public bool ComponentWasDetected(Vector3 position, out T component)
	{
		var raycastHitCollider = GetRaycastHitCollider(position);
		
		component = raycastHitCollider != null && raycastHitCollider.TryGetComponent<T>(out var foundMapTileStateController) ? foundMapTileStateController : default;
		
		return component != null;
	}

	private void Awake()
	{
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
	}

	private Collider2D GetRaycastHitCollider(Vector3 position)
	{
		if(mainSceneCamera == null)
		{
			return null;
		}
		
		var screenToWorldPosition = mainSceneCamera.GetScreenToWorldPointFrom(position);
		var raycastHit = Physics2D.Raycast(screenToWorldPosition, Vector2.zero, Mathf.Infinity, detectableGameObjects);

		return raycastHit.collider;
	}
}