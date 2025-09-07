using UnityEngine;

public class UIRefresherOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		Canvas.ForceUpdateCanvases();
	}
}