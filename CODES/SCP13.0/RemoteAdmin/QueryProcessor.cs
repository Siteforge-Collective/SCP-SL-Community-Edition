using CommandSystem;
using Mirror;
using PlayerRoles;
using RemoteAdmin.Communication;
using RemoteAdmin.Interfaces;
using RemoteAdmin.Menus;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EncryptedChannelManager;
using static RemoteAdmin.Communication.RaPlayerList;

namespace RemoteAdmin
{
    public class QueryProcessor : NetworkBehaviour
    {
        public struct CommandData
        {
            public string Command;
            public string[] Usage;
            public string Description;
            public string AliasOf;
            public bool Hidden;

            public override bool Equals(object obj)
            {
                if (obj is CommandData other)
                {
                    return Equals(other);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Command, Usage, Description, AliasOf, Hidden);
            }

            public bool Equals(CommandData other)
            {
                return Command == other.Command &&
                       Usage.SequenceEqual(other.Usage ?? Array.Empty<string>()) &&
                       Description == other.Description &&
                       AliasOf == other.AliasOf &&
                       Hidden == other.Hidden;
            }

            public static bool operator ==(CommandData lhs, CommandData rhs) => lhs.Equals(rhs);
            public static bool operator !=(CommandData lhs, CommandData rhs) => !lhs.Equals(rhs);
        }

        private PlayerCommandSender _sender;
        private RateLimit _commandRateLimit;
        private static CommandData[] _commands;
        private float _lastPlayerlistRequest;
        private bool _commandsSynced;
        private static bool _eventsAssigned;

        private const ushort AdminChatLengthThreshold = 400;
        private const ushort AdminChatShortTime = 5;
        private const ushort AdminChatLongTime = 8;
        private const ushort AdminChatMaxTime = 15;

        [SyncVar]
        [HideInInspector]
        public bool OverridePasswordEnabled;

        internal bool PasswordSent;

        private bool _gameplayData;
        private bool _gdDirty;

        private ReferenceHub _hub;

        private const int CommandDescriptionSyncMaxLength = 80;

        internal static readonly char[] SpaceArray = { ' ' };

        public DateTime ExpectingURL;

        public static readonly ClientCommandHandler DotCommandHandler;

        private string _ipAddress;

        public bool GameplayData
        {
            get => _gameplayData;
            set
            {
                _gameplayData = value;
                _gdDirty = true;
            }
        }

        static QueryProcessor()
        {
            DotCommandHandler = ClientCommandHandler.Create();
        }

        private void Awake()
        {
            _hub = ReferenceHub.GetHub(this);
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                EncryptedChannelManager.RegisterClientHandler(EncryptedChannel.RemoteAdmin, HandleServerReply);
                EncryptedChannelManager.RegisterClientHandler(EncryptedChannel.AdminChat, ClientHandleAdminChat);
            }

            if (isLocalPlayer && NetworkServer.active)
            {
                EncryptedChannelManager.RegisterServerHandler(EncryptedChannel.RemoteAdmin, ServerHandleCommandFromClient);
                EncryptedChannelManager.RegisterServerHandler(EncryptedChannel.AdminChat, ServerHandleAdminChat);
            }

            _commandRateLimit = _hub.playerRateLimitHandler.RateLimits[1];

            if (NetworkServer.active)
            {
                _ipAddress = connectionToClient?.address ?? "localhost";
                OverridePasswordEnabled = ServerStatic.PermissionsHandler.OverrideEnabled;

                if (isLocalPlayer)
                {
                    _commands = ParseCommandsToStruct(CommandProcessor.GetAllCommands());
                }
            }
            else if (isLocalPlayer)
            {
                _commands = null;
            }

            _sender = new PlayerCommandSender(_hub);

            if (isLocalPlayer)
            {
                InvokeRepeating(nameof(RefreshPlayerList), 0f, 1f);

                if (!_eventsAssigned)
                {
                    _eventsAssigned = true;
                    ReferenceHub.OnPlayerAdded += _ => StaticRefreshPlayerList();
                    ReferenceHub.OnPlayerRemoved += _ => StaticRefreshPlayerList();
                }
            }
        }

