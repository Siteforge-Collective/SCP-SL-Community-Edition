public class SqueakSpawner : global::Mirror.NetworkBehaviour
{
	[global::UnityEngine.SerializeField]
	private int spawnChancePercent = 10;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject[] mice;

	[global::Mirror.SyncVar(hook = "SyncMouseSpawn")]
	private byte syncSpawn;

	private global::Interactables.Interobjects.SqueakInteraction _spawnedMouse;

	public byte NetworksyncSpawn
	{
		get
		{
			return syncSpawn;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref syncSpawn))
			{
				byte oldValue = syncSpawn;
				SetSyncVar(value, ref syncSpawn, 1uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
				{
					setSyncVarHookGuard(1uL, value: true);
					SyncMouseSpawn(oldValue, value);
					setSyncVarHookGuard(1uL, value: false);
				}
			}
		}
	}

	private void Awake()
	{
		if (global::Mirror.NetworkServer.active && global::UnityEngine.Random.Range(0, 100) <= spawnChancePercent)
		{
			NetworksyncSpawn = (byte)global::UnityEngine.Random.Range(1, mice.Length + 1);
			SyncMouseSpawn(0, syncSpawn);
		}
	}

	[global::Mirror.TargetRpc]
	public void TargetHitMouse(global::Mirror.NetworkConnection networkConnection)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendTargetRPCInternal(networkConnection, typeof(SqueakSpawner), "TargetHitMouse", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void SyncMouseSpawn(byte oldValue, byte newValue)
	{
		if (newValue != 0)
		{
			global::UnityEngine.GameObject gameObject = mice[newValue - 1];
			gameObject.SetActive(value: true);
			_spawnedMouse = gameObject.GetComponent<global::Interactables.Interobjects.SqueakInteraction>();
		}
	}

	private void MirrorProcessed()
	{
	}

	public void UserCode_TargetHitMouse(global::Mirror.NetworkConnection networkConnection)
	{
	}

	protected static void InvokeUserCode_TargetHitMouse(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetHitMouse called on server.");
		}
		else
		{
			((SqueakSpawner)obj).UserCode_TargetHitMouse(global::Mirror.NetworkClient.readyConnection);
		}
	}

	static SqueakSpawner()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(SqueakSpawner), "TargetHitMouse", InvokeUserCode_TargetHitMouse);
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, syncSpawn);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, syncSpawn);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			byte b = syncSpawn;
			NetworksyncSpawn = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			if (!SyncVarEqual(b, ref syncSpawn))
			{
				SyncMouseSpawn(b, syncSpawn);
			}
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			byte b2 = syncSpawn;
			NetworksyncSpawn = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			if (!SyncVarEqual(b2, ref syncSpawn))
			{
				SyncMouseSpawn(b2, syncSpawn);
			}
		}
	}
}
