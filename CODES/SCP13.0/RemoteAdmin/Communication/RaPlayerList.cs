using System.Collections.Generic;
using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class RaPlayerList : IServerCommunication, IClientCommunication
	{
		public enum PlayerSorting
		{
			Ids = 0,
			Alphabetical = 1,
			Class = 2,
			Team = 3
		}

		private const string OverwatchBadge = "<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uf06e</color><color=white>]</color></link> ";

		private const string MutedBadge = "<link=RA_Muted><color=white>[</color>\ud83d\udd07<color=white>]</color></link> ";

		public int DataId => 0;

		public void ReceiveData(CommandSender sender, string data)
		{
		}

		private IEnumerable<ReferenceHub> SortPlayers(PlayerSorting sortingType)
		{
			return null;
		}

		private IEnumerable<ReferenceHub> SortPlayersDescending(PlayerSorting sortingType)
		{
			return null;
		}

		private static string GetPrefix(ReferenceHub hub, bool viewHiddenBadges = false, bool viewHiddenGlobalBadges = false)
		{
			return null;
		}

		public void ReceiveData(string data, bool secure)
		{
		}

		public static void Request(bool isSilent, PlayerSorting sortingType = PlayerSorting.Ids, bool isDescending = false)
		{
		}
	}
}
