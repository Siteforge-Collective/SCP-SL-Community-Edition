using Footprinting;
using InventorySystem.Items.MicroHID;

namespace PlayerStatsSystem
{
	public class MicroHidDamageHandler : AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		public override float Damage { get; internal set; }

		public override Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => null;

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public MicroHidDamageHandler(MicroHIDItem micro, float impulseDamage)
		{
		}
	}
}
