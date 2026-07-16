public interface IDestructible
{
	uint NetworkId { get; }

	global::UnityEngine.Vector3 CenterOfMass { get; }

	bool Damage(float damage, global::PlayerStatsSystem.DamageHandlerBase handler, global::UnityEngine.Vector3 exactHitPos);
}
