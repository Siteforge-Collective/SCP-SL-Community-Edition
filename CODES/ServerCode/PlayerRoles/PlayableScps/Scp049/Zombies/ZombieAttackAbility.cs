namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieAttackAbility : global::PlayerRoles.PlayableScps.ScpAttackAbilityBase<global::PlayerRoles.PlayableScps.Scp049.Zombies.ZombieRole>
	{
		public override float DamageAmount => 40f;

		protected override float AttackDelay => 0f;

		protected override float BaseCooldown => 1.3f;

		protected override bool CanTriggerAbility => base.Cooldown.IsReady;

		protected override bool SelfRepeating => false;

		protected override global::PlayerStatsSystem.DamageHandlerBase DamageHandler => new global::PlayerStatsSystem.Scp049DamageHandler(base.Owner, DamageAmount, global::PlayerStatsSystem.Scp049DamageHandler.AttackType.Scp0492);
	}
}
