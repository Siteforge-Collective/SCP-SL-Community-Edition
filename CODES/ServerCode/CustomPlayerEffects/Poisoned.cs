namespace CustomPlayerEffects
{
	public class Poisoned : global::CustomPlayerEffects.TickingEffectBase, global::CustomPlayerEffects.IHealablePlayerEffect, global::CustomPlayerEffects.IPulseEffect
	{
		private float damagePerTick = 2f;

		private readonly float minDamage = 2f;

		private readonly float maxDamage = 20f;

		private readonly float multPerTick = 2f;

		public override bool AllowEnabling => !global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub);

		public void ExecutePulse()
		{
		}

		public bool IsHealable(ItemType it)
		{
			return it == ItemType.SCP500;
		}

		protected override void OnTick()
		{
			if (global::Mirror.NetworkServer.active)
			{
				base.Hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(damagePerTick, global::PlayerStatsSystem.DeathTranslations.Poisoned));
				base.Hub.playerEffectsController.ServerSendPulse<global::CustomPlayerEffects.Poisoned>();
				damagePerTick *= multPerTick;
				damagePerTick = global::UnityEngine.Mathf.Clamp(damagePerTick, minDamage, maxDamage);
			}
		}

		protected override void Enabled()
		{
			if (global::Mirror.NetworkServer.active)
			{
				damagePerTick = minDamage;
			}
		}
	}
}
