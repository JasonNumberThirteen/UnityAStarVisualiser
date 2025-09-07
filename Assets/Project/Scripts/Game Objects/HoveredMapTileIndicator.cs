using DG.Tweening;
using UnityEngine;

public class HoveredMapTileIndicator : MonoBehaviour, IPrimaryWindowElement, IMapEditingElement
{
	[SerializeField, Min(0f)] private float animationTransitionDuration = 0.5f;

	private static readonly float ANIMATION_TRANSITION_MINIMUM_SCALE = 0.8f;
	private static readonly float ANIMATION_TRANSITION_MAXIMUM_SCALE = 0.9f;
	
	private bool indicatorWasHidden;
	private bool panelUIHoverWasDetected;
	private Tween movementTween;
	private MapTile hoveredMapTile;
	private MapTile selectedMapTile;
	private MapTile currentMapTile;
	private HoveredMapTileManager hoveredMapTileManager;
	private SelectedMapTileManager selectedMapTileManager;
	private PanelUIHoverDetectionManager panelUIHoverDetectionManager;

	public void SetPrimaryWindowElementActive(bool active)
	{
		indicatorWasHidden = !active;
		panelUIHoverWasDetected = false;

		UpdateActiveState();
	}

	public void SetMapEditingElementActive(bool active)
	{
		indicatorWasHidden = !active;
		
		UpdateActiveState();
	}

	private void Awake()
	{
		hoveredMapTileManager = ObjectMethods.FindComponentOfType<HoveredMapTileManager>();
		selectedMapTileManager = ObjectMethods.FindComponentOfType<SelectedMapTileManager>();
		panelUIHoverDetectionManager = ObjectMethods.FindComponentOfType<PanelUIHoverDetectionManager>();

		RegisterToListeners(true);
		StartMovingIfPossible();
	}

	private void OnDestroy()
	{
		RegisterToListeners(false);
		movementTween?.Kill();
	}

	private void OnEnable()
	{
		if(currentMapTile != null)
		{
			transform.position = currentMapTile.transform.position;
		}
	}

	private void RegisterToListeners(bool register)
	{
		if(register)
		{
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.AddListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.AddListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.AddListener(OnHoverDetectionStateWasChanged);
			}
		}
		else
		{
			if(hoveredMapTileManager != null)
			{
				hoveredMapTileManager.hoveredMapTileWasChangedEvent.RemoveListener(OnHoveredMapTileWasChanged);
			}

			if(selectedMapTileManager != null)
			{
				selectedMapTileManager.selectedMapTileWasChangedEvent.RemoveListener(OnSelectedMapTileWasChanged);
			}

			if(panelUIHoverDetectionManager != null)
			{
				panelUIHoverDetectionManager.hoverDetectionStateWasChangedEvent.RemoveListener(OnHoverDetectionStateWasChanged);
			}
		}
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnHoveredMapTileWasChanged(MapTile mapTile)
	{
		hoveredMapTile = mapTile;
		
		if(selectedMapTile != null)
		{
			return;
		}

		currentMapTile = hoveredMapTile;
		
		UpdateActiveState();
	}

	private void OnSelectedMapTileWasChanged(MapTile mapTile)
	{
		selectedMapTile = mapTile;
		currentMapTile = selectedMapTile == null ? hoveredMapTile : null;

		UpdateActiveState();
	}

	private void OnHoverDetectionStateWasChanged(bool detected)
	{
		panelUIHoverWasDetected = detected;
		
		UpdateActiveState();
	}

	private void UpdateActiveState()
	{
		gameObject.SetActive(!indicatorWasHidden && !panelUIHoverWasDetected && currentMapTile != null);
	}

	private void StartMovingIfPossible()
	{
		if(Mathf.Approximately(animationTransitionDuration, 0f))
		{
			return;
		}

		transform.localScale = ANIMATION_TRANSITION_MINIMUM_SCALE.ToUniformVector2();
		movementTween = transform.DOScale(ANIMATION_TRANSITION_MAXIMUM_SCALE.ToUniformVector2(), animationTransitionDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetRelative(false);
	}
}