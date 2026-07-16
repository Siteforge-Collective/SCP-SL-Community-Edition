using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class RaPlayerQR : IClientCommunication
	{
		public int DataId => 0;

		public void ReceiveData(string data, bool secure)
		{
		}

		public static void Send(CommandSender sender, bool isBig, string data)
		{
		}
	}
}
