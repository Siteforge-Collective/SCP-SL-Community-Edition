using Mirror;
using UnityEngine;

namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ScannerTeamFilterSelector : Scp079AbilityBase
	{
		[SerializeField]
		private Team[] _availableFilters;

		private bool[] _selectedTeams;

		private Team[] _tempTeams;

		public Team[] SelectedTeams => null;

		public bool AnySelected => false;

		private int GetTeamIndex(Team team)
		{
			return 0;
		}

		private void ResetArray()
		{
		}

		protected override void Awake()
		{
		}

		public bool GetTeamStatus(Team team)
		{
			return false;
		}

		public void SetTeamStatus(Team team, bool status)
		{
		}

		public override void ClientWriteCmd(NetworkWriter writer)
		{
		}

		public override void ServerProcessCmd(NetworkReader reader)
		{
		}

		public override void ServerWriteRpc(NetworkWriter writer)
		{
		}

		public override void ClientProcessRpc(NetworkReader reader)
		{
		}

		public override void ResetObject()
		{
		}
	}
}
