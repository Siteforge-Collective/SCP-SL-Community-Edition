namespace RemoteAdmin.Communication
{
	public class RaGlobalBan : global::RemoteAdmin.Interfaces.IServerCommunication, global::RemoteAdmin.Interfaces.IClientCommunication
	{
		public int DataId => 5;

		public void ReceiveData(CommandSender sender, string data)
		{
			string[] array = data.Split(' ');
			if (array.Length < 2 || !int.TryParse(array[0], out var result))
			{
				return;
			}
			bool flag = result == 1;
			data = string.Join(" ", global::System.Linq.Enumerable.Skip(array, 1));
			if (!(sender is global::RemoteAdmin.PlayerCommandSender playerCommandSender) || !playerCommandSender.ServerRoles.Staff)
			{
				return;
			}
			ReferenceHub referenceHub = null;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if ((flag && $"{allHub.PlayerId}" == data) || (!flag && string.Equals(allHub.nicknameSync.MyNick, data, global::System.StringComparison.CurrentCultureIgnoreCase)))
				{
					referenceHub = allHub;
					break;
				}
			}
			if (referenceHub == null)
			{
				sender.RaReply($"${DataId} 0", success: true, logToConsole: false, string.Empty);
			}
			else
			{
				sender.RaReply($"${DataId} 1 {referenceHub.characterClassManager.AuthToken}", success: true, logToConsole: false, string.Empty);
			}
		}

		public void ReceiveData(string data, bool secure)
		{
		}
	}
}
