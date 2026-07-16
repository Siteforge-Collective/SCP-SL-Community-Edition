using System.Text;
using PlayerRoles;
using TMPro;
using UnityEngine;

namespace Respawning.NamingRules
{
	public class UnitNamingHud : MonoBehaviour
	{
		public TextMeshProUGUI UnitsList;

		private SpawnableTeamType _localTeam;

		private int _localIndex;

		private static readonly StringBuilder StrBuilder;

		private void AppendUnit(int index, string unitName)
		{
		}

		private void OnRoleChanged(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
		{
		}

		private void OnMsgReceived(SpawnableTeamType team, string unit, int index)
		{
		}

		private void OnDestroy()
		{
		}

		private void Start()
		{
		}
	}
}
