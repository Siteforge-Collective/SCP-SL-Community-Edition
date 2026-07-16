using Footprinting;
using PlayerRoles.Ragdolls;

namespace PlayerStatsSystem
{
	public class DisruptorDamageHandler : AttackerDamageHandler
	{
		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => null;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public DisruptorDamageHandler(Footprint attacker, float damage)
		{
		}

		public override HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			return default(HandlerOutput);
		}

		public override void ProcessRagdoll(BasicRagdoll _)
		{
		}
	}
}
