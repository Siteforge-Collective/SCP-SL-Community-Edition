using TMPro;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079.GUI
{
	public class Scp079TierGui : Scp079BarBaseGui
	{
		[SerializeField]
		private TextMeshProUGUI _textTier;

		private bool _uiDirty;

		private Scp079TierManager _tierManager;

		private string _tierFormat;

		private string _expFormat;

		private string _maxTierText;

		private string _levelUpNotification;

		private string _cachedText;

		private float _cachedFill;

		public const string NewLineFormat = "\n  - ";

		protected override string Text => null;

		protected override float FillAmount => 0f;

		internal override void Init(Scp079Role role, ReferenceHub owner)
		{
		}

		private void OnDestroy()
		{
		}

		protected override void Update()
		{
		}

		private void SetDirty()
		{
		}

		private void OnLevelledUp()
		{
		}
	}
}
