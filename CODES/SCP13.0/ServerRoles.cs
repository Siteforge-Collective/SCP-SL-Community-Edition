using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using CentralAuth;
using Cryptography;
using NorthwoodLib;
using PlayerRoles;
using PlayerRoles.Spectating;
using PlayerStatsSystem;
using RemoteAdmin;

public class ServerRoles : NetworkBehaviour
{
    [Serializable]
    public class NamedColor
    {
        public string Name;
        public string ColorHex;
        public bool Restricted;

        [SerializeField]
        private string _speakingOverride;

        private Color _speakingColorCache;
        private bool _speakingColorSet;

        public Color SpeakingColor
        {
            get
            {
                if (!_speakingColorSet)
                {
                    _speakingColorSet = true;
                    string colorStr = "#" + (string.IsNullOrEmpty(_speakingOverride) ? ColorHex : _speakingOverride);
                    if (!ColorUtility.TryParseHtmlString(colorStr, out _speakingColorCache))
                    {
                        _speakingColorCache = Color.white;
                    }
                }
                return _speakingColorCache;
            }
        }
    }

    public enum BadgePreferences
    {
        NoPreference = 0,
        PreferGlobal = 1,
        PreferLocal = 2
    }

    public enum BadgeVisibilityPreferences
    {
        NoPreference = 0,
        Visible = 1,
        Hidden = 2
    }


    public NamedColor CurrentColor;
    public NamedColor[] NamedColors;

    [NonSerialized]
    private bool _bypassMode;

    private bool _authorizeBadge;
    internal UserGroup Group;
    private ReferenceHub _hub;

    private static readonly Dictionary<string, NamedColor> DictionarizedColorsCache = new Dictionary<string, NamedColor>();
    private static bool _colorDictionaryCacheSet;

    public const string DefaultColor = "default";
    public const string HiddenBadgeColor = "silver";
    public const ulong UserIdPerms = 18007046uL;

    private string _globalBadgeUnconfirmed;
    private string _prevColor;
    private string _prevText;
    private string _prevBadge;
    private string _authChallenge;
    private string _badgeChallenge;
    private string _bgc;
    private string _bgt;
    private bool _requested;
    private bool _publicPartRequested;
    private bool _badgeRequested;
    private bool _authRequested;
    private bool _noclipReady;

    internal bool BadgeCover;

    [NonSerialized]
    public BadgePreferences UserBadgePreferences;

    private BadgeVisibilityPreferences _globalBadgeVisibilityPreferences;
    private BadgeVisibilityPreferences _localBadgeVisibilityPreferences;

    [SyncVar(hook = nameof(SetTextHook))]
    private string _myText;

    [SyncVar(hook = nameof(SetColorHook))]
    private string _myColor;

    private string _clientText;
    private string _originalClientText;
    private bool _nullBadge;

    [SyncVar]
    public string GlobalBadge;

    [SyncVar]
    public string GlobalBadgeSignature;

    [NonSerialized]
    public bool RemoteAdmin;

    [NonSerialized]
    public ulong Permissions;

    [NonSerialized]
    public string HiddenBadge;

    [NonSerialized]
    public bool GlobalHidden;

    [NonSerialized]
    internal bool AdminChatPerms;

    [NonSerialized]
    internal ulong GlobalPerms;

    [NonSerialized]
    private bool _lastRealIdPerm;

    [NonSerialized]
    public string FixedBadge;

    private static BadgePreferences _userBadgePreferencesClient;
    private static BadgeVisibilityPreferences _globalBadgePreferencesClient;
    private static BadgeVisibilityPreferences _localBadgePreferencesClient;

    public bool BypassMode
    {
        get => _bypassMode;
        set
        {
            _bypassMode = value;
            _hub.playerStats.GetModule<AdminFlagsStat>().SetFlag(AdminFlags.BypassMode, value);
        }
    }

    public bool IsInOverwatch
    {
        get => _hub.roleManager.CurrentRole is OverwatchRole;
        set
        {
            if (value != IsInOverwatch)
            {
                _hub.roleManager.ServerSetRole(value ? RoleTypeId.Overwatch : RoleTypeId.Spectator, RoleChangeReason.RemoteAdmin);
            }
        }
    }

