using CentralAuth;
using Cryptography;
using GameCore;
using LiteNetLib;
using LiteNetLib.Utils;
using Mirror.LiteNetLib4Mirror;
using PluginAPI.Core;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using Color = UnityEngine.Color;
using Version = GameCore.Version;

public class CustomLiteNetLib4MirrorTransport : LiteNetLib4MirrorTransport
{
    private enum ClientType : byte
    {
        GameClient = 0,
        VerificationService = 1
    }

    private static readonly NetDataWriter RequestWriter;

    public static GeoblockingMode Geoblocking;

    public static ChallengeType ChallengeMode;

    public static ushort ChallengeInitLen;

    public static ushort ChallengeSecretLen;

    public static readonly Dictionary<IPEndPoint, PreauthItem> UserIds;

    public static readonly HashSet<string> UserIdFastReload;

    public static readonly Dictionary<string, PreauthChallengeItem> Challenges;

    public static bool UserRateLimiting;

    public static bool IpRateLimiting;

    public static bool UseGlobalBans;

    public static bool GeoblockIgnoreWhitelisted;

    public static bool UseChallenge;

    public static bool DisplayPreauthLogs;

    private static bool _delayConnections;

    public static bool SuppressRejections;

    public static bool SuppressIssued;

    public static uint Rejected;

    public static uint ChallengeIssued;

    public static byte DelayTime;

    internal static byte DelayVolume;

    internal static byte DelayVolumeThreshold;

    public static readonly HashSet<string> UserRateLimit;

    public static readonly HashSet<IPAddress> IpRateLimit;

    public static readonly HashSet<string> GeoblockingList;

    public static RejectionReason LastRejectionReason;

    public static string LastCustomReason;

    public static string VerificationChallenge;

    public static string VerificationResponse;

    public static long LastBanExpiration;

    public static bool IpPassthroughEnabled;

    public static HashSet<IPAddress> TrustedProxies;

    public static Dictionary<int, string> RealIpAddresses;

    public static uint RejectionThreshold;

    public static uint IssuedThreshold;

    private static readonly byte[] EmptyByte;

    private static int clientChallengeId;

    private static byte _redirectCounter;

    private static ushort clientChallengeSecretLen;

    private static byte[] clientChallenge;

    private static byte[] clientChallengeBase;

    private static byte[] clientChallengeResponse;

    private static ChallengeType clientChallengeType;

    public static ChallengeState ClientChallengeState;

    private static readonly Thread ChallengeThread;

    public static bool DelayConnections
    {
        get
        {
            return _delayConnections;
        }
        set
        {
            if (!value)
            {
                UserIds.Clear();
            }
            _delayConnections = value;
            ServerConsole.AddLog(value ? $"Incoming connections will be now delayed by {DelayTime} seconds." : "Incoming connections will be no longer delayed.");
        }
    }


    static CustomLiteNetLib4MirrorTransport()
    {
        RequestWriter = new NetDataWriter();
        RequestWriter.Put(0);
        RequestWriter.Put(0);
        UserIds = new Dictionary<IPEndPoint, PreauthItem>();
        UserIdFastReload = new HashSet<string>(StringComparer.Ordinal);
        Challenges = new Dictionary<string, PreauthChallengeItem>();
        UserRateLimit = new HashSet<string>(StringComparer.Ordinal);
        IpRateLimit = new HashSet<IPAddress>();
        GeoblockingList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        EmptyByte = new byte[0];
        ChallengeThread = new Thread(new ThreadStart(ProcessChallenge));
        ChallengeThread.Name = "Challenge thread";
        ChallengeThread.Priority = System.Threading.ThreadPriority.Normal;
        ChallengeThread.IsBackground = true;
        clientChallengeResponse = EmptyByte;
        ChallengeThread.Start();
        RejectionThreshold = 60u;
        IssuedThreshold = 50u;
    }

    internal static void SetReconnectionParameters(bool roundRestart)
    {
        Singleton.reconnectDelay = 600;
        Singleton.maxConnectAttempts = 3;
    }

