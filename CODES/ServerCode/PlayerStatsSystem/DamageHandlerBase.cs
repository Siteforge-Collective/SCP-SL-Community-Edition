namespace PlayerStatsSystem
{
	public abstract class DamageHandlerBase
	{
		public class CassieAnnouncement
		{
			public static readonly global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement Default = new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement
			{
				Announcement = "SUCCESSFULLY TERMINATED . TERMINATION CAUSE UNSPECIFIED",
				SubtitleParts = new global::Subtitles.SubtitlePart[1]
				{
					new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.TerminationCauseUnspecified, (string[])null)
				}
			};

			public string Announcement;

			public global::Subtitles.SubtitlePart[] SubtitleParts;
		}

		public enum HandlerOutput : byte
		{
			Nothing = 0,
			Damaged = 1,
			Death = 2
		}

		public abstract string ServerLogsText { get; }

		public abstract global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement { get; }

		public virtual void WriteDeathScreen(global::Mirror.NetworkWriter writer)
		{
			global::PlayerRoles.Spectating.SpectatorSpawnReasonReaderWriter.WriteSpawnReason(writer, global::PlayerRoles.Spectating.SpectatorSpawnReason.Other);
			writer.WriteDamageHandler(this);
		}

		public virtual void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
		}

		public virtual void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
		}

		public virtual void ProcessRagdoll(BasicRagdoll ragdoll)
		{
		}

		public abstract global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply);
	}
}
