using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(LocalizeStringEvent))]
public class AndroidLocalizedStringSetter : MonoBehaviour
{
	[SerializeField] private LocalizedString localizedString;

#if UNITY_ANDROID
	private LocalizeStringEvent localizeStringEvent;
#endif	

	private void Awake()
	{
#if UNITY_ANDROID
		localizeStringEvent = GetComponent<LocalizeStringEvent>();
		
		if(localizedString != null)
		{
			localizeStringEvent.StringReference = localizedString;
		}
#else
		Destroy(this);
#endif
	}
}