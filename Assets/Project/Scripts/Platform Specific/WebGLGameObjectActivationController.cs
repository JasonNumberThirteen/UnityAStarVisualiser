using UnityEngine;

[DefaultExecutionOrder(-100)]
public class WebGLGameObjectActivationController : MonoBehaviour
{
	[SerializeField] private bool activateIfWebGLIsSelected;

	private void Awake()
	{
		gameObject.SetActive(ShouldActivateGO());
	}

	private bool ShouldActivateGO()
	{
#if UNITY_WEBGL
		return activateIfWebGLIsSelected;
#else
		return !activateIfWebGLIsSelected;
#endif
	}
}