using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class RaClipboard : IClientCommunication
	{
		public enum RaClipBoardType
		{
			Ip = 0,
			UserId = 1,
			PlayerId = 2
		}

		public static string UserIds;

		public static string PlayerIps;

		public static string PlayerIds;

		public int DataId => 0;

		public static void Reset()
		{
		}

		public void ReceiveData(string data, bool secure = true)
		{
		}

		public static void Send(CommandSender sender, RaClipBoardType type, string data)
		{
		}
	}
}
