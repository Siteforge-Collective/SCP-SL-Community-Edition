using System.Text;
using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079AuxGui : Scp079BarBaseGui
	{
		[SerializeField]
		private TextMeshProUGUI _textBlockers;

		[SerializeField]
		private float _blockerHeaderSize;

		private Scp079AuxManager _auxManager;

		private StringBuilder _sb;

		private string _reducedHeader;

		private string _suspendedHeader;

		private const string Format = "{0} / {1}";

		protected override string Text => null;

		protected override float FillAmount => 0f;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private string BuildHeader(Scp079HudTranslation header)
		{
			return null;
		}

		protected override void Update()
		{
		}
	}
}
