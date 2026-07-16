using CentralAuth;
using GameCore;
using MEC;
using Mirror;
using Mirror.LiteNetLib4Mirror;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Events;
using Security;
using ServerOutput;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.NonAllocLINQ;

public class CharacterClassManager : NetworkBehaviour
{
    private ReferenceHub _hub;

    [NonSerialized]
    private bool _godMode;

    private bool _wasAnytimeAlive;

    internal static bool EnableSyncServerCmdBinding;

    [SyncVar]
    public string Pastebin;

    [SyncVar(hook = nameof(MaxPlayersHook))]
    public ushort MaxPlayers;

    internal static bool CuffedChangeTeam;

    [SyncVar]
    public bool RoundStarted;

    private RateLimit _commandRateLimit;

    public bool GodMode
    {
        get => _godMode;
        set
        {
            _godMode = value;
            _hub.playerStats.GetModule<AdminFlagsStat>().SetFlag(AdminFlags.GodMode, value);
        }
    }

    public static event Action OnRoundStarted;
    public static event Action<ushort> OnMaxPlayersChanged;

    private void Start()
    {
        _hub = ReferenceHub.GetHub(this);

        if (!NetworkServer.active) return;

        _commandRateLimit = _hub.playerRateLimitHandler.RateLimits[1];

        if (base.isLocalPlayer)
        {
            ServerLogs.StartLogging();
            FriendlyFireConfig.PauseDetector = false;
            CustomLiteNetLib4MirrorTransport.DelayConnections = false;
            IdleMode.PauseIdleMode = false;

            ServerConsole.AddOutputEntry(default(RoundRestartedEntry));
            Pastebin = ConfigFile.ServerConfig.GetString("serverinfo_pastebin_id", "");

            if (ServerStatic.IsDedicated)
            {
                ServerConsole.AddLog("Waiting for players...", ConsoleColor.Gray);
            }

            if (RoundStart.singleton != null)
                RoundStart.singleton.ShowButton();

            StartCoroutine(Init());
        }
    }

    private void Update()
    {
        if (base.isLocalPlayer && NetworkServer.active)
        {
            MaxPlayers = (ushort)CustomNetworkManager.slots;
        }
    }

    private void MaxPlayersHook(ushort prev, ushort cur)
    {
        if (prev != cur)
        {
            OnMaxPlayersChanged?.Invoke(cur);
        }
    }

    public void SyncServerCmdBinding()
    {
        if (!base.isServer || !EnableSyncServerCmdBinding) return;

        foreach (CmdBinding.Bind binding in CmdBinding.Bindings)
        {
            if (binding.command.StartsWith(".") || binding.command.StartsWith("/"))
            {
                TargetChangeCmdBinding(base.connectionToClient, binding.key, binding.command);
            }
        }
    }

    [TargetRpc]
    public void TargetChangeCmdBinding(NetworkConnection conn, KeyCode code, string cmd)
    {
        CmdBinding.ChangeKeybinding(code, cmd);
    }

    private IEnumerator Init()
    {
        if (NonFacilityCompatibility.currentSceneSettings.roundAutostart)
        {
            ForceRoundStart();
        }
        else
        {
            short originalTimeLeft = ConfigFile.ServerConfig.GetShort("lobby_waiting_time", 20);
            short timeLeft = originalTimeLeft;
            int topPlayers = 2;

            while (RoundStart.singleton.Timer != -1)
            {
                if (timeLeft == -2) timeLeft = originalTimeLeft;

                int readyPlayers = ReferenceHub.AllHubs.Count(x => x.Mode == ClientInstanceMode.ReadyClient);

                if (!RoundStart.LobbyLock && readyPlayers > 1)
                {
                    if (readyPlayers > topPlayers)
                    {
                        topPlayers = readyPlayers;
                        if (timeLeft < originalTimeLeft)
                        {
                            do { timeLeft++; } while (timeLeft % 5 == 0 && timeLeft < originalTimeLeft);
                        }
                    }
                    else
                    {
                        timeLeft--;
                    }

                    if (readyPlayers >= ((CustomNetworkManager)NetworkManager.singleton).ReservedMaxPlayers)
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

                if (RoundStart.singleton.Timer != -1)
                {
                    RoundStart.singleton.Timer = timeLeft;
                }

                yield return new WaitForSeconds(1f);
            }
        }

        RoundStarted = true;
        RpcRoundStarted();
    }

    public static bool ForceRoundStart()
    {
        if (!NetworkServer.active) return false;

        ServerLogs.AddLog(ServerLogs.Modules.Logger, "Round has been started.", ServerLogs.ServerLogType.GameEvent);
        ServerConsole.AddLog("New round has been started.", ConsoleColor.Gray);

        RoundStart.singleton.Timer = -1;
        RoundStart.RoundStartTimer.Restart();
        return true;
    }

    [TargetRpc]
    private void TargetSetDisconnectError(NetworkConnection conn, string message)
    {
        if (LiteNetLib4MirrorNetworkManager.singleton is CustomNetworkManager manager)
        {
            manager.disconnectMessage = message;
        }
        CmdConfirmDisconnect();
    }

    [Command(channel = 4)]
    private void CmdConfirmDisconnect()
    {
        base.connectionToClient?.Disconnect();
    }

    public void DisconnectClient(NetworkConnection conn, string message)
    {
        TargetSetDisconnectError(conn, message);
        Timing.RunCoroutine(_DisconnectAfterTimeout(conn), Segment.FixedUpdate);
    }

    private static IEnumerator<float> _DisconnectAfterTimeout(NetworkConnection conn)
    {
        for (int i = 0; i < 150; i++)
        {
            yield return Timing.WaitForOneFrame;
        }
        conn?.Disconnect();
    }

    [ClientRpc]
    private void RpcRoundStarted()
    {
        OnRoundStarted?.Invoke();
    }
}