    private Dictionary<string, NamedColor> NamedColorsDic
    {
        get
        {
            if (_colorDictionaryCacheSet)
                return DictionarizedColorsCache;

            if (NamedColors != null)
            {
                foreach (var namedColor in NamedColors)
                {
                    DictionarizedColorsCache[namedColor.Name] = namedColor;
                }
            }
            _colorDictionaryCacheSet = true;
            return DictionarizedColorsCache;
        }
    }

    internal byte KickPower
    {
        get
        {
            if (_hub.authManager.RemoteAdminGlobalAccess)
                return 255;
            return Group?.KickPower ?? 0;
        }
    }

    public string MyColor
    {
        get => _myColor;
        private set => _myColor = value;
    }

    public string MyText
    {
        get => _myText;
        private set => _myText = value;
    }

    public bool GlobalSet
    {
        get
        {
            if (!string.IsNullOrEmpty(GlobalBadge))
                return true;

            if (!string.IsNullOrEmpty(MyText) && MyText.StartsWith("[", StringComparison.Ordinal))
                return MyText.EndsWith("]", StringComparison.Ordinal);

            return false;
        }
    }

    private bool HasNotAllowedText
    {
        get
        {
            string text = MyText;
            if (string.IsNullOrEmpty(text)) return false;
            return text.Contains("[", StringComparison.Ordinal) ||
                   text.Contains("]", StringComparison.Ordinal) ||
                   text.Contains("<", StringComparison.Ordinal) ||
                   text.Contains(">", StringComparison.Ordinal) ||
                   text.Contains("\u003c", StringComparison.Ordinal) || // <
                   text.Contains("\u003e", StringComparison.Ordinal); // >
        }
    }

    public bool HasBadgeHidden
    {
        get
        {
            if (_hub.authManager.BypassBansFlagSet)
                return false;
            return !string.IsNullOrEmpty(HiddenBadge);
        }
    }

    public bool HasGlobalBadge => _hub.authManager.AuthenticationResponse.SignedBadgeToken != null;

    public static BadgeVisibilityPreferences GetGlobalBadgePreferences()
    {
        return _globalBadgePreferencesClient;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        _userBadgePreferencesClient = (BadgePreferences)UserSettings.UserSetting<int>.Get(UserSettings.OtherSettings.MiscPrivacySetting.BadgePreferences);
        _globalBadgePreferencesClient = (BadgeVisibilityPreferences)UserSettings.UserSetting<int>.Get(UserSettings.OtherSettings.MiscPrivacySetting.BadgePreferences);
        _localBadgePreferencesClient = (BadgeVisibilityPreferences)UserSettings.UserSetting<int>.Get(UserSettings.OtherSettings.MiscPrivacySetting.LocalBadgeVisibility);

        UserSettings.UserSetting<int>.AddListener(UserSettings.OtherSettings.MiscPrivacySetting.BadgePreferences, SetBadgePreferences);
        UserSettings.UserSetting<int>.AddListener(UserSettings.OtherSettings.MiscPrivacySetting.BadgePreferences, SetGlobalBadgePreferences);
        UserSettings.UserSetting<int>.AddListener(UserSettings.OtherSettings.MiscPrivacySetting.LocalBadgeVisibility, SetLocalBadgePreferences);
    }

    private static void SetBadgePreferences(int preferences)
    {
        _userBadgePreferencesClient = (BadgePreferences)preferences;
        ResyncPreferences();
    }

    private static void SetGlobalBadgePreferences(int preferences)
    {
        _globalBadgePreferencesClient = (BadgeVisibilityPreferences)preferences;
        ResyncPreferences();
    }

    private static void SetLocalBadgePreferences(int preferences)
    {
        _localBadgePreferencesClient = (BadgeVisibilityPreferences)preferences;
        ResyncPreferences();
    }

    private static void ResyncPreferences()
    {
        if (NetworkClient.active && ReferenceHub.TryGetLocalHub(out ReferenceHub hub))
        {
            hub.serverRoles.CmdSetLocalTagPreferences(_userBadgePreferencesClient, _globalBadgePreferencesClient, _localBadgePreferencesClient, true);
        }
    }

