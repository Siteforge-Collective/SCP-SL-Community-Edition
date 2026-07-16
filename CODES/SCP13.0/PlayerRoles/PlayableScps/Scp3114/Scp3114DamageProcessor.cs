using PlayerRoles.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114DamageProcessor : SubroutineBase, IDamageHandlerProcessingRole
	{
		[SerializeField]
		private float _explosionDamageMultiplier;

		[SerializeField]
		private float _disguisedHumeShieldDamageMultiplier;

		[SerializeField]
		private float _disguisedBaseHealthDamageMultiplier;

		private Scp3114Role _scpRole;

		private void DisableHitboxMultipliers(DamageHandlerBase handler)
		{
		}

		private void ApplyExplosionDamageReduction(ExplosionDamageHandler explosionHandler)
		{
		}

		private void ApplyDisguiseDamageReduction(FirearmDamageHandler firearmHandler)
		{
		}

		protected override void Awake()
		{
		}

		public DamageHandlerBase ProcessDamageHandler(DamageHandlerBase handler)
		{
			return null;
		}
	}
}
