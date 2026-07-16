using System.Collections.Generic;
using Mirror;
using PlayerRoles;

namespace Respawning.NamingRules
{
	public class NineTailedFoxNamingRule : UnitNamingRule
	{
		private readonly HashSet<int> _usedCombos;

		private static readonly string[] PossibleCodes;

		public override void GenerateNew(NetworkWriter writer)
		{
		}

		public override string ReadName(NetworkReader reader)
		{
			return null;
		}

		public override void PlayEntranceAnnouncement(string regular)
		{
		}

		public override string GetCassieUnitName(string regular)
		{
			return null;
		}

		public override int GetRolePower(RoleTypeId role)
		{
			return 0;
		}
	}
}
