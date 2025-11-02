using UnityEngine;

public class AndroidMapTileRaycaster : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private LayerMask detectableGameObjects;
	
	private MainSceneCamera mainSceneCamera;

	public bool MapTileWasTapped(Vector3 position, out MapTileStateController mapTileStateController)
	{
		var raycastHitCollider = GetRaycastHitCollider(position);
		
		mapTileStateController = raycastHitCollider != null && raycastHitCollider.TryGetComponent<MapTileStateController>(out var foundMapTileStateController) ? foundMapTileStateController : null;
		
		return mapTileStateController != null;
	}
#endif
	
	private void Awake()
	{
#if UNITY_ANDROID
		mainSceneCamera = ObjectMethods.FindComponentOfType<MainSceneCamera>();
#else
		Destroy(gameObject);
#endif
	}

#if UNITY_ANDROID
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
#endif
}