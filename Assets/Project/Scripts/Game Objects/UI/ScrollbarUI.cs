using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class ScrollbarUI : MonoBehaviour
{
	private Scrollbar scrollbar;

	private void Awake()
	{
		scrollbar = GetComponent<Scrollbar>();
	}
}