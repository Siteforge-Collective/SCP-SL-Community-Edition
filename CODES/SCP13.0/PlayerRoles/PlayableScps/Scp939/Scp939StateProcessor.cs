using PlayerRoles.FirstPersonControl;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939StateProcessor : FpcStateProcessor
	{
		public Scp939StateProcessor(ReferenceHub hub, FirstPersonMovementModule fpmm, float useRate, float regenCooldown, float regenRate, float rampupTime)
			: base(null, null)
		{
		}

		public override void ClientUpdateInput(FirstPersonMovementModule moduleRef, float walkSpeed, out PlayerMovementState valueToSend)
		{
			valueToSend = default(PlayerMovementState);
		}
	}
}
