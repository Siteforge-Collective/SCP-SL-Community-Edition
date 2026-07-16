using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Cryptography;
using GameCore;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using NorthwoodLib;
using Org.BouncyCastle.Crypto;
using RemoteAdmin;
using UnityEngine;
using VoiceChat;

namespace CentralAuth
{
    public class PlayerAuthenticationManager : NetworkBehaviour
    {
        public static event Action<ReferenceHub> OnSyncedUserIdAssigned;
        public static event Action<ReferenceHub, ClientInstanceMode> OnInstanceModeChanged;

        [SyncVar(hook = nameof(UserIdHook))]
        public string SyncedUserId;

        public static bool OnlineMode;
        internal static bool AllowSameAccountJoining;
        public static uint AuthenticationTimeout = 30;
        private static readonly Regex _saltRegex = new Regex("^[a-zA-Z0-9]{32}$", RegexOptions.Compiled);

        private bool _hubSet;
        private bool _authenticationRequested;
        private float _timeoutTimer;
        private float _passwordCooldown;
        private string _privUserId;
        private string _challenge;
        private uint _passwordAttempts;
        private ReferenceHub _hub;
        private ReferenceHub Hub
        {
            get
            {
                if (!_hubSet && ReferenceHub.TryGetHub(gameObject, out _hub))
                    _hubSet = true;
                return _hub;
            }
        }

        private ClientInstanceMode _targetInstanceMode;
        private byte[] _clientDerivedKey;
        private const int HashIterations = 1600;
        private const string HostId = "ID_Host";
        private const string DedicatedId = "ID_Dedicated";
        private const string OfflineModeIdPrefix = "ID_Offline_";

        public AuthenticationResponse AuthenticationResponse { get; private set; }
        public bool DoNotTrack { get; private set; }

        public string UserId
        {
            get
            {
                if (!NetworkServer.active)
                {
                    return SyncedUserId;
                }
                if (_privUserId == null)
                {
                    return null;
                }
                if (_privUserId.Contains("$"))
                {
                    return _privUserId.Substring(0, _privUserId.IndexOf("$", StringComparison.Ordinal));
                }
                return _privUserId;
            }
            set
            {
                if (NetworkServer.active)
                {
#if UNITY_EDITOR
                    _privUserId = "ID_Host";
                    UserIdHook(null, "ID_Host");
                    RefreshSyncedId();
                    Hub.serverRoles.RefreshRealId();
#else
                    _privUserId = value;
                    UserIdHook(null, value);
                    RefreshSyncedId();
                    Hub.serverRoles.RefreshRealId();
#endif
                }
            }
        }

        public string SaltedUserId
        {
            get
            {
                if (!NetworkServer.active)
                {
                    return SyncedUserId;
                }
                return _privUserId;
            }
        }

        public ClientInstanceMode InstanceMode
        {
            get => _targetInstanceMode;
            private set
            {
                if (value != _targetInstanceMode)
                {
                    _targetInstanceMode = value;
                    if (Hub != null)
                        OnInstanceModeChanged?.Invoke(Hub, _targetInstanceMode);
                }
            }
        }

        public bool NorthwoodStaff => AuthenticationResponse.BadgeToken?.Staff ?? false;
        public bool BypassBansFlagSet => AuthenticationResponse.AuthToken?.BypassBans ?? false;

        public bool RemoteAdminGlobalAccess
        {
            get
            {
                BadgeToken badgeToken = AuthenticationResponse.BadgeToken;
                return badgeToken != null && (badgeToken.Management || badgeToken.GlobalBanning);
            }
        }

        public override void OnStartLocalPlayer()
        {
            CustomLiteNetLib4MirrorTransport.ResetRedirectCounter();
            NetworkServer.ReplaceHandler<AuthenticationResponse>(ServerReceiveAuthenticationResponse);
            UserId = HostId;
            RequestAuthentication();
        }

        private void Start()
        {
            if (isLocalPlayer && ServerStatic.IsDedicated)
            {
                Debug.LogError("Dedicated server detected. Setting UserId to DedicatedId.");
                UserId = DedicatedId;
            }
            else if (!OnlineMode)
            {
                UserId = OfflineModeIdPrefix + netId + "_" + DateTimeOffset.Now.ToUnixTimeSeconds();
            }
        }

