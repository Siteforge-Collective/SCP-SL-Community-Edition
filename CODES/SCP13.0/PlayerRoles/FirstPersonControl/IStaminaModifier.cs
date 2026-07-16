namespace PlayerRoles.FirstPersonControl
{
	public interface IStaminaModifier
	{
		bool StaminaModifierActive { get; }

		float StaminaUsageMultiplier => 0f;

		float StaminaRegenMultiplier => 0f;

		bool SprintingDisabled => false;
	}
}
