public class FlickerableLightController : global::Mirror.NetworkBehaviour
{
	public const float HeightRadius = 100f;

	public static bool WarheadEnabled;

	private float _flickerDuration;

	[global::Mirror.SyncVar(hook = "LightsHook")]
	public bool LightsEnabled;

	public static readonly global::UnityEngine.Color DefaultWarheadColor;

	public static readonly global::System.Collections.Generic.List<FlickerableLightController> Instances;

	private global::UnityEngine.Light[] _allLights;

	private float[] _allLightDefaultIntensity;

	private bool initializedLightIntensity;

	[global::Mirror.SyncVar(hook = "UpdateLightsIntensity")]
	private float _lightIntensityMultiplier = 1f;

	[global::Mirror.SyncVar(hook = "UpdateWarheadLight")]
	private global::UnityEngine.Color _warheadLightColor = DefaultWarheadColor;

	[global::Mirror.SyncVar(hook = "UpdateWarheadLightOverride")]
	private bool _warheadLightOverride;

	public global::MapGeneration.RoomIdentifier Room { get; private set; }

	public float LightIntensityMultiplier
	{
		get
		{
			return _lightIntensityMultiplier;
		}
		set
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Tried changing light intensity on client.");
			}
			Network_lightIntensityMultiplier = value;
		}
	}

	public global::UnityEngine.Color WarheadLightColor
	{
		get
		{
			return _warheadLightColor;
		}
		set
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.Runtime.Remoting.ServerException("Tried changing warhead light color on client.");
			}
			Network_warheadLightColor = value;
		}
	}

	public bool WarheadLightOverride
	{
		get
		{
			return _warheadLightOverride;
		}
		set
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.Runtime.Remoting.ServerException("Tried changing warhead light override on client.");
			}
			Network_warheadLightOverride = value;
		}
	}

	public bool NetworkLightsEnabled
	{
		get
		{
			return LightsEnabled;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref LightsEnabled))
			{
				bool lightsEnabled = LightsEnabled;
				SetSyncVar(value, ref LightsEnabled, 1uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
				{
					setSyncVarHookGuard(1uL, value: true);
					LightsHook(lightsEnabled, value);
					setSyncVarHookGuard(1uL, value: false);
				}
			}
		}
	}

	public float Network_lightIntensityMultiplier
	{
		get
		{
			return _lightIntensityMultiplier;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _lightIntensityMultiplier))
			{
				float lightIntensityMultiplier = _lightIntensityMultiplier;
				SetSyncVar(value, ref _lightIntensityMultiplier, 2uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(2uL))
				{
					setSyncVarHookGuard(2uL, value: true);
					UpdateLightsIntensity(lightIntensityMultiplier, value);
					setSyncVarHookGuard(2uL, value: false);
				}
			}
		}
	}

	public global::UnityEngine.Color Network_warheadLightColor
	{
		get
		{
			return _warheadLightColor;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _warheadLightColor))
			{
				global::UnityEngine.Color warheadLightColor = _warheadLightColor;
				SetSyncVar(value, ref _warheadLightColor, 4uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(4uL))
				{
					setSyncVarHookGuard(4uL, value: true);
					UpdateWarheadLight(warheadLightColor, value);
					setSyncVarHookGuard(4uL, value: false);
				}
			}
		}
	}

	public bool Network_warheadLightOverride
	{
		get
		{
			return _warheadLightOverride;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _warheadLightOverride))
			{
				bool warheadLightOverride = _warheadLightOverride;
				SetSyncVar(value, ref _warheadLightOverride, 8uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(8uL))
				{
					setSyncVarHookGuard(8uL, value: true);
					UpdateWarheadLightOverride(warheadLightOverride, value);
					setSyncVarHookGuard(8uL, value: false);
				}
			}
		}
	}

	private void Awake()
	{
		WarheadEnabled = false;
	}

	private void Start()
	{
		if (global::Mirror.NetworkServer.active)
		{
			NetworkLightsEnabled = true;
		}
	}

	private void OnEnable()
	{
		Instances.Add(this);
	}

	private void OnDisable()
	{
		Instances.Remove(this);
	}

	[global::Mirror.Server]
	public static void ServerSendDataToClient(global::Mirror.NetworkConnection conn)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void FlickerableLightController::ServerSendDataToClient(Mirror.NetworkConnection)' called when server was not active");
			return;
		}
		foreach (FlickerableLightController instance in Instances)
		{
			if (!(instance == null))
			{
				if (instance._warheadLightOverride)
				{
					instance.TargetRpcUpdateWarheadOverride(conn, instance._warheadLightOverride);
				}
				if (instance._warheadLightColor != DefaultWarheadColor)
				{
					instance.TargetRpcUpdateWarheadLight(conn, instance._warheadLightColor);
				}
			}
		}
	}

	[global::Mirror.TargetRpc]
	private void TargetRpcUpdateWarheadLight(global::Mirror.NetworkConnection conn, global::UnityEngine.Color color)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteColor(writer, color);
		SendTargetRPCInternal(conn, typeof(FlickerableLightController), "TargetRpcUpdateWarheadLight", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.TargetRpc]
	private void TargetRpcUpdateWarheadOverride(global::Mirror.NetworkConnection conn, bool state)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, state);
		SendTargetRPCInternal(conn, typeof(FlickerableLightController), "TargetRpcUpdateWarheadOverride", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void FixedUpdate()
	{
		if (global::Mirror.NetworkServer.active && !(_flickerDuration <= 0f))
		{
			_flickerDuration -= global::UnityEngine.Time.fixedDeltaTime;
			if (_flickerDuration <= 0f)
			{
				SetLights(state: true);
			}
		}
	}

	private void SetLights(bool state)
	{
		if (global::Mirror.NetworkServer.active)
		{
			NetworkLightsEnabled = state;
		}
	}

	private void LightsHook(bool oldValue, bool newValue)
	{
	}

	[global::Mirror.Server]
	public void ServerFlickerLights(float dur)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void FlickerableLightController::ServerFlickerLights(System.Single)' called when server was not active");
		}
		else if (dur <= 0f)
		{
			_flickerDuration = 0f;
			SetLights(state: true);
		}
		else
		{
			_flickerDuration = dur;
			SetLights(state: false);
		}
	}

	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	private static void Init()
	{
		global::MapGeneration.SeedSynchronizer.OnMapGenerated += delegate
		{
			foreach (FlickerableLightController instance in Instances)
			{
				instance.Room = global::MapGeneration.RoomIdUtils.RoomAtPosition(instance.transform.position);
			}
		};
	}

	public static bool IsInDarkenedRoom(global::UnityEngine.Vector3 positionToCheck)
	{
		global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPosition(positionToCheck);
		if (roomIdentifier == null)
		{
			roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(positionToCheck);
		}
		foreach (FlickerableLightController instance in Instances)
		{
			if (!instance.LightsEnabled && !(instance.Room != roomIdentifier) && !(global::UnityEngine.Mathf.Abs(instance.transform.position.y - positionToCheck.y) > 100f))
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateLightsIntensity(float oldValue, float newValue)
	{
	}

	private void UpdateWarheadLight(global::UnityEngine.Color oldColor, global::UnityEngine.Color newColor)
	{
	}

	private void UpdateWarheadLightOverride(bool oldState, bool newState)
	{
	}

	static FlickerableLightController()
	{
		DefaultWarheadColor = new global::UnityEngine.Color(1f, 0.2f, 0.2f);
		Instances = new global::System.Collections.Generic.List<FlickerableLightController>();
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(FlickerableLightController), "TargetRpcUpdateWarheadLight", InvokeUserCode_TargetRpcUpdateWarheadLight);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(FlickerableLightController), "TargetRpcUpdateWarheadOverride", InvokeUserCode_TargetRpcUpdateWarheadOverride);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_TargetRpcUpdateWarheadLight(global::Mirror.NetworkConnection conn, global::UnityEngine.Color color)
	{
	}

	protected static void InvokeUserCode_TargetRpcUpdateWarheadLight(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetRpcUpdateWarheadLight called on server.");
		}
		else
		{
			((FlickerableLightController)obj).UserCode_TargetRpcUpdateWarheadLight(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadColor(reader));
		}
	}

	private void UserCode_TargetRpcUpdateWarheadOverride(global::Mirror.NetworkConnection conn, bool state)
	{
	}

	protected static void InvokeUserCode_TargetRpcUpdateWarheadOverride(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetRpcUpdateWarheadOverride called on server.");
		}
		else
		{
			((FlickerableLightController)obj).UserCode_TargetRpcUpdateWarheadOverride(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, LightsEnabled);
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _lightIntensityMultiplier);
			global::Mirror.NetworkWriterExtensions.WriteColor(writer, _warheadLightColor);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _warheadLightOverride);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, LightsEnabled);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, _lightIntensityMultiplier);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteColor(writer, _warheadLightColor);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _warheadLightOverride);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			bool lightsEnabled = LightsEnabled;
			NetworkLightsEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(lightsEnabled, ref LightsEnabled))
			{
				LightsHook(lightsEnabled, LightsEnabled);
			}
			float lightIntensityMultiplier = _lightIntensityMultiplier;
			Network_lightIntensityMultiplier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			if (!SyncVarEqual(lightIntensityMultiplier, ref _lightIntensityMultiplier))
			{
				UpdateLightsIntensity(lightIntensityMultiplier, _lightIntensityMultiplier);
			}
			global::UnityEngine.Color warheadLightColor = _warheadLightColor;
			Network_warheadLightColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
			if (!SyncVarEqual(warheadLightColor, ref _warheadLightColor))
			{
				UpdateWarheadLight(warheadLightColor, _warheadLightColor);
			}
			bool warheadLightOverride = _warheadLightOverride;
			Network_warheadLightOverride = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(warheadLightOverride, ref _warheadLightOverride))
			{
				UpdateWarheadLightOverride(warheadLightOverride, _warheadLightOverride);
			}
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			bool lightsEnabled2 = LightsEnabled;
			NetworkLightsEnabled = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(lightsEnabled2, ref LightsEnabled))
			{
				LightsHook(lightsEnabled2, LightsEnabled);
			}
		}
		if ((num & 2L) != 0L)
		{
			float lightIntensityMultiplier2 = _lightIntensityMultiplier;
			Network_lightIntensityMultiplier = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			if (!SyncVarEqual(lightIntensityMultiplier2, ref _lightIntensityMultiplier))
			{
				UpdateLightsIntensity(lightIntensityMultiplier2, _lightIntensityMultiplier);
			}
		}
		if ((num & 4L) != 0L)
		{
			global::UnityEngine.Color warheadLightColor2 = _warheadLightColor;
			Network_warheadLightColor = global::Mirror.NetworkReaderExtensions.ReadColor(reader);
			if (!SyncVarEqual(warheadLightColor2, ref _warheadLightColor))
			{
				UpdateWarheadLight(warheadLightColor2, _warheadLightColor);
			}
		}
		if ((num & 8L) != 0L)
		{
			bool warheadLightOverride2 = _warheadLightOverride;
			Network_warheadLightOverride = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			if (!SyncVarEqual(warheadLightOverride2, ref _warheadLightOverride))
			{
				UpdateWarheadLightOverride(warheadLightOverride2, _warheadLightOverride);
			}
		}
	}
}
