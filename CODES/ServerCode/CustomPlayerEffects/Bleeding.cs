namespace CustomPlayerEffects
{
	public class Bleeding : global::CustomPlayerEffects.TickingEffectBase, global::CustomPlayerEffects.IPulseEffect, global::CustomPlayerEffects.IHealablePlayerEffect
	{
		public float minDamage = 2f;

		public float maxDamage = 20f;

		public float multPerTick = 0.5f;

		public float damagePerTick = 20f;

		public override bool AllowEnabling => !global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub);

		public void ExecutePulse()
		{
		}

		protected override void OnTick()
		{
			if (global::Mirror.NetworkServer.active)
			{
				float damage = damagePerTick * global::CustomPlayerEffects.RainbowTaste.CurrentMultiplier(base.Hub);
				base.Hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(damage, global::PlayerStatsSystem.DeathTranslations.Bleeding));
				base.Hub.playerEffectsController.ServerSendPulse<global::CustomPlayerEffects.Bleeding>();
				damagePerTick *= multPerTick;
				damagePerTick = global::UnityEngine.Mathf.Clamp(damagePerTick, minDamage, maxDamage);
			}
		}

		protected override void Enabled()
		{
			if (global::Mirror.NetworkServer.active)
			{
				damagePerTick = maxDamage;
			}
		}

		public bool IsHealable(ItemType it)
		{
			if (it == ItemType.SCP500)
			{
				damagePerTick = minDamage;
			}
			return it == ItemType.Medkit;
		}
	}
}
