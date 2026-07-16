namespace RemoteAdmin.Communication
{
	public class CommunicationProcessor
	{
		public static readonly global::System.Collections.Generic.Dictionary<int, global::RemoteAdmin.Interfaces.IServerCommunication> ServerCommunication = new global::System.Collections.Generic.Dictionary<int, global::RemoteAdmin.Interfaces.IServerCommunication>
		{
			{
				0,
				new global::RemoteAdmin.Communication.RaPlayerList()
			},
			{
				1,
				new global::RemoteAdmin.Communication.RaPlayer()
			},
			{
				3,
				new global::RemoteAdmin.Communication.RaPlayerAuth()
			},
			{
				5,
				new global::RemoteAdmin.Communication.RaGlobalBan()
			},
			{
				7,
				new global::RemoteAdmin.Communication.RaServerStatus()
			},
			{
				8,
				new global::RemoteAdmin.Communication.RaTeamStatus()
			}
		};

		public static T RequestServerChannel<T>() where T : global::RemoteAdmin.Interfaces.IServerCommunication
		{
			foreach (global::RemoteAdmin.Interfaces.IServerCommunication value in ServerCommunication.Values)
			{
				global::RemoteAdmin.Interfaces.IServerCommunication current;
				if ((current = value) is T)
				{
					return (T)current;
				}
			}
			return default(T);
		}
	}
}