        internal void RefreshPlayerList()
        {
            if (!isLocalPlayer || !NetworkClient.active) return;

            var ui = UIController.Singleton;
            if (ui == null || !ui.IsEnabled || !ui.LoggedIn) return;

            _lastPlayerlistRequest = 0f;
            RaPlayerList.Request(RaSettings.Singleton.ToggleListOrder.Value, UIController.GetSorting());
        }

        internal static void StaticRefreshPlayerList()
        {
            if (!ReferenceHub.TryGetLocalHub(out var local) ||
                !local.isLocalPlayer ||
                !NetworkClient.active) return;

            var ui = UIController.Singleton;
            if (ui == null || !ui.IsEnabled || !ui.LoggedIn) return;

            local.queryProcessor._lastPlayerlistRequest = 0f;
            RaPlayerList.Request(RaSettings.Singleton.ToggleListOrder.Value, UIController.GetSorting());
        }

        private void Update()
        {
            if (isLocalPlayer)
            {
                _lastPlayerlistRequest += Time.deltaTime;
            }

            if (_gdDirty && NetworkServer.active)
            {
                _gdDirty = false;
                TargetSyncGameplayData(connectionToClient, _gameplayData);
            }
        }

        internal void SyncCommandsToClient()
        {
            if (_commandsSynced) return;
            _commandsSynced = true;
            TargetUpdateCommandList(_commands);
        }

