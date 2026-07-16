public class AspectRatioSync : global::Mirror.NetworkBehaviour
{
	private static float _defaultCameraFieldOfView;

	[global::System.Runtime.CompilerServices.CompilerGenerated]
	[global::Mirror.SyncVar]
	private float _003CXScreenEdge_003Ek__BackingField;

	private int _savedWidth;

	private int _savedHeight;

	public static float YScreenEdge { get; private set; }

	public float XScreenEdge
	{
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		get
		{
			return _003CXScreenEdge_003Ek__BackingField;
		}
		[global::System.Runtime.CompilerServices.CompilerGenerated]
		private set
		{
			Network_003CXScreenEdge_003Ek__BackingField = value;
		}
	}

	public float XplusY { get; private set; }

	public float AspectRatio { get; private set; }

	public float Network_003CXScreenEdge_003Ek__BackingField
	{
		get
		{
			return XScreenEdge;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref XScreenEdge))
			{
				float num = XScreenEdge;
				SetSyncVar(value, ref XScreenEdge, 1uL);
			}
		}
	}

	public static event global::System.Action OnAspectRatioChanged;

	private void Start()
	{
		if (base.isLocalPlayer)
		{
			global::UnityEngine.Camera component = GetComponent<ReferenceHub>().PlayerCameraReference.GetComponent<global::UnityEngine.Camera>();
			_defaultCameraFieldOfView = ((component == null) ? 70f : component.fieldOfView);
			YScreenEdge = _defaultCameraFieldOfView / 2f;
		}
	}

	private void UpdateAspectRatio()
	{
		_savedWidth = global::UnityEngine.Screen.width;
		_savedHeight = global::UnityEngine.Screen.height;
		float aspectRatio = (float)global::UnityEngine.Screen.width / (float)global::UnityEngine.Screen.height;
		CmdSetAspectRatio(aspectRatio);
	}

	private void FixedUpdate()
	{
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdSetAspectRatio(float aspectRatio)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteSingle(writer, aspectRatio);
		SendCommandInternal(typeof(AspectRatioSync), "CmdSetAspectRatio", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	public AspectRatioSync()
	{
		XScreenEdge = 35f;
		XplusY = 70f;
		AspectRatio = 1f;
		base._002Ector();
	}

	static AspectRatioSync()
	{
		YScreenEdge = 35f;
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(AspectRatioSync), "CmdSetAspectRatio", InvokeUserCode_CmdSetAspectRatio, requiresAuthority: true);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_CmdSetAspectRatio(float aspectRatio)
	{
		if (aspectRatio < 1f)
		{
			aspectRatio = 1f;
		}
		AspectRatio = aspectRatio;
		float num = global::UnityEngine.Mathf.Tan(_defaultCameraFieldOfView * ((float)global::System.Math.PI / 180f) * 0.5f);
		Network_003CXScreenEdge_003Ek__BackingField = global::UnityEngine.Mathf.Atan(num * aspectRatio) * 57.29578f;
		XplusY = XScreenEdge + YScreenEdge;
	}

	protected static void InvokeUserCode_CmdSetAspectRatio(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSetAspectRatio called on client.");
		}
		else
		{
			((AspectRatioSync)obj).UserCode_CmdSetAspectRatio(global::Mirror.NetworkReaderExtensions.ReadSingle(reader));
		}
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, XScreenEdge);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, XScreenEdge);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			float num = XScreenEdge;
			Network_003CXScreenEdge_003Ek__BackingField = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			return;
		}
		long num2 = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num2 & 1L) != 0L)
		{
			float num3 = XScreenEdge;
			Network_003CXScreenEdge_003Ek__BackingField = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}
	}
}
