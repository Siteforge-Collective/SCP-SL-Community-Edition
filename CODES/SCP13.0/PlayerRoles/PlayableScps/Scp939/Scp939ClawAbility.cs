using Mirror;
using PlayerRoles.PlayableScps.Subroutines;
using PlayerStatsSystem;

namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939ClawAbility : ScpAttackAbilityBase<Scp939Role>
	{
		public const float BaseDamage = 40f;

		public const int DamagePenetration = 75;

		private Scp939FocusAbility _focusAbility;

		private Scp939AmnesticCloudAbility _cloudAbility;

		public override float DamageAmount => 0f;

		protected override float AttackDelay => 0f;

		protected override bool KeyPressable => false;

		protected override float BaseCooldown => 0f;

		protected override bool CanTriggerAbility => false;

		protected override DamageHandlerBase DamageHandler(float damage)
		{
			return null;
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		protected override void DamagePlayers()
		{
		}

		protected override void Awake()
		{
		}
	}
}