        private void FixedUpdate()
        {
            if (_passwordCooldown > 0f)
            {
                _passwordCooldown -= Time.fixedDeltaTime;
            }

            if (InstanceMode == ClientInstanceMode.Unverified && !isLocalPlayer && _timeoutTimer >= 0f)
            {
                if (!_authenticationRequested && connectionToClient.isReady)
                {
                    RequestAuthentication();
                }

                _timeoutTimer += Time.fixedDeltaTime;

                if (_timeoutTimer > AuthenticationTimeout)
                {
                    _timeoutTimer = -1f;
                    RejectAuthentication("authentication timeout exceeded.");
                }
            }
        }

        [Server]
        private void RefreshSyncedId()
        {
            if (!NetworkServer.active) return;

            if (_privUserId == null)
            {
                SyncedUserId = null;
                return;
            }

            bool showRealId = isLocalPlayer || (_privUserId.EndsWith("@steam", StringComparison.Ordinal) && !DoNotTrack && !AuthenticationResponse.AuthToken.SyncHashed);
            SyncedUserId = showRealId ? _privUserId : Sha.HashToString(Sha.Sha512(_privUserId));
        }

        private static void ServerReceiveAuthenticationResponse(NetworkConnectionToClient conn, AuthenticationResponse msg)
        {
            if (NetworkServer.active && OnlineMode && ReferenceHub.TryGetHub(conn.identity.gameObject, out ReferenceHub hub))
            {
                hub.authManager.ProcessAuthenticationResponse(msg);
            }
        }

        public string GetAuthToken()
        {
            return AuthenticationResponse.SignedAuthToken != null ? JsonSerialize.ToJson(AuthenticationResponse.SignedAuthToken) : null;
        }

        [Server]
        private void RequestAuthentication()
        {
            if (!NetworkServer.active) return;

            if (!isLocalPlayer)
            {
                _authenticationRequested = true;
                Hub.encryptedChannelManager.PrepareExchange();
            }

            _challenge = RandomGenerator.GetStringSecure(24);
            string ecdhKey = Hub.encryptedChannelManager.EcdhKeys != null ? ECDSA.KeyToString(Hub.encryptedChannelManager.EcdhKeys.Public) : null;

            RpcRequestAuthentication(_challenge, ecdhKey);
        }

        [TargetRpc]
        private void RpcRequestAuthentication(string challenge, string ecdhPublicKey)
        {
            if (_authenticationRequested)
            {
                GameCore.Console.AddLog("Duplicate auth request ignored.", Color.yellow);
                return;
            }

            _authenticationRequested = true;

            CentralAuthManager.TokenChallenge = challenge;
            CentralAuthManager.ServerEcdhPublicKey = ecdhPublicKey;
            CentralAuthManager.RequestAuthToken = true;
        }

        [TargetRpc]
        internal void TargetSetRealId(NetworkConnection conn, string userId)
        {
            _privUserId = userId;
        }

