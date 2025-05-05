using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapTileRenderer : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;

	public Bounds GetBounds() => spriteRenderer.bounds;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
}