using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanelUI : PanelUI
{
	protected readonly List<IPrimaryWindowElement> primaryWindowElements = new();

	protected virtual void Awake()
	{
		primaryWindowElements.AddRange(ObjectMethods.FindComponentsOfType<MonoBehaviour>().OfType<IPrimaryWindowElement>());
	}

	protected virtual void OnEnable()
	{
		primaryWindowElements.ForEach(primaryWindowElement => primaryWindowElement.SetPrimaryWindowElementActive(false));
	}

	protected virtual void OnDisable()
	{
		primaryWindowElements.ForEach(primaryWindowElement => primaryWindowElement.SetPrimaryWindowElementActive(true));
	}
}