    public void Start()
    {
        _hub = ReferenceHub.GetHub(gameObject);
        if (isLocalPlayer)
        {
            CmdSetLocalTagPreferences(_userBadgePreferencesClient, _globalBadgePreferencesClient, _localBadgePreferencesClient, true);
        }
    }

    [TargetRpc]
    private void TargetSetHiddenRole(NetworkConnection connection, string role)
    {
        if (!isServer)
        {
            if (!string.IsNullOrEmpty(role))
            {
                SetColor("silver");
                string sanitized = Misc.SanitizeRichText(role.Replace("[", "").Replace("]", ""), "", "");
                string translated = TranslateGlobalBadge(sanitized, true);
                string hiddenSuffix = TranslationReader.Get("Legacy_Interfaces", 18, "(hidden)");
                FixedBadge = translated + " " + hiddenSuffix;
                SetText(FixedBadge);
            }
            else
            {
                SetColor(null);
                SetText(null);
                FixedBadge = null;
                SetText(null);
            }
        }
    }

    [ClientRpc]
    public void RpcResetFixed()
    {
        FixedBadge = null;
    }

    [Command(channel = 4)] 
    private void CmdSetLocalTagPreferences(BadgePreferences userPreferences, BadgeVisibilityPreferences globalPreferences, BadgeVisibilityPreferences localPreferences, bool refresh)
    {
        UserBadgePreferences = userPreferences;
        _globalBadgeVisibilityPreferences = globalPreferences;
        _localBadgeVisibilityPreferences = localPreferences;

        if (refresh)
        {
            if (userPreferences == BadgePreferences.PreferGlobal && HasGlobalBadge)
            {
                RefreshGlobalBadgeVisibility(globalPreferences);
            }
            else if (Group != null)
            {
                RefreshLocalBadgeVisibility(localPreferences);
            }
            else if (HasGlobalBadge)
            {
                RefreshGlobalBadgeVisibility(globalPreferences);
            }
        }
    }

    [Server]
    private void RefreshGlobalBadgeVisibility(BadgeVisibilityPreferences globalPreferences)
    {
        if (!NetworkServer.active) return;

        if (RefreshGlobalTag())
        {
            if (globalPreferences == BadgeVisibilityPreferences.Hidden)
            {
                TryHideTag();
            }
        }
    }

    [Server]
    private void RefreshLocalBadgeVisibility(BadgeVisibilityPreferences localPreferences)
    {
        if (!NetworkServer.active) return;

        RefreshLocalTag();
        if (localPreferences == BadgeVisibilityPreferences.Hidden)
        {
            TryHideTag();
        }
    }

    [Server]
    public void RefreshPermissions(bool disp = false)
    {
        if (!NetworkServer.active) return;

        UserGroup userGroup = ServerStatic.PermissionsHandler.GetUserGroup(_hub.authManager.UserId);
        if (userGroup != null)
        {
            SetGroup(userGroup, false, disp);
        }
    }

