namespace CustomPlayerEffects
{
	public class Hemorrhage : global::CustomPlayerEffects.TickingEffectBase
	{
		public float damagePerTick = 1f;

		private bool _isSprinting;

		public override bool AllowEnabling => !global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub);

		protected override void OnTick()
		{
			if (global::Mirror.NetworkServer.active && _isSprinting)
			{
				float damage = damagePerTick * global::CustomPlayerEffects.RainbowTaste.CurrentMultiplier(base.Hub);
				base.Hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(damage, global::PlayerStatsSystem.DeathTranslations.Bleeding));
			}
		}

		protected override void OnEffectUpdate()
		{
			base.OnEffectUpdate();
			if (base.Hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				_isSprinting = fpcRole.FpcModule.CurrentMovementState == global::PlayerRoles.FirstPersonControl.PlayerMovementState.Sprinting;
			}
		}
	}
}
