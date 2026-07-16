namespace CustomPlayerEffects
{
	public class Burned : global::CustomPlayerEffects.StatusEffectBase, global::CustomPlayerEffects.IHealablePlayerEffect, global::CustomPlayerEffects.IDamageModifierEffect
	{
		private const float BaseDamageMultiplier = 1.25f;

		public float DamageMultiplier => 0.25f * global::CustomPlayerEffects.RainbowTaste.CurrentMultiplier(base.Hub) + 1f;

		public override bool AllowEnabling => !global::CustomPlayerEffects.Vitality.CheckPlayer(base.Hub);

		public bool IsHealable(ItemType it)
		{
			if (it != ItemType.Medkit)
			{
				return it == ItemType.SCP500;
			}
			return true;
		}

		public float GetDamageModifier(float baseDamage, global::PlayerStatsSystem.DamageHandlerBase handler, HitboxType hitboxType)
		{
			return DamageMultiplier;
		}
	}
}
