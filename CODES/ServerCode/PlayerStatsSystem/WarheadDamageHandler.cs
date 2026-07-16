namespace PlayerStatsSystem
{
	public class WarheadDamageHandler : global::PlayerStatsSystem.StandardDamageHandler
	{
		public override global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement
		{
			get
			{
				global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement cassieAnnouncement = new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement();
				cassieAnnouncement.Announcement = "SUCCESSFULLY TERMINATED BY ALPHA WARHEAD";
				cassieAnnouncement.SubtitleParts = new global::Subtitles.SubtitlePart[1]
				{
					new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.TerminatedByWarhead, (string[])null)
				};
				return cassieAnnouncement;
			}
		}

		public override float Damage { get; protected set; }

		public override string ServerLogsText => "Died to alpha warhead.";

		public WarheadDamageHandler()
		{
			Damage = -1f;
		}
	}
}
