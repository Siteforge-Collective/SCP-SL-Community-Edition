namespace PlayerRoles.PlayableScps.Scp096
{
	public class Scp096Hud : global::PlayerRoles.PlayableScps.HUDs.ViewmodelScpHud
	{
		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.HUDs.AbilityHud _rageDuration;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.HUDs.AbilityHud _chargeCooldown;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image[] _keyCircles;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Image _rageEnterSustainCircle;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _docileCircles;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.HUDs.ScpWarningHud _rageInfo;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume _rageVolume;

		[global::UnityEngine.SerializeField]
		private float _rageVolumeDelta;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096Role _scp096;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096RageCycleAbility _rageCycle;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager _rageManager;

		private global::PlayerRoles.PlayableScps.Scp096.Scp096ChargeAbility _chargeAbility;

		internal override void Init(ReferenceHub hub)
		{
			base.Init(hub);
			_scp096 = hub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp096.Scp096Role;
			_scp096.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096RageCycleAbility>(out _rageCycle);
			_scp096.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096RageManager>(out _rageManager);
			_scp096.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp096.Scp096ChargeAbility>(out _chargeAbility);
			_rageDuration.Setup(_rageManager.HudRageDuration, null);
			_chargeCooldown.Setup(_chargeAbility.Cooldown, _chargeAbility.Duration);
			_rageInfo.gameObject.SetActive(hub.isLocalPlayer);
			UpdateColorGrading(1f);
		}

		protected override void Update()
		{
			base.Update();
			UpdateRageInfo();
			UpdateColorGrading(_rageVolumeDelta * global::UnityEngine.Time.deltaTime);
		}

		private void UpdateColorGrading(float maxDelta)
		{
			_rageVolume.weight = global::UnityEngine.Mathf.MoveTowards(_rageVolume.weight, _rageManager.IsEnragedOrDistressed ? 1 : 0, maxDelta);
		}

		private void UpdateRageInfo()
		{
			_keyCircles.ForEach(delegate(global::UnityEngine.UI.Image x)
			{
				x.fillAmount = _rageCycle.HudEnterRageKeyProgress;
			});
			switch (_scp096.StateController.RageState)
			{
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Docile:
			{
				float hudEnterRageSustain = _rageCycle.HudEnterRageSustain;
				_docileCircles.SetActive(hudEnterRageSustain > 0f);
				_rageEnterSustainCircle.fillAmount = hudEnterRageSustain;
				_rageDuration.Update(forceHidden: true);
				_chargeCooldown.Update(forceHidden: true);
				if (!(hudEnterRageSustain <= 0f))
				{
					SetWarning(global::PlayerRoles.PlayableScps.Scp096.Scp096HudTranslation.EnterRageKeyInfo, ActionName.Reload, hudEnterRageSustain);
				}
				break;
			}
			case global::PlayerRoles.PlayableScps.Scp096.Scp096RageState.Enraged:
				_rageDuration.Update();
				_chargeCooldown.Update();
				_docileCircles.SetActive(value: false);
				SetWarning(global::PlayerRoles.PlayableScps.Scp096.Scp096HudTranslation.ExitRageKeyInfo, ActionName.Reload);
				break;
			default:
				_rageDuration.Update(forceHidden: true);
				_chargeCooldown.Update(forceHidden: true);
				_docileCircles.SetActive(value: false);
				_rageInfo.SetText(string.Empty);
				break;
			}
		}

		private void SetWarning(global::PlayerRoles.PlayableScps.Scp096.Scp096HudTranslation key, ActionName action, float duration = 3.8f)
		{
			if (!Translations.TryGet(key, out var tr))
			{
				tr = "Hold {0} to " + key;
			}
			_rageInfo.SetText(string.Format(tr, $"<color=red>{new ReadableKeyCode(action)}</color>"), duration);
		}
	}
}
