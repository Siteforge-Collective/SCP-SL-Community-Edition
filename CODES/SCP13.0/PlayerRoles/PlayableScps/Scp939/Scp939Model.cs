using System.Diagnostics;
using PlayerRoles.FirstPersonControl.Thirdperson;
using PlayerRoles.PlayableScps.Subroutines;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Model : AnimatedCharacterModel
	{
		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private AnimationCurve _focusOverrideAnim;

		[SerializeField]
		private AnimationCurve _tiltOverTime;

		[SerializeField]
		private AnimationCurve _focusParamsCorrectionCurve;

		[SerializeField]
		private AudioClip[] _damagedVariants;

		[SerializeField]
		private AudioClip _cloudPlaceSound;

		[SerializeField]
		private float _tiltLerp;

		[SerializeField]
		private float _fadeSpeed;

		[SerializeField]
		private Vector2 _footstepPitchRand;

		[SerializeField]
		private float _amnesiaVisibleRange;

		private Scp939Role _scp939;

		private Scp939ClawAbility _clawAbility;

		private Scp939FocusAbility _focusAbility;

		private Scp939LungeAbility _lungeAbility;

		private Scp939AmnesticCloudAbility _amnesticAbility;

		private Transform _trModel;

		private Transform _trHub;

		private bool _prevFocus;

		private bool _isLunging;

		private float _curTilt;

		private readonly Stopwatch _lungeStopwatch;

		private readonly Stopwatch _fadeoutStopwatch;

		private const int FocusOverrideLayer = 4;

		private const int FocusHeadDirLayer = 6;

		private const float FocusHeadFadeTime = 0.4f;

		private const float FocusRotateRate = 3f;

		private const float LungeRotateSpeed = 7.5f;

		private const float DamagedSoundRange = 19f;

		private const float CloudSoundRange = 8f;

		private const float HiddenHeight = -3000f;

		private const float FullVisCooldown = 30f;

		private static readonly int GroundedHash;

		private static readonly int ClawHash;

		private static readonly int FocusStateHash;

		private static readonly int FocusHeadDirHash;

		private static readonly int LungeStateHash;

		private static readonly int LungeTriggerHash;

		private static readonly int DamagedVariantHash;

		private static readonly int DamagedTriggerHash;

		private static readonly int AmnesticChargingHash;

		private static readonly int AmnesticTriggerHash;

		private bool Visible => false;

		protected override bool FootstepPlayable => false;

		protected override int WalkLayer => 0;

		private void PlayClawAttack(AttackResult attackRes)
		{
		}

		private void ProcessLungeState(Scp939LungeState newState)
		{
		}

		private void OnSpectatorTargetChanged()
		{
		}

		private void UpdateFade()
		{
		}

		private void ForceFade(float delta)
		{
		}

		protected override void Awake()
		{
		}

		protected override void Update()
		{
		}

		public void PlayDamagedEffect(int rand)
		{
		}

		public void PlayCloudRelease()
		{
		}

		public override void UpdateAnimatorParameters(Vector2 movementDirection, float normalizedVelocity, float dampTime)
		{
		}

		private void LateUpdate()
		{
		}

		protected override Animator SetupAnimator()
		{
			return null;
		}

		protected override AudioSource PlayFootstepAudioClip(AudioClip clip, float dis, float vol)
		{
			return null;
		}

		public override void SpawnObject()
		{
		}

		public override void ResetObject()
		{
		}
	}
}