        public void ProcessAuthenticationResponse(AuthenticationResponse msg)
        {
            try
            {
                AuthenticationResponse = msg;

                if (msg.SignedAuthToken == null)
                {
                    RejectAuthentication("authentication token not provided.");
                    return;
                }

                if (!isLocalPlayer)
                {
                    if (msg.EcdhPublicKey == null || msg.EcdhPublicKeySignature == null)
                    {
                        RejectAuthentication("null ECDH public key or public key signature.");
                        return;
                    }

                    if (!ECDSA.VerifyBytes(msg.EcdhPublicKey, msg.EcdhPublicKeySignature, msg.PublicKey))
                    {
                        RejectAuthentication("invalid ECDH exchange public key signature.", msg.BadgeToken.UserId);
                        return;
                    }
                }

                AuthenticationToken token = msg.AuthToken;
                string unsaltedId = RemoveSalt(token.UserId);

                if (_challenge != token.Challenge)
                {
                    RejectAuthentication("invalid authentication challenge.", token.UserId);
                    return;
                }
                _challenge = null;

                if (token.PublicKey != msg.PublicKeyHash)
                {
                    RejectAuthentication("public key hash mismatch.", token.UserId);
                    return;
                }

                if (GameCore.Version.PrivateBeta && !token.PrivateBetaOwnership)
                {
                    RejectAuthentication("you don't own the Private Beta Access Pass DLC.", token.UserId);
                    return;
                }

                IPEndPoint endPoint = null;
                if (!isLocalPlayer)
                {
                    endPoint = LiteNetLib4MirrorServer.Peers[connectionToClient.connectionId].EndPoint;
                    if (endPoint != null)
                    {
                        bool hasPreauth = CustomLiteNetLib4MirrorTransport.UserIds.TryGetValue(endPoint, out var preauthItem);
                        bool isFastReload = CustomLiteNetLib4MirrorTransport.UserIdFastReload.Contains(unsaltedId);

                        if (!isFastReload && (!hasPreauth || !preauthItem.UserId.Equals(unsaltedId, StringComparison.Ordinal)))
                        {
                            Hub.gameConsoleTransmission.SendToClient("UserID mismatch between authentication and preauthentication token.", "red");
                            Hub.gameConsoleTransmission.SendToClient("Preauth: " + (hasPreauth ? preauthItem.UserId : "(null)"), "red");
                            Hub.gameConsoleTransmission.SendToClient("Auth: " + unsaltedId, "red");
                            RejectAuthentication("UserID mismatch between authentication and preauthentication token.", unsaltedId);
                            return;
                        }

                        if (hasPreauth)
                        {
                            CustomLiteNetLib4MirrorTransport.UserIds.Remove(endPoint);
                        }
                    }

                    if (CustomLiteNetLib4MirrorTransport.UserIdFastReload.Contains(unsaltedId))
                    {
                        CustomLiteNetLib4MirrorTransport.UserIdFastReload.Remove(unsaltedId);
                    }
                }

                if (!CheckBans(token, unsaltedId))
                {
                    return;
                }

                if (!isLocalPlayer && msg.EcdhPublicKey != null)
                {
                    Hub.encryptedChannelManager.ServerProcessExchange(msg.EcdhPublicKey);
                }

                string successLog = string.Format("{0} authenticated from endpoint {1}. Player ID assigned: {2}. Auth token serial number: {3}.", unsaltedId, endPoint?.ToString() ?? "(null)", Hub.PlayerId, token.Serial);
                ServerConsole.AddLog(successLog, ConsoleColor.Green);
                ServerLogs.AddLog(ServerLogs.Modules.Networking, successLog, ServerLogs.ServerLogType.ConnectionUpdate);

                FinalizeAuthentication();

                if (msg.SignedBadgeToken != null)
                {
                    BadgeToken badge = msg.BadgeToken;

                    if (badge.Serial != token.Serial)
                    {
                        RejectAuthentication("token serial number mismatch.");
                        return;
                    }
                    if (badge.UserId != Sha.HashToString(Sha.Sha512(SaltedUserId)))
                    {
                        RejectBadgeToken("badge token UserID mismatch.");
                        return;
                    }
                    if (StringUtils.Base64Decode(badge.Nickname) != Hub.nicknameSync.MyNick)
                    {
                        RejectBadgeToken("badge token nickname mismatch.");
                        return;
                    }

                    ApplyGlobalBadge(badge, msg.HideBadge);
                }
            }
            catch (Exception ex)
            {
                ServerConsole.AddLog("Exception during authentication: " + ex.Message, ConsoleColor.Magenta);
                RejectAuthentication("server exception during authentication!");
            }
        }

