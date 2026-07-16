using UnityEngine;
using UnityEngine.EventSystems;

public class SocialMediaButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public RectTransform Parent;

	private Animator _animator;

	private static readonly int _enable;

	private void Start()
	{
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}
}
