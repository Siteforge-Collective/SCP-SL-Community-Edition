using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class RaPlayerAuth : IServerCommunication
	{
		public int DataId => 0;

		public void ReceiveData(CommandSender sender, string data)
		{
		}

		public static void Request(string playerIds)
		{
		}
	}
}
