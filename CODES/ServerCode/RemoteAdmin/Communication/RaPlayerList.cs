namespace RemoteAdmin.Communication
{
	public class RaPlayerList : global::RemoteAdmin.Interfaces.IServerCommunication, global::RemoteAdmin.Interfaces.IClientCommunication
	{
		public enum PlayerSorting
		{
			Ids = 0,
			Alphabetical = 1,
			Class = 2,
			Team = 3
		}

		private const string OverwatchBadge = "<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uf06e</color><color=white>]</color></link> ";

		public int DataId => 0;

		public void ReceiveData(CommandSender sender, string data)
		{
			string[] array = data.Split(' ');
			if (array.Length != 3 || !int.TryParse(array[0], out var result) || !int.TryParse(array[1], out var result2) || !global::System.Enum.IsDefined(typeof(global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting), result2))
			{
				return;
			}
			bool flag = result == 1;
			bool num = array[2].Equals("1");
			global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting sortingType = (global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting)result2;
			bool viewHiddenBadges = global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
			bool viewHiddenGlobalBadges = global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);
			if (sender is global::RemoteAdmin.PlayerCommandSender playerCommandSender && playerCommandSender.ServerRoles.Staff)
			{
				viewHiddenBadges = true;
				viewHiddenGlobalBadges = true;
			}
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent("\n");
			foreach (ReferenceHub item in num ? SortPlayersDescending(sortingType) : SortPlayers(sortingType))
			{
				if (item.Mode != ClientInstanceMode.DedicatedServer && item.Mode != ClientInstanceMode.Unverified)
				{
					bool flag2 = false;
					stringBuilder.Append(GetPrefix(item, viewHiddenBadges, viewHiddenGlobalBadges));
					stringBuilder.Append(flag2 ? "<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uf06e</color><color=white>]</color></link> " : string.Empty);
					stringBuilder.Append("<color={RA_ClassColor}>(").Append(item.PlayerId).Append(") ");
					stringBuilder.Append(item.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");
					stringBuilder.AppendLine();
				}
			}
			sender.RaReply($"${DataId} {(global::NorthwoodLib.Pools.StringBuilderPool.Shared.ToStringReturn(stringBuilder))}", success: true, !flag, string.Empty);
		}

		private global::System.Collections.Generic.IEnumerable<ReferenceHub> SortPlayers(global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting sortingType)
		{
			switch (sortingType)
			{
			case global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting.Team:
				return global::System.Linq.Enumerable.OrderBy(ReferenceHub.AllHubs, (ReferenceHub h) => h.roleManager.CurrentRole.Team);
			case global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting.Alphabetical:
				return global::System.Linq.Enumerable.OrderBy(ReferenceHub.AllHubs, (ReferenceHub h) => h.nicknameSync.DisplayName ?? h.nicknameSync.MyNick);
			default:
				return global::System.Linq.Enumerable.OrderBy(ReferenceHub.AllHubs, (ReferenceHub h) => h.PlayerId);
			}
		}

		private global::System.Collections.Generic.IEnumerable<ReferenceHub> SortPlayersDescending(global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting sortingType)
		{
			switch (sortingType)
			{
			case global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting.Team:
				return global::System.Linq.Enumerable.OrderByDescending(ReferenceHub.AllHubs, (ReferenceHub h) => h.roleManager.CurrentRole.Team);
			case global::RemoteAdmin.Communication.RaPlayerList.PlayerSorting.Alphabetical:
				return global::System.Linq.Enumerable.OrderByDescending(ReferenceHub.AllHubs, (ReferenceHub h) => h.nicknameSync.DisplayName ?? h.nicknameSync.MyNick);
			default:
				return global::System.Linq.Enumerable.OrderByDescending(ReferenceHub.AllHubs, (ReferenceHub h) => h.PlayerId);
			}
		}

		private string GetPrefix(ReferenceHub hub, bool viewHiddenBadges = false, bool viewHiddenGlobalBadges = false)
		{
			ServerRoles serverRoles = hub.serverRoles;
			if (!string.IsNullOrEmpty(serverRoles.HiddenBadge) && (!serverRoles.GlobalHidden || !viewHiddenBadges) && (serverRoles.GlobalHidden || !viewHiddenGlobalBadges))
			{
				return string.Empty;
			}
			if (serverRoles.RaEverywhere)
			{
				return "<link=RA_RaEverywhere><color=white>[<color=#EFC01A>\uf3ed</color><color=white>]</color></link> ";
			}
			if (serverRoles.Staff)
			{
				return "<link=RA_StudioStaff><color=white>[<color=#005EBC>\uf0ad</color><color=white>]</color></link> ";
			}
			if (serverRoles.RemoteAdmin)
			{
				return "<link=RA_Admin><color=white>[\uf406]</color></link> ";
			}
			return string.Empty;
		}

		public void ReceiveData(string data, bool secure)
		{
		}
	}
}
