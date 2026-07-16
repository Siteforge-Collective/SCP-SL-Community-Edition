using PlayerRoles.PlayableScps.Subroutines;
using PlayerStatsSystem;

namespace PlayerRoles.PlayableScps.Scp049.Zombies
{
	public class ZombieAttackAbility : ScpAttackAbilityBase<ZombieRole>
	{
		private ZombieConsumeAbility _consumeAbility;

		public override float DamageAmount => 0f;

		protected override float AttackDelay => 0f;

		protected override float BaseCooldown => 0f;

		protected override bool CanTriggerAbility => false;

		protected override bool SelfRepeating => false;

		protected override DamageHandlerBase DamageHandler(float damage)
		{
			return null;
		}

		protected override void Awake()
		{
		}

		protected override void DamagePlayers()
		{
		}
	}
}
