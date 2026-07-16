namespace PlayerRoles.PlayableScps.Scp106
{
	public class Scp106SinkholeController : global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase, global::CursorManagement.ICursorOverride, global::GameObjectPools.IPoolResettable, global::GameObjectPools.IPoolSpawnable
	{
		public const float DefaultAnimTime = 3.3f;

		private const float CooldownDuration = 20f;

		private const float MinDuration = 0.03f;

		private const float AudioFadeIntensity = 8f;

		private const float AudioFadeAbs = 0.07f;

		private bool _state;

		private float _toggleTime;

		private int _vigorAbilitiesCount;

		private float _duration = 3.3f;

		private global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase[] _vigorAbilities;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _emergeSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _submergeSound;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioSource _toggleAudioSource;

		public readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown Cooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		public global::CursorManagement.CursorOverrideMode CursorOverride => global::CursorManagement.CursorOverrideMode.NoOverride;

		public bool LockMovement
		{
			get
			{
				if (base.Role.IsLocalPlayer)
				{
					return IsDuringAnimation;
				}
				return false;
			}
		}

		public float ElapsedToggle => CurTime - _toggleTime;

		public bool IsDuringAnimation => ElapsedToggle < TargetDuration;

		public bool IsHidden
		{
			get
			{
				if (State)
				{
					return !IsDuringAnimation;
				}
				return false;
			}
		}

		public float NormalizedState
		{
			get
			{
				float num = ElapsedToggle / TargetDuration;
				if (!State)
				{
					num = 1f - num;
				}
				return global::UnityEngine.Mathf.Clamp01(num);
			}
		}

		public bool State
		{
			get
			{
				return _state;
			}
			private set
			{
				if (_state != value)
				{
					_state = value;
					_toggleTime = CurTime;
					_toggleAudioSource.PlayOneShot(value ? _submergeSound : _emergeSound);
					if (!value && global::Mirror.NetworkServer.active)
					{
						Cooldown.Trigger(20f);
						ServerSendRpc(toAll: true);
					}
				}
			}
		}

		public float TargetDuration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = global::UnityEngine.Mathf.Max(0.03f, value);
			}
		}

		public float SpeedMultiplier
		{
			get
			{
				return 3.3f / TargetDuration;
			}
			set
			{
				TargetDuration = value * 3.3f;
			}
		}

		public void SpawnObject()
		{
			global::CursorManagement.CursorManager.Register(this);
		}

		public void ResetObject()
		{
			global::CursorManagement.CursorManager.Unregister(this);
			Cooldown.Clear();
			_state = false;
			_toggleTime = 0f;
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			Cooldown.WriteCooldown(writer);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			Cooldown.ReadCooldown(reader);
		}

		protected override void Awake()
		{
			base.Awake();
			global::PlayerRoles.PlayableScps.Subroutines.ScpSubroutineBase[] allSubroutines = (base.Role as global::PlayerRoles.PlayableScps.Scp106.Scp106Role).SubroutineModule.AllSubroutines;
			_vigorAbilities = new global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase[allSubroutines.Length];
			_vigorAbilitiesCount = 0;
			for (int i = 0; i < allSubroutines.Length; i++)
			{
				if (allSubroutines[i] is global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase scp106VigorAbilityBase)
				{
					_vigorAbilities[_vigorAbilitiesCount++] = scp106VigorAbilityBase;
				}
			}
		}

		private void Update()
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < _vigorAbilitiesCount; i++)
			{
				global::PlayerRoles.PlayableScps.Scp106.Scp106VigorAbilityBase scp106VigorAbilityBase = _vigorAbilities[i];
				flag |= scp106VigorAbilityBase.IsSubmerged;
				flag2 |= scp106VigorAbilityBase.ForceHumanAnimations;
			}
			State = flag;
			_toggleAudioSource.volume = 8f * (1f - NormalizedState) - 0.07f;
		}
	}
}
