public class CustomNetworkManager : global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorNetworkManager
{
	[global::System.Serializable]
	public class DisconnectLog
	{
		[global::System.Serializable]
		public class LogButton
		{
			public ConnInfoButton[] actions;
		}

		[global::UnityEngine.Multiline]
		public string msg_en;

		public CustomNetworkManager.DisconnectLog.LogButton button;

		public bool autoHideOnSceneLoad;
	}

	public static readonly global::System.Collections.Generic.HashSet<global::System.Func<CustomNetworkManager, bool>> TryStartClientChecks = new global::System.Collections.Generic.HashSet<global::System.Func<CustomNetworkManager, bool>>();

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject popup;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject createPopForce;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.GameObject loadingpop;

	public global::UnityEngine.GameObject createpop;

	public global::UnityEngine.RectTransform contSize;

	private static QueryServer _queryserver;

	public CustomNetworkManager.DisconnectLog[] logs;

	private int _curLogId;

	private int _queryPort;

	internal static bool reconnecting;

	internal static float reconnectTime;

	internal static float triggerReconnectTime;

	private bool _queryEnabled;

	private bool _configLoaded;

	private bool _activated;

	private float _dictCleanupTime;

	private float _ipRateLimitTime;

	private float _userIdRateLimitTime;

	private float _preauthChallengeTime;

	private float _delayVolumeResetTime;

	private float _rejectSuppressionTime;

	private float _issuedSuppressionTime;

	private bool _disconnectDrop;

	private static readonly int[] _loadingLogId = new int[4] { 13, 14, 17, 33 };

	private readonly global::System.Collections.Generic.HashSet<global::System.Net.IPEndPoint> _dictToRemove = new global::System.Collections.Generic.HashSet<global::System.Net.IPEndPoint>();

	private readonly global::System.Collections.Generic.HashSet<string> _dict2ToRemove = new global::System.Collections.Generic.HashSet<string>();

	private static ushort _ipRateLimitWindow;

	private static ushort _userIdLimitWindow;

	private static ushort _preauthChallengeWindow;

	private static ushort _preauthChallengeClean;

	public string disconnectMessage = "";

	public static string Ip = "";

	public static string ConnectionIp;

	public static string LastIp;

	[global::UnityEngine.Space(30f)]
	public int GameFilesVersion;

	public static bool Modded = false;

	public static bool HeavilyModded = false;

	public static bool UsingCustomGamemode = false;

	private static readonly int _expectedGameFilesVersion = 4;

	public static int slots;

	public static int reservedSlots;

	public static bool EnableFastRestart = true;

	public static float FastRestartDelay = 3.2f;

	public static CustomNetworkManager TypedSingleton => (CustomNetworkManager)global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorNetworkManager.singleton;

	public int MaxPlayers
	{
		get
		{
			return maxConnections;
		}
		set
		{
			maxConnections = value;
			global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.maxConnections = (ushort)value;
		}
	}

	public int ReservedMaxPlayers => slots;

	public static event global::System.Action OnClientReady;

