namespace PlayerStatsSystem
{
	public class Scp049DamageHandler : global::PlayerStatsSystem.AttackerDamageHandler
	{
		public enum AttackType : byte
		{
			Instakill = 0,
			CardiacArrest = 1,
			Scp0492 = 2
		}

		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerStatsSystem.Scp049DamageHandler.AttackType, string> LogReasons = new global::System.Collections.Generic.Dictionary<global::PlayerStatsSystem.Scp049DamageHandler.AttackType, string>
		{
			[global::PlayerStatsSystem.Scp049DamageHandler.AttackType.Instakill] = "Killed directly by SCP-049",
			[global::PlayerStatsSystem.Scp049DamageHandler.AttackType.CardiacArrest] = "Died to a heart-attack forced by SCP-049",
			[global::PlayerStatsSystem.Scp049DamageHandler.AttackType.Scp0492] = "Terminated by an instance of SCP-049-2"
		};

		private readonly string _ragdollInspectText;

		public override float Damage { get; protected set; }

		public override global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement CassieDeathAnnouncement => new global::PlayerStatsSystem.DamageHandlerBase.CassieAnnouncement();

		public override global::Footprinting.Footprint Attacker { get; protected set; }

		public override string ServerLogsText => LogReasons[DamageSubType] + " (" + Attacker.Nickname + ").";

		public override bool AllowSelfDamage => false;

		public global::PlayerStatsSystem.Scp049DamageHandler.AttackType DamageSubType { get; private set; }

		public Scp049DamageHandler(ReferenceHub attacker, float damage, global::PlayerStatsSystem.Scp049DamageHandler.AttackType attackType)
		{
			Damage = damage;
			DamageSubType = attackType;
			Attacker = new global::Footprinting.Footprint(attacker);
		}

		public Scp049DamageHandler(global::Footprinting.Footprint attacker, float damage, global::PlayerStatsSystem.Scp049DamageHandler.AttackType attackType)
		{
			Damage = damage;
			DamageSubType = attackType;
			Attacker = attacker;
		}

		public Scp049DamageHandler()
		{
		}

		public override global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput ApplyDamage(ReferenceHub ply)
		{
			global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput handlerOutput = base.ApplyDamage(ply);
			if (!global::Mirror.NetworkServer.active || handlerOutput != global::PlayerStatsSystem.DamageHandlerBase.HandlerOutput.Death)
			{
				return handlerOutput;
			}
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.roleManager.CurrentRole is global::PlayerRoles.PlayableScps.Scp049.Scp049Role scp049Role && scp049Role.SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp049.Scp049SenseAbility>(out var subroutine))
				{
					subroutine.ServerProcessKilledPlayer(ply);
				}
			}
			return handlerOutput;
		}

		public override void WriteDeathScreen(global::Mirror.NetworkWriter writer)
		{
			global::PlayerRoles.RoleTypeId role = ((Attacker.Role != global::PlayerRoles.RoleTypeId.Scp0492) ? global::PlayerRoles.RoleTypeId.Scp049 : global::PlayerRoles.RoleTypeId.Scp0492);
			global::PlayerRoles.Spectating.SpectatorSpawnReasonReaderWriter.WriteSpawnReason(writer, global::PlayerRoles.Spectating.SpectatorSpawnReason.KilledByPlayer);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, Attacker.Nickname);
			global::PlayerRoles.PlayerRoleEnumsReadersWriters.WriteRoleType(writer, role);
		}

		public override void WriteAdditionalData(global::Mirror.NetworkWriter writer)
		{
			base.WriteAdditionalData(writer);
			writer.WriteByte((byte)DamageSubType);
		}

		public override void ReadAdditionalData(global::Mirror.NetworkReader reader)
		{
			base.ReadAdditionalData(reader);
			DamageSubType = (global::PlayerStatsSystem.Scp049DamageHandler.AttackType)reader.ReadByte();
		}
	}
}
