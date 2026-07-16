using PlayerRoles.FirstPersonControl;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114MovementModule : FirstPersonMovementModule, IStaminaModifier
	{
		private Scp3114Role _scpRole;

		private float _skeletonWalkSpeed;

		private float _skeletonSprintSpeed;

		protected override FpcStateProcessor NewStateProcessor => null;

		public bool StaminaModifierActive => false;

		public float StaminaUsageMultiplier => 0f;

		private void Awake()
		{
		}

		private void OnStatusChanged()
		{
		}
	}
}
