public class Broadcast : global::Mirror.NetworkBehaviour
{
	[global::System.Flags]
	public enum BroadcastFlags : byte
	{
		Normal = 0,
		Truncated = 1,
		AdminChat = 2
	}

	private static Broadcast _broadcast;

	private static bool _broadcastSet;

	public static Broadcast Singleton
	{
		get
		{
			if (!_broadcastSet)
			{
				_broadcastSet = true;
				_broadcast = ReferenceHub.LocalHub.GetComponent<Broadcast>();
			}
			return _broadcast;
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this == _broadcast)
		{
			_broadcastSet = false;
		}
	}

	[global::Mirror.TargetRpc]
	public void TargetAddElement(global::Mirror.NetworkConnection conn, string data, ushort time, Broadcast.BroadcastFlags flags)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, data);
		global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, time);
		global::Mirror.GeneratedNetworkCode._Write_Broadcast_002FBroadcastFlags(writer, flags);
		SendTargetRPCInternal(conn, typeof(Broadcast), "TargetAddElement", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	public void RpcAddElement(string data, ushort time, Broadcast.BroadcastFlags flags)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, data);
		global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, time);
		global::Mirror.GeneratedNetworkCode._Write_Broadcast_002FBroadcastFlags(writer, flags);
		SendRPCInternal(typeof(Broadcast), "RpcAddElement", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.TargetRpc]
	public void TargetClearElements(global::Mirror.NetworkConnection conn)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendTargetRPCInternal(conn, typeof(Broadcast), "TargetClearElements", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	public void RpcClearElements()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(Broadcast), "RpcClearElements", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void MirrorProcessed()
	{
	}

	public void UserCode_TargetAddElement(global::Mirror.NetworkConnection conn, string data, ushort time, Broadcast.BroadcastFlags flags)
	{
	}

	protected static void InvokeUserCode_TargetAddElement(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetAddElement called on server.");
		}
		else
		{
			((Broadcast)obj).UserCode_TargetAddElement(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), global::Mirror.GeneratedNetworkCode._Read_Broadcast_002FBroadcastFlags(reader));
		}
	}

	public void UserCode_RpcAddElement(string data, ushort time, Broadcast.BroadcastFlags flags)
	{
	}

	protected static void InvokeUserCode_RpcAddElement(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcAddElement called on server.");
		}
		else
		{
			((Broadcast)obj).UserCode_RpcAddElement(global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadUInt16(reader), global::Mirror.GeneratedNetworkCode._Read_Broadcast_002FBroadcastFlags(reader));
		}
	}

	public void UserCode_TargetClearElements(global::Mirror.NetworkConnection conn)
	{
	}

	protected static void InvokeUserCode_TargetClearElements(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetClearElements called on server.");
		}
		else
		{
			((Broadcast)obj).UserCode_TargetClearElements(global::Mirror.NetworkClient.readyConnection);
		}
	}

	public void UserCode_RpcClearElements()
	{
	}

	protected static void InvokeUserCode_RpcClearElements(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcClearElements called on server.");
		}
		else
		{
			((Broadcast)obj).UserCode_RpcClearElements();
		}
	}

	static Broadcast()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(Broadcast), "RpcAddElement", InvokeUserCode_RpcAddElement);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(Broadcast), "RpcClearElements", InvokeUserCode_RpcClearElements);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(Broadcast), "TargetAddElement", InvokeUserCode_TargetAddElement);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(Broadcast), "TargetClearElements", InvokeUserCode_TargetClearElements);
	}
}
