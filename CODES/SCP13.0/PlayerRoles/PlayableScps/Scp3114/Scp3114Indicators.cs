using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Ragdolls;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Indicators : RagdollIndicatorsBase<Scp3114Role>
	{
		protected override bool ValidateRagdoll(BasicRagdoll ragdoll)
		{
			return false;
		}
	}
}
