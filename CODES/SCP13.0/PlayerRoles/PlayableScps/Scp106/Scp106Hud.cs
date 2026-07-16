using System.Diagnostics;
using CustomRendering;
using PlayerRoles.PlayableScps.HUDs;
using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106Hud : ScpHudBase
	{
		private readonly Stopwatch _vigorFlashSw;

		private readonly Stopwatch _cooldownFlashSw;

		[SerializeField]
		private AbilityHud _sinkholeCooldown;

		[SerializeField]
		private Graphic _cooldownFlasher;

		[SerializeField]
		private Volume _vignetteVolume;

		[SerializeField]
		private float _maxVignette;

		[SerializeField]
		private StatSlider _vigorSlider;

		[SerializeField]
		private AbilityHud _attackCooldownElement;

		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private float _flashSpeed;

		[SerializeField]
		private float _flashDuration;

		[SerializeField]
		private GameObject _diedRoot;

		private Scp106Role _role;

		private Scp106MovementModule _fpc;

		private Scp106SinkholeController _sinkholeController;

		private Vignette _vignette;

		private ScreenDissolve _dissolve;

		private Graphic[] _vigorGraphics;

		private float _vigorIdleTime;

		private float _cooldownIdleTime;

		private readonly AbilityCooldown _attackCooldown;

		private static Scp106Hud _singleton;

		private static bool _singletonSet;

		private static float CurTime => 0f;

		private void LateUpdate()
		{
		}

		private void UpdateFlash(Graphic targetGraphic, Stopwatch sw, Color normalColor, ref float idleTime)
		{
		}

		protected override void OnDestroy()
		{
		}

		internal override void OnDied()
		{
		}

		internal override void Init(ReferenceHub hub)
		{
		}

		public static void PlayCooldownAnimation(double nextTime)
		{
		}

		public static void PlayFlash(bool vigor)
		{
		}

		public static void SetDissolveAnimation(float amt)
		{
		}
	}
}
