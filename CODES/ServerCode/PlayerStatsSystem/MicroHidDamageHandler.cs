namespace PlayerStatsSystem
{
	public class MicroHidDamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		public override float Damage { get; protected set; }

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => false;

		public override string ServerLogsText => _serverLogsText;

		public MicroHidDamageHandler(global::InventorySystem.Items.MicroHID.MicroHIDItem micro, float impulseDamage)
		{
			if (!(micro == null))
			{
				Attacker = new global::Footprinting.Footprint(micro.Owner);
				_serverLogsText = "Deep fried by " + Attacker.Nickname + " using " + global::PlayerStatsSystem.DeathTranslations.MicroHID.LogLabel;
				Damage = impulseDamage;
			}
		}
	}
}
