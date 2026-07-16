namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939ClawAbility : global::PlayerRoles.PlayableScps.ScpAttackAbilityBase<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>
	{
		public const float BaseDamage = 40f;

		public const int DamagePenetration = 75;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility _focusAbility;

		private global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility _cloudAbility;

		public override float DamageAmount => 40f;

		protected override float BaseCooldown => 0.6f;

		protected override bool CanTriggerAbility
		{
			get
			{
				if (base.CanTriggerAbility && _focusAbility.State == 0f)
				{
					return !_cloudAbility.TargetState;
				}
				return false;
			}
		}

		protected override global::PlayerStatsSystem.DamageHandlerBase DamageHandler => new global::PlayerRoles.PlayableScps.Scp939.Scp939DamageHandler(base.ScpRole, global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.Claw);

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			if (_focusAbility.State == 0f)
			{
				base.ServerProcessCmd(reader);
			}
		}

		protected override void OnDestructibleDamaged(IDestructible dest)
		{
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp939Attack, base.Owner, dest))
			{
				base.OnDestructibleDamaged(dest);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939FocusAbility>(out _focusAbility);
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939AmnesticCloudAbility>(out _cloudAbility);
		}
	}
}
