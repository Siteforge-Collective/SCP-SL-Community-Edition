using System;
using Mirror;
using UnityEngine;

namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public static class FpcMessagesReadersWriters
	{
		[RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				NetworkClient.ReplaceHandler((Action<FpcPositionMessage>)delegate
				{
				}, true);
				NetworkClient.ReplaceHandler(delegate(FpcOverrideMessage msg)
				{
					msg.ProcessMessage();
				});
				NetworkClient.ReplaceHandler(delegate(FpcFallDamageMessage msg)
				{
					msg.ProcessMessage();
				});
				NetworkServer.ReplaceHandler(delegate(NetworkConnectionToClient conn, FpcFromClientMessage msg)
				{
					msg.ProcessMessage(conn);
				});
				NetworkServer.ReplaceHandler(delegate(NetworkConnectionToClient conn, FpcNoclipToggleMessage msg)
				{
					msg.ProcessMessage(conn);
				});
			};
		}

		public static void WriteFpcFromClientMessage(this NetworkWriter writer, FpcFromClientMessage msg)
		{
			msg.Write(writer);
		}

		public static FpcFromClientMessage ReadFpcFromClientMessage(this NetworkReader reader)
		{
			return new FpcFromClientMessage(reader);
		}

		public static void WriteFpcPositionMessage(this NetworkWriter writer, FpcPositionMessage msg)
		{
			msg.Write(writer);
		}

		public static FpcPositionMessage ReadFpcPositionMessage(this NetworkReader reader)
		{
			return new FpcPositionMessage(reader);
		}

		public static void WriteFpcOverrideMessage(this NetworkWriter writer, FpcOverrideMessage msg)
		{
			msg.Write(writer);
		}

		public static FpcOverrideMessage ReadFpcOverrideMessage(this NetworkReader reader)
		{
			return new FpcOverrideMessage(reader);
		}

		public static void WriteFpcFallDamageMessage(this NetworkWriter writer, FpcFallDamageMessage msg)
		{
			msg.Write(writer);
		}

		public static FpcFallDamageMessage ReadFpcFallDamageMessage(this NetworkReader reader)
		{
			return new FpcFallDamageMessage(reader);
		}
	}
}
