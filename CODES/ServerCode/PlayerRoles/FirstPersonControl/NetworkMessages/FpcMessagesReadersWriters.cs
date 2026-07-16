namespace PlayerRoles.FirstPersonControl.NetworkMessages
{
	public static class FpcMessagesReadersWriters
	{
		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			CustomNetworkManager.OnClientReady += delegate
			{
				global::Mirror.NetworkClient.ReplaceHandler((global::System.Action<global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage>)delegate
				{
				}, true);
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage msg)
				{
					msg.ProcessMessage();
				});
				global::Mirror.NetworkClient.ReplaceHandler(delegate(global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage msg)
				{
					msg.ProcessMessage();
				});
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage msg)
				{
					msg.ProcessMessage(conn);
				});
				global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection conn, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcNoclipToggleMessage msg)
				{
					msg.ProcessMessage(conn);
				});
			};
		}

		public static void WriteFpcFromClientMessage(this global::Mirror.NetworkWriter writer, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage msg)
		{
			msg.Write(writer);
		}

		public static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage ReadFpcFromClientMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFromClientMessage(reader);
		}

		public static void WriteFpcPositionMessage(this global::Mirror.NetworkWriter writer, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage msg)
		{
			msg.Write(writer);
		}

		public static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage ReadFpcPositionMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcPositionMessage(reader);
		}

		public static void WriteFpcOverrideMessage(this global::Mirror.NetworkWriter writer, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage msg)
		{
			msg.Write(writer);
		}

		public static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage ReadFpcOverrideMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcOverrideMessage(reader);
		}

		public static void WriteFpcFallDamageMessage(this global::Mirror.NetworkWriter writer, global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage msg)
		{
			msg.Write(writer);
		}

		public static global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage ReadFpcFallDamageMessage(this global::Mirror.NetworkReader reader)
		{
			return new global::PlayerRoles.FirstPersonControl.NetworkMessages.FpcFallDamageMessage(reader);
		}
	}
}
