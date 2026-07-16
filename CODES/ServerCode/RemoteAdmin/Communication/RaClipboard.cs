namespace RemoteAdmin.Communication
{
	public class RaClipboard : global::RemoteAdmin.Interfaces.IClientCommunication
	{
		public enum RaClipBoardType
		{
			Ip = 0,
			UserId = 1,
			PlayerId = 2
		}

		public int DataId => 6;

		public void ReceiveData(string data, bool secure = true)
		{
		}

		public static void Send(CommandSender sender, global::RemoteAdmin.Communication.RaClipboard.RaClipBoardType type, string data)
		{
			sender.RaReply($"$6 {(int)type} {data}", success: true, logToConsole: false, string.Empty);
		}
	}
}
