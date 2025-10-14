using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
public class SliderUI : MonoBehaviour
{
	protected Slider slider;

	public float GetValue() => slider.value;

	public void RegisterToValueChangeListener(UnityAction<float> onValueWasChanged, bool register)
	{
		if(register)
		{
			slider.onValueChanged.AddListener(onValueWasChanged);
		}
		else
		{
			slider.onValueChanged.RemoveListener(onValueWasChanged);
		}
	}

	protected virtual void Awake()
	{
		slider = GetComponent<Slider>();
	}
}