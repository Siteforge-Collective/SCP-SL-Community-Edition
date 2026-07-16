namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieMovementModule : global::PlayerRoles.FirstPersonControl.FirstPersonMovementModule
	{
		private const float MaxTargetTime = 5f;

		private const float MinTargetTime = 1f;

		private const float SpeedPerTick = 0.05f;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole _role;

		private global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieBloodlustAbility _visionTracker;

		private float _lookingTimer;

		private float _speedTickTimer;

		private bool _bloodlustActive;

		public bool CanMove { get; set; }

		public float BloodlustSpeed { get; private set; }

		public float NormalSpeed { get; private set; }

		private float MovementSpeed
		{
			get
			{
				return WalkSpeed;
			}
			set
			{
				WalkSpeed = value;
				SprintSpeed = value;
			}
		}

		private void Awake()
		{
			NormalSpeed = WalkSpeed;
			BloodlustSpeed = SprintSpeed;
			_role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieBloodlustAbility>(out _visionTracker);
		}

		protected override void UpdateMovement()
		{
			float deltaTime = global::UnityEngine.Time.deltaTime;
			UpdateBloodlustState(deltaTime);
			UpdateSpeed(deltaTime);
			base.UpdateMovement();
		}

		private void UpdateBloodlustState(float deltaTime)
		{
			float value = _lookingTimer + (_visionTracker.LookingAtTarget ? deltaTime : (0f - deltaTime));
			_lookingTimer = global::UnityEngine.Mathf.Clamp(value, 0f, 5f);
			if (_lookingTimer > 1f)
			{
				_bloodlustActive = true;
			}
			else if (_lookingTimer == 0f)
			{
				_bloodlustActive = false;
			}
		}

		private void UpdateSpeed(float deltaTime)
		{
			_speedTickTimer += deltaTime;
			if (!(_speedTickTimer < 1f))
			{
				_speedTickTimer = 0f;
				float value = MovementSpeed + (_bloodlustActive ? 0.05f : (-0.1f));
				MovementSpeed = global::UnityEngine.Mathf.Clamp(value, NormalSpeed, BloodlustSpeed);
			}
		}
	}
}