        private void ApplyGlobalBadge(BadgeToken token, bool hideBadge)
        {
            ulong perms = (token.RaPermissions == 0L || ServerStatic.PermissionsHandler.NorthwoodAccess) ? ServerStatic.PermissionsHandler.FullPerm : token.RaPermissions;

            if ((token.Management || token.GlobalBanning) && CustomNetworkManager.IsVerified)
            {
                Hub.serverRoles.GlobalPerms |= 8388608uL | 1048576uL;
            }
            if (AuthenticationResponse.BadgeToken.OverwatchMode)
            {
                Hub.serverRoles.GlobalPerms |= 4096uL;
            }
            if (token.Staff || token.Management || token.GlobalBanning)
            {
                Hub.serverRoles.GlobalPerms |= 16908288uL;
            }
            if ((token.Staff && ServerStatic.PermissionsHandler.NorthwoodAccess) ||
                (token.RemoteAdmin && ServerStatic.PermissionsHandler.StaffAccess) ||
                (token.Management && ServerStatic.PermissionsHandler.ManagersAccess) ||
                (token.GlobalBanning && ServerStatic.PermissionsHandler.BanningTeamAccess))
            {
                Hub.serverRoles.GlobalPerms |= perms;
            }

            if ((token.BadgeText != null && token.BadgeText != "(none)") || (token.BadgeColor != null && token.BadgeColor != "(none)"))
            {
                if (Hub.serverRoles.UserBadgePreferences == ServerRoles.BadgePreferences.PreferGlobal || !Hub.serverRoles.BadgeCover || Hub.serverRoles.Group == null)
                {
                    bool isHidden = hideBadge;

                    if (token.BadgeType == 1 && ConfigFile.ServerConfig.GetBool("hide_staff_badges_by_default")) isHidden = true;
                    if (token.BadgeType == 2 && ConfigFile.ServerConfig.GetBool("hide_management_badges_by_default")) isHidden = true;
                    if (token.BadgeType == 0 && (ConfigFile.ServerConfig.GetBool("hide_patreon_badges_by_default") && !CustomNetworkManager.IsVerified)) isHidden = true;
                    if (token.BadgeType == 3) isHidden = true;

                    if (isHidden)
                    {
                        Hub.serverRoles.HiddenBadge = token.BadgeText;
                        Hub.serverRoles.GlobalHidden = true;
                        Hub.serverRoles.RefreshHiddenTag();
                        Hub.gameConsoleTransmission.SendToClient("Your global badge has been granted, but it's hidden. Use \".gtag\" command in the game console to show your global badge.", "yellow");
                    }
                    else
                    {
                        Hub.serverRoles.HiddenBadge = null;
                        Hub.serverRoles.RpcResetFixed();
                        Hub.serverRoles.GlobalBadge = AuthenticationResponse.SignedBadgeToken.token;
                        Hub.serverRoles.GlobalBadgeSignature = AuthenticationResponse.SignedBadgeToken.signature;
                        Hub.gameConsoleTransmission.SendToClient("Your global badge has been granted.", "cyan");
                    }
                }
                else
                {
                    Hub.gameConsoleTransmission.SendToClient("Your global badge is covered by server badge. Use \".gtag\" command in the game console to show your global badge.", "yellow");
                }
            }
            Hub.serverRoles.FinalizeSetGroup();
        }

        [Server]
        private void RejectAuthentication(string reason, string userId = null, bool removeSalt = false)
        {
            if (!NetworkServer.active) return;

            if (userId != null && removeSalt)
            {
                userId = RemoveSalt(userId);
            }

            ServerConsole.AddLog("Player " + (userId ?? "(unknown)") + " (" + connectionToClient.address + ") failed to authenticate: " + reason, ConsoleColor.Red);
            Hub.gameConsoleTransmission.SendToClient("Authentication failure: " + reason, "red");
            ServerConsole.Disconnect(Hub.connectionToClient, "Authentication failure: " + reason);
        }

        [Server]
        private void RejectBadgeToken(string reason)
        {
            if (!NetworkServer.active) return;
            Hub.gameConsoleTransmission.SendToClient("Your global badge token is invalid. Reason: " + reason, "red");
        }

