[global::UnityEngine.RequireComponent(typeof(global::Mirror.NetworkIdentity))]
public class BlastDoor : global::Mirror.NetworkBehaviour
{
	public static readonly global::System.Collections.Generic.HashSet<BlastDoor> Instances = new global::System.Collections.Generic.HashSet<BlastDoor>();

	private static readonly int _close = global::UnityEngine.Animator.StringToHash("Close");

	[global::Mirror.SyncVar(hook = "SetClosed")]
	public bool isClosed;

	public bool NetworkisClosed
	{
		get
		{
			return isClosed;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref isClosed))
			{
				bool prev = isClosed;
				SetSyncVar(value, ref isClosed, 1uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
				{
					setSyncVarHookGuard(1uL, value: true);
					SetClosed(prev, value);
					setSyncVarHookGuard(1uL, value: false);
				}
			}
		}
	}

	private void Start()
	{
		Instances.Add(this);
	}

	private void OnDestroy()
	{
		Instances.Remove(this);
	}

	public void SetClosed(bool prev, bool b)
	{
		NetworkisClosed = b;
		if (isClosed)
		{
			GetComponent<global::UnityEngine.Animator>().SetTrigger(_close);
		}
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, isClosed);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, isClosed);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			bool flag = isClosed;
			NetworkisClosed = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(flag, ref isClosed))
			{
				SetClosed(flag, isClosed);
			}
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			bool flag2 = isClosed;
			NetworkisClosed = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(flag2, ref isClosed))
			{
				SetClosed(flag2, isClosed);
			}
		}
	}
}