    protected internal override void ProcessConnectionRequest(ConnectionRequest request)
    {
        byte clientType;
        if (!request.Data.TryGetByte(out clientType) || clientType >= 2)
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.InvalidToken);
            request.Reject(RequestWriter);
            return;
        }

        if (clientType == (byte)ClientType.VerificationService)
        {
            string challenge;
            if (VerificationChallenge != null && request.Data.TryGetString(out challenge) && challenge == VerificationChallenge)
            {
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.VerificationAccepted);
                RequestWriter.Put(VerificationResponse);
                request.Reject(RequestWriter);
                VerificationChallenge = null;
                VerificationResponse = null;
                ServerConsole.AddLog("Verification challenge and response have been sent.\nThe system has successfully checked your server, a verification response will be printed to your console shortly, please allow up to 5 minutes.", ConsoleColor.Green);
                return;
            }
            Rejected++;
            if (Rejected > RejectionThreshold)
            {
                SuppressRejections = true;
            }
            if (!SuppressRejections && DisplayPreauthLogs)
            {
                ServerConsole.AddLog($"Invalid verification challenge has been received from endpoint {request.RemoteEndPoint}.");
            }
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.VerificationRejected);
            request.Reject(RequestWriter);
            return;
        }

        byte clientMajor, clientMinor, clientRevision, clientBackwardRevision = 0;
        bool clientBackwardEnabled;
        if (!request.Data.TryGetByte(out clientMajor) || !request.Data.TryGetByte(out clientMinor) || !request.Data.TryGetByte(out clientRevision) || !request.Data.TryGetBool(out clientBackwardEnabled) || (clientBackwardEnabled && !request.Data.TryGetByte(out clientBackwardRevision)))
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.VersionMismatch);
            request.Reject(RequestWriter);
            return;
        }

        if (!Version.CompatibilityCheck(Version.Major, Version.Minor, Version.Revision, clientMajor, clientMinor, clientRevision, clientBackwardEnabled, clientBackwardRevision))
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.VersionMismatch);
            request.Reject(RequestWriter);
            return;
        }

        int challengeId;
        byte[] challengeResponse;
        if (!request.Data.TryGetInt(out challengeId) || !request.Data.TryGetBytesWithLength(out challengeResponse))
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.InvalidChallenge);
            request.Reject(RequestWriter);
            return;
        }

        if (DelayConnections)
        {
            PreauthDisableIdleMode();
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.Delay);
            RequestWriter.Put(DelayTime);
            if (DelayVolume < byte.MaxValue)
            {
                DelayVolume++;
            }
            if (DelayVolume < DelayVolumeThreshold)
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Delayed connection incoming from endpoint {request.RemoteEndPoint} by {DelayTime} seconds.");
                }
                request.Reject(RequestWriter);
            }
            else
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Force delayed connection incoming from endpoint {request.RemoteEndPoint} by {DelayTime} seconds.");
                }
                request.Reject(RequestWriter);
            }
            return;
        }

        if (UseChallenge)
        {
            if (challengeId == 0 || challengeResponse == null || challengeResponse.Length == 0)
            {
                if (!CheckIpRateLimit(request))
                {
                    return;
                }
                int num = 0;
                string key = string.Empty;
                for (byte b = 0; b < 3; b++)
                {
                    num = RandomGenerator.GetInt32();
                    if (num == 0)
                    {
                        num = 1;
                    }
                    key = request.RemoteEndPoint.Address?.ToString() + "-" + num;
                    if (!Challenges.ContainsKey(key))
                    {
                        break;
                    }
                    if (b == 2)
                    {
                        RequestWriter.Reset();
                        RequestWriter.Put((byte)RejectionReason.Error);
                        request.Reject(RequestWriter);
                        if (DisplayPreauthLogs)
                        {
                            ServerConsole.AddLog($"Failed to generate ID for challenge for incoming connection from endpoint {request.RemoteEndPoint}.");
                        }
                        return;
                    }
                }
                byte[] bytes = RandomGenerator.GetBytes(ChallengeInitLen + ChallengeSecretLen, true);
                ChallengeIssued++;
                if (ChallengeIssued > IssuedThreshold)
                {
                    SuppressIssued = true;
                }
                if (!SuppressIssued && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Requested challenge for incoming connection from endpoint {request.RemoteEndPoint}.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.Challenge);
                RequestWriter.Put((byte)ChallengeMode);
                RequestWriter.Put(num);
                switch (ChallengeMode)
                {
                    case ChallengeType.MD5:
                        RequestWriter.PutBytesWithLength(bytes, 0, ChallengeInitLen);
                        RequestWriter.Put(ChallengeSecretLen);
                        RequestWriter.PutBytesWithLength(Md.Md5(bytes));
                        Challenges.Add(key, new PreauthChallengeItem(new ArraySegment<byte>(bytes, ChallengeInitLen, ChallengeSecretLen)));
                        break;
                    case ChallengeType.SHA1:
                        RequestWriter.PutBytesWithLength(bytes, 0, ChallengeInitLen);
                        RequestWriter.Put(ChallengeSecretLen);
                        RequestWriter.PutBytesWithLength(Sha.Sha1(bytes));
                        Challenges.Add(key, new PreauthChallengeItem(new ArraySegment<byte>(bytes, ChallengeInitLen, ChallengeSecretLen)));
                        break;
                    default:
                        RequestWriter.PutBytesWithLength(bytes);
                        Challenges.Add(key, new PreauthChallengeItem(new ArraySegment<byte>(bytes)));
                        break;
                }
                request.Reject(RequestWriter);
                PreauthDisableIdleMode();
                return;
            }
            string key2 = request.RemoteEndPoint.Address?.ToString() + "-" + challengeId;
            if (!Challenges.ContainsKey(key2))
            {
                Rejected++;
                if (Rejected > RejectionThreshold)
                {
                    SuppressRejections = true;
                }
                if (!SuppressRejections && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been REJECTED (invalid Challenge ID).");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.InvalidChallengeKey);
                request.Reject(RequestWriter);
                return;
            }
            ArraySegment<byte> validResponse = Challenges[key2].ValidResponse;
            if (!challengeResponse.SequenceEqual(validResponse))
            {
                Rejected++;
                if (Rejected > RejectionThreshold)
                {
                    SuppressRejections = true;
                }
                if (!SuppressRejections && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been REJECTED (invalid response).");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.InvalidChallenge);
                request.Reject(RequestWriter);
                return;
            }
            Challenges.Remove(key2);
            PreauthDisableIdleMode();
            if (DisplayPreauthLogs)
            {
                ServerConsole.AddLog($"Security challenge response of incoming connection from endpoint {request.RemoteEndPoint} has been accepted.");
            }
        }
        else if (!CheckIpRateLimit(request))
        {
            return;
        }

        int position = request.Data.Position;
        if (!PlayerAuthenticationManager.OnlineMode)
        {
            KeyValuePair<BanDetails, BanDetails> banQuery = BanHandler.QueryBan(null, request.RemoteEndPoint.Address.ToString());
            if (banQuery.Value != null)
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player tried to connect from banned endpoint {request.RemoteEndPoint}.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.Banned);
                RequestWriter.Put(banQuery.Value.Expires);
                RequestWriter.Put(banQuery.Value?.Reason ?? string.Empty);
                request.Reject(RequestWriter);
                ResetIdleMode();
            }
            else
            {
                PreauthCancellationData cancellationData = EventManager.ExecuteEvent<PreauthCancellationData>(new PlayerPreauthEvent(null, request.RemoteEndPoint.Address.ToString(), 0L, CentralAuthPreauthFlags.None, null, null, request, position));
                if (ProcessCancellationData(request, cancellationData))
                {
                    request.Accept();
                    PreauthDisableIdleMode();
                }
            }
            return;
        }

        string userId;
        if (!request.Data.TryGetString(out userId) || userId == string.Empty)
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.AuthenticationRequired);
            request.Reject(RequestWriter);
            return;
        }

        long expiration;
        byte flagsByte;
        string country;
        byte[] signature;
        if (!request.Data.TryGetLong(out expiration) || !request.Data.TryGetByte(out flagsByte) || !request.Data.TryGetString(out country) || !request.Data.TryGetBytesWithLength(out signature))
        {
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.Error);
            request.Reject(RequestWriter);
            return;
        }

        string forwardedIp = null;
        string endpointText = (IpPassthroughEnabled && TrustedProxies.Contains(request.RemoteEndPoint.Address) && request.Data.TryGetString(out forwardedIp)) ? $"{forwardedIp} [routed via {request.RemoteEndPoint}]" : request.RemoteEndPoint.ToString();
        CentralAuthPreauthFlags flags = (CentralAuthPreauthFlags)flagsByte;

        try
        {
            if (!ECDSA.VerifyBytes($"{userId};{flagsByte};{country};{expiration}", signature, ServerConsole.PublicKey))
            {
                Rejected++;
                if (Rejected > RejectionThreshold)
                {
                    SuppressRejections = true;
                }
                if (!SuppressRejections && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player from endpoint {endpointText} sent preauthentication token with invalid digital signature.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.InvalidToken);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if (TimeBehaviour.CurrentUnixTimestamp > expiration)
            {
                Rejected++;
                if (Rejected > RejectionThreshold)
                {
                    SuppressRejections = true;
                }
                if (!SuppressRejections && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player from endpoint {endpointText} sent expired preauthentication token.");
                    ServerConsole.AddLog("Make sure that time and timezone set on server is correct. We recommend synchronizing the time.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.ExpiredAuth);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if (UserRateLimiting)
            {
                if (UserRateLimit.Contains(userId))
                {
                    Rejected++;
                    if (Rejected > RejectionThreshold)
                    {
                        SuppressRejections = true;
                    }
                    if (!SuppressRejections && DisplayPreauthLogs)
                    {
                        ServerConsole.AddLog($"Incoming connection from {userId} ({endpointText}) rejected due to exceeding the rate limit.");
                    }
                    RequestWriter.Reset();
                    RequestWriter.Put((byte)RejectionReason.RateLimit);
                    request.Reject(RequestWriter);
                    ResetIdleMode();
                    return;
                }
                UserRateLimit.Add(userId);
            }

            if (!flags.HasFlagFast(CentralAuthPreauthFlags.IgnoreBans) || !CustomNetworkManager.IsVerified)
            {
                KeyValuePair<BanDetails, BanDetails> banQuery2 = BanHandler.QueryBan(userId, forwardedIp ?? request.RemoteEndPoint.Address.ToString());
                if (banQuery2.Key != null || banQuery2.Value != null)
                {
                    Rejected++;
                    if (Rejected > RejectionThreshold)
                    {
                        SuppressRejections = true;
                    }
                    if (!SuppressRejections && DisplayPreauthLogs)
                    {
                        string banType = (banQuery2.Key == null) ? "Player" : "Banned player";
                        string ipBanType = (banQuery2.Value == null) ? "" : " banned";
                        ServerConsole.AddLog($"{banType} {userId} tried to connect from{ipBanType} endpoint {endpointText}.");
                        ServerLogs.AddLog(ServerLogs.Modules.Networking, $"{banType} {userId} tried to connect from{ipBanType} endpoint {endpointText}.", ServerLogs.ServerLogType.ConnectionUpdate);
                    }
                    RequestWriter.Reset();
                    RequestWriter.Put((byte)RejectionReason.Banned);
                    RequestWriter.Put(banQuery2.Key?.Expires ?? banQuery2.Value.Expires);
                    RequestWriter.Put(banQuery2.Key?.Reason ?? banQuery2.Value?.Reason ?? string.Empty);
                    request.Reject(RequestWriter);
                    ResetIdleMode();
                    return;
                }
            }

            if (flags.HasFlagFast(CentralAuthPreauthFlags.AuthRejected))
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player {userId} ({endpointText}) kicked due to auth rejection by central server.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.CentralServerAuthRejected);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if (flags.HasFlagFast(CentralAuthPreauthFlags.GloballyBanned) && (CustomNetworkManager.IsVerified || UseGlobalBans))
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player {userId} ({endpointText}) kicked due to an active global ban.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.GloballyBanned);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if ((!flags.HasFlagFast(CentralAuthPreauthFlags.IgnoreWhitelist) || !CustomNetworkManager.IsVerified) && !WhiteList.IsWhitelisted(userId))
            {
                if (DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player {userId} tried joined from endpoint {endpointText}, but is not whitelisted.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.NotWhitelisted);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if (Geoblocking != GeoblockingMode.None && (!flags.HasFlagFast(CentralAuthPreauthFlags.IgnoreGeoblock) || !ServerStatic.PermissionsHandler.BanTeamBypassGeo) && (!GeoblockIgnoreWhitelisted || !WhiteList.IsOnWhitelist(userId)) && ((Geoblocking == GeoblockingMode.Whitelist && !GeoblockingList.Contains(country)) || (Geoblocking == GeoblockingMode.Blacklist && GeoblockingList.Contains(country))))
            {
                Rejected++;
                if (Rejected > RejectionThreshold)
                {
                    SuppressRejections = true;
                }
                if (!SuppressRejections && DisplayPreauthLogs)
                {
                    ServerConsole.AddLog($"Player {userId} ({endpointText}) tried joined from blocked country {country}.");
                }
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.Geoblocked);
                request.Reject(RequestWriter);
                ResetIdleMode();
                return;
            }

            if (UserIdFastReload.Contains(userId))
            {
                UserIdFastReload.Remove(userId);
            }

            bool hasSlot = LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count < CustomNetworkManager.slots ||
                           (LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count != LiteNetLib4MirrorNetworkManager.singleton.maxConnections &&
                            ((flags.HasFlagFast(CentralAuthPreauthFlags.ReservedSlot) && ServerStatic.PermissionsHandler.BanTeamSlots) ||
                             (ConfigFile.ServerConfig.GetBool("use_reserved_slots", true) && ReservedSlot.HasReservedSlot(userId, out bool bypass) &&
                              (bypass || LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count < CustomNetworkManager.slots + CustomNetworkManager.reservedSlots))));

            if (hasSlot)
            {
                PreauthCancellationData cancellationData = EventManager.ExecuteEvent<PreauthCancellationData>(new PlayerPreauthEvent(userId, request.RemoteEndPoint.Address.ToString(), expiration, flags, country, signature, request, position));
                if (!ProcessCancellationData(request, cancellationData))
                {
                    return;
                }

                if (UserIds.ContainsKey(request.RemoteEndPoint))
                {
                    UserIds[request.RemoteEndPoint].SetUserId(userId);
                }
                else
                {
                    UserIds.Add(request.RemoteEndPoint, new PreauthItem(userId));
                }

                NetPeer peer = request.Accept();
                if (forwardedIp != null)
                {
                    if (RealIpAddresses.ContainsKey(peer.Id))
                    {
                        RealIpAddresses[peer.Id] = forwardedIp;
                    }
                    else
                    {
                        RealIpAddresses.Add(peer.Id, forwardedIp);
                    }
                }

                if (Statistics.PeakPlayers.Total < LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count)
                {
                    Statistics.PeakPlayers = new Statistics.Peak(LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count, DateTime.Now);
                }

                ServerConsole.AddLog($"Player {userId} preauthenticated from endpoint {endpointText}.");
                ServerLogs.AddLog(ServerLogs.Modules.Networking, $"{userId} preauthenticated from endpoint {endpointText}.", ServerLogs.ServerLogType.ConnectionUpdate);
                PreauthDisableIdleMode();
            }
            else
            {
                RequestWriter.Reset();
                RequestWriter.Put((byte)RejectionReason.ServerFull);
                request.Reject(RequestWriter);
                ResetIdleMode();
            }
        }
        catch (Exception ex)
        {
            Rejected++;
            if (Rejected > RejectionThreshold)
            {
                SuppressRejections = true;
            }
            if (!SuppressRejections && DisplayPreauthLogs)
            {
                ServerConsole.AddLog($"Player from endpoint {endpointText} sent an invalid preauthentication token. {ex.Message}");
            }
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.InvalidToken);
            request.Reject(RequestWriter);
            ResetIdleMode();
        }
    }

    private static bool ProcessCancellationData(ConnectionRequest request, PreauthCancellationData pcd)
    {
        if (!pcd.IsCancelled)
        {
            return true;
        }
        if (DisplayPreauthLogs)
        {
            ServerConsole.AddLog($"Incoming connection from {request.RemoteEndPoint} rejected by a plugin.");
        }
        bool forced;
        NetDataWriter writer = pcd.GenerateWriter(out forced);
        if (writer == null)
        {
            return false;
        }
        if (forced)
        {
            request.Reject(writer);
        }
        else
        {
            request.Reject(writer);
        }
        return false;
    }

    private static bool CheckIpRateLimit(ConnectionRequest request)
    {
        if (IpRateLimit.Contains(request.RemoteEndPoint.Address))
        {
            Rejected++;
            if (Rejected > RejectionThreshold)
            {
                SuppressRejections = true;
            }
            if (!SuppressRejections)
            {
                ServerConsole.AddLog($"Incoming connection from endpoint {request.RemoteEndPoint} rejected due to exceeding the rate limit.");
                ServerLogs.AddLog(ServerLogs.Modules.Networking, $"Incoming connection from endpoint {request.RemoteEndPoint} rejected due to exceeding the rate limit.", ServerLogs.ServerLogType.RateLimit);
            }
            RequestWriter.Reset();
            RequestWriter.Put((byte)RejectionReason.RateLimit);
            request.Reject(RequestWriter);
            return false;
        }
        IpRateLimit.Add(request.RemoteEndPoint.Address);
        return true;
    }

    protected internal override void GetConnectData(NetDataWriter writer)
    {
        writer.Put((byte)ClientType.GameClient);
        writer.Put(Version.Major);
        writer.Put(Version.Minor);
        writer.Put(Version.Revision);
        writer.Put(Version.BackwardCompatibility);

        if (Version.BackwardCompatibility)
            writer.Put(Version.BackwardRevision);

        writer.Put(clientChallengeId);

        writer.PutBytesWithLength(clientChallengeResponse ?? EmptyByte);

        CentralAuthPreauthToken? token = CentralAuthManager.PreauthToken;
        if (token is not null)
        {
            writer.Put(token.Value.UserId);
            writer.Put(token.Value.Expiration);
            writer.Put((byte)token.Value.Flags);
            writer.Put(token.Value.Country ?? string.Empty);

            byte[] signatureBytes = Convert.FromBase64String(token.Value.Signature);
            writer.PutBytesWithLength(signatureBytes);
        }
        else
        {
            writer.Put(string.Empty);
        }
    }

    protected internal override void OnConncetionRefused(DisconnectInfo disconnectinfo)
    {
        if (!disconnectinfo.AdditionalData.TryGetByte(out byte reason))
        {
            LastRejectionReason = RejectionReason.NotSpecified;
            LastCustomReason = string.Empty;
            LastBanExpiration = 0;
            return;
        }

        LastRejectionReason = (RejectionReason)reason;

        if (LastRejectionReason == RejectionReason.Banned)
        {
            if (disconnectinfo.AdditionalData.TryGetLong(out long expiration))
            {
                LastBanExpiration = expiration;
            }
            else
            {
                LastBanExpiration = 0;
            }

            if (disconnectinfo.AdditionalData.TryGetString(out string customReason))
            {
                LastCustomReason = customReason;
            }
            else
            {
                LastCustomReason = string.Empty;
            }
            return;
        }

        if (LastRejectionReason == RejectionReason.Custom)
        {
            if (disconnectinfo.AdditionalData.TryGetString(out string custom))
            {
                LastCustomReason = custom;
            }
            else
            {
                LastCustomReason = string.Empty;
            }
            return;
        }

        if (LastRejectionReason == RejectionReason.Redirect)
        {
            if (disconnectinfo.AdditionalData.TryGetUShort(out ushort port))
            {
                Singleton.reconnectDelay = 600;
                Singleton.maxConnectAttempts = 3;
                _redirectCounter++;
                GameCore.Console.AddLog($"Connection has been redirected to port {port} (Redirect handshake code)" + Singleton.ToString(), Color.gray);
                return;
            }
            else
            {
                GameCore.Console.AddLog("Connection has been redirected but no port was provided.", Color.gray);
                _redirectCounter = 0;
                LastRejectionReason = RejectionReason.NotSpecified;
                return;
            }
        }

        if (LastRejectionReason == RejectionReason.Delay)
        {
            if (disconnectinfo.AdditionalData.TryGetByte(out byte delay))
            {
                GameCore.Console.AddLog($"Connection has been delayed by {delay} seconds.", Color.gray);
                if (delay > 25)
                {
                    GameCore.Console.AddLog("Connection delay exceeds 25 seconds. Checking connection...", Color.gray);
                }
                CustomNetworkManager.triggerReconnectTime = delay;
                RoundRestarting.RoundRestart.ChangeLevel(true);
                return;
            }
            return;
        }

        if (LastRejectionReason == RejectionReason.Challenge)
        {
            if (disconnectinfo.AdditionalData.TryGetByte(out byte type))
            {
                clientChallengeType = (ChallengeType)type;
                if (disconnectinfo.AdditionalData.TryGetInt(out int id))
                {
                    clientChallengeId = id;
                }
                if (clientChallengeType == ChallengeType.Reply)
                {
                    if (disconnectinfo.AdditionalData.TryGetBytesWithLength(out byte[] challenge))
                    {
                        clientChallenge = challenge;
                        ClientChallengeState = ChallengeState.Processing;
                        return;
                    }
                }
                else if (clientChallengeType == ChallengeType.MD5 || clientChallengeType == ChallengeType.SHA1)
                {
                    if (disconnectinfo.AdditionalData.TryGetBytesWithLength(out byte[] baseChallenge) &&
                        disconnectinfo.AdditionalData.TryGetUShort(out ushort secretLen) &&
                        disconnectinfo.AdditionalData.TryGetBytesWithLength(out byte[] hashed))
                    {
                        clientChallengeBase = baseChallenge;
                        clientChallengeSecretLen = secretLen;
                        clientChallenge = hashed;
                        ClientChallengeState = ChallengeState.Done;
                        return;
                    }
                }
            }
            LastRejectionReason = RejectionReason.Error;
            return;
        }
    }

    internal static void ResetRedirectCounter()
    {
        _redirectCounter = 0;
    }

    private static void ProcessChallenge()
    {
        Thread.Sleep(250);
        if (clientChallengeBase == null || clientChallenge == null)
        {
            Debug.Log("No challenge data available.");
            return;
        }

        byte[] fullBuffer = new byte[clientChallengeBase.Length + clientChallengeSecretLen];
        Array.Copy(clientChallengeBase, fullBuffer, clientChallengeBase.Length);

        Debug.Log("Processing server challenge...");

        bool all255 = true;
        for (int i = clientChallengeBase.Length; i < fullBuffer.Length; i++)
        {
            fullBuffer[i] = (byte)UnityEngine.Random.Range(0, 256);
            if (fullBuffer[i] != 255)
            {
                all255 = false;
            }
        }

        Debug.Log("PROC: " + BitConverter.ToString(fullBuffer).Replace("-", ""));

        if (all255)
        {
            Debug.Log("Failed to process server challenge - allChecked.");
            return;
        }

        byte[] hashedResponse;
        if (clientChallengeType == ChallengeType.MD5)
        {
            hashedResponse = Md.Md5(fullBuffer);
        }
        else if (clientChallengeType == ChallengeType.SHA1)
        {
            hashedResponse = Sha.Sha1(fullBuffer);
        }
        else
        {
            hashedResponse = fullBuffer;
        }

        if (!Enumerable.SequenceEqual(hashedResponse, clientChallenge))
        {
            Debug.Log("Failed to process server challenge.");
            return;
        }

        Debug.Log("Server challenge processed.");
        clientChallengeResponse = new byte[clientChallengeSecretLen];
        Array.Copy(fullBuffer, clientChallengeBase.Length, clientChallengeResponse, 0, clientChallengeSecretLen);
        ClientChallengeState = ChallengeState.Done;
    }

    private static void ResetIdleMode()
    {
        if (LiteNetLib4MirrorCore.Host.ConnectedPeerList.Count == 0)
        {
            IdleMode.SetIdleMode(true);
        }
    }

    public static void PreauthDisableIdleMode()
    {
        IdleMode.PreauthStopwatch.Restart();
        IdleMode.SetIdleMode(false);
    }

    public static void ReloadChallengeOptions()
    {
        UseChallenge = ConfigFile.ServerConfig.GetBool("preauth_challenge", true);
        ChallengeInitLen = ConfigFile.ServerConfig.GetUShort("preauth_challenge_base_length", 16);
        ChallengeSecretLen = ConfigFile.ServerConfig.GetUShort("preauth_challenge_secret_length", 5);
        string modeStr = ConfigFile.ServerConfig.GetString("preauth_challenge_mode", "reply").ToLower();
        if (modeStr == "md5")
        {
            ChallengeMode = ChallengeType.MD5;
        }
        else if (modeStr == "sha1")
        {
            ChallengeMode = ChallengeType.SHA1;
        }
        else
        {
            ChallengeMode = ChallengeType.Reply;
            ChallengeSecretLen = 0;
        }
    }
}