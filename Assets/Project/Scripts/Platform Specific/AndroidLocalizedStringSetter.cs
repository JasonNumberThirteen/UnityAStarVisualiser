using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(LocalizeStringEvent))]
#endif
public class AndroidLocalizedStringSetter : MonoBehaviour
{
#if UNITY_ANDROID
	[SerializeField] private LocalizedString localizedString;
	
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