        [Server]
        private static CommandData[] ParseCommandsToStruct(List<ICommand> list)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] ParseCommandsToStruct called when server was not active");
                return null;
            }

            var result = new List<CommandData>();

            foreach (var item in list)
            {
                string desc = item.Description;
                if (string.IsNullOrWhiteSpace(desc))
                {
                    desc = null;
                }
                else if (desc.Length > CommandDescriptionSyncMaxLength)
                {
                    desc = desc.Substring(0, CommandDescriptionSyncMaxLength) + "...";
                }

                result.Add(new CommandData
                {
                    Command = item.Command,
                    Usage = (item is IUsageProvider up) ? up.Usage : null,
                    Description = desc,
                    AliasOf = null,
                    Hidden = item is IHiddenCommand
                });

                if (item.Aliases != null)
                {
                    foreach (var alias in item.Aliases)
                    {
                        result.Add(new CommandData
                        {
                            Command = alias,
                            Usage = null,
                            Description = null,
                            AliasOf = item.Command,
                            Hidden = item is IHiddenCommand
                        });
                    }
                }
            }

            return result.ToArray();
        }

        [TargetRpc]
        private void TargetUpdateCommandList(CommandData[] commands)
        {
            var commandList = TextBasedRemoteAdmin.Commands;
            commandList.Clear();

            foreach (var cmd in commands)
            {
                if (cmd.Command == null) continue;

                if (Misc.CommandRegex != null && !Misc.CommandRegex.IsMatch(cmd.Command))
                    continue;

                if (!string.IsNullOrEmpty(cmd.Description) &&
                    Misc.CommandDescriptionRegex != null &&
                    !Misc.CommandDescriptionRegex.IsMatch(cmd.Description))
                    continue;

                if (cmd.Usage != null)
                {
                    bool allValid = true;
                    foreach (var usage in cmd.Usage)
                    {
                        if (string.IsNullOrWhiteSpace(usage) ||
                            (Misc.CommandDescriptionRegex != null && !Misc.CommandDescriptionRegex.IsMatch(usage)))
                        {
                            allValid = false;
                            break;
                        }
                    }
                    if (!allValid) continue;
                }

                if (commandList.Any(x => string.Equals(x.Command, cmd.Command, StringComparison.OrdinalIgnoreCase)))
                    continue;

                commandList.Add(new CommandData
                {
                    Command = cmd.Command.ToLowerInvariant(),
                    Description = cmd.Description ?? "",
                    Usage = cmd.Usage ?? Array.Empty<string>(),
                    AliasOf = cmd.AliasOf,
                    Hidden = cmd.Hidden
                });
            }
        }

        [Server]
        internal void SendToClient(string content, bool isSuccess, bool logInConsole, string overrideDisplay)
        {
            if (!NetworkServer.active) return;

            var response = new RemoteAdminResponse(content, isSuccess, logInConsole, overrideDisplay);
            _hub.encryptedChannelManager.TrySendMessageToClient(JsonSerialize.ToJson(response), EncryptedChannel.RemoteAdmin);
        }

        private static void ServerHandleCommandFromClient(ReferenceHub hub, string content, SecurityLevel _)
        {
            var qp = hub.queryProcessor;
            if (qp._commandRateLimit.CanExecute() && hub.serverRoles.RemoteAdmin)
            {
                CommandProcessor.ProcessQuery(content, qp._sender);
            }
        }

        private static void ServerHandleAdminChat(ReferenceHub hub, string content, SecurityLevel _)
        {
            var qp = hub.queryProcessor;
            if (qp._commandRateLimit.CanExecute())
            {
                CommandProcessor.ProcessAdminChat(content, qp._sender);
            }
        }

        private void HandleServerReply(string content, SecurityLevel securityLevel)
        {
            RemoteAdminResponse response = JsonSerialize.FromJson<RemoteAdminResponse>(content);
            ProcessReply(response.Content, response.IsSuccess, response.LogInConsole, response.OverrideDisplay,
                         securityLevel == SecurityLevel.Unsecured);
        }

        private static void ClientHandleAdminChat(string content, SecurityLevel _)
        {
            var parts = content.Split('!', 2);
            if (parts.Length != 2) return;

            if (!uint.TryParse(parts[0], out uint sender)) return;

            var message = parts[1];
            ParseAdminChat(ref message, out var mono, out var time, out var silent);

            StaffChatMenu.TryAddMessage(sender, message);

            if (!silent && ReferenceHub.TryGetHubNetID(sender, out var hub))
            {
                Broadcast.AddElement($"{message} ~{hub.nicknameSync.MyNick}",
                                    time,
                                    Broadcast.BroadcastFlags.AdminChat);
            }
        }

        internal static void ParseAdminChat(ref string content, out bool monospaced, out ushort time, out bool silent)
        {
            silent = content.StartsWith("@@", StringComparison.Ordinal);
            if (silent)
            {
                monospaced = false;
                time = 0;
                content = content[2..];
                return;
            }

            monospaced = content.StartsWith("@", StringComparison.Ordinal);
            if (monospaced) content = content[1..];

            time = (ushort)(content.Length > AdminChatLengthThreshold ? AdminChatLongTime : AdminChatShortTime);

            if (content.StartsWith("!") && content.Contains(" "))
            {
                int spaceIdx = content.IndexOf(' ');
                if (ushort.TryParse(content[1..spaceIdx], out ushort parsed))
                {
                    time = Math.Min(parsed, AdminChatMaxTime);
                    if (time == 0) time = AdminChatShortTime;
                    content = content[(spaceIdx + 1)..];
                }
            }
        }

        private void ProcessReply(string content, bool isSuccess, bool logInConsole, string overrideDisplay, bool secure)
        {
            if (content.StartsWith("URL:"))
            {
                DateTime now = DateTime.Now;
                if (ExpectingURL < now)
                {
                    ExpectingURL = DateTime.MinValue;
                    string lookupMode = ServerConfigSynchronizer.Singleton.RemoteAdminExternalPlayerLookupMode;
                    if (!lookupMode.Contains("disabled"))
                    {
                        string urlPart = content.Substring(4);
                        int queryIndex = urlPart.IndexOf('?');
                        string append = string.Empty;
                        if (queryIndex > 0)
                        {
                            bool useAmp = lookupMode.Contains("ampersand");
                            append = useAmp ? "&" : "?";
                            string id = urlPart.Substring(0, queryIndex);
                            append += "lookup=" + id;
                            urlPart = urlPart.Substring(queryIndex + 1);
                        }
                        string baseUrl = lookupMode.Contains("custom") ? "custom_url" : ServerConfigSynchronizer.Singleton.RemoteAdminExternalPlayerLookupURL;
                        string fullUrl = baseUrl + append + urlPart;
                        Application.OpenURL(fullUrl);
                        Steamworks.SteamFriends.OpenWebOverlay(fullUrl, false);
                    }
                    else
                    {
                        TextBasedRemoteAdmin.AddLog("<color=white>[</color><color=red><link=TBRA_InternalError> (INTERNAL ERROR)</link></color><color=white>]</color> <color=red>Server attempted to send us a URL but external lookup is disabled! (This should never happen!)</color>");
                    }
                }
                return;
            }

            string display = overrideDisplay ?? string.Empty;
            if (content.StartsWith("!"))
            {
                int exclIndex = content.IndexOf('!');
                content = content.Remove(0, exclIndex + 1);
                int spaceIndex = content.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    overrideDisplay = content.Substring(0, spaceIndex);
                    content = content.Remove(0, spaceIndex + 1);
                }
            }

            if (logInConsole)
            {
                string prefix = isSuccess ? "<color=white>[</color><link=TBRA_CommandSuccess><color=green></color><color=white></link>] " : "<link=TBRA_CommandFail><color=red></color><color=white></link>]</color> <color=orange>";
                string logMessage = prefix + "[" + display + "] " + content + "</color>";
                TextBasedRemoteAdmin.AddLog(logMessage);
            }

            if (!string.IsNullOrEmpty(overrideDisplay))
            {
                if (string.Equals(overrideDisplay.ToUpperInvariant(), "WIKI"))
                {
                    Application.OpenURL("https://en.scpslgame.com/index.php?title=Remote_Admin");
                }
            }

            if (SubmenuSelector.Singleton?.SelectedMenu != null)
            {
                SubmenuSelector.Singleton.SelectedMenu.SetResponse(isSuccess, content);
            }

            string trimmed = content.Remove(0, 1);
            string[] parts = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && int.TryParse(parts[0], out int requestId))
            {
                if (Communication.CommunicationProcessor.ClientCommunication.TryGetValue(requestId, out IClientCommunication comm))
                {
                    string responseContent = string.Join(" ", parts.Skip(1));
                    comm.ReceiveData(responseContent);
                }
            }
        }

        public void SendQuery(string query, bool gban)
        {
            if (!NetworkClient.active)
            {
                Debug.LogWarning("[Client] SendQuery called when client was not active");
                return;
            }

            if (query == null) return;

            if (gban)
            {
                if (query.StartsWith("GBAN-KICK ", StringComparison.Ordinal))
                {
                    GameCore.Console.AddLog("Global BAN", Color.red);
                    return;
                }

                if (query.StartsWith("@", StringComparison.Ordinal))
                {
                    string subQuery = query.Substring(1);
                    if (_hub != null && _hub.encryptedChannelManager != null)
                    {
                        _hub.encryptedChannelManager.TrySendMessageToServer(subQuery, EncryptedChannel.RemoteAdmin);
                    }
                    return;
                }

                if (query.StartsWith("$", StringComparison.Ordinal)) return;

                string sanitized = Misc.SanitizeRichText(query, "＜", "＞");
                string cleaned = sanitized.Replace("\n", "").Replace("\r", "");
                string formatted = $"<color=purple> {cleaned}</color>";
                TextBasedRemoteAdmin.AddLog(formatted);
                return;
            }

            Debug.LogWarning("[Client] SendQuery called when client was not active");
        }

        internal void ProcessGameConsoleQuery(string query)
        {
            string[] array = query.Trim().Split(SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (DotCommandHandler.TryGetCommand(array[0], out var command))
            {
                try
                {
                    var args = array.AsSpan(1);
                    command.Execute(args.ToArray(), _sender, out var response);
                    response = Misc.CloseAllRichTextTags(response);
                    _hub.gameConsoleTransmission.SendToClient(response, "green");
                }
                catch (Exception ex)
                {
                    string text = "Command execution failed! Error: " + ex.ToString();
                    _hub.gameConsoleTransmission.SendToClient(text, "red");
                }
            }
            else
            {
                _hub.gameConsoleTransmission.SendToClient("Command not found.", "red");
            }
        }

        [TargetRpc]
        public void TargetSyncGameplayData(NetworkConnection conn, bool gd)
        {
            _gameplayData = gd;
        }

        private void OnDestroy()
        {
            if (NetworkServer.active && (!isLocalPlayer || !ServerStatic.IsDedicated))
            {
                string text = $"{_hub.LoggedNameFromRefHub()} disconnected from IP address {_ipAddress}. Last class: {_hub.GetRoleId()}.";
                ServerLogs.AddLog(ServerLogs.Modules.Networking, text, ServerLogs.ServerLogType.ConnectionUpdate);
                ServerConsole.AddLog(text);
            }
        }
    }
}