    [Server]
    public void SetGroup(UserGroup group, bool byAdmin = false, bool disp = false)
    {
        if (!NetworkServer.active) return;

        if (group != null)
        {
            _hub.gameConsoleTransmission.SendToClient(byAdmin ? "Updating your group on server (set by server administrator)..." : "Updating your group on server (local permissions)...", "cyan");
            Group = group;
            BadgeCover = group.Cover;

            if (ServerStatic.PermissionsHandler.IsRaPermitted(GlobalPerms))
            {
                Permissions = GlobalPerms;
                _hub.authManager.ResetPasswordAttempts();
                _hub.gameConsoleTransmission.SendToClient(byAdmin ? "Your remote admin access has been granted (set by server administrator)." : "Your remote admin access has been granted (local permissions).", "cyan");
            }
            else
            {
                Permissions = GlobalPerms;
            }

            ServerLogs.AddLog(ServerLogs.Modules.Permissions, $"{_hub.LoggedNameFromRefHub()} has been assigned to group {group.BadgeText}.", ServerLogs.ServerLogType.ConnectionUpdate);

            if (group.BadgeColor == "none")
            {
                RpcResetFixed();
                FinalizeSetGroup();
                return;
            }

            if ((_localBadgeVisibilityPreferences == BadgeVisibilityPreferences.Hidden && !disp) ||
                (group.HiddenByDefault && !disp && _localBadgeVisibilityPreferences != BadgeVisibilityPreferences.Visible))
            {
                BadgeCover = UserBadgePreferences == BadgePreferences.PreferLocal;
                if (!string.IsNullOrEmpty(MyText))
                {
                    SetText(MyText);
                }
                else
                {
                    SetText(null);
                    SetColor(DefaultColor);
                    GlobalHidden = false;
                    HiddenBadge = group.BadgeText;
                    RefreshHiddenTag();
                    TargetSetHiddenRole(connectionToClient, group.BadgeText);

                    string msg = "Your role has been granted" + (byAdmin ? " to you (set by server administrator)" : "") + ", but it's hidden. Use \"showtag\" command in the game console to show your server badge.";
                    _hub.gameConsoleTransmission.SendToClient(msg, byAdmin ? "cyan" : "yellow");
                }
            }
            else
            {
                HiddenBadge = null;
                GlobalHidden = false;
                RpcResetFixed();
                SetText(group.BadgeText);
                SetColor(group.BadgeColor);

                string msgRole = $"Your role \"{group.BadgeText}\"";
                string msgColor = !string.IsNullOrEmpty(group.BadgeColor) ? $" with color {group.BadgeColor}" : "";
                string msgSuffix = $" has been granted to you {(byAdmin ? "(set by server administrator)" : "(local permissions)")}.";

                _hub.gameConsoleTransmission.SendToClient(msgRole + msgColor + msgSuffix, "cyan");
            }
            FinalizeSetGroup();
        }
        else
        {
            GlobalHidden = false;
            RemoteAdmin = NetworkServer.active;
            Permissions = GlobalPerms;
            Group = null;
            SetColor(null);
            SetText(null);
            BadgeCover = false;
            RpcResetFixed();
            FinalizeSetGroup();
            _hub.gameConsoleTransmission.SendToClient("Your local permissions has been revoked by server administrator.", "red");
        }
    }

