using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderUI : MonoBehaviour
{
	protected Slider slider;

	protected virtual void Awake()
	{
		slider = GetComponent<Slider>();
	}
}