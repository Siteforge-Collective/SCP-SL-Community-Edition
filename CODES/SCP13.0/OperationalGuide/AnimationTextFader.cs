using UnityEngine;
using UnityEngine.EventSystems;

namespace OperationalGuide
{
	public class AnimationTextFader : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		public Animator Animator;

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
