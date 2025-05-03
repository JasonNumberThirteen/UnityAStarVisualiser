using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PanelUI : MonoBehaviour
{
	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}
}