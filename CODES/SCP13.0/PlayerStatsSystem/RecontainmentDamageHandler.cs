using Footprinting;

namespace PlayerStatsSystem
{
	public class RecontainmentDamageHandler : AttackerDamageHandler
	{
		private readonly string _ragdollinspectText;

		private readonly string _deathscreenText;

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override string ServerLogsText => null;

		public RecontainmentDamageHandler(Footprint attacker)
		{
		}
	}
}
