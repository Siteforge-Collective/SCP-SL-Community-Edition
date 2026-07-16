using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.Subroutines;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173Hud : ScpHudBase
	{
		[SerializeField]
		private Animator _hudAnimator;

		[SerializeField]
		private Image _eyeIndicator;

		[SerializeField]
		private Image _tantrumCooldown;

		[SerializeField]
		private Image _breakneckSpeedsCooldown;

		[SerializeField]
		private TextMeshProUGUI _timer;

		[SerializeField]
		private Sprite _bloodshotEye;

		[SerializeField]
		private Sprite _openEye;

		private Scp173ObserversTracker _observersTracker;

		private Scp173BlinkTimer _blinkAbility;

		private Scp173TantrumAbility _tantrumAbility;

		private Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private const float RotateSpeedFirst = 0f;

		private const float RotateSpeedLast = 0f;

		private static readonly int AnimatorHudShownHash;

		private static readonly int AnimatorHudReadyHash;

		internal override void Init(ReferenceHub hub)
		{
		}

		protected override void Update()
		{
		}

		private void UpdateCooldown(Image target, AbilityCooldown cooldown)
		{
		}
	}
}