    [Server]
    public void FinalizeSetGroup()
    {
        Permissions |= GlobalPerms;
        RemoteAdmin = ServerStatic.PermissionsHandler.IsRaPermitted(Permissions);
        AdminChatPerms = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.AdminChat); // 0x800000
        _hub.queryProcessor.GameplayData = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.GameplayData); // 0x800

        if (RemoteAdmin)
        {
            OpenRemoteAdmin();
        }
        else
        {
            TargetSetRemoteAdmin(false);
            _hub.queryProcessor.SyncCommandsToClient();
        }

        SendRealIds();

        bool viewHidden = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.ViewHiddenBadges); // 0x20000
        bool viewGlobalHidden = PermissionsHandler.IsPermitted(Permissions, PlayerPermissions.ViewHiddenGlobalBadges); // 0x1000000

        if (viewHidden || viewGlobalHidden)
        {
            foreach (var hub in ReferenceHub.AllHubs)
            {
                if (hub.Mode != ClientInstanceMode.DedicatedServer && !string.IsNullOrEmpty(hub.serverRoles.HiddenBadge))
                {
                    if (hub.serverRoles.GlobalHidden && !viewGlobalHidden) continue;
                    if (!hub.serverRoles.GlobalHidden && !viewHidden) continue;

                    hub.serverRoles.TargetSetHiddenRole(connectionToClient, hub.serverRoles.HiddenBadge);
                }
            }

            string hiddenMsg = "Hidden badges ";
            if (viewHidden && viewGlobalHidden) hiddenMsg += "(local and global)";
            else if (viewHidden) hiddenMsg += "(local only)";
            else hiddenMsg += "(global only)";
            hiddenMsg += " have been displayed for you (if there are any).";

            _hub.gameConsoleTransmission.SendToClient(hiddenMsg, "gray");
        }
    }

    [ServerCallback]
    public void RefreshHiddenTag()
    {
        if (!GlobalHidden && string.IsNullOrEmpty(HiddenBadge)) return;

        foreach (var hub in ReferenceHub.AllHubs)
        {
            if (hub.Mode != ClientInstanceMode.DedicatedServer)
            {
                bool viewHidden = PermissionsHandler.IsPermitted(hub.serverRoles.Permissions, PlayerPermissions.ViewHiddenBadges);
                bool viewGlobalHidden = PermissionsHandler.IsPermitted(hub.serverRoles.Permissions, PlayerPermissions.ViewHiddenGlobalBadges);

                if (GlobalHidden)
                {
                    if (viewGlobalHidden) TargetSetHiddenRole(hub.connectionToClient, HiddenBadge);
                }
                else
                {
                    if (viewHidden) TargetSetHiddenRole(hub.connectionToClient, HiddenBadge);
                }
            }
        }
    }

    [Server]
    public void RefreshRealId()
    {
        foreach (var hub in ReferenceHub.AllHubs)
        {
            if (hub.Mode != ClientInstanceMode.DedicatedServer)
            {
                if (hub.authManager.NorthwoodStaff || PermissionsHandler.IsPermitted(hub.serverRoles.Permissions, UserIdPerms))
                {
                    _hub.authManager.TargetSetRealId(hub.connectionToClient, _hub.authManager.UserId);
                }
            }
        }
    }

    [Server]
    private void SendRealIds()
    {
        if (_hub.Mode == ClientInstanceMode.DedicatedServer) return;

        bool isStaff = _hub.authManager.NorthwoodStaff;
        bool hasPerm = PermissionsHandler.IsPermitted(Permissions, UserIdPerms);

        if (!isStaff && !hasPerm && !_lastRealIdPerm) return;

        _lastRealIdPerm = hasPerm || isStaff;

        foreach (var hub in ReferenceHub.AllHubs)
        {
            hub.authManager.TargetSetRealId(_hub.connectionToClient, (isStaff || hasPerm) ? _hub.authManager.UserId : null);
        }
    }

    public string GetColoredRoleString(bool newLine = false)
    {
        if (string.IsNullOrEmpty(MyColor)) return string.Empty;
        if (string.IsNullOrEmpty(MyText)) return string.Empty;

        if (CurrentColor != null && CurrentColor.Restricted && !HasNotAllowedText && !_authorizeBadge)
        {
            // Logic mismatch in pseudocode vs clean logic, following pseudocode flow:
            // if restricted AND !auth -> empty
        }

        if ((CurrentColor != null && CurrentColor.Restricted) && !_authorizeBadge)
            return string.Empty;

        foreach (var col in NamedColors)
        {
            if (col.Name == MyColor)
            {
                return (newLine ? "\n" : "") + $"<color=#{col.ColorHex}>{MyText}</color>";
            }
        }
        return string.Empty;
    }

    public string GetUncoloredRoleString()
    {
        if (!string.IsNullOrEmpty(MyText))
        {
            if (string.IsNullOrEmpty(FixedBadge))
            {
                if (string.IsNullOrEmpty(MyColor) || CurrentColor == null) return string.Empty;
                if (CurrentColor.Restricted && !_authorizeBadge) return string.Empty;
                if (HasNotAllowedText && !_authorizeBadge) return string.Empty;
                return MyText;
            }
            return FixedBadge;
        }
        return string.Empty;
    }

    public Color GetColor()
    {
        if (string.IsNullOrEmpty(DefaultColor)) return Color.clear;

        NamedColor nc = NamedColors.FirstOrDefault(x => x.Name == MyColor);
        if (nc != null) return nc.SpeakingColor;

        NamedColor def = NamedColors.FirstOrDefault(x => x.Name == DefaultColor);
        return def?.SpeakingColor ?? Color.white;
    }

    public Color GetVoiceColor()
    {
        if (NamedColorsDic.TryGetValue(MyColor ?? "", out NamedColor nc))
        {
            return nc.SpeakingColor;
        }
        if (NamedColorsDic.TryGetValue(DefaultColor, out NamedColor def))
        {
            return def.SpeakingColor;
        }
        return Color.white;
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(FixedBadge) && MyText != FixedBadge)
        {
            SetText(FixedBadge);
            SetColor("silver");
            return;
        }
        if (!string.IsNullOrEmpty(FixedBadge))
        {
            if (CurrentColor != null && CurrentColor.Name != "silver")
            {
                SetColor("silver");
                return;
            }
        }

        if (GlobalBadge != _prevBadge)
        {
            _prevBadge = GlobalBadge;
            if (string.IsNullOrEmpty(GlobalBadge))
            {
                _bgc = null; _bgt = null; _authorizeBadge = false;
                _prevColor += "."; _prevText += ".";
                return;
            }

            string nick = _hub.nicknameSync.MyNick;
            GameCore.Console.AddDebugLog("SDAUTH", $"Validating global badge of user {nick}", MessageImportance.LessImportant);

            SignedToken token = new(GlobalBadge, GlobalBadgeSignature);
            if (!token.TryGetToken<BadgeToken>("Badge", out BadgeToken badgeToken, out string error, out string userId))
            {
                GameCore.Console.AddDebugLog("SDAUTH", $"<color=red>Validation of global badge of user {nick} failed - {error ?? "unknown"}.</color>", MessageImportance.Normal);
                _bgc = null;
                _bgt = null;
                _authorizeBadge = false;
                _prevColor += ".";
                _prevText += ".";
                return;
            }

            string salted = _hub.authManager.SaltedUserId;
            string hashedSalted = Sha.HashToString(Sha.Sha512(salted));

            if (badgeToken.UserId != salted && badgeToken.UserId != hashedSalted)
            {
                error = "badge token UserID mismatch.";
            }
            else if (StringUtils.Base64Decode(badgeToken.Nickname) != nick)
            {
                error = "badge token nickname mismatch.";
            }

            if (error != null)
            {
                GameCore.Console.AddDebugLog("SDAUTH", $"<color=red>Validation of global badge of user {nick} failed - {error}.</color>", MessageImportance.Normal);
                _bgc = null; _bgt = null; _authorizeBadge = false;
                _prevColor += "."; _prevText += ".";
                return;
            }

            string logMsg = $"Validation of global badge of user {nick} complete - badge signed by central server {badgeToken.IssuedBy}.";
            GameCore.Console.AddDebugLog("SDAUTH", logMsg, MessageImportance.LessImportant);

            if (badgeToken.BadgeText == null || badgeToken.BadgeText == "(none)" ||
                badgeToken.BadgeColor == null || badgeToken.BadgeColor == "(none)")
            {
                _bgc = "(none)"; _bgt = "(none)";
                SetColor("(none)");
            }
            else
            {
                string translated = TranslateGlobalBadge(badgeToken.BadgeText, false);
                _bgc = badgeToken.BadgeColor;
                _bgt = translated;

                if (_bgt != MyText) SetText(_bgt);

                string tagLog = $"Loaded translation for a global badge. {nick}: {MyText} - {_bgt} /-/ {MyColor} - {_bgc}";
                GameCore.Console.AddLog(tagLog, Color.green);

                SetText(_bgt);
            }
            _authorizeBadge = true;
        }

        if (_prevColor != MyColor || _prevText != MyText)
        {
            if (CurrentColor != null && CurrentColor.Restricted)
            {
                if (MyText != _bgt || MyColor != _bgc)
                {
                    GameCore.Console.AddLog($"TAG FAIL 1 - {MyText} - {_bgt} /-/ {MyColor} - {_bgc}", Color.gray);
                    _authorizeBadge = false;
                    SetColor(null);
                    _prevColor = null;
                    return;
                }
            }

            if (MyText != _bgt && HasNotAllowedText)
            {
                GameCore.Console.AddLog($"TAG FAIL 2 - {MyText} - {_bgt} /-/ {MyColor} - {_bgc}", Color.gray);
                _authorizeBadge = false;
                SetText(null);
                _prevText = null;
                return;
            }

            _prevColor = MyColor;
            _prevText = MyText;
            _prevBadge = GlobalBadge;
        }
    }

    private void SetColorHook(string p, string i)
    {
        _prevBadge = null; _bgc = null; _authorizeBadge = false;
        if (_prevColor == null) _prevColor = ".";
        else _prevColor += ".";
        SetColor(i);
    }

    public void SetColor(string i)
    {
        if (string.IsNullOrEmpty(i)) i = DefaultColor;

        if (NetworkServer.active) _myColor = i;
        MyColor = i;

        var named = NamedColors.FirstOrDefault(r => r.Name == MyColor);
        if (named == null && i != DefaultColor)
        {
            SetColor(DefaultColor);
        }
        else
        {
            CurrentColor = named;
        }
    }

    private void SetTextHook(string p, string i)
    {
        _prevBadge = null; _bgt = null; _authorizeBadge = false;
        if (_prevText == null) _prevText = ".";
        else _prevText += ".";
        SetText(i);
    }

    public void SetText(string i)
    {
        if (i == string.Empty) i = null;
        if (NetworkServer.active) _myText = i;
        MyText = i;

        var named = NamedColors.FirstOrDefault(r => r.Name == MyColor);
        if (named != null) CurrentColor = named;

        if (gameObject != null)
        {
            ReferenceHub hub = ReferenceHub.GetHub(gameObject);
            PlayerList.UpdatePlayerRole(hub);
        }
    }

    [Server]
    public bool RefreshGlobalTag()
    {
        if (!HasGlobalBadge) return false;

        SetColor(null);
        SetText(null);
        GlobalBadge = _hub.authManager.AuthenticationResponse.SignedBadgeToken.token;
        GlobalBadgeSignature = _hub.authManager.AuthenticationResponse.SignedBadgeToken.signature;
        HiddenBadge = null;
        GlobalHidden = false;
        RpcResetFixed();
        return true;
    }

    [Server]
    public void RefreshLocalTag()
    {
        GlobalBadge = null;
        GlobalBadgeSignature = null;
        HiddenBadge = null;
        GlobalHidden = false;
        RpcResetFixed();
        RefreshPermissions(true);
    }

    [Server]
    public bool TryHideTag()
    {
        if (!string.IsNullOrEmpty(MyText))
        {
            // Hide current manual text
            if (GlobalSet || MyText.StartsWith("[")) // simplified Logic
            {
                HiddenBadge = MyText;
                GlobalHidden = GlobalSet;
            }
            else
            {
                GlobalHidden = false;
                HiddenBadge = MyText;
            }
        }
        else if (HasGlobalBadge)
        {
            // Hide global badge
            var token = _hub.authManager.AuthenticationResponse.BadgeToken;
            if (token != null)
            {
                HiddenBadge = token.BadgeText;
                GlobalHidden = true;
            }
        }
        else
        {
            if (!_hub.authManager.BypassBansFlagSet) return false;
            GlobalHidden = false;
            HiddenBadge = null;
        }

        GlobalBadge = null;
        SetText(null);
        SetColor(null);
        RefreshHiddenTag();
        return true;
    }

    private static string TranslateGlobalBadge(string badge, bool hidden)
    {
        if (string.IsNullOrEmpty(badge)) return badge;

        if (!hidden && badge.StartsWith("[") && badge.EndsWith("]"))
            return badge; // No translation for brackets

        if (hidden)
        {
            // Remove brackets logic roughly from pseudocode
            if (badge.Length > 2 && badge.StartsWith("[") && badge.EndsWith("]"))
                badge = badge.Substring(1, badge.Length - 2);
        }

        string[] keys = TranslationReader.GetFallbackKeys("Badges");
        foreach (string key in keys)
        {
            if (key == badge)
            {
                string trans = TranslationReader.Get("Badges", 0, badge); // Index 0 placeholder
                if (!string.IsNullOrEmpty(trans)) return trans;
            }
        }

        // Sanitization fallback
        return Misc.SanitizeRichText("[" + badge + "]", "", "");
    }

    internal void OpenRemoteAdmin()
    {
        TargetSetRemoteAdmin(true);
        _hub.queryProcessor.SyncCommandsToClient();
    }

    [TargetRpc]
    private void TargetSetRemoteAdmin(bool open)
    {
        UIController.Singleton.SetRemoteAdminState(open);
    }
}