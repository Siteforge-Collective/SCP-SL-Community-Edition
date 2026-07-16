using System;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.FirstPersonControl.Thirdperson;

namespace PlayerRoles
{
    public static class PlayerRolesUtils
    {
        public static readonly CachedLayerMask BlockerMask = new CachedLayerMask("Default", "Door", "Glass");

        public static RoleTypeId GetRoleId(this ReferenceHub hub)
        {
            return hub.roleManager.CurrentRole.RoleTypeId;
        }

        public static Team GetTeam(this ReferenceHub hub)
        {
            return hub.roleManager.CurrentRole.Team;
        }

        public static Team GetTeam(this RoleTypeId role)
        {
            if (!PlayerRoleLoader.TryGetRoleTemplate<PlayerRoleBase>(role, out var result))
            {
                return Team.OtherAlive;
            }
            return result.Team;
        }

        public static Faction GetFaction(this ReferenceHub hub)
        {
            return hub.GetTeam().GetFaction();
        }

        public static Faction GetFaction(this RoleTypeId role)
        {
            return role.GetTeam().GetFaction();
        }

        public static Faction GetFaction(this Team t)
        {
            return t switch
            {
                Team.SCPs => Faction.SCP,
                Team.FoundationForces or Team.Scientists => Faction.FoundationStaff,
                Team.ChaosInsurgency or Team.ClassD => Faction.FoundationEnemy,
                _ => Faction.Unclassified,
            };
        }

        public static bool IsHuman(this RoleTypeId role)
        {
            Team team = role.GetTeam();
            if (team != Team.Dead)
            {
                return team != Team.SCPs;
            }
            return false;
        }

        public static bool IsHuman(this ReferenceHub hub)
        {
            if (hub.IsAlive())
            {
                return !hub.IsSCP();
            }
            return false;
        }

        public static bool IsAlive(this RoleTypeId role)
        {
            return role.GetTeam() != Team.Dead;
        }

        public static bool IsAlive(this ReferenceHub hub)
        {
            return hub.GetTeam() != Team.Dead;
        }

        public static bool IsSCP(this ReferenceHub hub, bool includeZombies = true)
        {
            if (hub.GetTeam() == Team.SCPs)
            {
                if (!includeZombies)
                {
                    return hub.GetRoleId() != RoleTypeId.Scp0492;
                }
                return true;
            }
            return false;
        }

        public static CharacterModel GetModel(this ReferenceHub hub)
        {
            return (hub.roleManager.CurrentRole as IFpcRole)?.FpcModule.CharacterModelInstance;
        }

        public static void ForEachRole<T>(Action<ReferenceHub, T> action) where T : PlayerRoleBase
        {
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.roleManager.CurrentRole is T arg)
                {
                    action?.Invoke(allHub, arg);
                }
            }
        }

        public static void ForEachRole<T>(Action<T> action) where T : PlayerRoleBase
        {
            ForEachRole(delegate (ReferenceHub x, T y)
            {
                action?.Invoke(y);
            });
        }

        public static void ForEachRole<T>(Action<ReferenceHub> action) where T : PlayerRoleBase
        {
            ForEachRole(delegate (ReferenceHub x, T y)
            {
                action?.Invoke(x);
            });
        }

        public static string GetColoredName(this PlayerRoleBase role)
        {
            return "<color=" + role.RoleColor.ToHex() + ">" + role.RoleName + "</color>";
        }
    }
}
