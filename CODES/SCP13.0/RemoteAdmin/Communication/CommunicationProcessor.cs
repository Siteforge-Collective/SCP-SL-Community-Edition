using System.Collections.Generic;
using RemoteAdmin.Interfaces;

namespace RemoteAdmin.Communication
{
	public class CommunicationProcessor
	{
		public static readonly Dictionary<int, IClientCommunication> ClientCommunication;

		public static readonly Dictionary<int, IServerCommunication> ServerCommunication;

		public static T RequestClientChannel<T>() where T : IClientCommunication
		{
			return default(T);
		}

		public static T RequestServerChannel<T>() where T : IServerCommunication
		{
			return default(T);
		}
	}
}
