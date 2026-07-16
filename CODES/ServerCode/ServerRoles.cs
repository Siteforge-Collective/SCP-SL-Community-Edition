public class ServerRoles : global::Mirror.NetworkBehaviour
{
	[global::System.Serializable]
	public class NamedColor
	{
		public string Name;

		public string ColorHex;

		public bool Restricted;

		[global::UnityEngine.SerializeField]
		private string _speakingOverride;

		private global::UnityEngine.Color _speakingColorCache;

		private bool _speakingColorSet;

		public global::UnityEngine.Color SpeakingColor
		{
			get
			{
				if (!_speakingColorSet)
				{
					_speakingColorSet = true;
					global::UnityEngine.ColorUtility.TryParseHtmlString("#" + (string.IsNullOrEmpty(_speakingOverride) ? ColorHex : _speakingOverride), out _speakingColorCache);
				}
				return _speakingColorCache;
			}
		}
	}

	[global::System.Serializable]
	public enum AccessMode : byte
	{
		LocalAccess = 1,
		GlobalAccess = 2,
		PasswordOverride = 3
	}

	public ServerRoles.NamedColor CurrentColor;

	public ServerRoles.NamedColor[] NamedColors;

	[global::System.NonSerialized]
	internal bool PublicKeyAccepted;

	[global::System.NonSerialized]
	public global::System.Collections.Generic.Dictionary<string, string> FirstVerResult;

	internal global::Org.BouncyCastle.Crypto.AsymmetricKeyParameter PublicKey;

	[global::System.NonSerialized]
	public bool BypassMode;

	[global::System.NonSerialized]
	public bool LocalRemoteAdmin;

	private bool _authorizeBadge;

	internal bool OverwatchPermitted;

	internal string PrevBadge;

	internal UserGroup Group;

	private CharacterClassManager _ccm;

	private ReferenceHub _hub;

	internal static bool AllowSameAccountJoining;

	private static readonly global::System.Collections.Generic.Dictionary<string, ServerRoles.NamedColor> DictionarizedColorsCache;

	private static bool _colorDictionaryCacheSet;

	private const string DefaultColor = "default";

	public const ulong UserIdPerms = 18007046uL;

	private string _globalBadgeUnconfirmed;

	private string _prevColor;

	private string _prevText;

	private string _prevBadge;

	private string _authChallenge;

	private string _badgeChallenge;

	private string _bgc;

	private string _bgt;

	private bool _hideLocalBadge;

	private bool _neverHideLocalBadge;

	private bool _neverCover;

	private bool _prefSet;

	private bool _badgeCover;

	private bool _requested;

	private bool _publicPartRequested;

	private bool _badgeRequested;

	private bool _authRequested;

	private bool _noclipReady;

	private bool _publicInfoDirty;

	[global::Mirror.SyncVar(hook = "SetTextHook")]
	private string _myText;

	[global::Mirror.SyncVar(hook = "SetColorHook")]
	private string _myColor;

	[global::Mirror.SyncVar(hook = "SetPublicInfo")]
	public string PublicPlayerInfoToken;

	[global::Mirror.SyncVar]
	public string GlobalBadge;

	[global::System.NonSerialized]
	public int GlobalBadgeType;

	[global::System.NonSerialized]
	public bool RemoteAdmin;

	[global::System.NonSerialized]
	public bool Staff;

	[global::System.NonSerialized]
	public bool BypassStaff;

	[global::System.NonSerialized]
	public bool RaEverywhere;

	[global::System.NonSerialized]
	public ulong Permissions;

	[global::System.NonSerialized]
	public string HiddenBadge;

	[global::System.NonSerialized]
	public bool GlobalHidden;

	[global::System.NonSerialized]
	internal bool AdminChatPerms;

	[global::System.NonSerialized]
	private ulong _globalPerms;

	[global::System.NonSerialized]
	private bool _lastRealIdPerm;

	[global::System.NonSerialized]
	public string FixedBadge;

	[global::System.NonSerialized]
	public bool DoNotTrack;

	[global::System.NonSerialized]
	public bool SyncHashed;

	[global::System.NonSerialized]
	public ServerRoles.AccessMode RemoteAdminMode;

	[global::System.NonSerialized]
	internal global::System.Collections.Generic.Dictionary<string, int> PlayerSkins;

	private global::Security.RateLimit _commandRateLimit;

	public bool IsInOverwatch
	{
		get
		{
			return _hub.roleManager.CurrentRole is global::PlayerRoles.Spectating.OverwatchRole;
		}
		set
		{
			if (value != IsInOverwatch)
			{
				_hub.roleManager.ServerSetRole(value ? global::PlayerRoles.RoleTypeId.Overwatch : global::PlayerRoles.RoleTypeId.Spectator, global::PlayerRoles.RoleChangeReason.RemoteAdmin);
			}
		}
	}

	private global::System.Collections.Generic.Dictionary<string, ServerRoles.NamedColor> NamedColorsDic
	{
		get
		{
			if (_colorDictionaryCacheSet)
			{
				return DictionarizedColorsCache;
			}
			ServerRoles.NamedColor[] namedColors = NamedColors;
			foreach (ServerRoles.NamedColor namedColor in namedColors)
			{
				DictionarizedColorsCache[namedColor.Name] = namedColor;
			}
			_colorDictionaryCacheSet = true;
			return DictionarizedColorsCache;
		}
	}

	private bool IsVerified => _ccm.InstanceMode != ClientInstanceMode.Unverified;

	internal byte KickPower
	{
		get
		{
			if (!RaEverywhere)
			{
				return Group?.KickPower ?? 0;
			}
			return byte.MaxValue;
		}
	}

	public string MyColor { get; private set; }

	public string MyText { get; private set; }

	public bool GlobalSet
	{
		get
		{
			if (MyText != null && MyText.StartsWith("[", global::System.StringComparison.Ordinal))
			{
				return MyText.EndsWith("]", global::System.StringComparison.Ordinal);
			}
			return false;
		}
	}

	public string Network_myText
	{
		get
		{
			return _myText;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _myText))
			{
				string myText = _myText;
				SetSyncVar(value, ref _myText, 1uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
				{
					setSyncVarHookGuard(1uL, value: true);
					SetTextHook(myText, value);
					setSyncVarHookGuard(1uL, value: false);
				}
			}
		}
	}

	public string Network_myColor
	{
		get
		{
			return _myColor;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref _myColor))
			{
				string myColor = _myColor;
				SetSyncVar(value, ref _myColor, 2uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(2uL))
				{
					setSyncVarHookGuard(2uL, value: true);
					SetColorHook(myColor, value);
					setSyncVarHookGuard(2uL, value: false);
				}
			}
		}
	}

	public string NetworkPublicPlayerInfoToken
	{
		get
		{
			return PublicPlayerInfoToken;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref PublicPlayerInfoToken))
			{
				string publicPlayerInfoToken = PublicPlayerInfoToken;
				SetSyncVar(value, ref PublicPlayerInfoToken, 4uL);
				if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(4uL))
				{
					setSyncVarHookGuard(4uL, value: true);
					SetPublicInfo(publicPlayerInfoToken, value);
					setSyncVarHookGuard(4uL, value: false);
				}
			}
		}
	}

	public string NetworkGlobalBadge
	{
		get
		{
			return GlobalBadge;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref GlobalBadge))
			{
				string globalBadge = GlobalBadge;
				SetSyncVar(value, ref GlobalBadge, 8uL);
			}
		}
	}

	public void Awake()
	{
		PlayerSkins = new global::System.Collections.Generic.Dictionary<string, int>();
	}

	public void Start()
	{
		_hub = ReferenceHub.GetHub(base.gameObject);
		_commandRateLimit = _hub.playerRateLimitHandler.RateLimits[2];
		_ccm = _hub.characterClassManager;
	}

	[global::Mirror.TargetRpc]
	private void TargetSetHiddenRole(global::Mirror.NetworkConnection connection, string role)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, role);
		SendTargetRPCInternal(connection, typeof(ServerRoles), "TargetSetHiddenRole", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	public void RpcResetFixed()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(ServerRoles), "RpcResetFixed", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdRequestBadge(string token)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, token);
		SendCommandInternal(typeof(ServerRoles), "CmdRequestBadge", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdSetPublicPart(string token)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, token);
		SendCommandInternal(typeof(ServerRoles), "CmdSetPublicPart", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdDoNotTrack()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendCommandInternal(typeof(ServerRoles), "CmdDoNotTrack", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdSetLocalTagPreferences(bool hide, bool neverHide, bool neverCover)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, hide);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, neverHide);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, neverCover);
		SendCommandInternal(typeof(ServerRoles), "CmdSetLocalTagPreferences", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ServerCallback]
	public void SetDoNotTrack()
	{
		if (global::Mirror.NetworkServer.active && !DoNotTrack)
		{
			DoNotTrack = true;
			if (!string.IsNullOrEmpty(GetComponent<NicknameSync>().MyNick))
			{
				LogDNT();
			}
			if (!base.isLocalPlayer)
			{
				GetComponent<GameConsoleTransmission>().SendToClient(base.connectionToClient, "Your \"Do not track\" request has been received.", "green");
			}
		}
	}

	[global::Mirror.ServerCallback]
	private void LogDNT()
	{
		if (global::Mirror.NetworkServer.active && _ccm.UserId != null)
		{
			ServerLogs.AddLog(ServerLogs.Modules.Networking, $"{_ccm}) connected from IP " + base.connectionToClient.address + " sent Do Not Track signal.", ServerLogs.ServerLogType.ConnectionUpdate);
			_ccm.RefreshSyncedId();
		}
	}

	[global::Mirror.ServerCallback]
	public void RefreshPermissions(bool disp = false)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		UserGroup userGroup = ServerStatic.PermissionsHandler.GetUserGroup(_ccm.UserId);
		if (userGroup != null)
		{
			SetGroup(userGroup, ovr: false, byAdmin: false, disp);
		}
		else if (_ccm.UserId2 != null)
		{
			userGroup = ServerStatic.PermissionsHandler.GetUserGroup(_ccm.UserId2);
			if (userGroup != null)
			{
				SetGroup(userGroup, ovr: false, byAdmin: false, disp);
			}
		}
		GetComponent<global::RemoteAdmin.QueryProcessor>().GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
	}

	[global::Mirror.ServerCallback]
	public void SetGroup(UserGroup group, bool ovr, bool byAdmin = false, bool disp = false)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		if (group == null)
		{
			if (!RaEverywhere || _globalPerms != ServerStatic.PermissionsHandler.FullPerm)
			{
				RemoteAdmin = _globalPerms != 0;
				Permissions = _globalPerms;
				_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
				RemoteAdminMode = ((_globalPerms == 0L) ? ServerRoles.AccessMode.LocalAccess : ServerRoles.AccessMode.GlobalAccess);
				AdminChatPerms = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.AdminChat) || RaEverywhere;
				Group = null;
				SetColor(null);
				SetText(null);
				_badgeCover = false;
				if (!string.IsNullOrEmpty(PrevBadge))
				{
					NetworkGlobalBadge = PrevBadge;
				}
				TargetCloseRemoteAdmin();
				SendRealIds();
				SendConsoleMsg("Your local permissions has been revoked by server administrator.", "red");
			}
			return;
		}
		SendConsoleMsg((!byAdmin) ? "Updating your group on server (local permissions)..." : "Updating your group on server (set by server administrator)...", "cyan");
		Group = group;
		_badgeCover = group.Cover;
		if (!OverwatchPermitted && PermissionsHandler.IsPermitted(group.Permissions, PlayerPermissions.Overwatch))
		{
			OverwatchPermitted = true;
		}
		if ((group.Permissions | _globalPerms) != 0 && ServerStatic.PermissionsHandler.IsRaPermitted(group.Permissions | _globalPerms))
		{
			RemoteAdmin = true;
			Permissions = group.Permissions | _globalPerms;
			_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
			RemoteAdminMode = ((_globalPerms != 0) ? ServerRoles.AccessMode.GlobalAccess : ((!ovr) ? ServerRoles.AccessMode.LocalAccess : ServerRoles.AccessMode.PasswordOverride));
			AdminChatPerms = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.AdminChat) || RaEverywhere;
			GetComponent<global::RemoteAdmin.QueryProcessor>().PasswordTries = 0;
			OpenRemoteAdmin(ovr);
			SendConsoleMsg((!byAdmin) ? "Your remote admin access has been granted (local permissions)." : "Your remote admin access has been granted (set by server administrator).", "cyan");
		}
		else
		{
			RemoteAdmin = false;
			Permissions = group.Permissions | _globalPerms;
			_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
			RemoteAdminMode = ((_globalPerms == 0) ? ServerRoles.AccessMode.LocalAccess : ServerRoles.AccessMode.GlobalAccess);
			AdminChatPerms = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.AdminChat) || RaEverywhere;
			TargetCloseRemoteAdmin();
		}
		SendRealIds();
		bool flag = Staff || PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.ViewHiddenBadges);
		bool flag2 = Staff || PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.ViewHiddenGlobalBadges);
		if (flag || flag2)
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub.Mode != ClientInstanceMode.DedicatedServer)
				{
					ServerRoles serverRoles = allHub.serverRoles;
					if (!string.IsNullOrEmpty(serverRoles.HiddenBadge) && (!serverRoles.GlobalHidden || flag2) && (serverRoles.GlobalHidden || flag))
					{
						serverRoles.TargetSetHiddenRole(base.connectionToClient, serverRoles.HiddenBadge);
					}
				}
			}
			if (flag && flag2)
			{
				SendConsoleMsg("Hidden badges (local and global) have been displayed for you (if there are any).", "gray");
			}
			else if (flag)
			{
				SendConsoleMsg("Hidden badges (local only) have been displayed for you (if there are any).", "gray");
			}
			else
			{
				SendConsoleMsg("Hidden badges (global only) have been displayed for you (if there are any).", "gray");
			}
		}
		ServerLogs.AddLog(ServerLogs.Modules.Permissions, _hub.LoggedNameFromRefHub() + " has been assigned to group " + group.BadgeText + ".", ServerLogs.ServerLogType.ConnectionUpdate);
		if (group.BadgeColor == "none")
		{
			return;
		}
		if (_hideLocalBadge || (group.HiddenByDefault && !disp && !_neverHideLocalBadge))
		{
			_badgeCover = false;
			if (string.IsNullOrEmpty(MyText))
			{
				SetText(null);
				SetColor("default");
				HiddenBadge = group.BadgeText;
				RefreshHiddenTag();
				TargetSetHiddenRole(base.connectionToClient, group.BadgeText);
				if (!byAdmin)
				{
					SendConsoleMsg("Your role has been granted, but it's hidden. Use \"showtag\" command in the game console to show your server badge.", "yellow");
				}
				else
				{
					SendConsoleMsg("Your role has been granted to you (set by server administrator), but it's hidden. Use \"showtag\" command in the game console to show your server badge.", "cyan");
				}
			}
			return;
		}
		HiddenBadge = null;
		RpcResetFixed();
		SetText(group.BadgeText);
		SetColor(group.BadgeColor);
		if (!byAdmin)
		{
			SendConsoleMsg("Your role \"" + group.BadgeText + "\" with color " + group.BadgeColor + " has been granted to you (local permissions).", "cyan");
		}
		else
		{
			SendConsoleMsg("Your role \"" + group.BadgeText + "\" with color " + group.BadgeColor + " has been granted to you (set by server administrator).", "cyan");
		}
	}

	[global::Mirror.ServerCallback]
	public void RefreshHiddenTag()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
		{
			if (allHub.Mode != ClientInstanceMode.DedicatedServer)
			{
				ServerRoles serverRoles = allHub.serverRoles;
				bool flag = serverRoles.Staff || PermissionsHandler.IsPermitted(serverRoles.Permissions, PlayerPermissions.ViewHiddenBadges);
				bool flag2 = serverRoles.Staff || PermissionsHandler.IsPermitted(serverRoles.Permissions, PlayerPermissions.ViewHiddenGlobalBadges);
				if ((!GlobalHidden || flag2) && (GlobalHidden || flag))
				{
					TargetSetHiddenRole(serverRoles.connectionToClient, HiddenBadge);
				}
			}
		}
	}

	[global::Mirror.Server]
	public void RefreshRealId()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerRoles::RefreshRealId()' called when server was not active");
			return;
		}
		foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
		{
			if (allHub.Mode != ClientInstanceMode.DedicatedServer && (PermissionsHandler.IsPermitted(allHub.serverRoles.Permissions, 18007046uL) || allHub.serverRoles.Staff || allHub.serverRoles.RaEverywhere))
			{
				_hub.characterClassManager.TargetSetRealId(allHub.networkIdentity.connectionToClient, _hub.characterClassManager.UserId);
			}
		}
	}

	[global::Mirror.Server]
	private void SendRealIds()
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerRoles::SendRealIds()' called when server was not active");
		}
		else
		{
			if (_hub.Mode == ClientInstanceMode.DedicatedServer)
			{
				return;
			}
			bool flag = Staff || RaEverywhere || PermissionsHandler.IsPermitted(Permissions, 18007046uL);
			if (!flag && !_lastRealIdPerm)
			{
				return;
			}
			_lastRealIdPerm = flag;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				allHub.characterClassManager.TargetSetRealId(_hub.networkIdentity.connectionToClient, flag ? allHub.characterClassManager.UserId : null);
			}
		}
	}

	private global::System.Collections.Generic.IEnumerator<float> _RequestRoleFromServer(string token)
	{
		if ((IsVerified || !string.IsNullOrEmpty(_ccm.UserId)) && CentralAuth.ValidatePartialAuthToken(token, _ccm.SaltedUserId, GetComponent<NicknameSync>().MyNick, _ccm.AuthTokenSerial, "Badge request") != null)
		{
			_globalBadgeUnconfirmed = token;
			StartServerChallenge(1);
		}
		yield break;
	}

	private global::System.Collections.Generic.IEnumerator<float> SetPublicTokenOnServer(string token)
	{
		if (IsVerified || !string.IsNullOrEmpty(_ccm.UserId))
		{
			global::System.Collections.Generic.Dictionary<string, string> dict = CentralAuth.ValidatePartialAuthToken(token, _ccm.SaltedUserId, GetComponent<NicknameSync>().MyNick, _ccm.AuthTokenSerial, "Public player info");
			if (dict != null)
			{
				SendConsoleMsg("Your public player info has been accepted.", "cyan");
				NetworkPublicPlayerInfoToken = token;
				ProcessSkins(ref dict);
			}
		}
		yield break;
	}

	public string GetColoredRoleString(bool newLine = false)
	{
		if (string.IsNullOrEmpty(MyColor) || string.IsNullOrEmpty(MyText) || CurrentColor == null)
		{
			return string.Empty;
		}
		if ((CurrentColor.Restricted || global::NorthwoodLib.StringUtils.Contains(MyText, "[", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "]", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "<", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, ">", global::System.StringComparison.Ordinal)) && !_authorizeBadge)
		{
			return string.Empty;
		}
		ServerRoles.NamedColor[] namedColors = NamedColors;
		foreach (ServerRoles.NamedColor namedColor in namedColors)
		{
			if (namedColor.Name == MyColor)
			{
				return (newLine ? "\n" : string.Empty) + "<color=#" + namedColor.ColorHex + ">" + MyText + "</color>";
			}
		}
		return string.Empty;
	}

	public string GetUncoloredRoleString()
	{
		if (string.IsNullOrEmpty(MyText) && !string.IsNullOrEmpty(FixedBadge))
		{
			return FixedBadge;
		}
		if (string.IsNullOrEmpty(MyColor) || string.IsNullOrEmpty(MyText) || CurrentColor == null)
		{
			return string.Empty;
		}
		if ((CurrentColor.Restricted || global::NorthwoodLib.StringUtils.Contains(MyText, "[", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "]", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "<", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, ">", global::System.StringComparison.Ordinal)) && !_authorizeBadge)
		{
			return string.Empty;
		}
		return MyText;
	}

	public global::UnityEngine.Color GetColor()
	{
		return global::UnityEngine.Color.clear;
	}

	public global::UnityEngine.Color GetVoiceColor()
	{
		if (string.IsNullOrEmpty(MyColor) || !NamedColorsDic.TryGetValue(MyColor, out var value))
		{
			return NamedColorsDic["default"].SpeakingColor;
		}
		return value.SpeakingColor;
	}

	private void Update()
	{
		if (CurrentColor == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(FixedBadge) && MyText != FixedBadge)
		{
			SetText(FixedBadge);
			SetColor("silver");
			return;
		}
		if (!string.IsNullOrEmpty(FixedBadge) && CurrentColor.Name != "silver")
		{
			SetColor("silver");
			return;
		}
		if (GlobalBadge != _prevBadge)
		{
			_prevBadge = GlobalBadge;
			if (string.IsNullOrEmpty(GlobalBadge))
			{
				_bgc = null;
				_bgt = null;
				_authorizeBadge = false;
				if (_prevColor != null)
				{
					_prevColor += ".";
				}
				else
				{
					_prevColor = ".";
				}
				if (_prevText != null)
				{
					_prevText += ".";
				}
				else
				{
					_prevText = ".";
				}
				return;
			}
			global::GameCore.Console.AddDebugLog("SDAUTH", "Validating global badge of user " + GetComponent<NicknameSync>().MyNick, MessageImportance.LessImportant);
			global::System.Collections.Generic.Dictionary<string, string> dictionary = CentralAuth.ValidatePartialAuthToken(GlobalBadge, _ccm.SaltedUserId, GetComponent<NicknameSync>().MyNick, null, "Badge request");
			if (dictionary == null)
			{
				global::GameCore.Console.AddDebugLog("SDAUTH", "<color=red>Validation of global badge of user " + GetComponent<NicknameSync>().MyNick + " failed - invalid digital signature.</color>", MessageImportance.Normal);
				_bgc = null;
				_bgt = null;
				_authorizeBadge = false;
				if (_prevColor != null)
				{
					_prevColor += ".";
				}
				else
				{
					_prevColor = ".";
				}
				if (_prevText != null)
				{
					_prevText += ".";
				}
				else
				{
					_prevText = ".";
				}
				return;
			}
			global::GameCore.Console.AddDebugLog("SDAUTH", "Validation of global badge of user " + GetComponent<NicknameSync>().MyNick + " complete - badge signed by central server " + dictionary["Issued by"] + ".", MessageImportance.LessImportant);
			if (dictionary["Badge text"] == "(none)" && dictionary["Badge color"] == "(none)")
			{
				_bgc = null;
				_bgt = null;
				_authorizeBadge = false;
			}
			else
			{
				_bgc = dictionary["Badge color"];
				SetColor(_bgc);
				_bgt = dictionary["Badge text"];
				MyText = _bgt;
				_authorizeBadge = true;
			}
		}
		if (!(_prevColor == MyColor) || !(_prevText == MyText))
		{
			if (CurrentColor.Restricted && (MyText != _bgt || MyColor != _bgc))
			{
				global::GameCore.Console.AddLog("TAG FAIL 1 - " + MyText + " - " + _bgt + " /-/ " + MyColor + " - " + _bgc, global::UnityEngine.Color.gray);
				_authorizeBadge = false;
				SetColor("default");
				SetText(null);
				_prevColor = "default";
				_prevText = null;
				PlayerList.UpdatePlayerRole(_hub);
			}
			else if (MyText != null && MyText != _bgt && (global::NorthwoodLib.StringUtils.Contains(MyText, "[", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "]", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, "<", global::System.StringComparison.Ordinal) || global::NorthwoodLib.StringUtils.Contains(MyText, ">", global::System.StringComparison.Ordinal)))
			{
				global::GameCore.Console.AddLog("TAG FAIL 2 - " + MyText + " - " + _bgt + " /-/ " + MyColor + " - " + _bgc, global::UnityEngine.Color.gray);
				_authorizeBadge = false;
				SetColor("default");
				SetText(null);
				_prevColor = "default";
				_prevText = null;
				PlayerList.UpdatePlayerRole(_hub);
			}
			else
			{
				_prevColor = MyColor;
				_prevText = MyText;
				_prevBadge = GlobalBadge;
				PlayerList.UpdatePlayerRole(_hub);
			}
		}
	}

	private void ProcessSkins(ref global::System.Collections.Generic.Dictionary<string, string> dict)
	{
		PlayerSkins.Clear();
		foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in dict)
		{
			if (item.Key.StartsWith("_") && int.TryParse(item.Value, out var result))
			{
				string key = item.Key.Substring(1);
				if (!PlayerSkins.ContainsKey(key))
				{
					PlayerSkins.Add(key, result);
				}
			}
		}
	}

	private void SendConsoleMsg(string text, string color)
	{
		_hub.gameConsoleTransmission.SendToClient(text, color);
	}

	private void SetColorHook(string p, string i)
	{
		SetColor(i);
	}

	public void SetColor(string i)
	{
		if (string.IsNullOrEmpty(i))
		{
			i = "default";
		}
		if (global::Mirror.NetworkServer.active)
		{
			Network_myColor = i;
		}
		MyColor = i;
		ServerRoles.NamedColor namedColor = global::System.Linq.Enumerable.FirstOrDefault(NamedColors, (ServerRoles.NamedColor row) => row.Name == MyColor);
		if (namedColor == null && i != "default")
		{
			SetColor("default");
		}
		else
		{
			CurrentColor = namedColor;
		}
	}

	private void SetTextHook(string p, string i)
	{
		SetText(i);
	}

	public void SetText(string i)
	{
		if (i == string.Empty)
		{
			i = null;
		}
		if (global::Mirror.NetworkServer.active)
		{
			Network_myText = i;
		}
		MyText = i;
		ServerRoles.NamedColor namedColor = global::System.Linq.Enumerable.FirstOrDefault(NamedColors, (ServerRoles.NamedColor row) => row.Name == MyColor);
		if (namedColor != null)
		{
			CurrentColor = namedColor;
		}
	}

	public void SetPublicInfo(string p, string i)
	{
		if (i == string.Empty)
		{
			NetworkPublicPlayerInfoToken = null;
		}
	}

	[global::Mirror.ServerCallback]
	public void StartServerChallenge(int selector)
	{
		if (global::Mirror.NetworkServer.active && (selector != 0 || string.IsNullOrEmpty(_authChallenge)) && (selector != 1 || string.IsNullOrEmpty(_badgeChallenge)) && selector <= 1 && selector >= 0)
		{
			byte[] array;
			using (global::System.Security.Cryptography.RandomNumberGenerator randomNumberGenerator = new global::System.Security.Cryptography.RNGCryptoServiceProvider())
			{
				array = new byte[32];
				randomNumberGenerator.GetBytes(array);
			}
			string text = global::System.Convert.ToBase64String(array);
			if (selector == 0)
			{
				_authChallenge = "auth-" + text;
				TargetSignServerChallenge(base.connectionToClient, _authChallenge);
			}
			else
			{
				_badgeChallenge = "badge-server-" + text;
				TargetSignServerChallenge(base.connectionToClient, _badgeChallenge);
			}
		}
	}

	[global::Mirror.TargetRpc]
	private void TargetSignServerChallenge(global::Mirror.NetworkConnection target, string challenge)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, challenge);
		SendTargetRPCInternal(target, typeof(ServerRoles), "TargetSignServerChallenge", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	private void CmdServerSignatureComplete(string challenge, string response, string publickey, bool hide)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteString(writer, challenge);
		global::Mirror.NetworkWriterExtensions.WriteString(writer, response);
		global::Mirror.NetworkWriterExtensions.WriteString(writer, publickey);
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, hide);
		SendCommandInternal(typeof(ServerRoles), "CmdServerSignatureComplete", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	internal void OpenRemoteAdmin(bool password)
	{
		TargetOpenRemoteAdmin(password);
		_hub.queryProcessor.SyncCommandsToClient();
	}

	[global::Mirror.TargetRpc]
	private void TargetOpenRemoteAdmin(bool password)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, password);
		SendTargetRPCInternal(null, typeof(ServerRoles), "TargetOpenRemoteAdmin", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.TargetRpc]
	private void TargetCloseRemoteAdmin()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendTargetRPCInternal(null, typeof(ServerRoles), "TargetCloseRemoteAdmin", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Command(channel = 4)]
	public void CmdSetOverwatchStatus(byte status)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteByte(writer, status);
		SendCommandInternal(typeof(ServerRoles), "CmdSetOverwatchStatus", writer, 4);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.Server]
	internal void SetOverwatchStatus(byte status)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void ServerRoles::SetOverwatchStatus(System.Byte)' called when server was not active");
			return;
		}
		switch (status)
		{
		case 0:
			IsInOverwatch = false;
			break;
		case 1:
			IsInOverwatch = true;
			break;
		case 2:
			IsInOverwatch = !IsInOverwatch;
			break;
		}
	}

	public void RequestBadge(string token)
	{
		CmdRequestBadge(token);
	}

	public void SetPublicPart(string token)
	{
		CmdSetPublicPart(token);
	}

	[global::Mirror.TargetRpc]
	private void TargetPublicKeyAccepted(global::Mirror.NetworkConnection conn)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendTargetRPCInternal(conn, typeof(ServerRoles), "TargetPublicKeyAccepted", writer, 0);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	static ServerRoles()
	{
		DictionarizedColorsCache = new global::System.Collections.Generic.Dictionary<string, ServerRoles.NamedColor>();
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdRequestBadge", InvokeUserCode_CmdRequestBadge, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdSetPublicPart", InvokeUserCode_CmdSetPublicPart, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdDoNotTrack", InvokeUserCode_CmdDoNotTrack, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdSetLocalTagPreferences", InvokeUserCode_CmdSetLocalTagPreferences, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdServerSignatureComplete", InvokeUserCode_CmdServerSignatureComplete, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterCommandDelegate(typeof(ServerRoles), "CmdSetOverwatchStatus", InvokeUserCode_CmdSetOverwatchStatus, requiresAuthority: true);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "RpcResetFixed", InvokeUserCode_RpcResetFixed);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "TargetSetHiddenRole", InvokeUserCode_TargetSetHiddenRole);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "TargetSignServerChallenge", InvokeUserCode_TargetSignServerChallenge);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "TargetOpenRemoteAdmin", InvokeUserCode_TargetOpenRemoteAdmin);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "TargetCloseRemoteAdmin", InvokeUserCode_TargetCloseRemoteAdmin);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(ServerRoles), "TargetPublicKeyAccepted", InvokeUserCode_TargetPublicKeyAccepted);
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_TargetSetHiddenRole(global::Mirror.NetworkConnection connection, string role)
	{
		if (!base.isServer)
		{
			if (string.IsNullOrEmpty(role))
			{
				SetColor(null);
				SetText(null);
				FixedBadge = null;
				SetText(null);
			}
			else
			{
				SetColor("silver");
				FixedBadge = role.Replace("[", string.Empty).Replace("]", string.Empty).Replace("<", string.Empty)
					.Replace(">", string.Empty) + " " + TranslationReader.Get("Legacy_Interfaces", 18, "(hidden)");
				SetText(FixedBadge);
			}
		}
	}

	protected static void InvokeUserCode_TargetSetHiddenRole(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetSetHiddenRole called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_TargetSetHiddenRole(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	public void UserCode_RpcResetFixed()
	{
		FixedBadge = null;
	}

	protected static void InvokeUserCode_RpcResetFixed(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcResetFixed called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_RpcResetFixed();
		}
	}

	private void UserCode_CmdRequestBadge(string token)
	{
		if (!_requested && token != null)
		{
			_requested = true;
			global::MEC.Timing.RunCoroutine(_RequestRoleFromServer(token), global::MEC.Segment.FixedUpdate);
		}
	}

	protected static void InvokeUserCode_CmdRequestBadge(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdRequestBadge called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdRequestBadge(global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	private void UserCode_CmdSetPublicPart(string token)
	{
		if (!_publicPartRequested && token != null)
		{
			_publicPartRequested = true;
			global::MEC.Timing.RunCoroutine(SetPublicTokenOnServer(token), global::MEC.Segment.FixedUpdate);
		}
	}

	protected static void InvokeUserCode_CmdSetPublicPart(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSetPublicPart called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdSetPublicPart(global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	private void UserCode_CmdDoNotTrack()
	{
		if (!DoNotTrack)
		{
			SetDoNotTrack();
		}
	}

	protected static void InvokeUserCode_CmdDoNotTrack(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdDoNotTrack called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdDoNotTrack();
		}
	}

	private void UserCode_CmdSetLocalTagPreferences(bool hide, bool neverHide, bool neverCover)
	{
		if (!_prefSet)
		{
			_prefSet = true;
			_hideLocalBadge = hide;
			_neverHideLocalBadge = neverHide;
			_neverCover = neverCover;
		}
	}

	protected static void InvokeUserCode_CmdSetLocalTagPreferences(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSetLocalTagPreferences called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdSetLocalTagPreferences(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	private void UserCode_TargetSignServerChallenge(global::Mirror.NetworkConnection target, string challenge)
	{
	}

	protected static void InvokeUserCode_TargetSignServerChallenge(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetSignServerChallenge called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_TargetSignServerChallenge(global::Mirror.NetworkClient.readyConnection, global::Mirror.NetworkReaderExtensions.ReadString(reader));
		}
	}

	private void UserCode_CmdServerSignatureComplete(string challenge, string response, string publickey, bool hide)
	{
		if (!_commandRateLimit.CanExecute())
		{
			return;
		}
		if (FirstVerResult == null)
		{
			FirstVerResult = CentralAuth.ValidatePartialAuthToken(_globalBadgeUnconfirmed, _ccm.SaltedUserId, GetComponent<NicknameSync>().MyNick, _ccm.AuthTokenSerial, "Badge request");
		}
		if (FirstVerResult == null)
		{
			return;
		}
		if (FirstVerResult["Public key"] != global::NorthwoodLib.StringUtils.Base64Encode(global::Cryptography.Sha.HashToString(global::Cryptography.Sha.Sha256(publickey))))
		{
			global::GameCore.Console.AddLog("Rejected signature of challenge " + challenge + " due to public key hash mismatch.", global::UnityEngine.Color.red);
			SendConsoleMsg("Challenge signature rejected due to public key mismatch.", "red");
			return;
		}
		if (PublicKey == null)
		{
			PublicKey = global::Cryptography.ECDSA.PublicKeyFromString(publickey);
		}
		if (!global::Cryptography.ECDSA.Verify(challenge, response, PublicKey))
		{
			global::GameCore.Console.AddLog("Rejected signature of challenge " + challenge + " due to signature mismatch.", global::UnityEngine.Color.red);
			SendConsoleMsg("Challenge signature rejected due to signature mismatch.", "red");
		}
		else
		{
			if (challenge.StartsWith("auth-") && challenge == _authChallenge)
			{
				_ccm.UserId = FirstVerResult["User ID"];
				_ccm.UserId2 = (FirstVerResult.ContainsKey("User ID 2") ? FirstVerResult["User ID 2"] : null);
				_hub.nicknameSync.UpdateNickname(global::NorthwoodLib.StringUtils.Base64Decode(FirstVerResult["Nickname"]));
				if (DoNotTrack)
				{
					LogDNT();
				}
				SendConsoleMsg("Hi " + global::NorthwoodLib.StringUtils.Base64Decode(FirstVerResult["Nickname"]) + "! Your challenge signature has been accepted.", "green");
				PublicKeyAccepted = true;
				TargetPublicKeyAccepted(base.connectionToClient);
				GetComponent<global::RemoteAdmin.RemoteAdminCryptographicManager>().StartExchange();
				RefreshPermissions();
				_authChallenge = null;
				if (!global::PluginAPI.Core.Player.PlayersUserIds.ContainsKey(_ccm.UserId))
				{
					global::PluginAPI.Core.Player.PlayersUserIds.Add(_ccm.UserId, _hub);
				}
				global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerJoined, _hub);
				if (AllowSameAccountJoining)
				{
					return;
				}
				int playerId = ReferenceHub.GetHub(base.gameObject).PlayerId;
				{
					foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
					{
						if (allHub.characterClassManager.UserId == _ccm.UserId && allHub.PlayerId != playerId && !allHub.isLocalPlayer)
						{
							ServerConsole.AddLog($"Player {_ccm.UserId} ({allHub.PlayerId}, {allHub.characterClassManager.connectionToClient.address}) has been kicked from the server, because he has just joined the server again from IP address {base.connectionToClient.address}.");
							ServerConsole.Disconnect(allHub.gameObject, "Only one player instance of the same player is allowed.");
						}
					}
					return;
				}
			}
			if (!challenge.StartsWith("badge-server-") || !(challenge == _badgeChallenge))
			{
				return;
			}
			global::System.Collections.Generic.Dictionary<string, string> dictionary = CentralAuth.ValidatePartialAuthToken(_globalBadgeUnconfirmed, _ccm.SaltedUserId, GetComponent<NicknameSync>().MyNick, _ccm.AuthTokenSerial, "Badge request");
			if (dictionary == null)
			{
				ServerConsole.AddLog("Rejected signature of challenge " + challenge + " due to signature mismatch.");
				SendConsoleMsg("Challenge signature rejected due to signature mismatch.", "red");
				return;
			}
			PrevBadge = _globalBadgeUnconfirmed;
			if (dictionary["Staff"] == "YES")
			{
				Staff = true;
			}
			if (dictionary["Management"] == "YES" || dictionary["Global banning"] == "YES")
			{
				Staff = true;
				RaEverywhere = true;
				AdminChatPerms = true;
			}
			if (dictionary["Overwatch mode"] == "YES")
			{
				OverwatchPermitted = true;
			}
			ulong result = ServerStatic.PermissionsHandler.FullPerm;
			if (dictionary.ContainsKey("RA Permissions"))
			{
				result = 0uL;
				ulong.TryParse(dictionary["RA Permissions"], out result);
			}
			if (Staff && ServerStatic.PermissionsHandler.NorthwoodAccess)
			{
				RemoteAdmin = true;
				_globalPerms = result;
				Permissions |= result;
				_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
				RemoteAdminMode = ServerRoles.AccessMode.GlobalAccess;
				_hub.queryProcessor.PasswordTries = 0;
				OpenRemoteAdmin(password: false);
				SendConsoleMsg("Your remote admin access has been granted (global permissions - northwood staff).", "cyan");
			}
			else if (dictionary["Remote admin"] == "YES" && ServerStatic.PermissionsHandler.StaffAccess)
			{
				RemoteAdmin = true;
				_globalPerms = result;
				Permissions |= result;
				_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
				RemoteAdminMode = ServerRoles.AccessMode.GlobalAccess;
				_hub.queryProcessor.PasswordTries = 0;
				OpenRemoteAdmin(password: false);
				SendConsoleMsg("Your remote admin access has been granted (global permissions - staff).", "cyan");
			}
			else if (dictionary["Management"] == "YES" && ServerStatic.PermissionsHandler.ManagersAccess)
			{
				RemoteAdmin = true;
				_globalPerms = result;
				Permissions |= result;
				_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
				RemoteAdminMode = ServerRoles.AccessMode.GlobalAccess;
				_hub.queryProcessor.PasswordTries = 0;
				OpenRemoteAdmin(password: false);
				SendConsoleMsg("Your remote admin access has been granted (global permissions - management).", "cyan");
			}
			else if (dictionary["Global banning"] == "YES" && ServerStatic.PermissionsHandler.BanningTeamAccess)
			{
				RemoteAdmin = true;
				_globalPerms = result;
				Permissions |= result;
				_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
				RemoteAdminMode = ServerRoles.AccessMode.GlobalAccess;
				_hub.queryProcessor.PasswordTries = 0;
				OpenRemoteAdmin(password: false);
				SendConsoleMsg("Your remote admin access has been granted (global permissions - banning team).", "cyan");
			}
			AdminChatPerms = RaEverywhere || PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.AdminChat);
			if (dictionary["Badge text"] != "(none)" || dictionary["Badge color"] != "(none)")
			{
				if (_neverCover || !_badgeCover || string.IsNullOrEmpty(MyText) || string.IsNullOrEmpty(MyColor))
				{
					if (int.TryParse(dictionary["Badge type"], out var result2))
					{
						GlobalBadgeType = result2;
					}
					switch (result2)
					{
					case 4:
						if (!global::GameCore.ConfigFile.ServerConfig.GetBool("hide_banteam_badges_by_default"))
						{
							break;
						}
						goto case 3;
					case 1:
						if (!global::GameCore.ConfigFile.ServerConfig.GetBool("hide_staff_badges_by_default"))
						{
							break;
						}
						goto case 3;
					case 2:
						if (!global::GameCore.ConfigFile.ServerConfig.GetBool("hide_management_badges_by_default"))
						{
							break;
						}
						goto case 3;
					case 0:
						if (!global::GameCore.ConfigFile.ServerConfig.GetBool("hide_patreon_badges_by_default") || ServerStatic.PermissionsHandler.IsVerified)
						{
							break;
						}
						goto case 3;
					case 3:
						hide = true;
						break;
					}
					if (hide)
					{
						HiddenBadge = dictionary["Badge text"];
						GlobalHidden = true;
						RefreshHiddenTag();
						SendConsoleMsg("Your global badge has been granted, but it's hidden. Use \"gtag\" command in the game console to show your global badge.", "yellow");
					}
					else
					{
						HiddenBadge = null;
						RpcResetFixed();
						NetworkGlobalBadge = _globalBadgeUnconfirmed;
						SendConsoleMsg("Your global badge has been granted.", "cyan");
					}
				}
				else
				{
					SendConsoleMsg("Your global badge is covered by server badge. Use \"gtag\" command in the game console to show your global badge.", "yellow");
				}
			}
			_badgeChallenge = null;
			_globalBadgeUnconfirmed = null;
			_hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData);
			SendRealIds();
			if (!Staff)
			{
				return;
			}
			foreach (ReferenceHub allHub2 in ReferenceHub.AllHubs)
			{
				if (allHub2.Mode != ClientInstanceMode.DedicatedServer)
				{
					ServerRoles serverRoles = allHub2.serverRoles;
					if (!string.IsNullOrEmpty(serverRoles.HiddenBadge))
					{
						serverRoles.TargetSetHiddenRole(base.connectionToClient, serverRoles.HiddenBadge);
					}
				}
			}
			SendConsoleMsg("Hidden badges have been displayed for you (if there are any).", "gray");
		}
	}

	protected static void InvokeUserCode_CmdServerSignatureComplete(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdServerSignatureComplete called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdServerSignatureComplete(global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadString(reader), global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	private void UserCode_TargetOpenRemoteAdmin(bool password)
	{
	}

	protected static void InvokeUserCode_TargetOpenRemoteAdmin(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetOpenRemoteAdmin called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_TargetOpenRemoteAdmin(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	private void UserCode_TargetCloseRemoteAdmin()
	{
	}

	protected static void InvokeUserCode_TargetCloseRemoteAdmin(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetCloseRemoteAdmin called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_TargetCloseRemoteAdmin();
		}
	}

	public void UserCode_CmdSetOverwatchStatus(byte status)
	{
		if (!_commandRateLimit.CanExecute())
		{
			return;
		}
		switch (status)
		{
		case 0:
			IsInOverwatch = false;
			return;
		case 1:
			if (OverwatchPermitted)
			{
				IsInOverwatch = true;
				return;
			}
			break;
		case 2:
			if (IsInOverwatch)
			{
				IsInOverwatch = false;
				return;
			}
			if (OverwatchPermitted)
			{
				IsInOverwatch = true;
				return;
			}
			break;
		}
		SendConsoleMsg("You don't have permissions to enable overwatch mode!", "red");
	}

	protected static void InvokeUserCode_CmdSetOverwatchStatus(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkServer.active)
		{
			global::UnityEngine.Debug.LogError("Command CmdSetOverwatchStatus called on client.");
		}
		else
		{
			((ServerRoles)obj).UserCode_CmdSetOverwatchStatus(global::Mirror.NetworkReaderExtensions.ReadByte(reader));
		}
	}

	private void UserCode_TargetPublicKeyAccepted(global::Mirror.NetworkConnection conn)
	{
	}

	protected static void InvokeUserCode_TargetPublicKeyAccepted(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("TargetRPC TargetPublicKeyAccepted called on server.");
		}
		else
		{
			((ServerRoles)obj).UserCode_TargetPublicKeyAccepted(global::Mirror.NetworkClient.readyConnection);
		}
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myText);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myColor);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, PublicPlayerInfoToken);
			global::Mirror.NetworkWriterExtensions.WriteString(writer, GlobalBadge);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myText);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, _myColor);
			result = true;
		}
		if ((base.syncVarDirtyBits & 4L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, PublicPlayerInfoToken);
			result = true;
		}
		if ((base.syncVarDirtyBits & 8L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteString(writer, GlobalBadge);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			string myText = _myText;
			Network_myText = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myText, ref _myText))
			{
				SetTextHook(myText, _myText);
			}
			string myColor = _myColor;
			Network_myColor = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myColor, ref _myColor))
			{
				SetColorHook(myColor, _myColor);
			}
			string publicPlayerInfoToken = PublicPlayerInfoToken;
			NetworkPublicPlayerInfoToken = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(publicPlayerInfoToken, ref PublicPlayerInfoToken))
			{
				SetPublicInfo(publicPlayerInfoToken, PublicPlayerInfoToken);
			}
			string globalBadge = GlobalBadge;
			NetworkGlobalBadge = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			string myText2 = _myText;
			Network_myText = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myText2, ref _myText))
			{
				SetTextHook(myText2, _myText);
			}
		}
		if ((num & 2L) != 0L)
		{
			string myColor2 = _myColor;
			Network_myColor = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(myColor2, ref _myColor))
			{
				SetColorHook(myColor2, _myColor);
			}
		}
		if ((num & 4L) != 0L)
		{
			string publicPlayerInfoToken2 = PublicPlayerInfoToken;
			NetworkPublicPlayerInfoToken = global::Mirror.NetworkReaderExtensions.ReadString(reader);
			if (!SyncVarEqual(publicPlayerInfoToken2, ref PublicPlayerInfoToken))
			{
				SetPublicInfo(publicPlayerInfoToken2, PublicPlayerInfoToken);
			}
		}
		if ((num & 8L) != 0L)
		{
			string globalBadge2 = GlobalBadge;
			NetworkGlobalBadge = global::Mirror.NetworkReaderExtensions.ReadString(reader);
		}
	}
}
