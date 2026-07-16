using System;
using System.Runtime.CompilerServices;
using PlayerRoles.PlayableScps.Subroutines;
using PlayerStatsSystem;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp3114
{
	public class Scp3114Slap : ScpAttackAbilityBase<Scp3114Role>
	{
		private const float HitHumeShieldReward = 25f;

		[SerializeField]
		private AudioClip[] _swingClips;

		[SerializeField]
		private float _swingSoundRange;

		private HumeShieldStat _humeShield;

		private Scp3114Strangle _strangle;

		public override float DamageAmount => 0f;

		protected override float AttackDelay => 0f;

		protected override float BaseCooldown => 0f;

		protected override bool CanTriggerAbility => false;

		private bool AbilityBlocked => false;

		public event Action ServerOnHit
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		public event Action ServerOnKill
		{
			[CompilerGenerated]
			add
			{
			}
			[CompilerGenerated]
			remove
			{
			}
		}

		private void PlaySwingSound()
		{
		}

		protected override DamageHandlerBase DamageHandler(float damage)
		{
			return null;
		}

		protected override void Awake()
		{
		}

		public override void SpawnObject()
		{
		}

		protected override void DamagePlayers()
		{
		}
	}
}
