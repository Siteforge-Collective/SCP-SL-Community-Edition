namespace CustomPlayerEffects
{
	public interface IDamageModifierEffect
	{
		float GetDamageModifier(float baseDamage, global::PlayerStatsSystem.DamageHandlerBase handler, HitboxType hitboxType);
	}
}
