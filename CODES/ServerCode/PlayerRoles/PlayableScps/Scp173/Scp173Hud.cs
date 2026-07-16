namespace PlayerRoles.PlayableScps.Scp173
{
	public class Scp173Hud : global::PlayerRoles.PlayableScps.HUDs.ScpHudBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _hudAnimator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _eyeIndicator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _tantrumCooldown;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _breakneckSpeedsCooldown;

		[global::UnityEngine.SerializeField]
		private global::TMPro.TextMeshProUGUI _timer;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _bloodshotEye;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite _openEye;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker _observersTracker;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BlinkTimer _blinkAbility;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173TantrumAbility _tantrumAbility;

		private global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility _breakneckSpeedsAbility;

		private const float RotateSpeedFirst = 0f;

		private const float RotateSpeedLast = 0f;

		private static readonly int AnimatorHudShownHash = global::UnityEngine.Animator.StringToHash("Shown");

		private static readonly int AnimatorHudReadyHash = global::UnityEngine.Animator.StringToHash("Ready");

		internal override void Init(ReferenceHub hub)
		{
			base.Init(hub);
			if (hub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp173.Scp173Role scp173Role)
			{
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173ObserversTracker>(out _observersTracker);
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BlinkTimer>(out _blinkAbility);
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173TantrumAbility>(out _tantrumAbility);
				scp173Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp173.Scp173BreakneckSpeedsAbility>(out _breakneckSpeedsAbility);
			}
		}

		protected override void Update()
		{
			base.Update();
			bool isObserved = _observersTracker.IsObserved;
			float remainingSustainPercent = _blinkAbility.RemainingSustainPercent;
			float remainingBlinkCooldown = _blinkAbility.RemainingBlinkCooldown;
			bool flag = isObserved || remainingSustainPercent > 0f;
			_hudAnimator.SetBool(AnimatorHudShownHash, flag);
			_hudAnimator.SetBool(AnimatorHudReadyHash, remainingBlinkCooldown <= 0f);
			_eyeIndicator.fillAmount = remainingSustainPercent;
			_timer.text = (flag ? $"{remainingBlinkCooldown:F1}s" : string.Empty);
			_timer.color = global::UnityEngine.Color.Lerp(global::UnityEngine.Color.clear, global::UnityEngine.Color.white, remainingSustainPercent);
			UpdateCooldown(_tantrumCooldown, _tantrumAbility.Cooldown);
			UpdateCooldown(_breakneckSpeedsCooldown, _breakneckSpeedsAbility.Cooldown);
			_eyeIndicator.sprite = ((remainingBlinkCooldown <= 0f) ? _bloodshotEye : _openEye);
		}

		private void UpdateCooldown(global::UnityEngine.UI.Image target, global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown cooldown)
		{
			global::UnityEngine.GameObject gameObject = target.transform.parent.gameObject;
			if (cooldown.IsReady)
			{
				gameObject.SetActive(value: false);
				return;
			}
			float readiness = cooldown.Readiness;
			float num = global::UnityEngine.Mathf.Lerp(0f, 0f, readiness);
			target.transform.Rotate(num * global::UnityEngine.Time.deltaTime * global::UnityEngine.Vector3.back);
			target.fillAmount = readiness;
			gameObject.SetActive(value: true);
		}
	}
}
