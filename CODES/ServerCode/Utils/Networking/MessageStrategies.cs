namespace Utils.Networking
{
	public static class MessageStrategies<TSelf>
	{
		public delegate global::Mirror.NetworkIdentity SelfToIdentity(TSelf self);

		public delegate bool ConnectionToSelf(global::Mirror.NetworkConnection source, out TSelf self);

		public delegate void MessageHandler<in TMessage>(TSelf self, TMessage message);

		public static global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> ToClient<TMessage>(global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> handler, global::Utils.Networking.MessageStrategies<TSelf>.SelfToIdentity converter) where TMessage : struct, global::Mirror.NetworkMessage
		{
			return delegate(TSelf self, TMessage message)
			{
				global::Mirror.NetworkIdentity networkIdentity = converter(self);
				if (networkIdentity.isLocalPlayer)
				{
					handler(self, message);
				}
				else
				{
					global::Mirror.NetworkServer.SendToClientOfPlayer(networkIdentity, message);
				}
			};
		}

		public static void RegisterFromClient<TMessage>(global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> handler, global::Utils.Networking.MessageStrategies<TSelf>.ConnectionToSelf converter) where TMessage : struct, global::Mirror.NetworkMessage
		{
			global::Mirror.NetworkServer.ReplaceHandler(delegate(global::Mirror.NetworkConnection connection, TMessage message)
			{
				if (converter(connection, out var self))
				{
					handler(self, message);
				}
			});
		}

		public static global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> ToServer<TMessage>(global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> handler, global::Utils.Networking.MessageStrategies<TSelf>.SelfToIdentity converter) where TMessage : struct, global::Mirror.NetworkMessage
		{
			return delegate(TSelf self, TMessage message)
			{
				if (converter(self).isLocalPlayer)
				{
					handler(self, message);
				}
				else
				{
					global::Mirror.NetworkClient.Send(message);
				}
			};
		}

		public static void RegisterFromServer<TMessage>(global::Utils.Networking.MessageStrategies<TSelf>.MessageHandler<TMessage> handler, TSelf self) where TMessage : struct, global::Mirror.NetworkMessage
		{
			global::Mirror.NetworkClient.ReplaceHandler(delegate(global::Mirror.NetworkConnection connection, TMessage message)
			{
				handler(self, message);
			});
		}
	}
}
