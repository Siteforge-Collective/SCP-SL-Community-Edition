namespace PlayerStatsSystem
{
	public abstract class StandardDamageHandler : global::PlayerStatsSystem.DamageHandlerBase
	{
		public const float KillValue = -1f;

		public HitboxType Hitbox;

		protected global::UnityEngine.Vector3 StartVelocity;

		private short _velX;

		private short _velY;

		private short _velZ;

		public abstract float Damage { get; protected set; }

		public float DealtHealthDamage { get; private set; }

		public float AbsorbedAhpDamage { get; private set; }

		public float AbsorbedHumeDamage { get; private set; }

		public override global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			StartVelocity = global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GetVelocity(ply);
			StartVelocity.y = global::UnityEngine.Mathf.Max(StartVelocity.y, 0f);
			global::PlayerStatsSystem.PlayerStats playerStats = ply.playerStats;
			if (ply.roleManager.CurrentRole is global::PlayerRoles.IHealthbarRole healthbarRole && healthbarRole.TargetStats != playerStats)
			{
				return global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Nothing;
			}
			global::PlayerStatsSystem.AhpStat module = playerStats.GetModule<global::PlayerStatsSystem.AhpStat>();
			global::PlayerStatsSystem.HealthStat module2 = playerStats.GetModule<global::PlayerStatsSystem.HealthStat>();
			global::PlayerStatsSystem.HumeShieldStat module3 = playerStats.GetModule<global::PlayerStatsSystem.HumeShieldStat>();
			if (Damage == -1f)
			{
				module.CurValue = 0f;
				module2.CurValue = 0f;
				return global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Death;
			}
			ProcessDamage(ply);
			global::CustomPlayerEffects.StatusEffectBase[] allEffects = ply.playerEffectsController.AllEffects;
			foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
			{
				if (statusEffectBase.IsEnabled && statusEffectBase is global::CustomPlayerEffects.IDamageModifierEffect damageModifierEffect)
				{
					Damage *= damageModifierEffect.GetDamageModifier(Damage, this, Hitbox);
				}
			}
			if (Damage <= 0f)
			{
				return global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Nothing;
			}
			float curValue = module2.CurValue;
			float num = module.ServerProcessDamage(Damage);
			AbsorbedAhpDamage = Damage - num;
			AbsorbedHumeDamage = global::UnityEngine.Mathf.Min(module3.CurValue, num);
			float num2 = module3.CurValue - num;
			if (num2 < 0f)
			{
				module2.CurValue += num2;
			}
			module3.CurValue = num2;
			DealtHealthDamage = curValue - module2.CurValue;
			if (!(module2.CurValue <= 0f))
			{
				return global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Damaged;
			}
			return global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Death;
		}

		protected virtual void ProcessDamage(ReferenceHub ply)
		{
		}

		public override void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
			base.WriteAdditionalData(writer);
			_velX = (short)global::UnityEngine.Mathf.RoundToInt(StartVelocity.x * 100f);
			_velY = (short)global::UnityEngine.Mathf.RoundToInt(StartVelocity.y * 100f);
			_velZ = (short)global::UnityEngine.Mathf.RoundToInt(StartVelocity.z * 100f);
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, _velX);
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, _velY);
			global::Mirror.NetworkWriterExtensions.WriteInt16(writer, _velZ);
		}

		public override void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
			base.ReadAdditionalData(reader);
			_velX = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
			_velY = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
			_velZ = global::Mirror.NetworkReaderExtensions.ReadInt16(reader);
		}

		public override void ProcessRagdoll(BasicRagdoll ragdoll)
		{
			base.ProcessRagdoll(ragdoll);
			if (ragdoll is global::PlayerRoles.Ragdolls.DynamicRagdoll dynamicRagdoll)
			{
				global::UnityEngine.Vector3 velocity = new global::UnityEngine.Vector3(_velX, _velY, _velZ) / 100f;
				global::UnityEngine.Rigidbody[] linkedRigidbodies = dynamicRagdoll.LinkedRigidbodies;
				for (int i = 0; i < linkedRigidbodies.Length; i++)
				{
					linkedRigidbodies[i].velocity = velocity;
				}
			}
		}
	}
}
