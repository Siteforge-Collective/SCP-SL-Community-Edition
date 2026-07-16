public class NicknameSync : global::Mirror.NetworkBehaviour
{
	public global::UnityEngine.LayerMask RaycastMask;

	private ReferenceHub _hub;

	private global::System.Text.RegularExpressions.Regex _nickFilter;

	private string _replacement;

	[global::Mirror.SyncVar]
	public float ViewRange;

	[global::Mirror.SyncVar(hook = "SetCustomInfo")]
	private string _customPlayerInfoString;

	[global::Mirror.SyncVar]
	private PlayerInfoArea _playerInfoToShow = PlayerInfoArea.Nickname | PlayerInfoArea.Badge | PlayerInfoArea.CustomInfo | PlayerInfoArea.Role | PlayerInfoArea.UnitName | PlayerInfoArea.PowerStatus;

	[global::Mirror.SyncVar(hook = "UpdatePlayerlistInstance")]
	private string _myNickSync;

	private string _firstNickname;

	private const ushort MaxNicknameLen = 48;

	[global::Mirror.SyncVar(hook = "UpdateCustomName")]
	private string _displayName;

	private string _cleanDisplayName;

	public bool NickSet { get; private set; }

	public PlayerInfoArea ShownPlayerInfo
	{
		get
		{
			return _playerInfoToShow;
		}
		set
		{
			if (global::Mirror.NetworkServer.active)
			{
				Network_playerInfoToShow = value;
			}
		}
	}

	public string CustomPlayerInfo
	{
		get
		{
			return _customPlayerInfoString;
		}
		set
		{
			if (global::Mirror.NetworkServer.active)
			{
				Network_customPlayerInfoString = value;
			}
		}
	}

	public string DisplayName
	{
		get
		{
			return (HasCustomName ? _cleanDisplayName : MyNick) ?? string.Empty;
		}
		set
		{
			Network_displayName = value;
			UpdatePlayerlistInstance(null, _displayName);
		}
	}

	public bool HasCustomName => _cleanDisplayName != null;

	public string CombinedName
	{
		get
		{
			if (!HasCustomName)
			{
				return MyNick;
			}
			return _cleanDisplayName + " (" + MyNick + ")";
		}
	}

	public string MyNick
	{
		get
		{
			if (NickSet)
			{
				return _firstNickname;
			}
			if (_myNickSync == null)
			{
				return "(null)";
			}
			NickSet = true;
			_firstNickname = _myNickSync.Replace("<", "＜").Replace(">", "＞").Replace("\n", string.Empty)
				.Replace("\r", string.Empty);
			if (_firstNickname.Length > 48)
			{
				_firstNickname = _firstNickname.Substring(0, 48);
			}
			return _firstNickname;
		}
		private set
		{
			if (value == null)
			{
				value = "(null)";
			}
			if (value.Length > 48)
			{
				string text = value.Replace("<", "＜").Replace(">", "＞");
				text = text.Substring(0, 48);
				Network_myNickSync = text;
			}
			else
			{
				Network_myNickSync = value.Replace("<", "＜").Replace(">", "＞");
			}
			if (global::Mirror.NetworkServer.active)
			{
				NickSet = true;
				_firstNickname = _myNickSync;
			}
		}
	}

	public float NetworkViewRange
	{
		get
		{
			return ViewRange;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref ViewRange))
			{
				float viewRange = ViewRange;
				SetSyncVar(value, ref ViewRange, 1uL);
			}
		}
	}

	public string Network_customPlayerInfoString
	{
		get
		{
			return _customPlayerInfoString;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _customPlayerInfoString))
			{
				string customPlayerInfoString = _customPlayerInfoString;
				SetSyncVar(value, ref _customPlayerInfoString, 2uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(2uL))
				{
					setSyncVarHookGuard(2uL, value: true);
					SetCustomInfo(customPlayerInfoString, value);
					setSyncVarHookGuard(2uL, value: false);
				}
			}
		}
	}

	public PlayerInfoArea Network_playerInfoToShow
	{
		get
		{
			return _playerInfoToShow;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _playerInfoToShow))
			{
				PlayerInfoArea playerInfoToShow = _playerInfoToShow;
				SetSyncVar(value, ref _playerInfoToShow, 4uL);
			}
		}
	}

	public string Network_myNickSync
	{
		get
		{
			return _myNickSync;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _myNickSync))
			{
				string myNickSync = _myNickSync;
				SetSyncVar(value, ref _myNickSync, 8uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(8uL))
				{
					setSyncVarHookGuard(8uL, value: true);
					UpdatePlayerlistInstance(myNickSync, value);
					setSyncVarHookGuard(8uL, value: false);
				}
			}
		}
	}

	public string Network_displayName
	{
		get
		{
			return _displayName;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _displayName))
			{
				string displayName = _displayName;
				SetSyncVar(value, ref _displayName, 16uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(16uL))
				{
					setSyncVarHookGuard(16uL, value: true);
					UpdateCustomName(displayName, value);
					setSyncVarHookGuard(16uL, value: false);
				}
			}
		}
	}

	private void UpdatePlayerlistInstance(string p, string username)
	{
	}

	private void UpdateCustomName(string p, string username)
	{
		if (string.IsNullOrWhiteSpace(username))
		{
			_cleanDisplayName = null;
		}
		else
		{
			_cleanDisplayName = username.Replace("<", "＜").Replace(">", "＞").Replace("\n", string.Empty)
				.Replace("\r", string.Empty)
				.Trim();
			if (_cleanDisplayName.Length > 48)
			{
				_cleanDisplayName = _cleanDisplayName.Substring(0, 48);
			}
			_cleanDisplayName += "<color=#855439>*</color>";
		}
		UpdatePlayerlistInstance(p, username);
	}

	private void Start()
	{
		_hub = ReferenceHub.GetHub(base.gameObject);
		_nickFilter = null;
		_replacement = "";
		if (global::Mirror.NetworkServer.active)
		{
			NetworkViewRange = global::GameCore.ConfigFile.ServerConfig.GetFloat("player_info_range", 10f);
			string text = global::GameCore.ConfigFile.ServerConfig.GetString("nickname_filter") ?? "";
			if (!string.IsNullOrEmpty(text))
			{
				_nickFilter = new global::System.Text.RegularExpressions.Regex(text, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Compiled, global::System.TimeSpan.FromMilliseconds(500.0));
				_replacement = global::GameCore.ConfigFile.ServerConfig.GetString("nickname_filter_replacement") ?? "";
			}
		}
		if (base.isLocalPlayer)
		{
			SetNick("Dedicated Server");
		}
	}

	private bool TryGetRayTransform(out global::UnityEngine.Transform tr)
	{
		if (_hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole)
		{
			tr = _hub.PlayerCameraReference;
			return true;
		}
		if (global::PlayerRoles.Spectating.SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) && hub.roleManager.CurrentRole is global::PlayerRoles.FirstPersonControl.IFpcRole)
		{
			tr = MainCameraController.CurrentCamera;
			return true;
		}
		tr = null;
		return false;
	}

	private void Update()
	{
	}

	private void SetCustomInfo(string oldValue, string newValue)
	{
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdSetNick(string n)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, n);
		SendCommandInternal(typeof(NicknameSync), "CmdSetNick", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ServerCallback]
	public void UpdateNickname(string n)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		NickSet = true;
		if (n == null)
		{
			ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing null name.");
			BanPlayer.BanUser(_hub, "Null name", 1577847600L);
			SetNick("(null)");
			return;
		}
		if (n.Length > 1024)
		{
			ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing a too long name.");
			BanPlayer.BanUser(_hub, "Too long name", 1577847600L);
			SetNick("(too long)");
			return;
		}
		global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent(n.Length);
		char c = '0';
		bool flag = false;
		foreach (char c2 in n)
		{
			if (char.IsLetterOrDigit(c2) || char.IsPunctuation(c2) || char.IsSymbol(c2))
			{
				flag = true;
				stringBuilder.Append(c2);
			}
			else if (char.IsWhiteSpace(c2) && c2 != '\n' && c2 != '\r' && c2 != '\t')
			{
				stringBuilder.Append(c2);
			}
			else if (char.IsHighSurrogate(c2))
			{
				c = c2;
			}
			else if (char.IsLowSurrogate(c2) && char.IsSurrogatePair(c, c2))
			{
				stringBuilder.Append(c);
				stringBuilder.Append(c2);
				flag = true;
			}
		}
		if (!flag)
		{
			ServerConsole.AddLog("Kicked " + base.connectionToClient.address + " for having an empty name.");
			ServerConsole.Disconnect(base.connectionToClient, "You may not have an empty name.");
			SetNick("Empty Name");
			global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
			return;
		}
		string text = stringBuilder.ToString();
		global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
		if (text.Length > 48)
		{
			text = text.Substring(0, 48);
		}
		SetNick(text);
	}

	[global::Mirror.Server]
	private void SetNick(string nick)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void NicknameSync::SetNick(System.String)' called when server was not active");
			return;
		}
		MyNick = nick;
		string text;
		try
		{
			text = _nickFilter?.Replace(nick, _replacement) ?? nick;
		}
		catch (global::System.Exception arg)
		{
			ServerConsole.AddLog($"Error when filtering nick {nick}: {arg}");
			text = "(filter failed)";
		}
		if (nick != text)
		{
			DisplayName = text;
		}
		if (!base.isLocalPlayer || !ServerStatic.IsDedicated)
		{
			ServerConsole.AddLog("Nickname of " + _hub.characterClassManager.UserId + " is now " + nick + ".");
			ServerLogs.AddLog(ServerLogs.Modules.Networking, "Nickname of " + _hub.characterClassManager.UserId + " is now " + nick + ".", ServerLogs.ServerLogType.ConnectionUpdate);
		}
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_CmdSetNick(string n)
	{
		if (base.isLocalPlayer)
		{
			MyNick = n;
		}
		else
		{
			if (NickSet || CharacterClassManager.OnlineMode)
			{
				return;
			}
			NickSet = true;
			if (n == null)
			{
				ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing null name.");
				BanPlayer.BanUser(_hub, "Null name", 1577847600L);
				SetNick("(null)");
				return;
			}
			if (n.Length > 1024)
			{
				ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing a too long name.");
				BanPlayer.BanUser(_hub, "Too long name", 1577847600L);
				SetNick("(too long)");
				return;
			}
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent(n.Length);
			char c = '0';
			bool flag = false;
			foreach (char c2 in n)
			{
				if (char.IsLetterOrDigit(c2) || char.IsPunctuation(c2) || char.IsSymbol(c2))
				{
					flag = true;
					stringBuilder.Append(c2);
				}
				else if (char.IsWhiteSpace(c2) && c2 != '\n' && c2 != '\r' && c2 != '\t')
				{
					stringBuilder.Append(c2);
				}
				else if (char.IsHighSurrogate(c2))
				{
					c = c2;
				}
				else if (char.IsLowSurrogate(c2) && char.IsSurrogatePair(c, c2))
				{
					stringBuilder.Append(c);
					stringBuilder.Append(c2);
					flag = true;
				}
			}
			if (!flag)
			{
				ServerConsole.AddLog("Kicked " + base.connectionToClient.address + " for having an empty name.");
				ServerConsole.Disconnect(base.connectionToClient, "You may not have an empty name.");
				SetNick("Empty Name");
				global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
				return;
			}
			string text = stringBuilder.ToString();
			global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
			text = text.Replace("<", "＜");
			text = text.Replace(">", "＞");
			text = text.Replace("[", "(");
			text = text.Replace("]", ")");
			if (text.Length > 48)
			{
				text = text.Substring(0, 48);
			}
			SetNick(text);
			_hub.characterClassManager.SyncServerCmdBinding();
		}
	}

	protected static void InvokeUserCode_CmdSetNick(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSetNick called on client.");
		}
		else
		{
			((NicknameSync)obj).UserCode_CmdSetNick(global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	static NicknameSync()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(NicknameSync), "CmdSetNick", InvokeUserCode_CmdSetNick, requiresAuthority: true);
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, ViewRange);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _customPlayerInfoString);
			global::Mirror.GeneratedNetworkCode._Write_PlayerInfoArea(writer, _playerInfoToShow);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myNickSync);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _displayName);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteSingle(writer, ViewRange);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _customPlayerInfoString);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			global::Mirror.GeneratedNetworkCode._Write_PlayerInfoArea(writer, _playerInfoToShow);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myNickSync);
			result = true;
		}
		if ((base.syncVarDirtyBits & 0x10L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _displayName);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			float viewRange = ViewRange;
			NetworkViewRange = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
			string customPlayerInfoString = _customPlayerInfoString;
			Network_customPlayerInfoString = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(customPlayerInfoString, ref _customPlayerInfoString))
			{
				SetCustomInfo(customPlayerInfoString, _customPlayerInfoString);
			}
			PlayerInfoArea playerInfoToShow = _playerInfoToShow;
			Network_playerInfoToShow = global::Mirror.GeneratedNetworkCode._Read_PlayerInfoArea(reader);
			string myNickSync = _myNickSync;
			Network_myNickSync = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myNickSync, ref _myNickSync))
			{
				UpdatePlayerlistInstance(myNickSync, _myNickSync);
			}
			string displayName = _displayName;
			Network_displayName = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(displayName, ref _displayName))
			{
				UpdateCustomName(displayName, _displayName);
			}
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			float viewRange2 = ViewRange;
			NetworkViewRange = global::Mirror.NetworkReaderExtensions.ReadSingle(reader);
		}
		if ((num & 2L) != 0L)
		{
			string customPlayerInfoString2 = _customPlayerInfoString;
			Network_customPlayerInfoString = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(customPlayerInfoString2, ref _customPlayerInfoString))
			{
				SetCustomInfo(customPlayerInfoString2, _customPlayerInfoString);
			}
		}
		if ((num & 4L) != 0L)
		{
			PlayerInfoArea playerInfoToShow2 = _playerInfoToShow;
			Network_playerInfoToShow = global::Mirror.GeneratedNetworkCode._Read_PlayerInfoArea(reader);
		}
		if ((num & 8L) != 0L)
		{
			string myNickSync2 = _myNickSync;
			Network_myNickSync = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myNickSync2, ref _myNickSync))
			{
				UpdatePlayerlistInstance(myNickSync2, _myNickSync);
			}
		}
		if ((num & 0x10L) != 0L)
		{
			string displayName2 = _displayName;
			Network_displayName = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(displayName2, ref _displayName))
			{
				UpdateCustomName(displayName2, _displayName);
			}
		}
	}
}
