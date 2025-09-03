public abstract class ListenableSliderUI : SliderUI
{
	protected abstract void OnValueWasChanged(float value);
	
	protected override void Awake()
	{
		base.Awake();
		RegisterToValueChangeListener(true);
	}

	private void OnDestroy()
	{
		RegisterToValueChangeListener(false);
	}

	private void RegisterToValueChangeListener(bool register)
	{
		if(register)
		{
			slider.onValueChanged.AddListener(OnValueWasChanged);
		}
		else
		{
			slider.onValueChanged.RemoveListener(OnValueWasChanged);
		}
	}
}