using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Slider))]
public class SliderUI : MonoBehaviour
{
	protected Slider slider;

	public float GetValue() => slider.value;

	public void SetValue(float value)
	{
		slider.value = value;
	}

	public void SetMinValue(float value)
	{
		slider.minValue = value;
	}

	public void SetMaxValue(float value)
	{
		slider.maxValue = value;
	}

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