using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SceneCamera : MonoBehaviour
{
	protected Camera thisCamera;
	protected MapGenerationManager mapGenerationManager;

	public bool IsOrthographic() => thisCamera.orthographic;
	public float GetOrthographicSize() => thisCamera.orthographicSize;
	public Vector3 GetPosition() => transform.position;
	public Vector3 GetScreenToWorldPointFrom(Vector3 position) => thisCamera.ScreenToWorldPoint(position);

	public void SetPosition(Vector2 position)
	{
		transform.position = position.ToVector3(transform.position.z);
	}

	public void SetOrthographicSize(float size)
	{
		thisCamera.orthographicSize = size;
	}

	protected virtual void Awake()
	{
		thisCamera = GetComponent<Camera>();
		mapGenerationManager = ObjectMethods.FindComponentOfType<MapGenerationManager>();
	}

	protected void SetPositionToCenterOfMap()
	{
		var centerOfMap = mapGenerationManager != null ? mapGenerationManager.GetCenterOfMap() : Vector2.zero;
		
		SetPosition(centerOfMap);
	}
}