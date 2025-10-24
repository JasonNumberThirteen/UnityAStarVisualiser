using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(LocalizeStringEvent))]
public class AndroidLocalizedStringSetter : MonoBehaviour
{
	[SerializeField] private LocalizedString localizedString;
	
	private LocalizeStringEvent localizeStringEvent;

	private void Awake()
	{
		localizeStringEvent = GetComponent<LocalizeStringEvent>();

#if UNITY_ANDROID
		if(localizedString != null)
		{
			localizeStringEvent.StringReference = localizedString;
		}
#endif
	}
}