namespace PlayerStatsSystem
{
	public class UniversalDamageHandler : global::PlayerStatsSystem.StandardDamageHandler
	{
		private string _ragdollInspectText;

		private string _deathscreenText;

		private string _logsText;

		private readonly global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement _cassieAnnouncement;

		public readonly byte TranslationId;

		public override float Damage { get; protected set; }

		public override global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement => _cassieAnnouncement;

		public override string ServerLogsText => _logsText;

		public UniversalDamageHandler()
		{
			TranslationId = 0;
			_cassieAnnouncement = global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement.Default;
		}

		public UniversalDamageHandler(float damage, global::PlayerStatsSystem.DeathTranslation deathReason, global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement cassieAnnouncement = null)
		{
			Damage = damage;
			ApplyTranslation(deathReason);
			TranslationId = deathReason.Id;
			_cassieAnnouncement = cassieAnnouncement ?? global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement.Default;
		}

		public override void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
			base.WriteAdditionalData(writer);
			writer.WriteByte(TranslationId);
		}

		public override void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
			base.ReadAdditionalData(reader);
			if (global::PlayerStatsSystem.DeathTranslations.TranslationsById.TryGetValue(reader.ReadByte(), out var value))
			{
				ApplyTranslation(value);
			}
		}

		private void ApplyTranslation(global::PlayerStatsSystem.DeathTranslation translation)
		{
			_logsText = translation.LogLabel;
		}
	}
}
