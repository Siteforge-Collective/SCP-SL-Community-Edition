using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryTooltipTarget : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler
	{
		[SerializeField]
		private Scp939HudTranslation _targetHint;

		private static MimicryTooltipTarget _curTarget;

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}

		public void OnPointerDown(PointerEventData eventData)
		{
		}

		private void OnDisable()
		{
		}

		private void Deselect()
		{
		}

		internal static bool TryGetHint(out Scp939HudTranslation hint)
		{
			hint = default(Scp939HudTranslation);
			return false;
		}
	}
}
