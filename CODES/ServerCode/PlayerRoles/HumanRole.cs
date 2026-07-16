namespace PlayerRoles
{
	public class HumanRole : global::PlayerRoles.FirstPersonControl.FpcStandardRoleBase, global::PlayerRoles.IArmoredRole
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint[]> SpawnpointsForRoles = new global::System.Collections.Generic.Dictionary<global::PlayerRoles.RoleTypeId, global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint[]>();

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.RoleTypeId _roleId;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.Team _team;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Color _roleColor;

		[global::UnityEngine.SerializeField]
		private global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint[] _spawnpoints;

		[field: global::UnityEngine.SerializeField]
		public global::Respawning.SpawnableTeamType AssignedSpawnableTeam { get; private set; }

		public override global::PlayerRoles.RoleTypeId RoleTypeId => _roleId;

		public override global::PlayerRoles.Team Team => _team;

		public override global::UnityEngine.Color RoleColor => _roleColor;

		public override float MaxHealth => 100f;

		public byte UnitNameId { get; private set; }

		public bool UsesUnitNames
		{
			get
			{
				global::Respawning.NamingRules.UnitNamingRule rule;
				return global::Respawning.NamingRules.UnitNamingRule.TryGetNamingRule(AssignedSpawnableTeam, out rule);
			}
		}

		public string UnitName
		{
			get
			{
				if (!global::Respawning.NamingRules.UnitNameMessageHandler.ReceivedNames.TryGetValue(AssignedSpawnableTeam, out var value))
				{
					return string.Empty;
				}
				int count = value.Count;
				if (count != 0 && UnitNameId < count)
				{
					return value[UnitNameId];
				}
				return string.Empty;
			}
		}

		public override global::PlayerRoles.FirstPersonControl.Spawnpoints.ISpawnpointHandler SpawnpointHandler
		{
			get
			{
				if (SpawnpointsForRoles.TryGetValue(_roleId, out var value))
				{
					return value?.RandomItem();
				}
				int num = _spawnpoints.Length;
				if (num == 0)
				{
					SpawnpointsForRoles.Add(RoleTypeId, null);
					return null;
				}
				value = new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint[num];
				for (int i = 0; i < num; i++)
				{
					value[i] = new global::PlayerRoles.FirstPersonControl.Spawnpoints.RoomRoleSpawnpoint(_spawnpoints[i]);
				}
				SpawnpointsForRoles.Add(RoleTypeId, value);
				return value.RandomItem();
			}
		}

		public override void WritePublicSpawnData(global::Mirror.NetworkWriter writer)
		{
			if (UsesUnitNames)
			{
				writer.WriteByte(UnitNameId);
			}
			base.WritePublicSpawnData(writer);
		}

		public override void ReadSpawnData(global::Mirror.NetworkReader reader)
		{
			if (UsesUnitNames)
			{
				UnitNameId = reader.ReadByte();
			}
			base.ReadSpawnData(reader);
		}

		internal override void Init(ReferenceHub hub, global::PlayerRoles.RoleChangeReason spawnReason, global::PlayerRoles.RoleSpawnFlags spawnFlags)
		{
			base.Init(hub, spawnReason, spawnFlags);
			if (global::Mirror.NetworkServer.active && UsesUnitNames)
			{
				UnitNameId = (byte)(global::Respawning.NamingRules.UnitNameMessageHandler.ReceivedNames.TryGetValue(AssignedSpawnableTeam, out var value) ? ((byte)value.Count) : 0);
				if (UnitNameId != 0 && spawnReason != global::PlayerRoles.RoleChangeReason.Respawn)
				{
					UnitNameId--;
				}
			}
		}

		public int GetArmorEfficacy(HitboxType hitbox)
		{
			if (!TryGetOwner(out var hub) || !global::InventorySystem.Items.Armor.BodyArmorUtils.TryGetBodyArmor(hub.inventory, out var bodyArmor))
			{
				return 0;
			}
			switch (hitbox)
			{
			case HitboxType.Headshot:
				return bodyArmor.HelmetEfficacy;
			case HitboxType.Body:
				return bodyArmor.VestEfficacy;
			default:
				return 0;
			}
		}
	}
}
