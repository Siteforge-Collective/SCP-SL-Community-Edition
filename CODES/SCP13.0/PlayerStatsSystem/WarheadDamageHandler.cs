namespace PlayerStatsSystem
{
	public class WarheadDamageHandler : StandardDamageHandler
	{
		private readonly string _ragdollinspectText;

		private readonly string _deathscreenText;

		public override CassieAnnouncement CassieDeathAnnouncement => null;

		public override float Damage { get; internal set; }

		public override string RagdollInspectText => null;

		public override string DeathScreenText => null;

		public override string ServerLogsText => null;
	}
}
