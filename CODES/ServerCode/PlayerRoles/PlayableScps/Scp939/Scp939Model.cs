namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939Model : global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Animator _animator;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _focusOverrideAnim;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _tiltOverTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _focusParamsCorrectionCurve;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _damagedVariants;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _cloudPlaceSound;

		[global::UnityEngine.SerializeField]
		private float _tiltLerp;

		[global::UnityEngine.SerializeField]
		private float _fadeSpeed;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector2 _footstepPitchRand;

		[global::UnityEngine.SerializeField]
		private float _amnesiaVisibleRange;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939Role _scp939;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939ClawAbility _clawAbility;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focusAbility;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility _lungeAbility;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility _amnesticAbility;

		private global::UnityEngine.Transform _trModel;

		private global::UnityEngine.Transform _trHub;

		private bool _prevFocus;

		private bool _isLunging;

		private float _curTilt;

		private readonly global::System.Diagnostics.Stopwatch _lungeStopwatch = new global::System.Diagnostics.Stopwatch();

		private readonly global::System.Diagnostics.Stopwatch _fadeoutStopwatch = global::System.Diagnostics.Stopwatch.StartNew();

		private const int FocusOverrideLayer = 4;

		private const int FocusHeadDirLayer = 6;

		private const float FocusHeadFadeTime = 0.4f;

		private const float FocusRotateRate = 3f;

		private const float LungeRotateSpeed = 7.5f;

		private const float DamagedSoundRange = 19f;

		private const float CloudSoundRange = 8f;

		private const float HiddenHeight = -3000f;

		private const float FullVisCooldown = 30f;

		private static readonly int GroundedHash = global::UnityEngine.Animator.StringToHash("IsGrounded");

		private static readonly int ClawHash = global::UnityEngine.Animator.StringToHash("Claw");

		private static readonly int FocusStateHash = global::UnityEngine.Animator.StringToHash("Focus");

		private static readonly int FocusHeadDirHash = global::UnityEngine.Animator.StringToHash("FocusDirection");

		private static readonly int LungeStateHash = global::UnityEngine.Animator.StringToHash("LungeState");

		private static readonly int LungeTriggerHash = global::UnityEngine.Animator.StringToHash("LungeTrigger");

		private static readonly int DamagedVariantHash = global::UnityEngine.Animator.StringToHash("DamagedVariant");

		private static readonly int DamagedTriggerHash = global::UnityEngine.Animator.StringToHash("DamagedTrigger");

		private static readonly int AmnesticChargingHash = global::UnityEngine.Animator.StringToHash("AmnesticCharging");

		private static readonly int AmnesticTriggerHash = global::UnityEngine.Animator.StringToHash("AmnesticCreated");

		private bool Visible
		{
			get
			{
				if (!global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) && !ReferenceHub.TryGetLocalHub(out hub))
				{
					return true;
				}
				if (!(hub.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
				{
					return true;
				}
				global::UnityEngine.Vector3 position = humanRole.FpcModule.Position;
				if (global::UnityEngine.Vector3.Distance(position, _scp939.FpcModule.Position) < _amnesiaVisibleRange)
				{
					return true;
				}
				if (!hub.playerEffectsController.TryGetEffect<global::CustomPlayerEffects.AmnesiaVision>(out var playerEffect))
				{
					return true;
				}
				if (playerEffect.IsEnabled)
				{
					return false;
				}
				if (playerEffect.LastActive < 30f)
				{
					return true;
				}
				foreach (global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance activeInstance in global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudInstance.ActiveInstances)
				{
					if (activeInstance.IsInArea(activeInstance.SourcePosition, position))
					{
						return false;
					}
				}
				return true;
			}
		}

		protected override bool FootstepPlayable
		{
			get
			{
				if (base.FootstepPlayable)
				{
					return base.FpcModule.CurrentMovementState == global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting;
				}
				return false;
			}
		}

		protected override int WalkLayer => 5;

		private void PlayClawAttack(global::PlayerRoles.PlayableScps.AttackResult attackRes)
		{
			if (attackRes != global::PlayerRoles.PlayableScps.AttackResult.None)
			{
				_animator.SetTrigger(ClawHash);
			}
		}

		private void ProcessLungeState(global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState newState)
		{
			switch (newState)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered:
				_lungeStopwatch.Restart();
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.None:
				_isLunging = false;
				return;
			}
			_isLunging = true;
			_animator.SetInteger(LungeStateHash, (int)newState);
			_animator.SetTrigger(LungeTriggerHash);
		}

		private void OnSpectatorTargetChanged()
		{
			ForceFade(1f);
		}

		private void UpdateFade()
		{
			ForceFade(_fadeSpeed * global::UnityEngine.Time.deltaTime);
		}

		private void ForceFade(float delta)
		{
			Fade += (Visible ? delta : (0f - delta));
			if (!(Fade > 0f) && !base.Role.IsLocalPlayer && !global::Mirror.NetworkServer.active)
			{
				_fadeoutStopwatch.Restart();
				base.FpcModule.Position = global::UnityEngine.Vector3.up * -3000f;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			_trModel = base.transform;
		}

		protected override void Update()
		{
			base.Update();
			base.Animator.SetBool(GroundedHash, base.FpcModule.IsGrounded);
			base.Animator.SetBool(AmnesticChargingHash, _amnesticAbility.TargetState);
			float state = _focusAbility.State;
			base.Animator.SetFloat(FocusStateHash, state);
			base.Animator.SetLayerWeight(6, state);
			if (_isLunging)
			{
				base.Animator.SetLayerWeight(4, 1f);
			}
			else
			{
				base.Animator.SetLayerWeight(4, _focusOverrideAnim.Evaluate(state));
			}
		}

		public void PlayDamagedEffect(int rand)
		{
			rand %= _damagedVariants.Length;
			_animator.SetFloat(DamagedVariantHash, rand);
			_animator.SetTrigger(DamagedTriggerHash);
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_damagedVariants[rand], base.transform, 19f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.NoDucking);
		}

		public void PlayCloudRelease()
		{
			_animator.SetTrigger(AmnesticTriggerHash);
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_cloudPlaceSound, base.transform, 8f);
		}

		public override void UpdateAnimatorParameters(global::UnityEngine.Vector2 movementDirection, float normalizedVelocity, float dampTime)
		{
			if (_focusAbility.State > 0f)
			{
				float b = _focusParamsCorrectionCurve.Evaluate(_focusAbility.State);
				float f = global::UnityEngine.Vector3.Dot(movementDirection.normalized, global::UnityEngine.Vector2.up);
				normalizedVelocity *= global::UnityEngine.Mathf.Lerp(1f, b, global::UnityEngine.Mathf.Abs(f));
			}
			base.UpdateAnimatorParameters(movementDirection, normalizedVelocity, dampTime);
		}

		private void LateUpdate()
		{
			float t = global::UnityEngine.Time.deltaTime * _tiltLerp;
			if (_lungeAbility.State == global::PlayerRoles.PlayableScps.Scp939.Scp939LungeState.Triggered)
			{
				double totalSeconds = _lungeStopwatch.Elapsed.TotalSeconds;
				float b = _tiltOverTime.Evaluate((float)totalSeconds);
				_curTilt = global::UnityEngine.Mathf.Lerp(_curTilt, b, t);
			}
			else
			{
				_curTilt = global::UnityEngine.Mathf.Lerp(_curTilt, 0f, t);
			}
			if (_focusAbility.State == 0f)
			{
				if (_prevFocus)
				{
					_trModel.localRotation = global::UnityEngine.Quaternion.identity;
					_prevFocus = false;
				}
				return;
			}
			if (!_prevFocus)
			{
				_prevFocus = true;
				return;
			}
			float t2;
			if (_isLunging)
			{
				double totalSeconds2 = _lungeStopwatch.Elapsed.TotalSeconds;
				t2 = 1f - (float)totalSeconds2 * 7.5f;
			}
			else
			{
				t2 = _focusAbility.State * 3f;
			}
			global::UnityEngine.Quaternion b2 = global::UnityEngine.Quaternion.Euler(0f, _focusAbility.FrozenRotation, 0f);
			_trModel.rotation = global::UnityEngine.Quaternion.Slerp(_trHub.rotation, b2, t2);
			_trModel.Rotate(global::UnityEngine.Vector3.right, _curTilt, global::UnityEngine.Space.Self);
			float value = global::UnityEngine.Mathf.DeltaAngle(_trHub.eulerAngles.y, _trModel.eulerAngles.y);
			base.Animator.SetFloat(FocusHeadDirHash, value, 0.4f, global::UnityEngine.Time.deltaTime);
		}

		protected override global::UnityEngine.Animator SetupAnimator()
		{
			return _animator;
		}

		protected override global::UnityEngine.AudioSource PlayFootstepAudioClip(global::UnityEngine.AudioClip clip, float dis, float vol)
		{
			global::UnityEngine.AudioSource audioSource = base.PlayFootstepAudioClip(clip, dis, vol);
			audioSource.pitch = global::UnityEngine.Random.Range(_footstepPitchRand.x, _footstepPitchRand.y);
			return audioSource;
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			_trHub = base.OwnerHub.transform;
			_scp939 = base.OwnerHub.roleManager.CurrentRole as global::PlayerRoles.PlayableScps.Scp939.Scp939Role;
			_scp939.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939ClawAbility>(out _clawAbility);
			_scp939.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focusAbility);
			_scp939.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility>(out _lungeAbility);
			_scp939.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility>(out _amnesticAbility);
			_clawAbility.OnAttacked += PlayClawAttack;
			_lungeAbility.OnStateChanged += ProcessLungeState;
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated += UpdateFade;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Combine(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
		}

		public override void ResetObject()
		{
			base.ResetObject();
			_clawAbility.OnAttacked -= PlayClawAttack;
			_lungeAbility.OnStateChanged -= ProcessLungeState;
			_curTilt = 0f;
			_prevFocus = false;
			_isLunging = false;
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule.OnPositionUpdated -= UpdateFade;
			global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged = (global::System.Action)global::System.Delegate.Remove(global::PlayerRoles.Spectating.SpectatorTargetTracker.OnTargetChanged, new global::System.Action(OnSpectatorTargetChanged));
		}
	}
}
