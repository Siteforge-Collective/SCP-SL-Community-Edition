namespace RoundRestarting
{
	public static class RoundRestart
	{
		private const string RoundrestartTimeKey = "LastRoundrestartTime";

		private static global::System.DateTime _lastRestartTime;

		public static bool IsRoundRestarting { get; private set; }

		public static int UptimeRounds { get; private set; }

		private static int LastRestartTime => PlayerPrefsSl.Get("LastRoundrestartTime", 5000);

		public static event global::System.Action OnRestartTriggered;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Inventory.OnServerStarted += OnServerStarted;
			global::InventorySystem.Inventory.OnLocalClientStarted += OnClientStarted;
		}

		private static void OnMessageReceived(global::RoundRestarting.RoundRestartMessage msg)
		{
		}

		private static void OnClientStarted()
		{
			IsRoundRestarting = false;
			global::Mirror.NetworkClient.RegisterHandler<global::RoundRestarting.RoundRestartMessage>(OnMessageReceived);
		}

		private static void OnServerStarted()
		{
			global::System.TimeSpan timeSpan = global::System.DateTime.Now - _lastRestartTime;
			if (!(timeSpan.TotalSeconds > 20.0))
			{
				PlayerPrefsSl.Set("LastRoundrestartTime", (LastRestartTime + (int)timeSpan.TotalMilliseconds) / 2);
			}
		}

		public static void InitiateRoundRestart()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Round restart can only be triggerred by the server!");
			}
			global::PluginAPI.Core.Facility.Reset();
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.RoundRestart);
			global::GameObjectPools.PoolManager.Singleton.ReturnAllPoolObjects();
			if (IsRoundRestarting)
			{
				return;
			}
			IsRoundRestarting = true;
			CustomLiteNetLib4MirrorTransport.DelayConnections = true;
			CustomLiteNetLib4MirrorTransport.UserIdFastReload.Clear();
			IdleMode.PauseIdleMode = true;
			if (CustomNetworkManager.EnableFastRestart)
			{
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					if (allHub.Mode != ClientInstanceMode.DedicatedServer)
					{
						try
						{
							CustomLiteNetLib4MirrorTransport.UserIdFastReload.Add(allHub.characterClassManager.UserId);
						}
						catch (global::System.Exception ex)
						{
							ServerConsole.AddLog("Exception occured during processing online player list for Fast Restart: " + ex.Message, global::System.ConsoleColor.Yellow);
						}
					}
				}
				global::Mirror.NetworkServer.SendToAll(new global::RoundRestarting.RoundRestartMessage(global::RoundRestarting.RoundRestartType.FastRestart, 0f, 0, reconnect: true, extendedReconnectionPeriod: true));
				ChangeLevel(noShutdownMessage: false);
			}
			else
			{
				if (ServerStatic.StopNextRound == ServerStatic.NextRoundAction.DoNothing)
				{
					float offset = (float)LastRestartTime / 1000f;
					global::Mirror.NetworkServer.SendToAll(new global::RoundRestarting.RoundRestartMessage(global::RoundRestarting.RoundRestartType.FullRestart, offset, 0, reconnect: true, extendedReconnectionPeriod: true));
				}
				ChangeLevel(noShutdownMessage: false);
			}
		}

		internal static void ChangeLevel(bool noShutdownMessage)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::Mirror.NetworkManager.singleton.StopClient();
				return;
			}
			IdleMode.PauseIdleMode = true;
			bool flag = false;
			global::RoundRestarting.RoundRestart.OnRestartTriggered?.Invoke();
			try
			{
				int num = global::GameCore.ConfigFile.ServerConfig.GetInt("restart_after_rounds");
				flag = num > 0 && UptimeRounds >= num;
			}
			catch (global::System.Exception ex)
			{
				ServerConsole.AddLog("Failed to check the restart_after_rounds config value: " + ex.Message, global::System.ConsoleColor.Red);
			}
			switch ((byte)ServerStatic.StopNextRound)
			{
			case 0:
				if (!flag)
				{
					global::System.GC.Collect();
					_lastRestartTime = global::System.DateTime.Now;
					UptimeRounds++;
					global::Mirror.NetworkManager.singleton.ServerChangeScene(global::Mirror.NetworkManager.singleton.onlineScene);
					break;
				}
				goto case 1;
			case 1:
			{
				ServerShutdown.ShutdownState = ServerShutdown.ServerShutdownState.Complete;
				ServerConsole.AddOutputEntry(default(global::ServerOutput.ExitActionRestartEntry));
				if (!noShutdownMessage)
				{
					ServerConsole.AddLog(flag ? "Restarting the server (rounds limit set in the server config exceeded)..." : "Restarting the server (RestartNextRound command was used)...");
				}
				float offset = global::GameCore.ConfigFile.ServerConfig.GetInt("full_restart_rejoin_time", 25);
				global::Mirror.NetworkServer.SendToAll(new global::RoundRestarting.RoundRestartMessage(global::RoundRestarting.RoundRestartType.FullRestart, offset, 0, reconnect: true, extendedReconnectionPeriod: true));
				Shutdown.Quit();
				break;
			}
			case 2:
				ServerConsole.AddOutputEntry(default(global::ServerOutput.ExitActionShutdownEntry));
				if (!noShutdownMessage)
				{
					ServerConsole.AddLog("Shutting down the server (StopNextRound command was used)...");
				}
				global::Mirror.NetworkServer.SendToAll(new global::RoundRestarting.RoundRestartMessage(global::RoundRestarting.RoundRestartType.FullRestart, 0f, 0, reconnect: false, extendedReconnectionPeriod: false));
				Shutdown.Quit();
				break;
			default:
				throw new global::System.ArgumentOutOfRangeException();
			}
		}
	}
}
