namespace PlayerRoles.PlayableScps.Scp939
{
	public class Scp939DamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		private global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType _damageType;

		private RagdollAnimationTemplate _lungeTemplate;

		private global::RelativePositioning.RelativePosition _hitPos;

		private bool _lungeTemplateSet;

		private const float LungeUpwardsSpeed = 3.5f;

		private const float LungeTotalSpeed = 5.5f;

		public override bool AllowSelfDamage => false;

		public override float Damage { get; protected set; }

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override string ServerLogsText => $"Killed by SCP-939 ({Attacker.Nickname}) with {_damageType}.";

		private RagdollAnimationTemplate LungeTemplate
		{
			get
			{
				if (_lungeTemplateSet)
				{
					return _lungeTemplate;
				}
				if (!global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayableScps.Scp939.Scp939Role>(global::PlayerRoles.RoleTypeId.Scp939, out var result))
				{
					return null;
				}
				if (!result.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp939.Scp939LungeAbility>(out var subroutine))
				{
					return null;
				}
				_lungeTemplate = subroutine.LungeDeathAnim;
				_lungeTemplateSet = true;
				return LungeTemplate;
			}
		}

		public Scp939DamageHandler(global::PlayerRoles.PlayableScps.Scp939.Scp939Role scp939, global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType type = global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.None)
		{
			switch (type)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.None:
				return;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.Claw:
				Damage = 40f;
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeTarget:
				Damage = 120f;
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeSecondary:
				Damage = 30f;
				_hitPos = new global::RelativePositioning.RelativePosition(scp939.FpcModule.Position);
				break;
			}
			if (scp939.TryGetOwner(out var hub))
			{
				Attacker = new global::Footprinting.Footprint(hub);
				_damageType = type;
			}
		}

		protected override void ProcessDamage(ReferenceHub ply)
		{
			if (!(ply.roleManager.CurrentRole is global::PlayerRoles.HumanRole humanRole))
			{
				base.ProcessDamage(ply);
				return;
			}
			global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType damageType = _damageType;
			if (damageType == global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.Claw)
			{
				int armorEfficacy = humanRole.GetArmorEfficacy(HitboxType.Body);
				int bulletPenetrationPercent = 75;
				Damage = global::InventorySystem.Items.Armor.BodyArmorUtils.ProcessDamage(armorEfficacy, Damage, bulletPenetrationPercent);
			}
			base.ProcessDamage(ply);
		}

		public override void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
			base.WriteAdditionalData(writer);
			writer.WriteByte((byte)_damageType);
			if (_damageType == global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeSecondary)
			{
				global::RelativePositioning.RelativePositionSerialization.WriteRelativePosition(writer, _hitPos);
			}
		}

		public override void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
			base.ReadAdditionalData(reader);
			_damageType = (global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType)reader.ReadByte();
			if (_damageType == global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeSecondary)
			{
				_hitPos = global::RelativePositioning.RelativePositionSerialization.ReadRelativePosition(reader);
			}
		}

		public override void ProcessRagdoll(BasicRagdoll ragdoll)
		{
			if (!(ragdoll is global::PlayerRoles.Ragdolls.DynamicRagdoll dynamicRagdoll))
			{
				return;
			}
			switch (_damageType)
			{
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeTarget:
				LungeTemplate.ProcessRagdoll(ragdoll);
				break;
			case global::PlayerRoles.PlayableScps.Scp939.Scp939DamageType.LungeSecondary:
			{
				global::UnityEngine.Vector3 vector = ragdoll.Info.StartPosition - _hitPos.Position;
				vector.y = 3.5f;
				vector = vector.normalized * 5.5f;
				global::UnityEngine.Rigidbody[] linkedRigidbodies = dynamicRagdoll.LinkedRigidbodies;
				for (int i = 0; i < linkedRigidbodies.Length; i++)
				{
					linkedRigidbodies[i].velocity = vector;
				}
				break;
			}
			default:
				base.ProcessRagdoll(ragdoll);
				break;
			}
		}
	}
}
