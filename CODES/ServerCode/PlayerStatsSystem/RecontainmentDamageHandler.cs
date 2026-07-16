namespace PlayerStatsSystem
{
	public class RecontainmentDamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => true;

		public override float Damage { get; protected set; }

		public override string ServerLogsText => "Recontained by " + Attacker.Nickname;

		public RecontainmentDamageHandler(global::Footprinting.Footprint attacker)
		{
			Attacker = attacker;
			Damage = -1f;
		}
	}
}
