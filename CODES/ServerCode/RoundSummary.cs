public class RoundSummary : global::Mirror.NetworkBehaviour
{
	public enum LeadingTeam : byte
	{
		FacilityForces = 0,
		ChaosInsurgency = 1,
		Anomalies = 2,
		Draw = 3
	}

	[global::System.Serializable]
	public struct SumInfo_ClassList : global::System.IEquatable<RoundSummary.SumInfo_ClassList>
	{
		public int class_ds;

		public int scientists;

		public int chaos_insurgents;

		public int mtf_and_guards;

		public int scps_except_zombies;

		public int zombies;

		public int warhead_kills;

		public bool Equals(RoundSummary.SumInfo_ClassList other)
		{
			if (class_ds == other.class_ds && scientists == other.scientists && chaos_insurgents == other.chaos_insurgents && mtf_and_guards == other.mtf_and_guards && scps_except_zombies == other.scps_except_zombies && zombies == other.zombies)
			{
				return warhead_kills == other.warhead_kills;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is RoundSummary.SumInfo_ClassList other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((((((((class_ds * 397) ^ scientists) * 397) ^ chaos_insurgents) * 397) ^ mtf_and_guards) * 397) ^ scps_except_zombies) * 397) ^ zombies) * 397) ^ warhead_kills;
		}

		public static bool operator ==(RoundSummary.SumInfo_ClassList left, RoundSummary.SumInfo_ClassList right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(RoundSummary.SumInfo_ClassList left, RoundSummary.SumInfo_ClassList right)
		{
			return !left.Equals(right);
		}
	}

	private bool _roundEnded;

	private bool _summaryActive;

	public bool KeepRoundOnOne;

	public RoundSummary.SumInfo_ClassList classlistStart;

	public global::UnityEngine.GameObject ui_root;

	public static bool RoundLock;

	public static RoundSummary singleton;

	private static bool _singletonSet;

	public static int roundTime;

	public static bool SummaryActive
	{
		get
		{
			if (_singletonSet)
			{
				return singleton._summaryActive;
			}
			return false;
		}
	}

	public static int Kills { get; private set; }

	public static int EscapedClassD { get; private set; }

	public static int EscapedScientists { get; private set; }

	public static int SurvivingSCPs { get; private set; }

	public static int KilledBySCPs { get; private set; }

	public static int ChangedIntoZombies { get; private set; }

	private void Start()
	{
		singleton = this;
		_singletonSet = true;
		if (global::Mirror.NetworkServer.active)
		{
			roundTime = 0;
			KeepRoundOnOne = !global::GameCore.ConfigFile.ServerConfig.GetBool("end_round_on_one_player");
			global::MEC.Timing.RunCoroutine(_ProcessServerSideCode(), global::MEC.Segment.FixedUpdate);
			KilledBySCPs = 0;
			EscapedClassD = 0;
			EscapedScientists = 0;
			ChangedIntoZombies = 0;
			Kills = 0;
			global::PlayerRoles.PlayerRoleManager.OnServerRoleSet += OnRoleChanged;
			global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied += OnAnyPlayerDied;
		}
	}

	private void OnDestroy()
	{
		_singletonSet = false;
		global::PlayerRoles.PlayerRoleManager.OnServerRoleSet -= OnRoleChanged;
		global::PlayerStatsSystem.PlayerStats.OnAnyPlayerDied -= OnAnyPlayerDied;
	}

	private void OnAnyPlayerDied(ReferenceHub ply, global::PlayerStatsSystem.DamageHandlerBase handler)
	{
		Kills++;
		global::PlayerRoles.PlayerRoleBase result;
		if (handler is global::PlayerStatsSystem.UniversalDamageHandler universalDamageHandler)
		{
			if (universalDamageHandler.TranslationId != global::PlayerStatsSystem.DeathTranslations.PocketDecay.Id)
			{
				return;
			}
		}
		else if (!(handler is global::PlayerStatsSystem.AttackerDamageHandler attackerDamageHandler) || !global::PlayerRoles.PlayerRoleLoader.TryGetRoleTemplate<global::PlayerRoles.PlayerRoleBase>(attackerDamageHandler.Attacker.Role, out result) || result.Team != global::PlayerRoles.Team.SCPs)
		{
			return;
		}
		KilledBySCPs++;
	}

	private void OnRoleChanged(ReferenceHub userHub, global::PlayerRoles.RoleTypeId newRole, global::PlayerRoles.RoleChangeReason reason)
	{
		switch (reason)
		{
		case global::PlayerRoles.RoleChangeReason.RoundStart:
		case global::PlayerRoles.RoleChangeReason.LateJoin:
			AddSpawnedTeam(global::PlayerRoles.PlayerRolesUtils.GetTeam(newRole));
			break;
		case global::PlayerRoles.RoleChangeReason.Escaped:
			if (!global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(userHub.inventory))
			{
				switch (global::PlayerRoles.PlayerRolesUtils.GetTeam(newRole))
				{
				case global::PlayerRoles.Team.FoundationForces:
					EscapedScientists++;
					break;
				case global::PlayerRoles.Team.ChaosInsurgency:
					EscapedClassD++;
					break;
				}
			}
			break;
		case global::PlayerRoles.RoleChangeReason.Revived:
			ChangedIntoZombies++;
			classlistStart.zombies++;
			break;
		case global::PlayerRoles.RoleChangeReason.Respawn:
		case global::PlayerRoles.RoleChangeReason.Died:
			break;
		}
	}

	private void AddSpawnedTeam(global::PlayerRoles.Team t)
	{
		switch (t)
		{
		case global::PlayerRoles.Team.ChaosInsurgency:
			classlistStart.chaos_insurgents++;
			break;
		case global::PlayerRoles.Team.ClassD:
			classlistStart.class_ds++;
			break;
		case global::PlayerRoles.Team.FoundationForces:
			classlistStart.mtf_and_guards++;
			break;
		case global::PlayerRoles.Team.Scientists:
			classlistStart.scientists++;
			break;
		case global::PlayerRoles.Team.SCPs:
			classlistStart.scps_except_zombies++;
			break;
		}
	}

	public void ForceEnd()
	{
		_roundEnded = true;
	}

	public int CountRole(global::PlayerRoles.RoleTypeId role)
	{
		return global::System.Linq.Enumerable.Count(ReferenceHub.AllHubs, (ReferenceHub x) => global::PlayerRoles.PlayerRolesUtils.GetRoleId(x) == role);
	}

	public int CountTeam(global::PlayerRoles.Team team)
	{
		return global::System.Linq.Enumerable.Count(ReferenceHub.AllHubs, (ReferenceHub x) => global::PlayerRoles.PlayerRolesUtils.GetTeam(x) == team);
	}

	private global::System.Collections.Generic.IEnumerator<float> _ProcessServerSideCode()
	{
		float time = global::UnityEngine.Time.unscaledTime;
		while (this != null)
		{
			yield return global::MEC.Timing.WaitForSeconds(2.5f);
			if (RoundLock || (KeepRoundOnOne && global::System.Linq.Enumerable.Count(ReferenceHub.AllHubs, (ReferenceHub x) => x.characterClassManager.InstanceMode != ClientInstanceMode.DedicatedServer) < 2) || !RoundInProgress() || global::UnityEngine.Time.unscaledTime - time < 15f)
			{
				continue;
			}
			RoundSummary.SumInfo_ClassList newList = default(RoundSummary.SumInfo_ClassList);
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				switch (global::PlayerRoles.PlayerRolesUtils.GetTeam(allHub))
				{
				case global::PlayerRoles.Team.ClassD:
					newList.class_ds++;
					break;
				case global::PlayerRoles.Team.ChaosInsurgency:
					newList.chaos_insurgents++;
					break;
				case global::PlayerRoles.Team.FoundationForces:
					newList.mtf_and_guards++;
					break;
				case global::PlayerRoles.Team.Scientists:
					newList.scientists++;
					break;
				case global::PlayerRoles.Team.SCPs:
					if (global::PlayerRoles.PlayerRolesUtils.GetRoleId(allHub) == global::PlayerRoles.RoleTypeId.Scp0492)
					{
						newList.zombies++;
					}
					else
					{
						newList.scps_except_zombies++;
					}
					break;
				}
			}
			yield return float.NegativeInfinity;
			newList.warhead_kills = (AlphaWarheadController.Detonated ? AlphaWarheadController.Singleton.WarheadKills : (-1));
			yield return float.NegativeInfinity;
			int facilityForces = newList.mtf_and_guards + newList.scientists;
			int chaosInsurgency = newList.chaos_insurgents + newList.class_ds;
			int anomalies = newList.scps_except_zombies + newList.zombies;
			int num = newList.class_ds + EscapedClassD;
			int num2 = newList.scientists + EscapedScientists;
			SurvivingSCPs = newList.scps_except_zombies;
			float dEscapePercentage = ((classlistStart.class_ds != 0) ? (num / classlistStart.class_ds) : 0);
			float sEscapePercentage = ((classlistStart.scientists == 0) ? 1 : (num2 / classlistStart.scientists));
			bool flag;
			if (newList.class_ds <= 0 && facilityForces <= 0)
			{
				flag = true;
			}
			else
			{
				int num3 = 0;
				if (facilityForces > 0)
				{
					num3++;
				}
				if (chaosInsurgency > 0)
				{
					num3++;
				}
				if (anomalies > 0)
				{
					num3++;
				}
				flag = num3 <= 1;
			}
			if (!_roundEnded)
			{
				global::PluginAPI.Events.RoundEndConditionsCheckCancellationData.RoundEndConditionsCheckCancellation cancellation = global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.RoundEndConditionsCheckCancellationData>(global::PluginAPI.Enums.ServerEventType.RoundEndConditionsCheck, new object[1] { flag }).Cancellation;
				int num4 = (int)cancellation;
				if (num4 != 1)
				{
					if (num4 == 2 && !_roundEnded)
					{
						continue;
					}
					if (flag)
					{
						_roundEnded = true;
					}
				}
				else
				{
					_roundEnded = true;
				}
			}
			if (!_roundEnded)
			{
				continue;
			}
			bool num5 = facilityForces > 0;
			bool flag2 = chaosInsurgency > 0;
			bool flag3 = anomalies > 0;
			RoundSummary.LeadingTeam leadingTeam = RoundSummary.LeadingTeam.Draw;
			if (num5)
			{
				leadingTeam = ((EscapedScientists < EscapedClassD) ? RoundSummary.LeadingTeam.Draw : RoundSummary.LeadingTeam.FacilityForces);
			}
			else if (flag3 || (flag3 && flag2))
			{
				leadingTeam = ((EscapedClassD > SurvivingSCPs) ? RoundSummary.LeadingTeam.ChaosInsurgency : ((SurvivingSCPs > EscapedScientists) ? RoundSummary.LeadingTeam.Anomalies : RoundSummary.LeadingTeam.Draw));
			}
			else if (flag2)
			{
				leadingTeam = ((EscapedClassD >= EscapedScientists) ? RoundSummary.LeadingTeam.ChaosInsurgency : RoundSummary.LeadingTeam.Draw);
			}
			global::PluginAPI.Events.RoundEndCancellationData roundEndCancellationData = global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.RoundEndCancellationData>(global::PluginAPI.Enums.ServerEventType.RoundEnd, new object[1] { leadingTeam });
			while (roundEndCancellationData.IsCancelled)
			{
				if (roundEndCancellationData.Delay <= 0f)
				{
					yield break;
				}
				yield return global::MEC.Timing.WaitForSeconds(roundEndCancellationData.Delay);
				roundEndCancellationData = global::PluginAPI.Events.EventManager.ExecuteEvent<global::PluginAPI.Events.RoundEndCancellationData>(global::PluginAPI.Enums.ServerEventType.RoundEnd, new object[1] { leadingTeam });
			}
			if (global::PluginAPI.Core.Statistics.FastestEndedRound.Duration > global::GameCore.RoundStart.RoundLength)
			{
				global::PluginAPI.Core.Statistics.FastestEndedRound = new global::PluginAPI.Core.Statistics.FastestRound(leadingTeam, global::GameCore.RoundStart.RoundLength, global::System.DateTime.Now);
			}
			global::PluginAPI.Core.Statistics.CurrentRound.ClassDAlive = newList.class_ds;
			global::PluginAPI.Core.Statistics.CurrentRound.ScientistsAlive = newList.scientists;
			global::PluginAPI.Core.Statistics.CurrentRound.MtfAndGuardsAlive = newList.mtf_and_guards;
			global::PluginAPI.Core.Statistics.CurrentRound.ChaosInsurgencyAlive = newList.chaos_insurgents;
			global::PluginAPI.Core.Statistics.CurrentRound.ZombiesAlive = newList.zombies;
			global::PluginAPI.Core.Statistics.CurrentRound.ScpsAlive = newList.scps_except_zombies;
			global::PluginAPI.Core.Statistics.CurrentRound.WarheadKills = newList.warhead_kills;
			FriendlyFireConfig.PauseDetector = true;
			string text = "Round finished! Anomalies: " + anomalies + " | Chaos: " + chaosInsurgency + " | Facility Forces: " + facilityForces + " | D escaped percentage: " + dEscapePercentage + " | S escaped percentage: : " + sEscapePercentage;
			global::GameCore.Console.AddLog(text, global::UnityEngine.Color.gray);
			ServerLogs.AddLog(ServerLogs.Modules.Logger, text, ServerLogs.ServerLogType.GameEvent);
			yield return global::MEC.Timing.WaitForSeconds(1.5f);
			int num6 = global::UnityEngine.Mathf.Clamp(global::GameCore.ConfigFile.ServerConfig.GetInt("auto_round_restart_time", 10), 5, 1000);
			if (this != null)
			{
				RpcShowRoundSummary(classlistStart, newList, leadingTeam, EscapedClassD, EscapedScientists, KilledBySCPs, num6, (int)global::GameCore.RoundStart.RoundLength.TotalSeconds);
			}
			yield return global::MEC.Timing.WaitForSeconds(num6 - 1);
			RpcDimScreen();
			yield return global::MEC.Timing.WaitForSeconds(1f);
			global::RoundRestarting.RoundRestart.InitiateRoundRestart();
		}
	}

	[global::Mirror.ClientRpc]
	private void RpcShowRoundSummary(RoundSummary.SumInfo_ClassList listStart, RoundSummary.SumInfo_ClassList listFinish, RoundSummary.LeadingTeam leadingTeam, int eDS, int eSc, int scpKills, int roundCd, int seconds)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.GeneratedNetworkCode._Write_RoundSummary_002FSumInfo_ClassList(writer, listStart);
		global::Mirror.GeneratedNetworkCode._Write_RoundSummary_002FSumInfo_ClassList(writer, listFinish);
		global::Mirror.GeneratedNetworkCode._Write_RoundSummary_002FLeadingTeam(writer, leadingTeam);
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, eDS);
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, eSc);
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, scpKills);
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, roundCd);
		global::Mirror.NetworkWriterExtensions.WriteInt32(writer, seconds);
		SendRPCInternal(typeof(RoundSummary), "RpcShowRoundSummary", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	[global::Mirror.ClientRpc]
	private void RpcDimScreen()
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		SendRPCInternal(typeof(RoundSummary), "RpcDimScreen", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	public static bool RoundInProgress()
	{
		if (ReferenceHub.LocalHub.characterClassManager.RoundStarted)
		{
			return !singleton._roundEnded;
		}
		return false;
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_RpcShowRoundSummary(RoundSummary.SumInfo_ClassList listStart, RoundSummary.SumInfo_ClassList listFinish, RoundSummary.LeadingTeam leadingTeam, int eDS, int eSc, int scpKills, int roundCd, int seconds)
	{
		_summaryActive = true;
	}

	protected static void InvokeUserCode_RpcShowRoundSummary(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcShowRoundSummary called on server.");
		}
		else
		{
			((RoundSummary)obj).UserCode_RpcShowRoundSummary(global::Mirror.GeneratedNetworkCode._Read_RoundSummary_002FSumInfo_ClassList(reader), global::Mirror.GeneratedNetworkCode._Read_RoundSummary_002FSumInfo_ClassList(reader), global::Mirror.GeneratedNetworkCode._Read_RoundSummary_002FLeadingTeam(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader), global::Mirror.NetworkReaderExtensions.ReadInt32(reader));
		}
	}

	private void UserCode_RpcDimScreen()
	{
	}

	protected static void InvokeUserCode_RpcDimScreen(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcDimScreen called on server.");
		}
		else
		{
			((RoundSummary)obj).UserCode_RpcDimScreen();
		}
	}

	static RoundSummary()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(RoundSummary), "RpcShowRoundSummary", InvokeUserCode_RpcShowRoundSummary);
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(RoundSummary), "RpcDimScreen", InvokeUserCode_RpcDimScreen);
	}
}
