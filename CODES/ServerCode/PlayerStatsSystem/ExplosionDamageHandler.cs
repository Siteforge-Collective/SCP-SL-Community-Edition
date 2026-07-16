namespace PlayerStatsSystem
{
	public class ExplosionDamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		private readonly string _deathScreenText;

		private readonly string _serverLogsText;

		private readonly string _ragdollInspectText;

		private readonly global::UnityEngine.Vector3 _force;

		private const float ForceMultiplier = 1.3f;

		public override float Damage { get; protected set; }

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override bool AllowSelfDamage => true;

		public override string ServerLogsText => _serverLogsText;

		public override global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput result = base.ApplyDamage(ply);
			StartVelocity += _force * 1.3f;
			return result;
		}

		public ExplosionDamageHandler(global::Footprinting.Footprint attacker, global::UnityEngine.Vector3 force, float damage, int armorPenetration)
		{
			if (armorPenetration != 0)
			{
				Attacker = attacker;
				_force = force;
				_serverLogsText = global::PlayerStatsSystem.DeathTranslations.Explosion.LogLabel + " caused by " + attacker.Nickname;
				Damage = global::InventorySystem.Items.Armor.BodyArmorUtils.ProcessDamage((attacker.Hub != null && global::InventorySystem.Items.Armor.BodyArmorUtils.TryGetBodyArmor(attacker.Hub.inventory, out var bodyArmor)) ? bodyArmor.VestEfficacy : 0, damage, armorPenetration);
			}
		}
	}
}
