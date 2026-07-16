using System.Collections.Generic;
using System.Text;
using Mirror;
using PlayerRoles;
using UnityEngine;

namespace Respawning.NamingRules
{
	public abstract class UnitNamingRule
	{
        private static readonly Dictionary<SpawnableTeamType, UnitNamingRule> AllNamingRules = new() 
		{ 
			[SpawnableTeamType.NineTailedFox] = new NineTailedFoxNamingRule() 
		};

        public static bool TryGetNamingRule(SpawnableTeamType type, out UnitNamingRule rule)
        {
            return AllNamingRules.TryGetValue(type, out rule);
        }

        public virtual void AppendName(StringBuilder sb, string unitName, RoleTypeId theirRole, PlayerInfoArea infoFlags)
        {
            if (!ReferenceHub.TryGetLocalHub(out var hub) || !(hub.roleManager.CurrentRole is HumanRole humanRole))
            {
                return;
            }
            if ((infoFlags & PlayerInfoArea.UnitName) == PlayerInfoArea.UnitName)
            {
                sb.Append(" (");
                sb.Append(unitName);
                sb.Append(")");
            }
            sb.AppendLine("\n");
            if ((infoFlags & PlayerInfoArea.PowerStatus) == PlayerInfoArea.PowerStatus)
            {
                int rolePower = GetRolePower(humanRole.RoleTypeId);
                int rolePower2 = GetRolePower(theirRole);
                sb.Append("<b>");
                if (rolePower > rolePower2)
                {
                    sb.Append(Translations.Get(LegacyInterfaces.GiveOrders));
                }
                else if (rolePower < rolePower2)
                {
                    sb.Append(Translations.Get(LegacyInterfaces.FollowOrders));
                }
                else if (rolePower == rolePower2)
                {
                    sb.Append(Translations.Get(LegacyInterfaces.SameRank));
                }
                sb.Append("</b>");
            }
        }

        public virtual string GetCassieUnitName(string regular)
        {
            return string.Empty;
        }

        public virtual void PlayEntranceAnnouncement(string regular)
        {
        }

        public abstract void GenerateNew(NetworkWriter writer);

        public abstract string ReadName(NetworkReader reader);

        public abstract int GetRolePower(RoleTypeId role);

        internal void ConfirmAnnouncement(string completeAnnouncement)
        {
            float num = (AlphaWarheadController.Detonated ? 2.5f : 1f);
            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(completeAnnouncement, Random.Range(0.08f, 0.1f) * num, Random.Range(0.07f, 0.09f) * num);
        }

        internal void ConfirmAnnouncement(ref StringBuilder sb)
        {
            ConfirmAnnouncement(sb.ToString());
        }
    }
}