	public static event global::System.Action OnClientStarted;

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		_dictCleanupTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_ipRateLimitTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_userIdRateLimitTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_preauthChallengeTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_delayVolumeResetTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_rejectSuppressionTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		_issuedSuppressionTime += global::UnityEngine.Time.fixedUnscaledDeltaTime;
		if (_ipRateLimitTime >= (float)(int)_ipRateLimitWindow)
		{
			_ipRateLimitTime = 0f;
			CustomLiteNetLib4MirrorTransport.IpRateLimit.Clear();
		}
		if (_userIdRateLimitTime >= (float)(int)_userIdLimitWindow)
		{
			_userIdRateLimitTime = 0f;
			CustomLiteNetLib4MirrorTransport.UserRateLimit.Clear();
		}
		if (_delayVolumeResetTime > 5.5f)
		{
			_delayVolumeResetTime = 0f;
			CustomLiteNetLib4MirrorTransport.DelayVolume = 0;
		}
		if (_rejectSuppressionTime > 10f)
		{
			_rejectSuppressionTime = 0f;
			if (CustomLiteNetLib4MirrorTransport.SuppressRejections)
			{
				if (CustomLiteNetLib4MirrorTransport.Rejected <= CustomLiteNetLib4MirrorTransport.RejectionThreshold)
				{
					CustomLiteNetLib4MirrorTransport.SuppressRejections = false;
				}
				ServerConsole.AddLog($"{CustomLiteNetLib4MirrorTransport.Rejected} incoming connections have been rejected within the last 10 seconds.", global::System.ConsoleColor.Yellow);
			}
			CustomLiteNetLib4MirrorTransport.Rejected = 0u;
		}
		if (_issuedSuppressionTime > 10f)
		{
			_issuedSuppressionTime = 0f;
			if (CustomLiteNetLib4MirrorTransport.SuppressIssued)
			{
				if (CustomLiteNetLib4MirrorTransport.ChallengeIssued <= CustomLiteNetLib4MirrorTransport.IssuedThreshold)
				{
					CustomLiteNetLib4MirrorTransport.SuppressIssued = false;
				}
				ServerConsole.AddLog($"{CustomLiteNetLib4MirrorTransport.ChallengeIssued} challenges have been requested within the last 10 seconds.", global::System.ConsoleColor.Yellow);
			}
			CustomLiteNetLib4MirrorTransport.ChallengeIssued = 0u;
		}
		if (_preauthChallengeTime >= (float)(int)_preauthChallengeClean)
		{
			_preauthChallengeTime = 0f;
			long ticks = global::System.DateTime.Now.AddSeconds(_preauthChallengeWindow * -1).Ticks;
			foreach (global::System.Collections.Generic.KeyValuePair<string, PreauthChallengeItem> challenge in CustomLiteNetLib4MirrorTransport.Challenges)
			{
				if (challenge.Value.Added <= ticks)
				{
					_dict2ToRemove.Add(challenge.Key);
				}
			}
			foreach (string item in _dict2ToRemove)
			{
				if (CustomLiteNetLib4MirrorTransport.Challenges.ContainsKey(item))
				{
					CustomLiteNetLib4MirrorTransport.Challenges.Remove(item);
				}
			}
			_dict2ToRemove.Clear();
		}
		if (_dictCleanupTime <= 20f)
		{
			return;
		}
		_dictCleanupTime = 0f;
		long ticks2 = global::System.DateTime.Now.AddSeconds(-200.0).Ticks;
		foreach (global::System.Collections.Generic.KeyValuePair<global::System.Net.IPEndPoint, PreauthItem> userId in CustomLiteNetLib4MirrorTransport.UserIds)
		{
			if (userId.Value.Added <= ticks2)
			{
				_dictToRemove.Add(userId.Key);
			}
		}
		foreach (global::System.Net.IPEndPoint item2 in _dictToRemove)
		{
			if (CustomLiteNetLib4MirrorTransport.UserIds.ContainsKey(item2))
			{
				CustomLiteNetLib4MirrorTransport.UserIds.Remove(item2);
			}
		}
		_dictToRemove.Clear();
	}

	internal static void InvokeOnClientReady()
	{
		CustomNetworkManager.OnClientReady?.Invoke();
	}

	public override void OnClientConnect(global::Mirror.NetworkConnection conn)
	{
		CustomNetworkManager.OnClientReady?.Invoke();
		base.OnClientConnect(conn);
	}

	public override void ServerChangeScene(string newSceneName)
	{
		if (string.IsNullOrEmpty(newSceneName))
		{
			global::UnityEngine.Debug.LogError("ServerChangeScene empty scene name");
			return;
		}
		global::Mirror.NetworkServer.SetAllClientsNotReady();
		global::Mirror.NetworkManager.networkSceneName = newSceneName;
		OnServerChangeScene(newSceneName);
		global::Mirror.Transport.activeTransport.enabled = false;
		global::Mirror.NetworkManager.loadingSceneAsync = global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(newSceneName);
		if (global::Mirror.NetworkServer.active)
		{
			if (EnableFastRestart)
			{
				global::MEC.Timing.CallDelayed(FastRestartDelay, delegate
				{
					global::Mirror.NetworkServer.SendToAll(new global::Mirror.SceneMessage
					{
						sceneName = newSceneName
					});
				});
			}
			else
			{
				global::Mirror.NetworkServer.SendToAll(new global::Mirror.SceneMessage
				{
					sceneName = newSceneName
				});
			}
		}
		global::Mirror.NetworkManager.startPositionIndex = 0;
		global::Mirror.NetworkManager.startPositions.Clear();
	}

	public override void OnClientDisconnect(global::Mirror.NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
	}

	private static void PrintConnectionDebug(string reason)
	{
		global::GameCore.Console.AddLog(reason, global::UnityEngine.Color.red);
		global::GameCore.Console.AddLog("IP: " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.clientAddress, global::UnityEngine.Color.red);
		global::GameCore.Console.AddLog("Port: " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port, global::UnityEngine.Color.red);
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		CustomNetworkManager.OnClientStarted?.Invoke();
		StartCoroutine(_ConnectToServer());
	}

	public override void StartClient()
	{
		bool allow = true;
		global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(TryStartClientChecks, delegate(global::System.Func<CustomNetworkManager, bool> x)
		{
			allow = x(this) && allow;
		});
		if (allow)
		{
			ShowLoadingScreen(0);
			base.StartClient();
		}
	}

	private global::System.Collections.Generic.IEnumerator<float> _ConnectToServer()
	{
		while (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.State == global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.States.ClientConnecting || global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.State == global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorCore.States.ClientConnected)
		{
			if (global::Mirror.NetworkClient.isConnected)
			{
				ShowLoadingScreen(2);
				break;
			}
			yield return 0f;
		}
	}

	public bool IsFacilityLoading()
	{
		return _curLogId == 17;
	}

	public override void OnServerDisconnect(global::Mirror.NetworkConnection conn)
	{
		if (_disconnectDrop)
		{
			global::Mirror.NetworkIdentity identity = conn.identity;
			if (identity != null && ReferenceHub.TryGetHubNetID(identity.netId, out var hub))
			{
				hub.playerStats.DealDamage(new global::PlayerStatsSystem.UniversalDamageHandler(-1f, global::PlayerStatsSystem.DeathTranslations.Unknown));
			}
		}
		if (CustomLiteNetLib4MirrorTransport.IpPassthroughEnabled)
		{
			int id = global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorServer.Peers[conn.connectionId].Id;
			if (CustomLiteNetLib4MirrorTransport.RealIpAddresses.ContainsKey(id))
			{
				CustomLiteNetLib4MirrorTransport.RealIpAddresses.Remove(id);
			}
		}
		base.OnServerDisconnect(conn);
		conn.Disconnect();
	}

	private void OnLevelFinishedLoading(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		if (global::NorthwoodLib.StringUtils.Contains(scene.name, "menu", global::System.StringComparison.OrdinalIgnoreCase))
		{
			_curLogId = 0;
			if (!_activated)
			{
				_activated = true;
			}
		}
		if (!(reconnectTime <= 0f))
		{
			Invoke("Reconnect", 3f);
		}
	}

	public override void OnClientSceneChanged(global::Mirror.NetworkConnection conn)
	{
		CustomNetworkManager.OnClientReady?.Invoke();
		base.OnClientSceneChanged(conn);
		if (reconnectTime <= 0f && logs[_curLogId].autoHideOnSceneLoad)
		{
			popup.SetActive(value: false);
			loadingpop.SetActive(value: false);
		}
	}

	public bool ShouldPlayIntensive()
	{
		if (_curLogId != 13)
		{
			return IsFacilityLoading();
		}
		return true;
	}

	private void Reconnect()
	{
		if (!(reconnectTime <= 0f))
		{
			reconnecting = true;
			CustomLiteNetLib4MirrorTransport.DelayConnections = true;
			IdleMode.PauseIdleMode = true;
			Invoke("TryConnecting", reconnectTime);
			reconnectTime = 0f;
		}
	}

	public void TryConnecting()
	{
		if (reconnecting)
		{
			StartClient();
		}
	}

	public void StopReconnecting()
	{
		reconnecting = false;
		triggerReconnectTime = 0f;
		reconnectTime = 0f;
	}

	public void ShowLog(int id, string obj1 = "", string obj2 = "", string obj3 = "", string textOverride = null)
	{
	}

	public void ShowLoadingScreen(int id)
	{
	}

	private void LoadConfigs(bool firstTime = false)
	{
		if (!_configLoaded)
		{
			_configLoaded = true;
			if (global::System.IO.File.Exists("hoster_policy.txt"))
			{
				global::GameCore.ConfigFile.HosterPolicy = new YamlConfig("hoster_policy.txt");
			}
			else if (global::System.IO.File.Exists(FileManager.GetAppFolder() + "hoster_policy.txt"))
			{
				global::GameCore.ConfigFile.HosterPolicy = new YamlConfig(FileManager.GetAppFolder() + "hoster_policy.txt");
			}
			else
			{
				global::GameCore.ConfigFile.HosterPolicy = new YamlConfig();
			}
			FileManager.RefreshAppFolder();
			if (!ServerStatic.IsDedicated)
			{
				ServerConsole.AddLog("Loading configs...");
				global::GameCore.ConfigFile.ReloadGameConfigs(firstTime);
				ServerConsole.AddLog("Config file loaded!");
			}
		}
	}

	public override void ConfigureServerFrameRate()
	{
	}

	public override void Start()
	{
		base.Start();
		LoadConfigs(firstTime: true);
		if (global::UnityEngine.SystemInfo.operatingSystemFamily == global::UnityEngine.OperatingSystemFamily.Linux && !global::System.IO.File.Exists("/etc/ssl/certs/ca-certificates.crt"))
		{
			if (global::System.IO.File.Exists("/etc/pki/tls/certs/ca-bundle.crt"))
			{
				ServerConsole.AddLog("System CA Cert store not available! Unity expects it to be in /etc/ssl/certs/ca-certificates.crt, but we've detected it's present in /etc/pki/tls/certs/ca-bundle.crt on your system, please symlink your store to the required location!");
			}
			else if (global::System.IO.File.Exists("/etc/ssl/ca-bundle.pem"))
			{
				ServerConsole.AddLog("System CA Cert store not available! Unity expects it to be in /etc/ssl/certs/ca-certificates.crt, but we've detected it's present in /etc/ssl/ca-bundle.pem on your system, please symlink your store to the required location!");
			}
			else if (global::System.IO.File.Exists("/etc/pki/tls/cacert.pem"))
			{
				ServerConsole.AddLog("System CA Cert store not available! Unity expects it to be in /etc/ssl/certs/ca-certificates.crt, but we've detected it's present in /etc/pki/tls/cacert.pem on your system, please symlink your store to the required location!");
			}
			else if (global::System.IO.File.Exists("/etc/pki/ca-trust/extracted/pem/tls-ca-bundle.pem"))
			{
				ServerConsole.AddLog("System CA Cert store not available! Unity expects it to be in /etc/ssl/certs/ca-certificates.crt, but we've detected it's present in /etc/pki/ca-trust/extracted/pem/tls-ca-bundle.pem on your system, please symlink your store to the required location!");
			}
			else
			{
				ServerConsole.AddLog("System CA Cert store not available! Unity expects it to be in /etc/ssl/certs/ca-certificates.crt and we couldn't detect its location! Please provide access to it in the specified path!");
			}
		}
	}

	public void CreateMatch()
	{
		CustomLiteNetLib4MirrorTransport.DelayConnections = true;
		IdleMode.PauseIdleMode = true;
		LoadConfigs();
		ShowLoadingScreen(0);
		createpop.SetActive(value: false);
		ServerConsole.AddLog("Loading configs...");
		global::GameCore.ConfigFile.ReloadGameConfigs();
		global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port = (ServerStatic.IsDedicated ? ServerStatic.ServerPort : GetFreePort());
		global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.useUpnP = global::GameCore.ConfigFile.ServerConfig.GetBool("forward_ports", def: true);
		slots = global::GameCore.ConfigFile.ServerConfig.GetInt("max_players", 20);
		CustomLiteNetLib4MirrorTransport.DelayVolumeThreshold = (byte)(global::UnityEngine.Mathf.Clamp(slots, 5, 125) * 2);
		reservedSlots = global::UnityEngine.Mathf.Max(global::GameCore.ConfigFile.ServerConfig.GetInt("reserved_slots", ReservedSlot.Users.Count), 0);
		_disconnectDrop = global::GameCore.ConfigFile.ServerConfig.GetBool("disconnect_drop", def: true);
		MaxPlayers = (slots + reservedSlots) * 2 + 50;
		int num = global::GameCore.ConfigFile.HosterPolicy.GetInt("players_limit", -1);
		if (num > 0 && slots + reservedSlots > num)
		{
			MaxPlayers = num * 2 + 50;
			ServerConsole.AddLog("You have exceeded players limit set by your hosting provider. Max players value set to " + num);
		}
		ServerConsole.AddLog("Config files loaded from " + FileManager.GetAppFolder(addSeparator: true, serverConfig: true));
		_queryEnabled = global::GameCore.ConfigFile.ServerConfig.GetBool("enable_query");
		string text = FileManager.GetAppFolder(addSeparator: true, serverConfig: true) + "config_remoteadmin.txt";
		if (!global::System.IO.File.Exists(text))
		{
			global::System.IO.File.Copy("ConfigTemplates/config_remoteadmin.template.txt", text);
		}
		ServerConsole.AddLog("Loading server permissions configuration...");
		ServerStatic.RolesConfigPath = text;
		ServerStatic.RolesConfig = new YamlConfig(text);
		ServerStatic.SharedGroupsConfig = ((global::GameCore.ConfigSharing.Paths[4] == null) ? null : new YamlConfig(global::GameCore.ConfigSharing.Paths[4] + "shared_groups.txt"));
		ServerStatic.SharedGroupsMembersConfig = ((global::GameCore.ConfigSharing.Paths[5] == null) ? null : new YamlConfig(global::GameCore.ConfigSharing.Paths[5] + "shared_groups_members.txt"));
		ServerStatic.PermissionsHandler = new PermissionsHandler(ref ServerStatic.RolesConfig, ref ServerStatic.SharedGroupsConfig, ref ServerStatic.SharedGroupsMembersConfig);
		ServerConsole.AddLog("Server permissions configuration loaded.");
		CustomLiteNetLib4MirrorTransport.UseGlobalBans = global::GameCore.ConfigFile.ServerConfig.GetBool("use_global_bans", def: true);
		CustomLiteNetLib4MirrorTransport.ReloadChallengeOptions();
		global::MEC.Timing.RunCoroutine(_CreateLobby());
	}

	internal static void ReloadTimeWindows()
	{
		_ipRateLimitWindow = global::GameCore.ConfigFile.ServerConfig.GetUShort("ip_ratelimit_window", 3);
		_userIdLimitWindow = global::GameCore.ConfigFile.ServerConfig.GetUShort("userid_ratelimit_window", 5);
		_preauthChallengeWindow = global::GameCore.ConfigFile.ServerConfig.GetUShort("preauth_challenge_time_window", 8);
		_preauthChallengeClean = global::GameCore.ConfigFile.ServerConfig.GetUShort("preauth_challenge_clean_period", 4);
		if (_ipRateLimitWindow == 0)
		{
			_ipRateLimitWindow = 1;
		}
		if (_userIdLimitWindow == 0)
		{
			_userIdLimitWindow = 1;
		}
		if (_preauthChallengeWindow == 0)
		{
			_preauthChallengeWindow = 1;
		}
		if (_preauthChallengeClean == 0)
		{
			_preauthChallengeClean = 1;
		}
	}

	private global::System.Collections.Generic.IEnumerator<float> _CreateLobby()
	{
		if (GameFilesVersion != _expectedGameFilesVersion)
		{
			ServerConsole.AddLog("This source code file is made for different version of the game!");
			ServerConsole.AddLog("Please validate game files integrity using steam!");
			ServerConsole.AddLog("Aborting server startup.");
			yield break;
		}
		ServerConsole.AddLog("Game version: " + global::GameCore.Version.VersionString);
		if (global::GameCore.Version.PrivateBeta)
		{
			ServerConsole.AddLog("PRIVATE BETA VERSION - DO NOT SHARE");
		}
		yield return float.NegativeInfinity;
		ServerConsole.AddLog(global::GameCore.ConfigFile.ServerConfig.GetBool("online_mode", def: true) ? "Online mode is ENABLED." : "Online mode is DISABLED - SERVER CANNOT VALIDATE USER ID OF CONNECTING PLAYERS!!! Features like User ID admin authentication won't work.");
		ServerConsole.RunRefreshPublicKey();
		if (_queryEnabled)
		{
			_queryPort = global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + global::GameCore.ConfigFile.ServerConfig.GetInt("query_port_shift");
			ServerConsole.AddLog("Query port will be enabled on port " + _queryPort + " TCP.");
			_queryserver = new QueryServer(_queryPort, global::GameCore.ConfigFile.ServerConfig.GetBool("query_use_IPv6", def: true));
			_queryserver.StartServer();
		}
		else
		{
			ServerConsole.AddLog("Query port disabled in config!");
		}
		ServerConsole.AddLog("Starting server...");
		if (global::GameCore.ConfigFile.HosterPolicy.GetString("server_ip", "none") != "none")
		{
			Ip = global::GameCore.ConfigFile.HosterPolicy.GetString("server_ip", "none");
			ServerConsole.AddLog("Server IP set to " + Ip + " by your hosting provider.");
		}
		else if (global::GameCore.ConfigFile.ServerConfig.GetBool("online_mode", def: true) && ServerStatic.IsDedicated)
		{
			if (global::GameCore.ConfigFile.ServerConfig.GetString("server_ip", "auto") != "auto")
			{
				Ip = global::GameCore.ConfigFile.ServerConfig.GetString("server_ip", "auto");
				ServerConsole.AddLog("Custom config detected. Your game-server IP will be " + Ip);
			}
			else
			{
				ServerConsole.AddLog("Obtaining your external IP address...");
				using (global::UnityEngine.Networking.UnityWebRequest www = global::UnityEngine.Networking.UnityWebRequest.Get(CentralServer.StandardUrl + "ip.php"))
				{
					yield return global::MEC.Timing.WaitUntilDone(www.SendWebRequest());
					if (!string.IsNullOrEmpty(www.error))
					{
						ServerConsole.AddLog("Error: connection to " + CentralServer.StandardUrl + " failed. Website returned: " + www.error + " | Aborting startup...", global::System.ConsoleColor.DarkRed);
						yield break;
					}
					Ip = (www.downloadHandler.text.EndsWith(".") ? www.downloadHandler.text.Remove(www.downloadHandler.text.Length - 1) : www.downloadHandler.text);
					ServerConsole.AddLog("Done, your game-server IP will be " + Ip);
				}
			}
		}
		else
		{
			Ip = "127.0.0.1";
		}
		ServerConsole.Ip = Ip;
		ServerConsole.AddLog("Initializing game server...");
		if (!ServerStatic.IsDedicated)
		{
			yield break;
		}
		if (global::GameCore.ConfigFile.HosterPolicy.GetString("ipv4_bind_ip", "none") != "none")
		{
			global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress = global::GameCore.ConfigFile.HosterPolicy.GetString("ipv4_bind_ip", "0.0.0.0");
			if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress == "0.0.0.0")
			{
				ServerConsole.AddLog("Server starting at all IPv4 addresses and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + " - set by your hosting provider.");
			}
			else
			{
				ServerConsole.AddLog("Server starting at IPv4 " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress + " and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + " - set by your hosting provider.");
			}
		}
		else
		{
			global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress = global::GameCore.ConfigFile.ServerConfig.GetString("ipv4_bind_ip", "0.0.0.0");
			if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress == "0.0.0.0")
			{
				ServerConsole.AddLog("Server starting at all IPv4 addresses and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port);
			}
			else
			{
				ServerConsole.AddLog("Server starting at IPv4 " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv4BindAddress + " and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port);
			}
		}
		if (global::GameCore.ConfigFile.HosterPolicy.GetString("ipv6_bind_ip", "none") != "none")
		{
			global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress = global::GameCore.ConfigFile.HosterPolicy.GetString("ipv6_bind_ip", "::");
			if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress == "::")
			{
				ServerConsole.AddLog("Server starting at all IPv6 addresses and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + " - set by your hosting provider.");
			}
			else
			{
				ServerConsole.AddLog("Server starting at IPv6 " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress + " and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port + " - set by your hosting provider.");
			}
		}
		else
		{
			global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress = global::GameCore.ConfigFile.ServerConfig.GetString("ipv6_bind_ip", "::");
			if (global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress == "::")
			{
				ServerConsole.AddLog("Server starting at all IPv6 addresses and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port);
			}
			else
			{
				ServerConsole.AddLog("Server starting at IPv6 " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.serverIPv6BindAddress + " and port " + global::Mirror.LiteNetLib4Mirror.LiteNetLib4MirrorTransport.Singleton.port);
			}
		}
		if (ServerConsole.PublicKey == null && global::GameCore.ConfigFile.ServerConfig.GetBool("online_mode", def: true))
		{
			ServerConsole.AddLog("Central server public key is not loaded. Waiting...");
			while (ServerConsole.PublicKey == null)
			{
				yield return global::MEC.Timing.WaitForSeconds(0.25f);
			}
			ServerConsole.AddLog("Continuing server startup sequence...");
		}
		StartHost();
		while (global::UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Facility")
		{
			yield return float.NegativeInfinity;
		}
		ServerConsole.AddLog("Level loaded. Creating match...");
		if (!global::GameCore.ConfigFile.ServerConfig.GetBool("online_mode", def: true))
		{
			ServerConsole.AddLog("Server WON'T be visible on the public list due to online_mode turned off in server configuration.", global::System.ConsoleColor.DarkRed);
			yield break;
		}
		if (!global::GameCore.ConfigFile.ServerConfig.GetBool("use_global_bans", def: true))
		{
			ServerConsole.AddLog("Server WON'T be visible on the public list due to use_global_bans turned off in server configuration.", global::System.ConsoleColor.DarkRed);
			yield break;
		}
		if (global::GameCore.ConfigFile.ServerConfig.GetBool("disable_global_badges"))
		{
			ServerConsole.AddLog("Server WON'T be visible on the public list due to disable_global_badges turned on in server configuration (this is servermod function - if you are not using servermod, you can safely remove this config value, it won't change anything).", global::System.ConsoleColor.DarkRed);
			yield break;
		}
		if (global::GameCore.ConfigFile.ServerConfig.GetBool("hide_global_badges"))
		{
			ServerConsole.AddLog("Server WON'T be visible on the public list due to hide_global_badges turned on in server configuration. You can still disable specific badges instead of using this command. (this is servermod function - if you are not using servermod, you can safely remove this config value, it won't change anything).", global::System.ConsoleColor.DarkRed);
			yield break;
		}
		if (global::GameCore.ConfigFile.ServerConfig.GetBool("disable_ban_bypass"))
		{
			ServerConsole.AddLog("Server WON'T be visible on the public list due to disable_ban_bypass turned on in server configuration. (this is servermod function - if you are not using servermod, you can safely remove this config value, it won't change anything).", global::System.ConsoleColor.DarkRed);
			yield break;
		}
		if (global::GameCore.ConfigFile.ServerConfig.GetBool("hide_patreon_badges_by_default") || global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_patreon_badges") || global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_banteam_badges") || global::GameCore.ConfigFile.ServerConfig.GetBool("block_gtag_management_badges"))
		{
			ServerConsole.AddLog("If your server is verified (put in the official server list) some badge settings enabled in your config will be ignored. If your server isn't on the public list - ignore this message.", global::System.ConsoleColor.DarkRed);
		}
		global::UnityEngine.Object.FindObjectOfType<ServerConsole>().RunServer();
	}

	public ushort GetFreePort()
	{
		return ServerStatic.ServerPort;
	}
}
