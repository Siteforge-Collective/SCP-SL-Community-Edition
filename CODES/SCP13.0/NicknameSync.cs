using CentralAuth;
using CustomPlayerEffects;
using Discord;
using GameCore;
using Mirror;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Spectating;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class NicknameSync : NetworkBehaviour
{
    public LayerMask RaycastMask;

    private ReferenceHub _hub;

    private const float FlashedThreshold = 0.5f;

    private Text _nText;

    private CanvasRenderer _renderer;

    private float _transparency;

    private Flashed _localFlashed;

    private Regex _nickFilter;

    private string _replacement;

    [SyncVar]
    public float ViewRange;

    [SyncVar(hook = nameof(SetCustomInfo))]
    private string _customPlayerInfoString;

    [SyncVar]
    private PlayerInfoArea _playerInfoToShow;

    [SyncVar(hook = nameof(UpdatePlayerlistInstance))]
    private string _myNickSync;

    private string _firstNickname;

    private const ushort MaxNicknameLen = 48;

    [SyncVar(hook = nameof(UpdateCustomName))]
    private string _displayName;

    private string _cleanDisplayName;

    public bool NickSet { get; private set; }

    public string SanitizedPlayerInfoString { get; private set; }

    public PlayerInfoArea ShownPlayerInfo
    {
        get
        {
            return _playerInfoToShow;
        }
        set
        {
            if (NetworkServer.active)
            {
                _playerInfoToShow = value;
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
            if (NetworkServer.active)
            {
                SetCustomInfo(_customPlayerInfoString, value);
                _customPlayerInfoString = value;
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
            _displayName = value;
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
            _firstNickname = Misc.SanitizeRichText(_myNickSync.Replace("\n", string.Empty).Replace("\r", string.Empty), "＜", "＞");
            if (_firstNickname.Length > 48)
            {
                _firstNickname = _firstNickname[..48];
            }
            return _firstNickname;
        }
        private set
        {
            if (value == null)
            {
                value = "(null)";
            }
            string text = Misc.SanitizeRichText(value, "＜", "＞");
            _myNickSync = ((value.Length > 48) ? text[..48] : text);
            if (NetworkServer.active)
            {
                NickSet = true;
                _firstNickname = _myNickSync;
            }
        }
    }

    private void UpdatePlayerlistInstance(string p, string username)
    {
        PlayerList.UpdatePlayerNickname(_hub);
    }

    private void UpdateCustomName(string p, string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            _cleanDisplayName = null;
        }
        else
        {
            _cleanDisplayName = Misc.SanitizeRichText(username.Replace("\n", string.Empty).Replace("\r", string.Empty)).Trim();
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
        if (NetworkServer.active)
        {
            ViewRange = ConfigFile.ServerConfig.GetFloat("player_info_range", 10f);
            string text = ConfigFile.ServerConfig.GetString("nickname_filter") ?? "";
            if (!string.IsNullOrEmpty(text))
            {
                _nickFilter = new Regex(text, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(500.0));
                _replacement = ConfigFile.ServerConfig.GetString("nickname_filter_replacement") ?? "";
            }
        }
        if (base.isLocalPlayer)
        {
            string personaName = "";
            switch (CentralAuth.CentralAuthManager.Platform)
            {
                case GameCore.DistributionPlatform.Steam:
                    personaName = SteamManager.GetPersonaName(SteamManager.SteamId64);
                    break;
                default:
                    GameCore.Console.AddLog("Unknown platform, setting nickname to default.", Color.red);
                    break;
            }

            if (string.IsNullOrEmpty(personaName))
            {
                personaName = "SCP:SL Player";
            }

            if (!PlayerPrefsSl.HasKey("nickname", PlayerPrefsSl.DataType.String))
            {
                PlayerPrefsSl.Set("nickname", personaName);
            }

            string savedNick = PlayerPrefsSl.Get("nickname", "");
            if (string.IsNullOrEmpty(savedNick))
            {
                savedNick = "SCP:SL Player";
            }

            CmdSetNick(savedNick);
            SetNick("Dedicated Server");
            GameObject nicknameTextObj = GameObject.Find("Nickname Text");
            _nText = nicknameTextObj.GetComponent<Text>();
            _renderer = _nText.GetComponent<CanvasRenderer>();
        }
    }

    private bool TryGetRayTransform(out Transform tr)
    {
        if (_hub.roleManager.CurrentRole is IFpcRole)
        {
            tr = _hub.PlayerCameraReference;
            return true;
        }
        if (SpectatorTargetTracker.TryGetTrackedPlayer(out var hub) && hub.roleManager.CurrentRole is IFpcRole)
        {
            tr = MainCameraController.CurrentCamera;
            return true;
        }
        tr = null;
        return false;
    }

    private void Update()
    {
        if (base.isLocalPlayer)
        {
            return;
        }

        if (TryGetRayTransform(out Transform rayTransform))
        {
            Ray ray = new Ray(rayTransform.position, rayTransform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, ViewRange, RaycastMask))
            {
                if (hit.collider != null)
                {
                    Transform hitTransform = hit.collider.transform;
                    if (hitTransform != null)
                    {
                        ReferenceHub hitHub;
                        if (!ReferenceHub.TryGetHub(hitTransform.gameObject, out hitHub))
                        {
                            hitTransform = hitTransform.parent;
                            if (hitTransform == null || !ReferenceHub.TryGetHub(hitTransform.gameObject, out hitHub))
                            {
                                return;
                            }
                        }

                        if (hitHub == _hub || hitHub.isLocalPlayer)
                        {
                            return;
                        }

                        if (_localFlashed != null && _localFlashed.Intensity > FlashedThreshold)
                        {
                            return;
                        }

                        if (hitHub.roleManager.CurrentRole is not IFpcRole)
                        {
                            return;
                        }

                        StringBuilder sb = StringBuilderPool.Shared.Rent();
                        Color color;
                        WriteDefaultInfo(hitHub, sb, out color, _playerInfoToShow);

                        string info = sb.ToString();
                        StringBuilderPool.Shared.Return(sb);

                        _nText.text = info;
                        _renderer.SetColor(color);
                        _transparency = 1f;
                    }
                }
            }
            else
            {
                _transparency -= Time.deltaTime;
                if (_transparency < 0f)
                {
                    _transparency = 0f;
                    _nText.text = "";
                }
            }
        }
    }

    private void SetCustomInfo(string oldValue, string newValue)
    {
        SanitizedPlayerInfoString = Misc.SanitizeRichText(newValue ?? string.Empty, "＜", "＞");
    }

    [Command(channel = 4)]
    private void CmdSetNick(string n)
    {
        if (base.isLocalPlayer)
        {
            MyNick = n;
            return;
        }

        if (NickSet)
        {
            return;
        }

        if (PlayerAuthenticationManager.OnlineMode)
        {
            return;
        }

        NickSet = true;
        if (n == null)
        {
            ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing null name.", ConsoleColor.Gray);
            BanPlayer.BanUser(_hub, "Null name", 1577847600L);
            SetNick("(null)");
            return;
        }

        if (n.Length > 1024)
        {
            ServerConsole.AddLog("Banned " + base.connectionToClient.address + " for passing a too long name.", ConsoleColor.Gray);
            BanPlayer.BanUser(_hub, "Too long name", 1577847600L);
            SetNick("(too long)");
            return;
        }

        string cleaned = CleanNickName(n, out bool printable);
        if (!printable)
        {
            ServerConsole.AddLog("Kicked " + base.connectionToClient.address + " for having an empty name.", ConsoleColor.Gray);
            ServerConsole.Disconnect(base.connectionToClient, "You may not have an empty name.");
            SetNick("Empty Name");
            return;
        }

        cleaned = Misc.SanitizeRichText(cleaned, "＜", "＞");
        cleaned = cleaned.Replace("[", "(");
        cleaned = cleaned.Replace("]", ")");
        if (cleaned.Length > 48)
        {
            cleaned = cleaned.Substring(0, 48);
        }

        SetNick(cleaned);
        _hub.characterClassManager.SyncServerCmdBinding();
    }

    [ServerCallback]
    public void UpdateNickname(string n)
    {
        if (!NetworkServer.active)
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

        StringBuilder sb = StringBuilderPool.Shared.Rent(n.Length);
        char highSurrogate = '\0';
        bool hasPrintable = false;
        foreach (char c in n)
        {
            if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
            {
                hasPrintable = true;
                sb.Append(c);
            }
            else if (char.IsWhiteSpace(c) && c != '\n' && c != '\r' && c != '\t')
            {
                sb.Append(c);
            }
            else if (char.IsHighSurrogate(c))
            {
                highSurrogate = c;
            }
            else if (char.IsLowSurrogate(c) && char.IsSurrogatePair(highSurrogate, c))
            {
                sb.Append(highSurrogate);
                sb.Append(c);
                hasPrintable = true;
            }
        }

        if (!hasPrintable)
        {
            ServerConsole.AddLog("Kicked " + base.connectionToClient.address + " for having an empty name.");
            ServerConsole.Disconnect(base.connectionToClient, "You may not have an empty name.");
            SetNick("Empty Name");
            StringBuilderPool.Shared.Return(sb);
            return;
        }

        string cleaned = sb.ToString();
        StringBuilderPool.Shared.Return(sb);
        if (cleaned.Length > 48)
        {
            cleaned = cleaned.Substring(0, 48);
        }

        SetNick(cleaned);
    }

    [Server]
    private void SetNick(string nick)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("[Server] function 'System.Void NicknameSync::SetNick(System.String)' called when server was not active");
            return;
        }

        MyNick = nick;
        string filtered;
        try
        {
            filtered = _nickFilter?.Replace(nick, _replacement) ?? nick;
        }
        catch (Exception ex)
        {
            ServerConsole.AddLog($"Error when filtering nick {nick}: {ex}");
            filtered = "(filter failed)";
        }

        if (nick != filtered)
        {
            DisplayName = filtered;
        }

        if (!base.isLocalPlayer || !ServerStatic.IsDedicated)
        {
            ServerConsole.AddLog("Nickname of " + _hub.authManager.UserId + " is now " + nick + ".");
            ServerLogs.AddLog(ServerLogs.Modules.Networking, "Nickname of " + _hub.authManager.UserId + " is now " + nick + ".", ServerLogs.ServerLogType.ConnectionUpdate);
        }
    }

    public static void WriteDefaultInfo(ReferenceHub owner, StringBuilder sb, out Color texColor, PlayerInfoArea? flagsOverride = null)
    {
        texColor = owner.roleManager.CurrentRole.RoleColor;
        PlayerInfoArea flags = flagsOverride ?? owner.nicknameSync.ShownPlayerInfo;

        if ((flags & PlayerInfoArea.Role) != 0)
        {
            sb.AppendLine(owner.roleManager.CurrentRole.RoleColor.ToString());
        }

        if ((flags & PlayerInfoArea.CustomInfo) != 0 && !string.IsNullOrEmpty(owner.nicknameSync.CustomPlayerInfo))
        {
            sb.AppendLine(owner.nicknameSync.SanitizedPlayerInfoString);
        }

        if ((flags & PlayerInfoArea.Nickname) != 0)
        {
            sb.Append(owner.nicknameSync.CombinedName);
        }
    }

    private string CleanNickName(string input, out bool printable)
    {
        StringBuilder sb = StringBuilderPool.Shared.Rent(input.Length);
        char highSurrogate = '\0';
        printable = false;
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
            {
                printable = true;
                sb.Append(c);
            }
            else if (char.IsWhiteSpace(c) && c != '\n' && c != '\r' && c != '\t')
            {
                sb.Append(c);
            }
            else if (char.IsHighSurrogate(c))
            {
                highSurrogate = c;
            }
            else if (char.IsLowSurrogate(c) && char.IsSurrogatePair(highSurrogate, c))
            {
                sb.Append(highSurrogate);
                sb.Append(c);
                printable = true;
            }
        }

        return StringBuilderPool.Shared.ToStringReturn(sb);
    }
}