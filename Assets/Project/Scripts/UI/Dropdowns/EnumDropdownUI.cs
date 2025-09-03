using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnumDropdownUI<T, U> : DropdownUI where T : Component where U : Enum
{
	protected T component;

	protected virtual List<string> GetOptions() => Enum.GetNames(typeof(U)).ToList();

	protected abstract U GetInitialValue();
	protected abstract void OnEnumValueWasChanged(U @enum);
	
	protected override void Awake()
	{
		base.Awake();
		dropdown.AddOptions(GetOptions());
		RegisterToListeners(true);

		component = ObjectMethods.FindComponentOfType<T>();
		dropdown.value = GetInitialValue().ToInt();
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		RegisterToValueChangeListener(OnValueWasChanged, register);
	}

	private void OnValueWasChanged(int value)
	{
		OnEnumValueWasChanged(value.ToEnumValue<U>());
	}

	private void RegisterToValueChangeListener(UnityAction<int> action, bool register)
	{
		if(register)
		{
			dropdown.onValueChanged.AddListener(action);
		}
		else
		{
			dropdown.onValueChanged.RemoveListener(action);
		}
	}
}