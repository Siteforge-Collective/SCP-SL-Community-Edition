namespace RemoteAdmin.Communication
{
	public class RaPlayerAuth : global::RemoteAdmin.Interfaces.IServerCommunication
	{
		public int DataId => 3;

		public void ReceiveData(CommandSender sender, string data)
		{
			if (sender is global::RemoteAdmin.PlayerCommandSender playerCommandSender && !playerCommandSender.ServerRoles.Staff && !global::RemoteAdmin.CommandProcessor.CheckPermissions(sender, PlayerPermissions.PlayerSensitiveDataAccess))
			{
				return;
			}
			string[] newargs;
			global::System.Collections.Generic.List<ReferenceHub> list = global::Utils.RAUtils.ProcessPlayerIdOrNamesList(new global::System.ArraySegment<string>(data.Split(' ')), 0, out newargs);
			if (list.Count != 0 && list.Count <= 1)
			{
				if (string.IsNullOrEmpty(list[0].characterClassManager.AuthToken))
				{
					sender.RaReply("PlayerInfo#Can't obtain auth token. Is server using offline mode or you selected the host?", success: false, logToConsole: true, "PlayerInfo");
					return;
				}
				ServerLogs.AddLog(ServerLogs.Modules.DataAccess, $"{sender.LogName} accessed authentication token of player {list[0].PlayerId} ({list[0].nicknameSync.MyNick}).", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
				sender.RaReply($"PlayerInfo#<color=white>Authentication token of player {list[0].nicknameSync.MyNick} ({list[0].PlayerId}):\n{list[0].characterClassManager.AuthToken}</color>", success: true, logToConsole: true, "null");
				global::RemoteAdmin.Communication.RaPlayerQR.Send(sender, isBig: true, list[0].characterClassManager.AuthToken);
			}
		}
	}
}
