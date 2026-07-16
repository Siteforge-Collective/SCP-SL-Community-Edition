using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public abstract class Scp079BarBaseGui : Scp079GuiElementBase
	{
		[SerializeField]
		private Image _slider;

		[SerializeField]
		private TextMeshProUGUI _textNormal;

		[SerializeField]
		private TextMeshProUGUI _textInverted;

		private RectMask2D _rectMask;

		private float _width;

		protected abstract string Text { get; }

		protected abstract float FillAmount { get; }

		private void Awake()
		{
		}

		protected virtual void Update()
		{
		}
	}
}
