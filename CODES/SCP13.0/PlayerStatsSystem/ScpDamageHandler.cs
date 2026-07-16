using Footprinting;
using Mirror;

namespace PlayerStatsSystem
{
	public class ScpDamageHandler : AttackerDamageHandler
	{
		private string _ragdollInspectText;

		private readonly byte _translationId;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override Footprint Attacker { get; protected set; }

		public override string ServerLogsText => null;

		public override bool AllowSelfDamage => false;

		public ScpDamageHandler()
		{
		}

		public ScpDamageHandler(ReferenceHub attacker, float damage, DeathTranslation deathReason)
		{
		}

		public ScpDamageHandler(ReferenceHub attacker, DeathTranslation deathReason)
		{
		}

		public override void WriteAdditionalData(NetworkWriter writer)
		{
		}

		public override void ReadAdditionalData(NetworkReader reader)
		{
		}
	}
}