        [Server]
        private void FinalizeAuthentication()
        {
            if (!NetworkServer.active) return;

            UserId = AuthenticationResponse.AuthToken.UserId;
            DoNotTrack = AuthenticationResponse.DoNotTrack || AuthenticationResponse.AuthToken.DoNotTrack;
            Hub.nicknameSync.UpdateNickname(StringUtils.Base64Decode(AuthenticationResponse.AuthToken.Nickname));

            if (DoNotTrack)
            {
                ServerLogs.AddLog(ServerLogs.Modules.Networking, Hub.LoggedNameFromRefHub() + " connected from IP address " + connectionToClient.address + " sent Do Not Track signal.", ServerLogs.ServerLogType.ConnectionUpdate);
            }

            Hub.gameConsoleTransmission.SendToClient("Hi " + Hub.nicknameSync.MyNick + "! You have been authenticated on this server.", "green");
            Hub.serverRoles.RefreshPermissions();

            if (!AllowSameAccountJoining)
            {
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    if (hub.authManager.UserId == UserId && hub.PlayerId != Hub.PlayerId && !hub.isLocalPlayer)
                    {
                        ServerConsole.AddLog($"Player {UserId} ({hub.PlayerId}, {connectionToClient.address}) has been kicked from the server, because he has just joined the server again from IP address {connectionToClient.address}.");
                        ServerConsole.Disconnect(hub.gameObject, "Only one player instance of the same player is allowed.");
                    }
                }
            }
        }

        [Server]
        private bool CheckBans(AuthenticationToken token, string unsalted)
        {
            if (!NetworkServer.active) return false;

            if ((!token.BypassBans || !CustomNetworkManager.IsVerified) && BanHandler.QueryBan(unsalted, null).Key != null)
            {
                Hub.gameConsoleTransmission.SendToClient("You are banned from this server.", "red");
                ServerConsole.AddLog("Player kicked due to local UserID ban.");
                ServerConsole.Disconnect(Hub.connectionToClient, "You are banned from this server.");
                return false;
            }

            if (CustomNetworkManager.IsVerified || CustomLiteNetLib4MirrorTransport.UseGlobalBans)
            {
                if (token.GlobalBan != "NO")
                {
                    if (token.GlobalBan == "M1")
                    {
                        ServerConsole.AddLog("Player " + token.UserId + " is globally muted.");
                    }
                    else if (token.GlobalBan == "M2")
                    {
                        ServerConsole.AddLog("Player " + token.UserId + " is globally muted on intercom.");
                    }
                    else
                    {
                        Hub.gameConsoleTransmission.SendToClient(token.GlobalBan, "red");
                        ServerConsole.AddLog("Player " + token.UserId + " has been kicked due to an active global ban: " + token.GlobalBan);
                        ServerConsole.Disconnect(Hub.connectionToClient, token.GlobalBan);
                        return false;
                    }
                }
            }

            if (!token.SkipIpCheck && !token.RequestIp.Equals("N/A", StringComparison.Ordinal) && ServerConsole.EnforceSameIp)
            {
                string address = Hub.connectionToClient.address;
                if ((address.Contains(".") && token.RequestIp.Contains(".")) || (address.Contains(":") && token.RequestIp.Contains(":")))
                {
                    bool allowLocal = false;
                    if (ServerConsole.SkipEnforcementForLocalAddresses)
                    {
                        allowLocal = address == "127.0.0.1" || address.StartsWith("10.") || address.StartsWith("192.168.");
                        if (!allowLocal && address.StartsWith("172."))
                        {
                            string[] ipParts = address.Split('.');
                            if (ipParts.Length == 4 && byte.TryParse(ipParts[1], out var secondByte) && secondByte >= 16 && secondByte <= 31)
                            {
                                allowLocal = true;
                            }
                        }
                    }

                    if (!allowLocal && address != token.RequestIp)
                    {
                        Hub.gameConsoleTransmission.SendToClient("Authentication token has been issued to a different IP address.", "red");
                        Hub.gameConsoleTransmission.SendToClient("Your IP address: " + address, "red");
                        Hub.gameConsoleTransmission.SendToClient("Issued to: " + token.RequestIp, "red");
                        ServerConsole.AddLog("Player kicked due to IP addresses mismatch.");
                        ServerConsole.Disconnect(Hub.connectionToClient, "Authentication token has been issued to a different IP address. You can find details in the game console.");
                        return false;
                    }
                }
            }

            VcMuteFlags muteFlags = VcMuteFlags.None;

            if (VoiceChatMutes.QueryLocalMute(unsalted))
            {
                muteFlags |= VcMuteFlags.LocalRegular;
                Hub.gameConsoleTransmission.SendToClient("You are muted on the voice chat by the server administrator.", "red");
            }

            if ((ConfigFile.ServerConfig.GetBool("global_mutes_voicechat", true) || CustomNetworkManager.IsVerified) && token.GlobalBan == "M1")
            {
                muteFlags |= VcMuteFlags.GlobalRegular;
                Hub.gameConsoleTransmission.SendToClient("You are globally muted on the voice chat.", "red");
            }

            if (VoiceChatMutes.QueryLocalMute(unsalted, true))
            {
                muteFlags |= VcMuteFlags.LocalIntercom;
                Hub.gameConsoleTransmission.SendToClient("You are muted on the intercom by the server administrator.", "red");
            }
            else if ((ConfigFile.ServerConfig.GetBool("global_mutes_intercom", true) || CustomNetworkManager.IsVerified) && token.GlobalBan == "M2")
            {
                muteFlags |= VcMuteFlags.GlobalIntercom;
                Hub.gameConsoleTransmission.SendToClient("You are globally muted on the intercom.", "red");
            }

            if (token.BypassBans)
            {
                muteFlags = VcMuteFlags.None;
            }

            VoiceChatMutes.SetFlags(Hub, muteFlags);
            return true;
        }

