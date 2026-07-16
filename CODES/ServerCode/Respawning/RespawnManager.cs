namespace Respawning
{
	public class RespawnManager : global::UnityEngine.MonoBehaviour
	{
		public enum RespawnSequencePhase : byte
		{
			RespawnCooldown = 0,
			SelectingTeam = 1,
			PlayingEntryAnimations = 2,
			SpawningSelectedTeam = 3
		}

		public static readonly global::System.Collections.Generic.Dictionary<global::Respawning.SpawnableTeamType, global::Respawning.SpawnableTeamHandlerBase> SpawnableTeams = new global::System.Collections.Generic.Dictionary<global::Respawning.SpawnableTeamType, global::Respawning.SpawnableTeamHandlerBase>
		{
			[global::Respawning.SpawnableTeamType.ChaosInsurgency] = new global::Respawning.ChaosInsurgencySpawnHandler("maximum_CI_respawn_amount", 15, "respawn_tickets_ci_initial_count", 18),
			[global::Respawning.SpawnableTeamType.NineTailedFox] = new global::Respawning.NineTailedFoxSpawnHandler("maximum_MTF_respawn_amount", 15, "respawn_tickets_mtf_initial_count", 24)
		};

		public static global::Respawning.RespawnManager Singleton;

		public global::Respawning.SpawnableTeamType NextKnownTeam;

		public global::Respawning.NamingRules.UnitNamingHud NamingManager;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		private bool _prioritySpawn;

		private global::Respawning.RespawnManager.RespawnSequencePhase _curSequence;

		private float _timeForNextSequence;

		private bool _started;

		public int TimeTillRespawn => global::UnityEngine.Mathf.RoundToInt(Singleton._timeForNextSequence - (float)Singleton._stopwatch.Elapsed.TotalSeconds);

		public static event global::System.Action<global::Respawning.SpawnableTeamType, global::System.Collections.Generic.List<ReferenceHub>> ServerOnRespawned;

		private void Awake()
		{
			Singleton = this;
		}

		private void Start()
		{
			_prioritySpawn = global::GameCore.ConfigFile.ServerConfig.GetBool("priority_mtf_respawn", def: true);
		}

		public static global::Respawning.RespawnManager.RespawnSequencePhase CurrentSequence()
		{
			return Singleton._curSequence;
		}

		private bool ReadyToCommence()
		{
			if (_started)
			{
				return true;
			}
			if (global::GameCore.RoundStart.singleton.Timer == -1)
			{
				RestartSequence();
				_started = true;
			}
			return _started;
		}

		public static string GetRemoteAdminInfoString()
		{
			global::System.Text.StringBuilder stringBuilder = global::NorthwoodLib.Pools.StringBuilderPool.Shared.Rent(64);
			if (!Singleton._started)
			{
				stringBuilder.Append("The respawn system is off (or waiting for the round to start).");
			}
			else
			{
				int num = Singleton.TimeTillRespawn;
				int num2 = 0;
				while (num >= 60)
				{
					num -= 60;
					num2++;
				}
				switch (Singleton._curSequence)
				{
				case global::Respawning.RespawnManager.RespawnSequencePhase.RespawnCooldown:
					stringBuilder.Append("Next team selection in ");
					break;
				case global::Respawning.RespawnManager.RespawnSequencePhase.PlayingEntryAnimations:
					stringBuilder.Append("The selected team (");
					stringBuilder.Append(Singleton.NextKnownTeam);
					stringBuilder.Append(") will arrive in ");
					break;
				default:
					stringBuilder.Append("Respawn Manager reports the code ");
					stringBuilder.Append((int)Singleton._curSequence);
					stringBuilder.Append(" status. Try again in ");
					break;
				}
				stringBuilder.Append(num2);
				stringBuilder.Append("m ");
				stringBuilder.Append(num);
				stringBuilder.Append("s");
			}
			string result = stringBuilder.ToString();
			global::NorthwoodLib.Pools.StringBuilderPool.Shared.Return(stringBuilder);
			return result;
		}

		private void RestartSequence()
		{
			_timeForNextSequence = global::UnityEngine.Random.Range(global::GameCore.ConfigFile.ServerConfig.GetFloat("minimum_MTF_time_to_spawn", 280f), global::GameCore.ConfigFile.ServerConfig.GetFloat("maximum_MTF_time_to_spawn", 350f));
			_curSequence = global::Respawning.RespawnManager.RespawnSequencePhase.RespawnCooldown;
			if (_stopwatch.IsRunning)
			{
				_stopwatch.Restart();
			}
			else
			{
				_stopwatch.Start();
			}
		}

		private bool CheckSpawnable(ReferenceHub ply)
		{
			if (ply.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole spectatorRole)
			{
				return spectatorRole.ReadyToRespawn;
			}
			return false;
		}

		private void Update()
		{
			if (!global::Mirror.NetworkServer.active || !ReadyToCommence())
			{
				return;
			}
			if (_stopwatch.Elapsed.TotalSeconds > (double)_timeForNextSequence)
			{
				_curSequence++;
			}
			if (_curSequence == global::Respawning.RespawnManager.RespawnSequencePhase.SelectingTeam)
			{
				if (!global::Utils.NonAllocLINQ.HashsetExtensions.Any(ReferenceHub.AllHubs, CheckSpawnable))
				{
					RestartSequence();
					return;
				}
				global::Respawning.SpawnableTeamType dominatingTeam = global::Respawning.RespawnTokensManager.DominatingTeam;
				if (!SpawnableTeams.TryGetValue(dominatingTeam, out var value))
				{
					throw new global::System.NotImplementedException($"{dominatingTeam} was returned as dominating team despite not being implemented.");
				}
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.TeamRespawnSelected, dominatingTeam))
				{
					RestartSequence();
					return;
				}
				NextKnownTeam = dominatingTeam;
				_curSequence = global::Respawning.RespawnManager.RespawnSequencePhase.PlayingEntryAnimations;
				_stopwatch.Restart();
				_timeForNextSequence = value.EffectTime;
				global::Respawning.RespawnEffectsController.ExecuteAllEffects(global::Respawning.RespawnEffectsController.EffectType.Selection, dominatingTeam);
			}
			if (_curSequence == global::Respawning.RespawnManager.RespawnSequencePhase.SpawningSelectedTeam)
			{
				Spawn();
				RestartSequence();
			}
		}

		public void ForceSpawnTeam(global::Respawning.SpawnableTeamType teamToSpawn)
		{
			NextKnownTeam = teamToSpawn;
			Spawn();
			RestartSequence();
		}

		public void Spawn()
		{
			if (!SpawnableTeams.TryGetValue(NextKnownTeam, out var value) || NextKnownTeam == global::Respawning.SpawnableTeamType.None)
			{
				ServerConsole.AddLog(string.Concat("Fatal error. Team '", NextKnownTeam, "' is undefined."), global::System.ConsoleColor.Red);
				return;
			}
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.TeamRespawn, NextKnownTeam))
			{
				global::Respawning.RespawnEffectsController.ExecuteAllEffects(global::Respawning.RespawnEffectsController.EffectType.UponRespawn, NextKnownTeam);
				NextKnownTeam = global::Respawning.SpawnableTeamType.None;
				return;
			}
			global::System.Collections.Generic.List<ReferenceHub> list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(ReferenceHub.AllHubs, CheckSpawnable));
			if (_prioritySpawn)
			{
				list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.OrderByDescending(list, (ReferenceHub item) => item.roleManager.CurrentRole.ActiveTime));
			}
			else
			{
				list.ShuffleList();
			}
			int maxWaveSize = value.MaxWaveSize;
			int num = list.Count;
			if (num > maxWaveSize)
			{
				list.RemoveRange(maxWaveSize, num - maxWaveSize);
				num = maxWaveSize;
			}
			if (num > 0 && global::Respawning.NamingRules.UnitNamingRule.TryGetNamingRule(NextKnownTeam, out var rule))
			{
				global::Respawning.NamingRules.UnitNameMessageHandler.SendNew(NextKnownTeam, rule);
			}
			list.ShuffleList();
			global::System.Collections.Generic.List<ReferenceHub> list2 = global::NorthwoodLib.Pools.ListPool<ReferenceHub>.Shared.Rent();
			global::System.Collections.Generic.Queue<global::PlayerRoles.RoleTypeId> queue = new global::System.Collections.Generic.Queue<global::PlayerRoles.RoleTypeId>();
			value.GenerateQueue(queue, list.Count);
			foreach (ReferenceHub item in list)
			{
				try
				{
					global::PlayerRoles.RoleTypeId newRole = queue.Dequeue();
					item.roleManager.ServerSetRole(newRole, global::PlayerRoles.RoleChangeReason.Respawn);
					list2.Add(item);
					ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Player " + item.LoggedNameFromRefHub() + " respawned as " + newRole.ToString() + ".", ServerLogs.ServerLogType.GameEvent);
				}
				catch (global::System.Exception ex)
				{
					if (item != null)
					{
						ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Player " + item.LoggedNameFromRefHub() + " couldn't be spawned. Err msg: " + ex.Message, ServerLogs.ServerLogType.GameEvent);
					}
					else
					{
						ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "Couldn't spawn a player - target's ReferenceHub is null.", ServerLogs.ServerLogType.GameEvent);
					}
				}
			}
			if (list2.Count > 0)
			{
				ServerLogs.AddLog(ServerLogs.Modules.ClassChange, "RespawnManager has successfully spawned " + list2.Count + " players as " + NextKnownTeam.ToString() + "!", ServerLogs.ServerLogType.GameEvent);
				global::Respawning.RespawnEffectsController.ExecuteAllEffects(global::Respawning.RespawnEffectsController.EffectType.UponRespawn, NextKnownTeam);
			}
			global::Respawning.RespawnManager.ServerOnRespawned?.Invoke(NextKnownTeam, list2);
			global::NorthwoodLib.Pools.ListPool<ReferenceHub>.Shared.Return(list2);
			NextKnownTeam = global::Respawning.SpawnableTeamType.None;
		}
	}
}
