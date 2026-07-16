using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
	[SerializeField]
	private bool _playHover;

	[SerializeField]
	private bool _playClick;

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerClick(PointerEventData eventData)
	{
	}
}
