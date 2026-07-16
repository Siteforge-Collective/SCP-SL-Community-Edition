namespace PlayerStatsSystem
{
	public class ScpDamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		private string _ragdollInspectText;

		private readonly byte _translationId;

		public override float Damage { get; protected set; }

		public override global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement => new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement();

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override string ServerLogsText => "Died to SCP (" + Attacker.Nickname + ", " + Attacker.Role.ToString() + ")";

		public override bool AllowSelfDamage => false;

		public ScpDamageHandler()
		{
		}

		public ScpDamageHandler(ReferenceHub attacker, float damage, global::PlayerStatsSystem.DeathTranslation deathReason)
		{
			Attacker = new global::Footprinting.Footprint(attacker);
			Damage = damage;
			_translationId = deathReason.Id;
		}

		public ScpDamageHandler(ReferenceHub attacker, global::PlayerStatsSystem.DeathTranslation deathReason)
		{
			Attacker = new global::Footprinting.Footprint(attacker);
			Damage = -1f;
			_translationId = deathReason.Id;
		}

		public override void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
			base.WriteAdditionalData(writer);
			writer.WriteByte(_translationId);
		}

		public override void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
			base.ReadAdditionalData(reader);
			global::PlayerStatsSystem.DeathTranslations.TranslationsById.TryGetValue(reader.ReadByte(), out var _);
		}
	}
}
