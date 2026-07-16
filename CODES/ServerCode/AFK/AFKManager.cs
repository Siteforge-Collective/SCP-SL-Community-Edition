namespace AFK
{
	public static class AFKManager
	{
		public delegate void AFKKick(ReferenceHub userHub);

		private static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::System.Diagnostics.Stopwatch> AFKTimers = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::System.Diagnostics.Stopwatch>();

		private static float _kickTime;

		private static bool _constantlyCheck;

		private static string _kickMessage;

		private static bool _eventsStatus;

		public static event global::AFK.AFKManager.AFKKick OnAFKKick;

		[global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void Init()
		{
			global::GameCore.ConfigFile.OnConfigReloaded = (global::System.Action)global::System.Delegate.Combine(global::GameCore.ConfigFile.OnConfigReloaded, new global::System.Action(ConfigReloaded));
		}

		private static void ConfigReloaded()
		{
			_constantlyCheck = global::GameCore.ConfigFile.ServerConfig.GetBool("constantly_check_afk");
			_kickTime = global::GameCore.ConfigFile.ServerConfig.GetFloat("afk_time", 90f);
			_kickMessage = global::GameCore.ConfigFile.ServerConfig.GetString("afk_kick_message", "AFK");
			if (_kickTime <= 0f)
			{
				if (_eventsStatus)
				{
					_eventsStatus = false;
					ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Remove(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(AddPlayer));
					ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Remove(ReferenceHub.OnPlayerRemoved, new global::System.Action<ReferenceHub>(RemovePlayer));
					StaticUnityMethods.OnUpdate -= OnUpdate;
					global::PlayerRoles.PlayerRoleManager.OnRoleChanged -= RoleChange;
				}
			}
			else if (!_eventsStatus)
			{
				_eventsStatus = true;
				ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(AddPlayer));
				ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, new global::System.Action<ReferenceHub>(RemovePlayer));
				StaticUnityMethods.OnUpdate += OnUpdate;
				global::PlayerRoles.PlayerRoleManager.OnRoleChanged += RoleChange;
			}
		}

		public static void AddPlayer(ReferenceHub hub)
		{
			if (global::Mirror.NetworkServer.active && !(hub == ReferenceHub.HostHub) && !AFKTimers.ContainsKey(hub) && !PermissionsHandler.IsPermitted(hub.serverRoles.Permissions, PlayerPermissions.AFKImmunity))
			{
				AFKTimers.Add(hub, global::System.Diagnostics.Stopwatch.StartNew());
			}
		}

		private static void RemovePlayer(ReferenceHub hub)
		{
			AFKTimers.Remove(hub);
		}

		private static void RoleChange(ReferenceHub hub, global::PlayerRoles.PlayerRoleBase oldRole, global::PlayerRoles.PlayerRoleBase newRole)
		{
			if (global::Mirror.NetworkServer.active && AFKTimers.TryGetValue(hub, out var value))
			{
				if (PermissionsHandler.IsPermitted(hub.serverRoles.Permissions, PlayerPermissions.AFKImmunity) || hub == ReferenceHub.HostHub)
				{
					AFKTimers.Remove(hub);
				}
				else
				{
					value.Restart();
				}
			}
		}

		private static void OnUpdate()
		{
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (!AFKTimers.TryGetValue(allHub, out var value) || !value.IsRunning || !(allHub.roleManager.CurrentRole is global::PlayerRoles.IAFKRole iAFKRole))
				{
					continue;
				}
				if (!iAFKRole.IsAFK)
				{
					if (_constantlyCheck)
					{
						value.Restart();
					}
					else
					{
						value.Reset();
					}
				}
				else if (value.Elapsed.TotalSeconds >= (double)_kickTime)
				{
					value.Reset();
					global::AFK.AFKManager.OnAFKKick?.Invoke(allHub);
					BanPlayer.KickUser(allHub, _kickMessage);
				}
			}
		}
	}
}
