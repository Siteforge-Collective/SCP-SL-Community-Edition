using System.Collections.Generic;
using System.Text;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Spawnpoints;
using Respawning;
using Respawning.NamingRules;
using UnityEngine;

namespace PlayerRoles
{
    public class HumanRole : FpcStandardRoleBase, IArmoredRole, IInventoryRole, ICustomNicknameDisplayRole
    {
        private static readonly Dictionary<RoleTypeId, RoomRoleSpawnpoint[]> SpawnpointsForRoles = new Dictionary<RoleTypeId, RoomRoleSpawnpoint[]>();

        [SerializeField]
        private RoleTypeId _roleId;

        [SerializeField]
        private Team _team;

        [SerializeField]
        private Color _roleColor;

        [SerializeField]
        private RoomRoleSpawnpoint[] _spawnpoints;

        [field: SerializeField]
        public SpawnableTeamType AssignedSpawnableTeam { get; private set; }

        public override RoleTypeId RoleTypeId => _roleId;

        public override Team Team => _team;

        public override Color RoleColor => _roleColor;

        public override float MaxHealth => 100f;

        public byte UnitNameId { get; private set; }

        private bool UsesUnitNames
        {
            get
            {
                UnitNamingRule rule;
                return UnitNamingRule.TryGetNamingRule(AssignedSpawnableTeam, out rule);
            }
        }

        public override ISpawnpointHandler SpawnpointHandler
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
                value = new RoomRoleSpawnpoint[num];
                for (int i = 0; i < num; i++)
                {
                    value[i] = new RoomRoleSpawnpoint(_spawnpoints[i]);
                }
                SpawnpointsForRoles.Add(RoleTypeId, value);
                return value.RandomItem();
            }
        }

        public override void WritePublicSpawnData(NetworkWriter writer)
        {
            if (UsesUnitNames)
            {
                writer.WriteByte(UnitNameId);
            }
            base.WritePublicSpawnData(writer);
        }

        public override void ReadSpawnData(NetworkReader reader)
        {
            if (UsesUnitNames)
            {
                UnitNameId = reader.ReadByte();
            }
            base.ReadSpawnData(reader);
        }

        internal override void Init(ReferenceHub hub, RoleChangeReason spawnReason, RoleSpawnFlags spawnFlags)
        {
            base.Init(hub, spawnReason, spawnFlags);
            if (NetworkServer.active && UsesUnitNames)
            {
                UnitNameId = (byte)(UnitNameMessageHandler.ReceivedNames.TryGetValue(AssignedSpawnableTeam, out var value) ? ((byte)value.Count) : 0);
                if (UnitNameId != 0 && spawnReason != RoleChangeReason.Respawn)
                {
                    UnitNameId--;
                }
            }
        }

        public int GetArmorEfficacy(HitboxType hitbox)
        {
            if (!TryGetOwner(out var hub) || !hub.inventory.TryGetBodyArmor(out var bodyArmor))
            {
                return 0;
            }
            return hitbox switch
            {
                HitboxType.Headshot => bodyArmor.HelmetEfficacy,
                HitboxType.Body => bodyArmor.VestEfficacy,
                _ => 0,
            };
        }

        public void WriteNickname(ReferenceHub owner, StringBuilder sb, out Color texColor)
        {
            NicknameSync.WriteDefaultInfo(owner, sb, out texColor);
            if (UnitNamingRule.TryGetNamingRule(AssignedSpawnableTeam, out var rule))
            {
                PlayerInfoArea shownPlayerInfo = owner.nicknameSync.ShownPlayerInfo;
                string received = UnitNameMessageHandler.GetReceived(AssignedSpawnableTeam, UnitNameId);
                rule.AppendName(sb, received, RoleTypeId, shownPlayerInfo);
            }
        }

        public bool AllowDisarming(ReferenceHub detainer)
        {
            if (Team.GetFaction() == detainer.GetFaction())
            {
                return false;
            }
            if (!TryGetOwner(out var hub))
            {
                return false;
            }
            if (hub.interCoordinator.AnyBlocker(BlockedInteraction.BeDisarmed))
            {
                return false;
            }
            return true;
        }

        public bool AllowUndisarming(ReferenceHub releaser)
        {
            return releaser.IsHuman();
        }
    }
}
