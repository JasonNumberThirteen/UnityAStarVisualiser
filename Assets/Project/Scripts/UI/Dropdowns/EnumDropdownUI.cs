using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public abstract class EnumDropdownUI<T, U> : DropdownUI where T : Component where U : Enum
{
	protected T component;

	private readonly List<LocalizedString> localizedStrings = new();

	protected virtual List<string> GetOptions() => GetLocalizationTableEntryReferenceKeys().ToList();

	protected abstract U GetInitialValue();
	protected abstract void OnEnumValueWasChanged(U @enum);
	protected abstract string GetLocalizationTableReferenceKey();
	
	protected override void Awake()
	{
		base.Awake();
		AddOptions();
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

		if(register)
		{
			localizedStrings.ForEach(localizedString => localizedString.StringChanged += OnLocalizedStringWasChanged);
		}
		else
		{
			localizedStrings.ForEach(localizedString => localizedString.StringChanged -= OnLocalizedStringWasChanged);
		}
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

	private void AddOptions()
	{
		var options = GetOptions();
		var localizationTableEntryReferenceKeys = GetLocalizationTableEntryReferenceKeys();

		options.ForEachIndexed((option, index) => localizedStrings.Add(new LocalizedString(GetLocalizationTableReferenceKey(), localizationTableEntryReferenceKeys[index])));
		dropdown.AddOptions(options);
	}

	private void OnLocalizedStringWasChanged(string value)
	{
		var localizedStringIndex = localizedStrings.IndexOf(GetLocalizedStringBy(value));

		SetOptionTextAt(localizedStringIndex, value);
		dropdown.RefreshShownValue();
	}

	private void SetOptionTextAt(int index, string value)
	{
		if(index >= 0 && index < dropdown.options.Count)
		{
			dropdown.options[index].text = value;
		}
	}

	private string[] GetLocalizationTableEntryReferenceKeys() => Enum.GetNames(typeof(U));
	private LocalizedString GetLocalizedStringBy(string value) => localizedStrings.FirstOrDefault(localizedString => localizedString.GetLocalizedString().Equals(value));
}