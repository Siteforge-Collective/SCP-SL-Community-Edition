using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class RaPlayer : IServerCommunication, IClientCommunication
	{
		public int DataId => 0;

		public void ReceiveData(CommandSender sender, string data)
		{
		}

		public void ReceiveData(string data, bool secure)
		{
		}

		public static void Request(bool isShort, string playerIds)
		{
		}
	}
}
