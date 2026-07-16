public class VersionCheck : global::Mirror.NetworkBehaviour
{
	private void Start()
	{
		if (global::Mirror.NetworkServer.active && (!global::GameCore.Version.AlwaysAcceptReleaseBuilds || global::GameCore.Version.BuildType != global::GameCore.Version.VersionType.Release))
		{
			if (global::GameCore.Version.ExtendedVersionCheckNeeded)
			{
				TargetCheckExactVersion(base.connectionToClient, global::GameCore.Version.VersionString);
			}
			else
			{
				TargetCheckVersion(base.connectionToClient, global::GameCore.Version.Major, global::GameCore.Version.Minor, global::GameCore.Version.Revision);
			}
		}
	}

	[global::Mirror.TargetRpc]
	private void TargetCheckVersion(global::Mirror.NetworkConnection conn, byte major, byte minor, byte revision)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteByte(writer, major);
		global::Mirror.NetworkWriterExtensions.WriteByte(writer, minor);
		global::Mirror.NetworkWriterExtensions.WriteByte(writer, revision);
		SendTargetRPCInternal(conn, typeof(VersionCheck), "TargetCheckVersion", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.TargetRpc]
	private void TargetCheckExactVersion(global::Mirror.NetworkConnection conn, string version)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, version);
		SendTargetRPCInternal(conn, typeof(VersionCheck), "TargetCheckExactVersion", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_TargetCheckVersion(global::Mirror.NetworkConnection conn, byte major, byte minor, byte revision)
	{
	}

	protected static void InvokeUserCode_TargetCheckVersion(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetCheckVersion called on server.");
		}
		else
		{
			((VersionCheck)obj).UserCode_TargetCheckVersion(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadByte(reader), global::Mirror.NetworkReaderExtensions.ReadByte(reader), global::Mirror.NetworkReaderExtensions.ReadByte(reader));
		}
	}

	private void UserCode_TargetCheckExactVersion(global::Mirror.NetworkConnection conn, string version)
	{
	}

	protected static void InvokeUserCode_TargetCheckExactVersion(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetCheckExactVersion called on server.");
		}
		else
		{
			((VersionCheck)obj).UserCode_TargetCheckExactVersion(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	static VersionCheck()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(VersionCheck), "TargetCheckVersion", InvokeUserCode_TargetCheckVersion);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(VersionCheck), "TargetCheckExactVersion", InvokeUserCode_TargetCheckExactVersion);
	}
}
