namespace CustomPlayerEffects
{
	public class CardiacArrest : global::CustomPlayerEffects.ParentEffectBase<global::CustomPlayerEffects.SubEffectBase>, global::CustomPlayerEffects.IHealablePlayerEffect, global::PlayerRoles.FirstPersonControl.IStaminaModifier
	{
		private const float SprintStaminaUsage = 3f;

		private const float DamagePerTick = 8f;

		private global::Footprinting.Footprint _attacker;

		[global::UnityEngine.Tooltip("Used to track intervals/timers/etc without every effect needing to redefine a unique float.")]
		public float TimeBetweenTicks;

		private float _timeTillTick;

		public bool StaminaModifierActive => base.IsEnabled;

		public float StaminaUsageMultiplier => 3f;

		public float StaminaRegenMultiplier => 1f;

		public bool SprintingDisabled => false;

		protected override void Enabled()
		{
			base.Enabled();
			if (global::Mirror.NetworkServer.active)
			{
				_timeTillTick = 0f;
			}
		}

		protected override void Disabled()
		{
			base.Disabled();
			_attacker = default(global::Footprinting.Footprint);
		}

		public void SetAttacker(ReferenceHub ply)
		{
			_attacker = new global::Footprinting.Footprint(ply);
		}

		public bool IsHealable(ItemType it)
		{
			if (it != ItemType.SCP500)
			{
				return it == ItemType.Adrenaline;
			}
			return true;
		}

		protected override void OnEffectUpdate()
		{
			if (global::Mirror.NetworkServer.active)
			{
				ServerUpdate();
			}
			UpdateSubEffects();
		}

		private void ServerUpdate()
		{
			_timeTillTick -= global::UnityEngine.Time.deltaTime;
			if (!(_timeTillTick > 0f))
			{
				_timeTillTick += TimeBetweenTicks;
				base.Hub.playerStats.DealDamage(new global::PlayerStatsSystem.Scp049DamageHandler(_attacker, 8f, global::PlayerStatsSystem.Scp049DamageHandler.AttackType.CardiacArrest));
			}
		}
	}
}
