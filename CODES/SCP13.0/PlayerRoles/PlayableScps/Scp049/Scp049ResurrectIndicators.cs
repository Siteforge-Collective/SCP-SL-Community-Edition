using PlayerRoles.Ragdolls;

namespace PlayerRoles.PlayableScps.Scp049
{
	public class Scp049ResurrectIndicators : RagdollIndicatorsBase<Scp049Role>
	{
		private Scp049ResurrectAbility _resurrectAbility;

		protected override void Awake()
		{
		}

		protected override bool ValidateRagdoll(BasicRagdoll ragdoll)
		{
			return false;
		}
	}
}