        private static string RemoveSalt(string userId)
        {
            if (userId != null)
            {
                if (userId.Contains("$"))
                {
                    return userId.Substring(0, userId.IndexOf("$", StringComparison.Ordinal));
                }
                return userId;
            }
            return null;
        }

        public void ClientStartAuthentication(string password)
        {
            string serverSalt = ReferenceHub.HostHub.encryptedChannelManager.ServerRandom;
            string clientSalt = RandomGenerator.GetStringSecure(32);
            byte[] derivedKey = DerivePassword(password, serverSalt, clientSalt);
            string ecdhKey = ECDSA.KeyToString(Hub.encryptedChannelManager.EcdhKeys.Public);
            byte[] signature = Sha.Sha512Hmac(derivedKey, ecdhKey);
            CmdHandlePasswordAuthentication(clientSalt, signature, ecdhKey);
        }

        [Command]
        private void CmdHandlePasswordAuthentication(string clientSalt, byte[] signature, string ecdhPublicKey)
        {
            if (_passwordAttempts > 2)
            {
                Hub.gameConsoleTransmission.SendToClient("Limit of RA password auth attempts exceeded.", "red");
                return;
            }
            if (_passwordCooldown > 0f)
            {
                Hub.gameConsoleTransmission.SendToClient("Please wait before trying to use password again!", "red");
                return;
            }
            _passwordCooldown = 1.8f;

            if (isLocalPlayer)
            {
                Hub.gameConsoleTransmission.SendToClient("Password authentication is not available for the host.", "red");
                return;
            }

            ReferenceHub hostHub = ReferenceHub.HostHub;
            if (!hostHub.queryProcessor.OverridePasswordEnabled)
            {
                Hub.gameConsoleTransmission.SendToClient("Password authentication is disabled on this server!", "red");
                return;
            }

            if (clientSalt == null || signature == null)
            {
                Hub.gameConsoleTransmission.SendToClient("Invalid password auth request - null parameters.", "red");
                return;
            }

            if (!_saltRegex.IsMatch(clientSalt))
            {
                Hub.gameConsoleTransmission.SendToClient("Invalid password auth request - invalid client salt.", "red");
                return;
            }

            byte[] key = DerivePassword(ServerStatic.PermissionsHandler.OverridePassword, hostHub.encryptedChannelManager.ServerRandom, clientSalt);

            if (OnlineMode)
            {
                if (SyncedUserId == null)
                {
                    Hub.gameConsoleTransmission.SendToClient("Can't process password auth request - not authenticated while server is in online mode.", "red");
                }
                else if (!Sha.Sha512Hmac(key, SyncedUserId).SequenceEqual(signature))
                {
                    _passwordAttempts++;
                    string text = Hub.LoggedNameFromRefHub() + " attempted to use an invalid RemoteAdmin override password.";
                    ServerConsole.AddLog(text, ConsoleColor.Magenta);
                    ServerLogs.AddLog(ServerLogs.Modules.Permissions, text, ServerLogs.ServerLogType.ConnectionUpdate);
                    RpcAnimateInvalidPassword();
                }
                else
                {
                    AssignPasswordOverrideGroup();
                }
            }
            else
            {
                if (ecdhPublicKey == null)
                {
                    Hub.gameConsoleTransmission.SendToClient("Can't process password auth request - ecdhPublicKey is null in offline mode.", "red");
                }
                else if (!Sha.Sha512Hmac(key, ecdhPublicKey).SequenceEqual(signature))
                {
                    _passwordAttempts++;
                    string text2 = Hub.LoggedNameFromRefHub() + " attempted to use an invalid RemoteAdmin override password.";
                    ServerConsole.AddLog(text2, ConsoleColor.Magenta);
                    ServerLogs.AddLog(ServerLogs.Modules.Permissions, text2, ServerLogs.ServerLogType.ConnectionUpdate);
                    RpcAnimateInvalidPassword();
                }
                else
                {
                    Hub.encryptedChannelManager.PrepareExchange();
                    Hub.encryptedChannelManager.ServerProcessExchange(ecdhPublicKey);
                    string myPublicKey = ECDSA.KeyToString(Hub.encryptedChannelManager.EcdhKeys.Public);
                    byte[] responseSignature = Sha.Sha512Hmac(key, myPublicKey);
                    RpcFinishExchange(myPublicKey, responseSignature);
                    AssignPasswordOverrideGroup();
                }
            }
        }

