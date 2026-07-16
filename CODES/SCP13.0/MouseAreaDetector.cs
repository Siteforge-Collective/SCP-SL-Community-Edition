using UnityEngine;
using UnityEngine.UI;

public class MouseAreaDetector : MonoBehaviour
{
	public CanvasScaler SourceCanvas;

	public RectTransform TopLeftBorder;

	public RectTransform TopRightBorder;

	public RectTransform BottomLeftBorder;

	public RectTransform BottomRightBorder;

	private void Start()
	{
	}

	public bool IsInBorders()
	{
		return false;
	}
}
