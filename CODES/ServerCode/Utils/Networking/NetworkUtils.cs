namespace Utils.Networking
{
	public static class NetworkUtils
	{
		private static global::System.ArraySegment<byte> _segmentNonAlloc;

		public static void SendToAuthenticated<T>(this T message, int channelId = 0) where T : struct, global::Mirror.NetworkMessage
		{
			message.SendToHubsConditionally((ReferenceHub x) => x.Mode != ClientInstanceMode.Unverified, channelId);
		}

		public static void SendToHubsConditionally<T>(this T msg, global::System.Func<ReferenceHub, bool> predicate, int channelId = 0) where T : struct, global::Mirror.NetworkMessage
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Can not use SendToHubsConditionally because NetworkServer is not active!");
			}
			using (global::Mirror.PooledNetworkWriter pooledNetworkWriter = global::Mirror.NetworkWriterPool.GetWriter())
			{
				bool flag = false;
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					if (predicate(allHub))
					{
						if (!flag)
						{
							global::Mirror.MessagePacking.Pack(msg, pooledNetworkWriter);
							_segmentNonAlloc = pooledNetworkWriter.ToArraySegment();
							flag = true;
						}
						allHub.connectionToClient.Send(_segmentNonAlloc, channelId);
					}
				}
			}
		}
	}
}