        [TargetRpc]
        private void RpcFinishExchange(string publicKey, byte[] signature)
        {
            byte[] expected = Sha.Sha512Hmac(_clientDerivedKey, publicKey);
            bool valid = expected.SequenceEqual(signature);
            _clientDerivedKey = null;

            if (valid)
            {
                Hub.encryptedChannelManager.ServerProcessExchange(publicKey);
                Debug.Log("Password auth exchange finished.");
            }
            else
            {
                GameCore.Console.AddLog("Password exchange: invalid server signature!", Color.red);
            }
        }

        [TargetRpc]
        private void RpcAnimateInvalidPassword()
        {
            UIController.Singleton.AnimateInvalidPassword();
        }

        [Server]
        public void ResetPasswordAttempts()
        {
            if (NetworkServer.active)
            {
                _passwordAttempts = 0u;
            }
        }

        [Server]
        private void AssignPasswordOverrideGroup()
        {
            if (!NetworkServer.active) return;

            UserGroup overrideGroup = ServerStatic.PermissionsHandler.OverrideGroup;
            if (overrideGroup != null)
            {
                string text = Hub.LoggedNameFromRefHub() + " used a valid RemoteAdmin override password.";
                ServerConsole.AddLog(text, ConsoleColor.DarkYellow);
                ServerLogs.AddLog(ServerLogs.Modules.Permissions, text, ServerLogs.ServerLogType.ConnectionUpdate);
                Hub.serverRoles.SetGroup(overrideGroup, true);
            }
            else
            {
                ServerConsole.AddLog("Non-existing group is assigned for override password!", ConsoleColor.Red);
                Hub.gameConsoleTransmission.SendToClient("Non-existing group is assigned for override password!", "red");
            }
        }

        private static byte[] DerivePassword(string password, string serversalt, string clientsalt)
        {
            byte[] salt = Sha.Sha512(serversalt + "/" + clientsalt);
            return PBKDF2.Pbkdf2HashBytes(password, salt, HashIterations, 512);
        }

        private void UserIdHook(string p, string i)
        {
            PlayerAuthenticationManager.OnSyncedUserIdAssigned?.Invoke(Hub);
            if (string.IsNullOrEmpty(i))
            {
                InstanceMode = ClientInstanceMode.Unverified;
                return;
            }
            ClientInstanceMode instanceMode = ((i == "ID_Dedicated") ? ClientInstanceMode.DedicatedServer : ((!(i == "ID_Host")) ? ClientInstanceMode.ReadyClient : ClientInstanceMode.Host));
            InstanceMode = instanceMode;
        }
    }
}