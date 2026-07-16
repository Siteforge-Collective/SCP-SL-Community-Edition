using PlayerRoles.FirstPersonControl;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939MovementModule : FirstPersonMovementModule
	{
		private const float StaminaUseRate = 0.1f;

		private const float StaminaRegenRate = 0.1f;

		private const float StaminaRegenCooldown = 7f;

		private const float StaminaRampupTime = 0.1f;

		protected override FpcMotor NewMotor => null;

		protected override FpcMouseLook NewMouseLook => null;

		protected override FpcStateProcessor NewStateProcessor => null;

		protected override PlayerMovementState ValidateMovementState(PlayerMovementState state)
		{
			return default(PlayerMovementState);
		}
	}
}
