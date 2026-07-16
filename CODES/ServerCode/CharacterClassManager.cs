public class CharacterClassManager : global::Mirror.NetworkBehaviour
{
	private CentralAuthInterface _centralAuthInt;

	private ReferenceHub _hub;

	private bool _hubSet;

	[global::System.NonSerialized]
	public string UserId2;

	public static bool OnlineMode;

	[global::System.NonSerialized]
	public bool GodMode;

	private bool _wasAnytimeAlive;

	private bool _commandtokensent;

	internal static bool EnableSyncServerCmdBinding;

	[global::System.NonSerialized]
	internal string AuthToken;

	[global::System.NonSerialized]
	internal string AuthTokenSerial;

	[global::System.NonSerialized]
	public string RequestIp;

	[global::System.NonSerialized]
	public string Asn;

	[global::Mirror.SyncVar]
	public string Pastebin;

	[global::Mirror.SyncVar]
	public byte MaxPlayers;

	internal static bool CuffedChangeTeam;

	internal static bool ForceCuffedChangeTeam;

	[global::Mirror.SyncVar]
	public bool RoundStarted;

	[global::Mirror.SyncVar(hook = "UserIdHook")]
	public string SyncedUserId;

	private string _privUserId;

	private ClientInstanceMode _targetInstanceMode;

	private const string HostId = "ID_Host";

	private const string DedicatedId = "ID_Dedicated";

	private global::Security.RateLimit _interactRateLimit;

	private global::Security.RateLimit _commandRateLimit;

	private readonly global::Security.RateLimit _deathScreenRateLimit = new global::Security.RateLimit(2, 4f);

	internal ServerRoles SrvRoles { get; private set; }

	internal global::Mirror.NetworkConnection Connection { get; private set; }

	private ReferenceHub Hub
	{
		get
		{
			if (!_hubSet && ReferenceHub.TryGetHub(base.gameObject, out _hub))
			{
				_hubSet = true;
			}
			return _hub;
		}
	}

	public string VacSession { get; internal set; }

	public ClientInstanceMode InstanceMode
	{
		get
		{
			return _targetInstanceMode;
		}
		private set
		{
			if (value != _targetInstanceMode)
			{
				_targetInstanceMode = value;
				CharacterClassManager.OnInstanceModeChanged?.Invoke(_hub, _targetInstanceMode);
			}
		}
	}

	public string UserId
	{
		get
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return SyncedUserId;
			}
			if (_privUserId == null)
			{
				return null;
			}
			if (_privUserId.Contains("$"))
			{
				return _privUserId.Substring(0, _privUserId.IndexOf("$", global::System.StringComparison.Ordinal));
			}
			return _privUserId;
		}
		set
		{
			if (global::Mirror.NetworkServer.active)
			{
				_privUserId = value;
				RefreshSyncedId();
				Hub.serverRoles.RefreshRealId();
			}
		}
	}

	public string SaltedUserId
	{
		get
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return SyncedUserId;
			}
			return _privUserId;
		}
	}

	public string NetworkPastebin
	{
		get
		{
			return Pastebin;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref Pastebin))
			{
				string pastebin = Pastebin;
				SetSyncVar(value, ref Pastebin, 1uL);
			}
		}
	}

	public byte NetworkMaxPlayers
	{
		get
		{
			return MaxPlayers;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref MaxPlayers))
			{
				byte maxPlayers = MaxPlayers;
				SetSyncVar(value, ref MaxPlayers, 2uL);
			}
		}
	}

	public bool NetworkRoundStarted
	{
		get
		{
			return RoundStarted;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref RoundStarted))
			{
				bool roundStarted = RoundStarted;
				SetSyncVar(value, ref RoundStarted, 4uL);
			}
		}
	}

	public string NetworkSyncedUserId
	{
		get
		{
			return SyncedUserId;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref SyncedUserId))
			{
				string syncedUserId = SyncedUserId;
				SetSyncVar(value, ref SyncedUserId, 8uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(8uL))
				{
					setSyncVarHookGuard(8uL, value: true);
					UserIdHook(syncedUserId, value);
					setSyncVarHookGuard(8uL, value: false);
				}
			}
		}
	}

	public static event global::System.Action OnRoundStarted;

	public static event global::System.Action<ReferenceHub> OnSyncedUserIdAssigned;

	public static event global::System.Action<ReferenceHub, ClientInstanceMode> OnInstanceModeChanged;

	[global::Mirror.Server]
	public void RefreshSyncedId()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void CharacterClassManager::RefreshSyncedId()' called when server was not active");
			return;
		}
		if (_privUserId == null)
		{
			NetworkSyncedUserId = null;
			return;
		}
		bool flag = base.isLocalPlayer || (_privUserId.EndsWith("@steam") && !SrvRoles.DoNotTrack && !SrvRoles.SyncHashed);
		NetworkSyncedUserId = (flag ? _privUserId : global::Cryptography.Sha.HashToString(global::Cryptography.Sha.Sha512(_privUserId)));
	}

	private void Awake()
	{
		SrvRoles = GetComponent<ServerRoles>();
	}

	private void Start()
	{
		_interactRateLimit = Hub.playerRateLimitHandler.RateLimits[0];
		_commandRateLimit = Hub.playerRateLimitHandler.RateLimits[2];
		if (base.isLocalPlayer && global::Mirror.NetworkServer.active)
		{
			NetworkPastebin = global::GameCore.ConfigFile.ServerConfig.GetString("serverinfo_pastebin_id");
		}
		if (!string.IsNullOrEmpty(UserId))
		{
			UserIdHook(string.Empty, UserId);
		}
		_centralAuthInt = new CentralAuthInterface(Hub, base.isServer);
		Connection = base.connectionToClient;
		StartCoroutine(Init());
		if (base.isLocalPlayer && !ServerStatic.IsDedicated)
		{
			CentralAuth.singleton.GenerateToken(_centralAuthInt);
		}
	}

	private void Update()
	{
		if (global::Mirror.NetworkServer.active && base.isLocalPlayer)
		{
			NetworkMaxPlayers = (byte)global::Mirror.NetworkManager.singleton.maxConnections;
		}
	}

	public void UserIdHook(string p, string i)
	{
		CharacterClassManager.OnSyncedUserIdAssigned?.Invoke(Hub);
		if (string.IsNullOrEmpty(i))
		{
			InstanceMode = ClientInstanceMode.Unverified;
		}
		else if (!(i == "ID_Dedicated"))
		{
			if (i == "ID_Host")
			{
				InstanceMode = ClientInstanceMode.Host;
			}
			else
			{
				InstanceMode = ClientInstanceMode.ReadyClient;
			}
		}
		else
		{
			InstanceMode = ClientInstanceMode.DedicatedServer;
		}
	}

	[global::Mirror.TargetRpc]
	internal void TargetSetRealId(global::Mirror.NetworkConnection conn, string userId)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, userId);
		SendTargetRPCInternal(conn, typeof(CharacterClassManager), "TargetSetRealId", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	public void SyncServerCmdBinding()
	{
		if (!base.isServer || !EnableSyncServerCmdBinding)
		{
			return;
		}
		foreach (CmdBinding.Bind binding in CmdBinding.Bindings)
		{
			if (binding.command.StartsWith(".") || binding.command.StartsWith("/"))
			{
				TargetChangeCmdBinding(base.connectionToClient, binding.key, binding.command);
			}
		}
	}

	[global::Mirror.TargetRpc]
	public void TargetChangeCmdBinding(global::Mirror.NetworkConnection connection, global::UnityEngine.KeyCode code, string cmd)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.GeneratedNetworkCode._Write_UnityEngine_002EKeyCode(writer, code);
		global::Mirror.NetworkWriterExtensions.WriteString(writer, cmd);
		SendTargetRPCInternal(connection, typeof(CharacterClassManager), "TargetChangeCmdBinding", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void ConsolePrint(string text, string color)
	{
		Hub.gameConsoleTransmission.SendToClient(base.connectionToClient, text, color);
	}

	private global::System.Collections.IEnumerator Init()
	{
		if (global::Mirror.NetworkServer.active)
		{
			if (base.isLocalPlayer)
			{
				ServerLogs.StartLogging();
			}
			if (OnlineMode && !base.isLocalPlayer)
			{
				float timeout = 0f;
				while (true)
				{
					timeout += global::MEC.Timing.DeltaTime;
					yield return null;
					if (!string.IsNullOrEmpty(UserId))
					{
						break;
					}
					if (!(timeout < 45f))
					{
						ServerConsole.Disconnect(base.connectionToClient, "Your client has failed to authenticate in time.");
						yield break;
					}
				}
			}
			else if (base.isLocalPlayer)
			{
				UserId = (ServerStatic.IsDedicated ? "ID_Dedicated" : "ID_Host");
			}
			else
			{
				_privUserId = $"OFFLINE_MODE_{base.netId}_{(global::System.DateTimeOffset.Now.ToUnixTimeSeconds())}";
				NetworkSyncedUserId = _privUserId;
			}
		}
		ReferenceHub hub;
		while (!ReferenceHub.TryGetHostHub(out hub))
		{
			yield return null;
		}
		if (!base.isLocalPlayer || !global::Mirror.NetworkServer.active)
		{
			yield break;
		}
		FriendlyFireConfig.PauseDetector = false;
		CustomLiteNetLib4MirrorTransport.DelayConnections = false;
		IdleMode.PauseIdleMode = false;
		ServerConsole.AddOutputEntry(default(global::ServerOutput.RoundRestartedEntry));
		if (ServerStatic.IsDedicated)
		{
			global::PluginAPI.Core.Statistics.CurrentRound = new global::PluginAPI.Core.Statistics.Round();
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.WaitingForPlayers);
			ServerConsole.AddLog("Waiting for players...");
		}
		if (NonFacilityCompatibility.currentSceneSettings.roundAutostart)
		{
			ForceRoundStart();
		}
		else
		{
			short originalTimeLeft = global::GameCore.ConfigFile.ServerConfig.GetShort("lobby_waiting_time", 20);
			short timeLeft = originalTimeLeft;
			int topPlayers = 2;
			while (global::GameCore.RoundStart.singleton.Timer != -1)
			{
				if (timeLeft == -2)
				{
					timeLeft = originalTimeLeft;
				}
				int num = global::System.Linq.Enumerable.Count(ReferenceHub.AllHubs, (ReferenceHub x) => x.Mode == ClientInstanceMode.ReadyClient);
				if (!global::GameCore.RoundStart.LobbyLock && num > 1)
				{
					if (num > topPlayers)
					{
						topPlayers = num;
						if (timeLeft < originalTimeLeft)
						{
							do
							{
								timeLeft++;
							}
							while (timeLeft % 5 == 0 && timeLeft < originalTimeLeft);
						}
					}
					else
					{
						timeLeft--;
					}
					if (num >= ((CustomNetworkManager)global::Mirror.NetworkManager.singleton).ReservedMaxPlayers)
					{
						timeLeft = -1;
					}
					if (timeLeft == -1)
					{
						ForceRoundStart();
					}
				}
				else
				{
					timeLeft = -2;
				}
				if (global::GameCore.RoundStart.singleton.Timer != -1)
				{
					global::GameCore.RoundStart.singleton.NetworkTimer = timeLeft;
				}
				yield return new global::UnityEngine.WaitForSeconds(1f);
			}
		}
		global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.RoundStart);
		NetworkRoundStarted = true;
		RpcRoundStarted();
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdSendToken(string token)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, token);
		SendCommandInternal(typeof(CharacterClassManager), "CmdSendToken", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdRequestContactEmail()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(CharacterClassManager), "CmdRequestContactEmail", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdRequestServerConfig()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(CharacterClassManager), "CmdRequestServerConfig", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdRequestServerGroups()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(CharacterClassManager), "CmdRequestServerGroups", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdRequestHideTag()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(CharacterClassManager), "CmdRequestHideTag", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdRequestShowTag(bool global)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, global);
		SendCommandInternal(typeof(CharacterClassManager), "CmdRequestShowTag", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	public static bool ForceRoundStart()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return false;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Logger, "Round has been started.", ServerLogs.ServerLogType.GameEvent);
		ServerConsole.AddLog("New round has been started.");
		global::GameCore.RoundStart.singleton.NetworkTimer = -1;
		global::GameCore.RoundStart.RoundStartTimer.Restart();
		return true;
	}

	[global::Mirror.TargetRpc]
	private void TargetSetDisconnectError(global::Mirror.NetworkConnection conn, string message)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, message);
		SendTargetRPCInternal(conn, typeof(CharacterClassManager), "TargetSetDisconnectError", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdConfirmDisconnect()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(CharacterClassManager), "CmdConfirmDisconnect", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	public void DisconnectClient(global::Mirror.NetworkConnection conn, string message)
	{
		TargetSetDisconnectError(conn, message);
		global::MEC.Timing.RunCoroutine(_DisconnectAfterTimeout(conn), global::MEC.Segment.FixedUpdate);
	}

	private global::System.Collections.Generic.IEnumerator<float> _DisconnectAfterTimeout(global::Mirror.NetworkConnection conn)
	{
		for (int i = 0; i < 150; i++)
		{
			yield return 0f;
		}
		conn?.Disconnect();
	}

	[global::Mirror.ClientRpc]
	private void RpcRoundStarted()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(CharacterClassManager), "RpcRoundStarted", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	static CharacterClassManager()
	{
		CuffedChangeTeam = true;
		ForceCuffedChangeTeam = true;
		CharacterClassManager.OnRoundStarted = delegate
		{
		};
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdSendToken", InvokeUserCode_CmdSendToken, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdRequestContactEmail", InvokeUserCode_CmdRequestContactEmail, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdRequestServerConfig", InvokeUserCode_CmdRequestServerConfig, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdRequestServerGroups", InvokeUserCode_CmdRequestServerGroups, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdRequestHideTag", InvokeUserCode_CmdRequestHideTag, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdRequestShowTag", InvokeUserCode_CmdRequestShowTag, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(CharacterClassManager), "CmdConfirmDisconnect", InvokeUserCode_CmdConfirmDisconnect, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(CharacterClassManager), "RpcRoundStarted", InvokeUserCode_RpcRoundStarted);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(CharacterClassManager), "TargetSetRealId", InvokeUserCode_TargetSetRealId);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(CharacterClassManager), "TargetChangeCmdBinding", InvokeUserCode_TargetChangeCmdBinding);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(CharacterClassManager), "TargetSetDisconnectError", InvokeUserCode_TargetSetDisconnectError);
	}

	private void MirrorProcessed()
	{
	}

	internal void UserCode_TargetSetRealId(global::Mirror.NetworkConnection conn, string userId)
	{
	}

	protected static void InvokeUserCode_TargetSetRealId(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetSetRealId called on server.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_TargetSetRealId(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	public void UserCode_TargetChangeCmdBinding(global::Mirror.NetworkConnection connection, global::UnityEngine.KeyCode code, string cmd)
	{
	}

	protected static void InvokeUserCode_TargetChangeCmdBinding(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetChangeCmdBinding called on server.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_TargetChangeCmdBinding(global::Mirror.NetworkClient.readyConnection, global::Mirror.GeneratedNetworkCode._Read_UnityEngine_002EKeyCode(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	public void UserCode_CmdSendToken(string token)
	{
		if (_commandtokensent && !base.isLocalPlayer)
		{
			ServerConsole.Disconnect(base.connectionToClient, "Your client sent second authentication token.");
			return;
		}
		if (OnlineMode)
		{
			if (string.IsNullOrEmpty(token) || _commandtokensent)
			{
				if (!base.isLocalPlayer || !base.isServer)
				{
					ServerConsole.Disconnect(base.connectionToClient, "Your client sent an empty authentication token. Make sure you are running the game by steam.");
					return;
				}
			}
			else if (!base.isLocalPlayer || !base.isServer)
			{
				if (token.StartsWith("ERROR: "))
				{
					ServerConsole.AddLog("Player from IP " + base.connectionToClient.address + " kicked due to authentication error: " + token.Substring(7));
					ServerLogs.AddLog(ServerLogs.Modules.Networking, "Player from IP " + base.connectionToClient.address + " kicked due to authentication error: " + token.Substring(7), ServerLogs.ServerLogType.ConnectionUpdate);
					ServerConsole.Disconnect(base.connectionToClient, "Error during authentication: " + token.Substring(7));
				}
				else
				{
					CentralAuth.singleton.StartValidateToken(_centralAuthInt, token, global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorServer.Peers[base.connectionToClient.connectionId].EndPoint);
					AuthToken = token;
				}
			}
			else
			{
				if (token.StartsWith("ERROR: "))
				{
					global::GameCore.Console.AddLog("Error during authentication: " + token.Substring(7), global::UnityEngine.Color.red);
				}
				else
				{
					CentralAuth.singleton.StartValidateToken(_centralAuthInt, token, null);
				}
				AuthToken = token;
			}
		}
		_commandtokensent = true;
	}

	protected static void InvokeUserCode_CmdSendToken(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSendToken called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdSendToken(global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	public void UserCode_CmdRequestContactEmail()
	{
		if (_commandRateLimit.CanExecute())
		{
			if (SrvRoles.RemoteAdmin || SrvRoles.Staff)
			{
				ConsolePrint("Contact email address: " + global::GameCore.ConfigFile.ServerConfig.GetString("contact_email"), "green");
			}
			else
			{
				ConsolePrint("You don't have permissions to execute this command.", "red");
			}
		}
	}

	protected static void InvokeUserCode_CmdRequestContactEmail(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestContactEmail called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdRequestContactEmail();
		}
	}

	public void UserCode_CmdRequestServerConfig()
	{
		if (_commandRateLimit.CanExecute())
		{
			YamlConfig serverConfig = global::GameCore.ConfigFile.ServerConfig;
			if (base.isLocalPlayer || SrvRoles.Staff || SrvRoles.RaEverywhere || PermissionsHandler.IsPermitted(SrvRoles.Permissions, PlayerPermissions.ServerConsoleCommands | PlayerPermissions.ServerConfigs))
			{
				ConsolePrint("Extended server configuration:\nServer name: " + serverConfig.GetString("server_name") + "\nServer IP: " + serverConfig.GetString("server_ip") + "\nCurrent Server IP: " + CustomNetworkManager.Ip + "\nServer port: " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + "\nServer pastebin ID: " + serverConfig.GetString("serverinfo_pastebin_id") + "\nServer max players: " + serverConfig.GetInt("max_players") + "\nOnline mode: " + OnlineMode.ToString() + "\nRA password authentication: " + GetComponent<global::RemoteAdmin.QueryProcessor>().OverridePasswordEnabled.ToString() + "\nIP banning: " + serverConfig.GetBool("ip_banning").ToString() + "\nWhitelist: " + serverConfig.GetBool("enable_whitelist").ToString() + "\nQuery status: " + serverConfig.GetBool("enable_query").ToString() + " with port shift " + serverConfig.GetInt("query_port_shift") + "\nFriendly fire: " + ServerConsole.FriendlyFire.ToString() + "\nMap seed: " + serverConfig.GetInt("map_seed"), "green");
			}
			else
			{
				ConsolePrint("Basic server configuration:\nServer name: " + serverConfig.GetString("server_name") + "\nServer pastebin ID: " + serverConfig.GetString("serverinfo_pastebin_id") + "\nServer max players: " + serverConfig.GetInt("max_players") + "\nRA password authentication: " + GetComponent<global::RemoteAdmin.QueryProcessor>().OverridePasswordEnabled.ToString() + "\nOnline mode: " + OnlineMode.ToString() + "\nWhitelist: " + serverConfig.GetBool("enable_whitelist").ToString() + "\nFriendly fire: " + ServerConsole.FriendlyFire.ToString() + "\nFriendly fire: " + ServerConsole.FriendlyFire.ToString(), "green");
			}
		}
	}

	protected static void InvokeUserCode_CmdRequestServerConfig(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestServerConfig called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdRequestServerConfig();
		}
	}

	public void UserCode_CmdRequestServerGroups()
	{
		if (!_commandRateLimit.CanExecute())
		{
			return;
		}
		string text = "Groups defined on this server:";
		global::System.Collections.Generic.Dictionary<string, UserGroup> allGroups = ServerStatic.PermissionsHandler.GetAllGroups();
		ServerRoles.NamedColor[] namedColors = SrvRoles.NamedColors;
		foreach (global::System.Collections.Generic.KeyValuePair<string, UserGroup> permentry in allGroups)
		{
			try
			{
				if (namedColors != null)
				{
					text = text + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - <color=#" + global::System.Linq.Enumerable.FirstOrDefault(namedColors, (ServerRoles.NamedColor x) => x.Name == permentry.Value.BadgeColor)?.ColorHex + ">" + permentry.Value.BadgeText + "</color> in color " + permentry.Value.BadgeColor;
				}
			}
			catch
			{
				text = text + "\n" + permentry.Key + " (" + permentry.Value.Permissions + ") - " + permentry.Value.BadgeText + " in color " + permentry.Value.BadgeColor;
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.KickingAndShortTermBanning))
			{
				text += " BN1";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.BanningUpToDay))
			{
				text += " BN2";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.LongTermBanning))
			{
				text += " BN3";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassSelf))
			{
				text += " FSE";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassToSpectator))
			{
				text += " FSP";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ForceclassWithoutRestrictions))
			{
				text += " FWR";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GivingItems))
			{
				text += " GIV";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.WarheadEvents))
			{
				text += " EWA";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RespawnEvents))
			{
				text += " ERE";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.RoundEvents))
			{
				text += " ERO";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.SetGroup))
			{
				text += " SGR";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.GameplayData))
			{
				text += " GMD";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Overwatch))
			{
				text += " OVR";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FacilityManagement))
			{
				text += " FCM";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayersManagement))
			{
				text += " PLM";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PermissionsManagement))
			{
				text += " PRM";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConsoleCommands))
			{
				text += " SCC";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenBadges))
			{
				text += " VHB";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ServerConfigs))
			{
				text += " CFG";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Broadcasting))
			{
				text += " BRC";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.PlayerSensitiveDataAccess))
			{
				text += " CDA";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Noclip))
			{
				text += " NCP";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AFKImmunity))
			{
				text += " AFK";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.AdminChat))
			{
				text += " ATC";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.ViewHiddenGlobalBadges))
			{
				text += " GHB";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Announcer))
			{
				text += " ANN";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.Effects))
			{
				text += " EFF";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorImmunity))
			{
				text += " FFI";
			}
			if (PermissionsHandler.IsPermitted(permentry.Value.Permissions, PlayerPermissions.FriendlyFireDetectorTempDisable))
			{
				text += " FFT";
			}
		}
		ConsolePrint("Defined groups on server " + text, "grey");
	}

	protected static void InvokeUserCode_CmdRequestServerGroups(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestServerGroups called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdRequestServerGroups();
		}
	}

	public void UserCode_CmdRequestHideTag()
	{
		if (_commandRateLimit.CanExecute())
		{
			if (!string.IsNullOrEmpty(SrvRoles.HiddenBadge))
			{
				ConsolePrint("Your badge is already hidden.", "yellow");
				return;
			}
			if (string.IsNullOrEmpty(SrvRoles.MyText))
			{
				ConsolePrint("You don't have a badge.", "red");
				return;
			}
			SrvRoles.GlobalHidden = SrvRoles.GlobalSet;
			SrvRoles.HiddenBadge = SrvRoles.MyText;
			SrvRoles.NetworkGlobalBadge = null;
			SrvRoles.SetText(null);
			SrvRoles.SetColor(null);
			SrvRoles.RefreshHiddenTag();
			ConsolePrint("Badge hidden.", "green");
		}
	}

	protected static void InvokeUserCode_CmdRequestHideTag(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestHideTag called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdRequestHideTag();
		}
	}

	public void UserCode_CmdRequestShowTag(bool global)
	{
		if (!_commandRateLimit.CanExecute())
		{
			return;
		}
		if (global)
		{
			if (string.IsNullOrEmpty(SrvRoles.PrevBadge))
			{
				ConsolePrint("You don't have a global tag.", "magenta");
				return;
			}
			if ((string.IsNullOrEmpty(SrvRoles.MyText) || !SrvRoles.RemoteAdmin) && (((SrvRoles.GlobalBadgeType == 3 || SrvRoles.GlobalBadgeType == 4) && global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_banteam_badges") && !ServerStatic.PermissionsHandler.IsVerified) || (SrvRoles.GlobalBadgeType == 1 && global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_staff_badges")) || (SrvRoles.GlobalBadgeType == 2 && global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_management_badges") && !ServerStatic.PermissionsHandler.IsVerified) || (SrvRoles.GlobalBadgeType == 0 && global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_patreon_badges") && !ServerStatic.PermissionsHandler.IsVerified)))
			{
				ConsolePrint("You can't show this type of global badge on this server. Try joining server with global badges allowed.", "red");
				return;
			}
			SrvRoles.NetworkGlobalBadge = SrvRoles.PrevBadge;
			SrvRoles.GlobalHidden = false;
			SrvRoles.HiddenBadge = null;
			SrvRoles.RpcResetFixed();
			ConsolePrint("Global tag refreshed.", "green");
		}
		else
		{
			SrvRoles.NetworkGlobalBadge = null;
			SrvRoles.HiddenBadge = null;
			SrvRoles.RpcResetFixed();
			SrvRoles.RefreshPermissions(disp: true);
			ConsolePrint("Local tag refreshed.", "green");
		}
	}

	protected static void InvokeUserCode_CmdRequestShowTag(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestShowTag called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdRequestShowTag(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	private void UserCode_TargetSetDisconnectError(global::Mirror.NetworkConnection conn, string message)
	{
		((CustomNetworkManager)global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorNetworkManager.singleton).disconnectMessage = message;
		CmdConfirmDisconnect();
	}

	protected static void InvokeUserCode_TargetSetDisconnectError(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetSetDisconnectError called on server.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_TargetSetDisconnectError(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	private void UserCode_CmdConfirmDisconnect()
	{
		if (base.connectionToClient != null)
		{
			base.connectionToClient.Disconnect();
		}
	}

	protected static void InvokeUserCode_CmdConfirmDisconnect(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdConfirmDisconnect called on client.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_CmdConfirmDisconnect();
		}
	}

	private void UserCode_RpcRoundStarted()
	{
		CharacterClassManager.OnRoundStarted?.Invoke();
	}

	protected static void InvokeUserCode_RpcRoundStarted(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcRoundStarted called on server.");
		}
		else
		{
			((CharacterClassManager)obj).UserCode_RpcRoundStarted();
		}
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, Pastebin);
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, MaxPlayers);
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, RoundStarted);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, SyncedUserId);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, Pastebin);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteByte(writer, MaxPlayers);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, RoundStarted);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, SyncedUserId);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			string pastebin = Pastebin;
			NetworkPastebin = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			byte maxPlayers = MaxPlayers;
			NetworkMaxPlayers = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			bool roundStarted = RoundStarted;
			NetworkRoundStarted = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			string syncedUserId = SyncedUserId;
			NetworkSyncedUserId = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(syncedUserId, ref SyncedUserId))
			{
				UserIdHook(syncedUserId, SyncedUserId);
			}
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			string pastebin2 = Pastebin;
			NetworkPastebin = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}
		if ((num & 2L) != 0L)
		{
			byte maxPlayers2 = MaxPlayers;
			NetworkMaxPlayers = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
		}
		if ((num & 4L) != 0L)
		{
			bool roundStarted2 = RoundStarted;
			NetworkRoundStarted = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
		}
		if ((num & 8L) != 0L)
		{
			string syncedUserId2 = SyncedUserId;
			NetworkSyncedUserId = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(syncedUserId2, ref SyncedUserId))
			{
				UserIdHook(syncedUserId2, SyncedUserId);
			}
		}
	}
}
