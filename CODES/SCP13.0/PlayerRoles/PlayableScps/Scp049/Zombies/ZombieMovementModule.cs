using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieMovementModule : FirstPersonMovementModule
	{
		public const float MaxTargetTime = 5f;

		private const float MinTargetTime = 1f;

		private const float SpeedPerTick = 0.05f;

		[SerializeField]
		private ZombieRole _role;

		private ZombieBloodlustAbility _visionTracker;

		private float _speedTickTimer;

		private bool _bloodlustActive;

		private float _lookingTimer;

		public bool CanMove { get; set; }

		public float BloodlustSpeed { get; private set; }

		public float NormalSpeed { get; private set; }

		private float MovementSpeed
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public void ForceBloodlustSpeed()
		{
		}

		private void Awake()
		{
		}

		protected override void UpdateMovement()
		{
		}

		private void UpdateBloodlustState(float deltaTime)
		{
		}

		private void UpdateSpeed(float deltaTime)
		{
		}
	}
}
