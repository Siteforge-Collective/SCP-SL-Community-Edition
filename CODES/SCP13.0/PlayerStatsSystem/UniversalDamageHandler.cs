using Mirror;

namespace PlayerStatsSystem
{
	public class UniversalDamageHandler : StandardDamageHandler
	{
		private string _ragdollInspectText;

		private string _deathscreenText;

		private string _logsText;

		private readonly CassieAnnouncement _cassieAnnouncement;

		public readonly byte TranslationId;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override string ServerLogsText => null;

		public UniversalDamageHandler()
		{
		}

		public UniversalDamageHandler(float damage, DeathTranslation deathReason, CassieAnnouncement cassieAnnouncement = null)
		{
		}

		public override void WriteAdditionalData(NetworkWriter writer)
		{
		}

		public override void ReadAdditionalData(NetworkReader reader)
		{
		}

		private void ApplyTranslation(DeathTranslation translation)
		{
		}
	}
}
