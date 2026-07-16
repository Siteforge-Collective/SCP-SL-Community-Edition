namespace PlayerRoles.FirstPersonControl.Thirdperson
{
	public class AnimatedCharacterModel : global::PlayerRoles.FirstPersonControl.Thirdperson.CharacterModel, global::GameObjectPools.IPoolResettable, global::GameObjectPools.IPoolSpawnable
	{
		private enum FootstepLoudness
		{
			Civilian = 8,
			FoundationForces = 12,
			Chaos = 30,
			Scp = 35
		}

		public static global::System.Action<global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel, float> OnFootstepPlayed;

		private static readonly int HashForward = global::UnityEngine.Animator.StringToHash("Forward");

		private static readonly int HashStrafe = global::UnityEngine.Animator.StringToHash("Strafe");

		private static readonly int HashSpeed = global::UnityEngine.Animator.StringToHash("Speed");

		private readonly global::System.Diagnostics.Stopwatch _lastTouchdownSw = global::System.Diagnostics.Stopwatch.StartNew();

		private int _lastFootstep;

		private bool _forceUpdate;

		private const float SilentVelocityMultiplier = 0.7f;

		private const float SprintingLoudnessMultiplier = 2f;

		private const float MinimalFootstepSoundCooldown = 0.2f;

		private const float SpawnGroundedSuppression = 0.3f;

		[global::UnityEngine.Header("Animation settings")]
		[global::UnityEngine.SerializeField]
		private float _firstpersonDampTime;

		[global::UnityEngine.SerializeField]
		private float _thirdpersonDampTime;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _walkVelocityScale;

		[global::UnityEngine.Header("Footsteps")]
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip[] _footstepClips;

		[global::UnityEngine.Range(0f, 1f)]
		[global::UnityEngine.SerializeField]
		private float[] _footstepTimes;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.FirstPersonControl.Thirdperson.AnimatedCharacterModel.FootstepLoudness _footstepLoudness;

		public global::PlayerRoles.FirstPersonControl.Thirdperson.ModelSharedSettings SharedSettings;

		public global::UnityEngine.AudioClip RandomFootstep => _footstepClips.RandomItem();

		public global::UnityEngine.Vector3 HeadBobPosition { get; protected set; }

		public bool IsTracked
		{
			get
			{
				if (!base.Pooled)
				{
					if (!base.OwnerHub.isLocalPlayer)
					{
						return global::PlayerRoles.Spectating.SpectatorNetworking.IsLocallySpectated(base.OwnerHub);
					}
					return true;
				}
				return false;
			}
		}

		internal global::UnityEngine.Animator Animator { get; private set; }

		internal global::UnityEngine.AnimatorOverrideController AnimatorOverride { get; private set; }

		protected global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule FpcModule { get; private set; }

		protected global::PlayerRoles.PlayerRoleBase Role { get; private set; }

		protected virtual float FootstepLoudnessDistance
		{
			get
			{
				float num = (float)_footstepLoudness;
				if (FpcModule.CurrentMovementState == global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting)
				{
					num *= 2f;
				}
				return num;
			}
		}

		protected virtual bool FootstepPlayable
		{
			get
			{
				if (!FpcModule.IsGrounded || !FpcModule.Motor.MovementDetected)
				{
					return false;
				}
				float num = FpcModule.VelocityForState(global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sneaking, applyCrouch: false);
				if (FpcModule.MaxMovementSpeed <= num)
				{
					return false;
				}
				num *= 0.7f;
				return FpcModule.Motor.Velocity.SqrMagnitudeIgnoreY() >= num * num;
			}
		}

		protected virtual int WalkLayer => 0;

		protected virtual bool LandingFootstepPlayable => true;

		protected override void Awake()
		{
			base.Awake();
			Animator = SetupAnimator();
		}

		protected virtual void Update()
		{
			if (!base.Pooled)
			{
				float dampTime = (base.OwnerHub.isLocalPlayer ? _firstpersonDampTime : _thirdpersonDampTime);
				float num = Animator.GetCurrentAnimatorStateInfo(WalkLayer).normalizedTime;
				if (float.IsNaN(num))
				{
					num = 0f;
				}
				global::UnityEngine.Vector2 movementDirection;
				float normalizedVelocity;
				if (!FpcModule.IsGrounded)
				{
					movementDirection = global::UnityEngine.Vector2.zero;
					normalizedVelocity = 0f;
				}
				else
				{
					global::UnityEngine.Vector3 vector = base.CachedTransform.InverseTransformDirection(FpcModule.Motor.Velocity);
					global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(vector.x, vector.z);
					float magnitude = vector2.magnitude;
					movementDirection = ((magnitude <= float.Epsilon) ? global::UnityEngine.Vector2.zero : (vector2 / magnitude));
					float walkSpeed = FpcModule.WalkSpeed;
					normalizedVelocity = ((walkSpeed == 0f) ? 1f : (magnitude / walkSpeed));
				}
				UpdateHeadBob(num);
				UpdateFootsteps(num);
				UpdateAnimatorParameters(movementDirection, normalizedVelocity, dampTime);
			}
		}

		protected virtual global::UnityEngine.Animator SetupAnimator()
		{
			global::UnityEngine.Animator component = GetComponent<global::UnityEngine.Animator>();
			AnimatorOverride = new global::UnityEngine.AnimatorOverrideController(component.runtimeAnimatorController);
			component.runtimeAnimatorController = AnimatorOverride;
			return component;
		}

		protected virtual global::UnityEngine.AudioSource PlayFootstepAudioClip(global::UnityEngine.AudioClip clip, float dis, float vol)
		{
			return global::AudioPooling.AudioSourcePoolManager.PlaySound(RandomFootstep, base.transform, dis, vol, FalloffType.Footstep);
		}

		private void UpdateHeadBob(float time)
		{
		}

		private void UpdateFootsteps(float time)
		{
			time -= (float)(int)time;
			int num = _footstepTimes.Length;
			if (_lastFootstep < num)
			{
				if (!(time < _footstepTimes[_lastFootstep]))
				{
					_lastFootstep++;
					if (FootstepPlayable)
					{
						PlayFootstep();
					}
				}
			}
			else if (num > 0 && time < _footstepTimes[0])
			{
				_lastFootstep = 0;
			}
		}

		private void OnGrounded()
		{
			if (!(base.OwnerHub.roleManager.CurrentRole.ActiveTime < 0.3f) && LandingFootstepPlayable)
			{
				PlayFootstep();
				_lastTouchdownSw.Restart();
				if (IsTracked)
				{
					SharedSettings.PlayLandingAnimation();
				}
			}
		}

		private void PlayFootstep()
		{
			float footstepLoudnessDistance = FootstepLoudnessDistance;
			float num = 1f;
			bool flag = true;
			PlayerEffectsController playerEffectsController = base.OwnerHub.playerEffectsController;
			int num2 = playerEffectsController.AllEffects.Length;
			for (int i = 0; i < num2; i++)
			{
				if (playerEffectsController.AllEffects[i].IsEnabled && playerEffectsController.AllEffects[i] is global::CustomPlayerEffects.IFootstepEffect footstepEffect)
				{
					float num3 = footstepEffect.ProcessFootstepOverrides(footstepLoudnessDistance);
					if (num3 >= 0f)
					{
						flag = false;
					}
					num = global::UnityEngine.Mathf.Min(num, num3);
				}
			}
			if (!flag || num >= 0f)
			{
				OnFootstepPlayed?.Invoke(this, footstepLoudnessDistance);
			}
		}

		public virtual void UpdateAnimatorParameters(global::UnityEngine.Vector2 movementDirection, float normalizedVelocity, float dampTime)
		{
			float value = _walkVelocityScale.Evaluate(normalizedVelocity);
			movementDirection *= normalizedVelocity;
			if (_forceUpdate)
			{
				Animator.SetFloat(HashForward, movementDirection.y);
				Animator.SetFloat(HashStrafe, movementDirection.x);
				Animator.SetFloat(HashSpeed, value);
			}
			else
			{
				Animator.SetFloat(HashForward, movementDirection.y, dampTime, global::UnityEngine.Time.deltaTime);
				Animator.SetFloat(HashStrafe, movementDirection.x, dampTime, global::UnityEngine.Time.deltaTime);
				Animator.SetFloat(HashSpeed, value, dampTime, global::UnityEngine.Time.deltaTime);
			}
		}

		public virtual void ForceUpdate()
		{
			_forceUpdate = true;
			Update();
			_forceUpdate = false;
		}

		public override void ResetObject()
		{
			base.ResetObject();
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = FpcModule;
			fpcModule.OnGrounded = (global::System.Action)global::System.Delegate.Remove(fpcModule.OnGrounded, new global::System.Action(OnGrounded));
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			Role = base.OwnerHub.roleManager.CurrentRole;
			FpcModule = (Role as global::PlayerRoles.FirstPersonControl.IFpcRole).FpcModule;
			global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule fpcModule = FpcModule;
			fpcModule.OnGrounded = (global::System.Action)global::System.Delegate.Combine(fpcModule.OnGrounded, new global::System.Action(OnGrounded));
			Animator.Rebind();
			HitboxIdentity[] hitboxes = Hitboxes;
			foreach (HitboxIdentity hitboxIdentity in hitboxes)
			{
				HitboxIdentity.Instances.Add(hitboxIdentity);
				hitboxIdentity.SetColliders(!base.OwnerHub.isLocalPlayer);
			}
		}
	}
}
