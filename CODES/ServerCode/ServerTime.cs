public class ServerTime : global::Mirror.NetworkBehaviour
{
	[global::Mirror.SyncVar]
	public int timeFromStartup;

	public static int time;

	private const int AllowedDeviation = 2;

	private bool _rateLimit;

	public int NetworktimeFromStartup
	{
		get
		{
			return timeFromStartup;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref timeFromStartup))
			{
				int num = timeFromStartup;
				SetSyncVar(value, ref timeFromStartup, 1uL);
			}
		}
	}

	public static bool CheckSynchronization(int myTime)
	{
		int num = global::UnityEngine.Mathf.Abs(myTime - time);
		if (num > 2)
		{
			global::GameCore.Console.AddLog("Damage sync error.", new global::UnityEngine.Color32(byte.MaxValue, 200, 0, byte.MaxValue));
		}
		return num <= 2;
	}

	private void Update()
	{
		_rateLimit = false;
		if (timeFromStartup != 0)
		{
			time = timeFromStartup;
		}
	}

	private void Start()
	{
		if (base.isLocalPlayer && global::Mirror.NetworkServer.active)
		{
			InvokeRepeating("IncreaseTime", 1f, 1f);
		}
	}

	private void IncreaseTime()
	{
		NetworktimeFromStartup = timeFromStartup + 1;
	}

	private void MirrorProcessed()
	{
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, timeFromStartup);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteInt32(writer, timeFromStartup);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			int num = timeFromStartup;
			NetworktimeFromStartup = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
			return;
		}
		long num2 = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num2 & 1L) != 0L)
		{
			int num3 = timeFromStartup;
			NetworktimeFromStartup = global::Mirror.NetworkReaderExtensions.ReadInt32(reader);
		}
	}
}
