namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Hud : global::PlayerRoles.PlayableScps.HUDs.ScpHudBase
	{
		private readonly global::System.Diagnostics.Stopwatch _vigorFlashSw = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _cooldownFlashSw = new global::System.Diagnostics.Stopwatch();

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.HUDs.AbilityHud _sinkholeCooldown;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Graphic _cooldownFlasher;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Rendering.PostProcessing.PostProcessVolume _vignetteVolume;

		[global::UnityEngine.SerializeField]
		private float _maxVignette;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Slider _vigorSlider;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _vigorPercent;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.HUDs.AbilityHud _attackCooldownElement;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _normalColor;

		[global::UnityEngine.SerializeField]
		private float _flashSpeed;

		[global::UnityEngine.SerializeField]
		private float _flashDuration;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Graphic[] _vigorGraphics;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _diedRoot;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106Role _role;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106MovementModule _fpc;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor _vigor;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController _sinkholeController;

		private global::UnityEngine.Rendering.PostProcessing.Vignette _vignette;

		private global::PostProcessing.ScreenDissolve _dissolve;

		private float _vigorIdleTime;

		private float _cooldownIdleTime;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _attackCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private static global::PlayerRoles.PlayableScps.Scp106.Scp106Hud _singleton;

		private static bool _singletonSet;

		private static float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		private void LateUpdate()
		{
			_vignette.intensity.value = _fpc.CurSlowdown * _maxVignette;
			_vigorSlider.value = _vigor.DisplayedVigor;
			_vigorPercent.text = global::UnityEngine.Mathf.FloorToInt(_vigor.VigorAmount * 100f) + "%";
			_sinkholeCooldown.Update();
			_attackCooldownElement.Update();
			UpdateFlash(_cooldownFlasher, _cooldownFlashSw, global::UnityEngine.Color.white, ref _cooldownIdleTime);
			_vigorGraphics.ForEach(delegate(global::UnityEngine.UI.Graphic x)
			{
				UpdateFlash(x, _vigorFlashSw, _normalColor, ref _vigorIdleTime);
			});
		}

		private void UpdateFlash(global::UnityEngine.UI.Graphic targetGraphic, global::System.Diagnostics.Stopwatch sw, global::UnityEngine.Color normalColor, ref float idleTime)
		{
			global::UnityEngine.Color color;
			if (sw.IsRunning && sw.Elapsed.TotalSeconds < (double)_flashDuration)
			{
				float f = global::UnityEngine.Mathf.Sin((CurTime - idleTime) * _flashSpeed * (float)global::System.Math.PI);
				color = global::UnityEngine.Color.Lerp(normalColor, global::UnityEngine.Color.red, global::UnityEngine.Mathf.Abs(f));
			}
			else
			{
				color = global::UnityEngine.Color.Lerp(targetGraphic.color, normalColor, global::UnityEngine.Time.deltaTime * _flashSpeed);
				idleTime = CurTime;
			}
			targetGraphic.color = new global::UnityEngine.Color(color.r, color.g, color.b, targetGraphic.color.a);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (!(this != _singleton))
			{
				_singletonSet = false;
			}
		}

		internal override void OnDied()
		{
			base.enabled = false;
			_diedRoot.SetActive(value: false);
		}

		internal override void Init(ReferenceHub hub)
		{
			base.Init(hub);
			_role = hub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp106.Scp106Role;
			_fpc = _role.FpcModule as global::PlayerRoles.PlayableScps.Scp106.Scp106MovementModule;
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106Vigor>(out _vigor);
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp106.Scp106SinkholeController>(out _sinkholeController);
			global::UnityEngine.Rendering.PostProcessing.PostProcessProfile profile = _vignetteVolume.profile;
			_vignette = profile.GetSetting<global::UnityEngine.Rendering.PostProcessing.Vignette>();
			_dissolve = profile.GetSetting<global::PostProcessing.ScreenDissolve>();
			_attackCooldownElement.Setup(_attackCooldown, null);
			_sinkholeCooldown.Setup(_sinkholeController.Cooldown, null);
			_singleton = this;
			_singletonSet = true;
		}

		public static void PlayCooldownAnimation(double nextTime)
		{
			if (_singletonSet)
			{
				float cooldown = (float)(nextTime - global::Mirror.NetworkTime.time);
				_singleton._attackCooldown.Trigger(cooldown);
			}
		}

		public static void PlayFlash(bool vigor)
		{
			if (_singletonSet)
			{
				(vigor ? _singleton._vigorFlashSw : _singleton._cooldownFlashSw).Restart();
			}
		}

		public static void SetDissolveAnimation(float amt)
		{
			if (_singletonSet)
			{
				_singleton._dissolve.DissolveAmount.value = global::UnityEngine.Mathf.Clamp01(amt);
			}
		}
	}
}
