using UnityEngine;
#if UNITY_STANDALONE || UNITY_WEBGL
using UnityEngine.EventSystems;
#endif

[RequireComponent(typeof(MapTileStateController))]
public class MapTileMouseEventsSender : MonoBehaviour, IPrimaryWindowElement
#if UNITY_STANDALONE || UNITY_WEBGL
, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
#endif
{
	private bool inputIsActive = true;
#if UNITY_STANDALONE || UNITY_WEBGL
	private MapTileStateController mapTileStateController;
#endif
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		inputIsActive = active;
	}

#if UNITY_STANDALONE || UNITY_WEBGL
	public void OnPointerEnter(PointerEventData eventData)
	{
		if(inputIsActive)
		{
			mapTileStateController.IsHovered = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mapTileStateController.IsHovered = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if(inputIsActive)
		{
			mapTileStateController.IsSelected = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if(mapTileStateController.IsSelected)
		{
			mapTileStateController.IsSelected = false;
		}
	}
#endif

	private void Awake()
	{
#if UNITY_STANDALONE || UNITY_WEBGL
		mapTileStateController = GetComponent<MapTileStateController>();
#endif
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

		RegisterToListeners(true);
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		inputIsActive = !detected;
	}
}