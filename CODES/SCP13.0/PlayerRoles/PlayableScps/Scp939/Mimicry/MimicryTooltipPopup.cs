using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp939.Mimicry
{
	public class MimicryTooltipPopup : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _text;

		[SerializeField]
		private RectTransform _root;

		[SerializeField]
		private RectTransform _arrowOffset;

		[SerializeField]
		private float _panning;

		private Scp939HudTranslation _prevHint;

		private LayoutElement _textLayout;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
