public class AlphaWarheadController : global::Mirror.NetworkBehaviour
{
	[global::System.Serializable]
	private class DetonationScenario
	{
		public global::UnityEngine.AudioClip Clip;

		public int TimeToDetonate;

		public int AdditionalTime;

		public int TotalTime => TimeToDetonate + AdditionalTime;
	}

	private global::UnityEngine.AudioSource _alarmSource;

	private bool _doorsAlreadyOpen;

	private bool _blastDoorsShut;

	private bool _openDoors;

	private int _cooldown;

	private bool _isAutomatic;

	private bool _alreadyDetonated;

	private bool _fogEnabled;

	private float _autoDetonateTime;

	private bool _autoDetonate;

	private bool _autoDetonateLock;

	private global::Footprinting.Footprint _triggeringPlayer;

	private AlphaWarheadSyncInfo _prevInfo;

	[global::UnityEngine.SerializeField]
	private AlphaWarheadController.DetonationScenario[] _startScenarios;

	[global::UnityEngine.SerializeField]
	private AlphaWarheadController.DetonationScenario[] _resumeScenarios;

	[global::UnityEngine.SerializeField]
	private global::UnityEngine.AudioClip _cancelSound;

	[global::UnityEngine.SerializeField]
	private int _defaultScenarioId;

	[global::Mirror.SyncVar]
	public AlphaWarheadSyncInfo Info;

	[global::Mirror.SyncVar]
	public double CooldownEndTime;

	public static AlphaWarheadController Singleton;

	internal static bool AutoWarheadBroadcastEnabled;

	internal static string WarheadBroadcastMessage;

	internal static string WarheadExplodedBroadcastMessage;

	internal static ushort WarheadBroadcastMessageTime;

	internal static ushort WarheadExplodedBroadcastMessageTime;

	private const float FacilityDetectionThreshold = 900f;

	private const float DetonationTokenReward = 6f;

	private AlphaWarheadController.DetonationScenario CurScenario => (Info.ResumeScenario ? _resumeScenarios : _startScenarios)[Info.ScenarioId];

	public int WarheadKills { get; private set; }

	public bool IsLocked { get; set; }

	public static ReferenceHub WarheadTriggeredby => Singleton?._triggeringPlayer.Hub;

	public static bool Detonated
	{
		get
		{
			if (InProgress)
			{
				return TimeUntilDetonation == 0f;
			}
			return false;
		}
	}

	public static bool InProgress => Singleton.Info.InProgress;

	public static float TimeUntilDetonation => global::UnityEngine.Mathf.Max(0f, (float)(Singleton.Info.StartTime + (double)Singleton.CurScenario.TotalTime - global::Mirror.NetworkTime.time));

	public AlphaWarheadSyncInfo NetworkInfo
	{
		get
		{
			return Info;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref Info))
			{
				AlphaWarheadSyncInfo info = Info;
				SetSyncVar(value, ref Info, 1uL);
			}
		}
	}

	public double NetworkCooldownEndTime
	{
		get
		{
			return CooldownEndTime;
		}
		[param: global::System.Runtime.InteropServices.In]
		set
		{
			if (!SyncVarEqual(value, ref CooldownEndTime))
			{
				double cooldownEndTime = CooldownEndTime;
				SetSyncVar(value, ref CooldownEndTime, 2uL);
			}
		}
	}

	private void Start()
	{
		Singleton = this;
		_alarmSource = GetComponent<global::UnityEngine.AudioSource>();
		if (!global::Mirror.NetworkServer.active)
		{
			return;
		}
		NetworkCooldownEndTime = 0.0;
		_autoDetonateTime = global::GameCore.ConfigFile.ServerConfig.GetFloat("auto_warhead_start_minutes") * 60f;
		_autoDetonate = _autoDetonateTime > 0f;
		_autoDetonateLock = global::GameCore.ConfigFile.ServerConfig.GetBool("auto_warhead_lock");
		_openDoors = global::GameCore.ConfigFile.ServerConfig.GetBool("open_doors_on_countdown", def: true);
		_cooldown = global::GameCore.ConfigFile.ServerConfig.GetInt("warhead_cooldown", 40);
		AlphaWarheadSyncInfo networkInfo = default(AlphaWarheadSyncInfo);
		int num = global::GameCore.ConfigFile.ServerConfig.GetInt("warhead_tminus_start_duration", 90);
		networkInfo.ScenarioId = _defaultScenarioId;
		for (byte b = 0; b < _startScenarios.Length; b++)
		{
			if (_startScenarios[b].TimeToDetonate == num)
			{
				networkInfo.ScenarioId = b;
			}
		}
		NetworkInfo = networkInfo;
	}

	private void Update()
	{
		if (Info != _prevInfo)
		{
			OnInfoUpdated();
			_prevInfo = Info;
		}
		FlickerableLightController.WarheadEnabled = Info.InProgress;
		UpdateFog();
		ServerUpdateDetonationTime();
		ServerUpdateAutonuke();
	}

	private bool TryGetBroadcaster(out Broadcast broadcaster)
	{
		broadcaster = null;
		if (ReferenceHub.TryGetLocalHub(out var hub))
		{
			return hub.TryGetComponent<Broadcast>(out broadcaster);
		}
		return false;
	}

	private void OnInfoUpdated()
	{
		_alarmSource.Stop();
		if (!Info.InProgress)
		{
			_alarmSource.PlayOneShot(_cancelSound);
			return;
		}
		_alarmSource.volume = 1f;
		_alarmSource.clip = CurScenario.Clip;
		float num = (float)(global::Mirror.NetworkTime.time - Info.StartTime);
		if (num < 0f)
		{
			_alarmSource.PlayDelayed(0f - num);
		}
		else if (num < _alarmSource.clip.length)
		{
			_alarmSource.Play();
			_alarmSource.time = num;
		}
	}

	public void ForceTime(float remaining)
	{
		InstantPrepare();
		StartDetonation(isAutomatic: false, suppressSubtitles: true);
		AlphaWarheadSyncInfo info = Info;
		remaining -= (float)CurScenario.TotalTime;
		info.StartTime = global::Mirror.NetworkTime.time + (double)remaining;
		NetworkInfo = info;
	}

	public void InstantPrepare()
	{
		AlphaWarheadSyncInfo info = Info;
		info.StartTime = 0.0;
		NetworkInfo = info;
		NetworkCooldownEndTime = 0.0;
	}

	public void StartDetonation(bool isAutomatic = false, bool suppressSubtitles = false, ReferenceHub trigger = null)
	{
		if (Info.InProgress || CooldownEndTime > global::Mirror.NetworkTime.time || IsLocked || !global::PluginAPI.Events.EventManager.ExecuteEvent<bool>(global::PluginAPI.Enums.ServerEventType.WarheadStart, new object[3] { isAutomatic, trigger, Info.ResumeScenario }))
		{
			return;
		}
		_isAutomatic = isAutomatic;
		_alreadyDetonated = false;
		if (isAutomatic)
		{
			IsLocked |= _autoDetonateLock;
			if (!_alreadyDetonated && !Info.InProgress && AutoWarheadBroadcastEnabled && TryGetBroadcaster(out var broadcaster))
			{
				broadcaster.RpcAddElement(WarheadBroadcastMessage, WarheadBroadcastMessageTime, Broadcast.BroadcastFlags.Normal);
			}
		}
		_doorsAlreadyOpen = false;
		ServerLogs.AddLog(ServerLogs.Modules.Warhead, "Countdown started.", ServerLogs.ServerLogType.GameEvent);
		_triggeringPlayer = new global::Footprinting.Footprint(trigger);
		AlphaWarheadSyncInfo info = Info;
		info.StartTime = global::Mirror.NetworkTime.time;
		NetworkInfo = info;
		if (!suppressSubtitles)
		{
			global::Subtitles.SubtitleType subtitle = (Info.ResumeScenario ? global::Subtitles.SubtitleType.AlphaWarheadResumed : global::Subtitles.SubtitleType.AlphaWarheadEngage);
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::Subtitles.SubtitleMessage(new global::Subtitles.SubtitlePart(subtitle, CurScenario.TimeToDetonate.ToString())));
		}
	}

	public void CancelDetonation()
	{
		CancelDetonation(null);
	}

	public void CancelDetonation(ReferenceHub disabler)
	{
		if (!Info.InProgress || TimeUntilDetonation <= 10f || IsLocked || !global::PluginAPI.Events.EventManager.ExecuteEvent<bool>(global::PluginAPI.Enums.ServerEventType.WarheadStop, new object[1] { disabler }))
		{
			return;
		}
		ServerLogs.AddLog(ServerLogs.Modules.Warhead, "Detonation cancelled.", ServerLogs.ServerLogType.GameEvent);
		if (TimeUntilDetonation <= 15f && disabler != null)
		{
			global::Achievements.AchievementHandlerBase.ServerAchieve(disabler.connectionToClient, global::Achievements.AchievementName.ThatWasClose);
		}
		AlphaWarheadSyncInfo info = Info;
		info.StartTime = 0.0;
		int num = (int)global::UnityEngine.Mathf.Min(TimeUntilDetonation, CurScenario.TimeToDetonate);
		int num2 = int.MaxValue;
		info.ResumeScenario = true;
		for (int i = 0; i < _resumeScenarios.Length; i++)
		{
			int num3 = _resumeScenarios[i].TimeToDetonate - num;
			if (num3 >= 0 && num3 <= num2)
			{
				num2 = num3;
				info.ScenarioId = i;
			}
		}
		NetworkInfo = info;
		NetworkCooldownEndTime = global::Mirror.NetworkTime.time + (double)_cooldown;
		global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.OpenerEventType.WarheadCancel);
		if (global::Mirror.NetworkServer.active)
		{
			_isAutomatic = false;
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::Subtitles.SubtitleMessage(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.AlphaWarheadCancelled, (string[])null)));
		}
	}

	private void Detonate()
	{
		if (!global::PluginAPI.Events.EventManager.ExecuteEvent<bool>(global::PluginAPI.Enums.ServerEventType.WarheadDetonation, global::System.Array.Empty<object>()))
		{
			return;
		}
		if (_isAutomatic && !_alreadyDetonated && !Info.InProgress && AutoWarheadBroadcastEnabled && TryGetBroadcaster(out var broadcaster))
		{
			broadcaster.RpcAddElement(WarheadExplodedBroadcastMessage, WarheadExplodedBroadcastMessageTime, Broadcast.BroadcastFlags.Normal);
		}
		ServerLogs.AddLog(ServerLogs.Modules.Warhead, "Warhead detonated.", ServerLogs.ServerLogType.GameEvent);
		if (global::LightContainmentZoneDecontamination.DecontaminationController.Singleton.DecontaminationOverride != global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Disabled)
		{
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, "LCZ decontamination has been disabled by detonation of the Alpha Warhead.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			global::LightContainmentZoneDecontamination.DecontaminationController.Singleton.NetworkDecontaminationOverride = global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Disabled;
		}
		_alreadyDetonated = true;
		global::PluginAPI.Core.Statistics.CurrentRound.WarheadDetonated = true;
		global::System.Collections.Generic.HashSet<global::PlayerRoles.Team> hashSet = new global::System.Collections.Generic.HashSet<global::PlayerRoles.Team>();
		foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
		{
			global::PlayerRoles.PlayerRoleBase currentRole = allHub.roleManager.CurrentRole;
			if (global::PlayerRoles.PlayerRolesUtils.IsAlive(allHub) && (!(currentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole) || CanBeDetonated(fpcRole.FpcModule.Position)))
			{
				hashSet.Add(global::PlayerRoles.PlayerRolesUtils.GetTeam(allHub));
				allHub.playerStats.DealDamage(new global::PlayerStatsSystem.WarheadDamageHandler());
				WarheadKills++;
			}
		}
		foreach (global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup instance in global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup.Instances)
		{
			if (CanBeDetonated(instance.Info.Position, includeOnlyLifts: true))
			{
				instance.DestroySelf();
			}
		}
		foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant allDoor in global::Interactables.Interobjects.DoorUtils.DoorVariant.AllDoors)
		{
			if (allDoor is global::Interactables.Interobjects.ElevatorDoor elevatorDoor)
			{
				elevatorDoor.NetworkActiveLocks = (ushort)(elevatorDoor.ActiveLocks | 4);
			}
		}
		RpcShake(achieve: true);
		if (!_triggeringPlayer.IsSet)
		{
			return;
		}
		switch ((byte)global::PlayerRoles.PlayerRolesUtils.GetFaction(_triggeringPlayer.Role))
		{
		case 2:
			if (!hashSet.Contains(global::PlayerRoles.Team.ClassD))
			{
				global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.ChaosInsurgency, 6f);
			}
			break;
		case 1:
			if (!hashSet.Contains(global::PlayerRoles.Team.Scientists))
			{
				global::Respawning.RespawnTokensManager.GrantTokens(global::Respawning.SpawnableTeamType.NineTailedFox, 6f);
			}
			break;
		}
	}

	private static bool CanBeDetonated(global::UnityEngine.Vector3 pos, bool includeOnlyLifts = false)
	{
		if (pos.y < 900f && !includeOnlyLifts)
		{
			return true;
		}
		foreach (global::System.Collections.Generic.List<global::Interactables.Interobjects.ElevatorDoor> value in global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors.Values)
		{
			if (value.Count != 0 && value[0].TargetPanel.AssignedChamber.WorldspaceBounds.Contains(pos))
			{
				return true;
			}
		}
		return false;
	}

	[global::Mirror.ClientRpc]
	private void RpcShake(bool achieve)
	{
		global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
		global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, achieve);
		SendRPCInternal(typeof(AlphaWarheadController), "RpcShake", writer, 0, includeOwner: true);
		global::Mirror.NetworkWriterPool.Recycle(writer);
	}

	private void UpdateFog()
	{
	}

	[global::Mirror.ServerCallback]
	private void ServerUpdateAutonuke()
	{
		if (global::Mirror.NetworkServer.active && global::Mirror.NetworkServer.active && global::GameCore.RoundStart.RoundStarted && _autoDetonate && !_alreadyDetonated && !Info.InProgress)
		{
			_autoDetonateTime -= global::UnityEngine.Time.deltaTime;
			if (!(_autoDetonateTime > 0f))
			{
				StartDetonation(isAutomatic: true);
				_autoDetonate = false;
			}
		}
	}

	[global::Mirror.ServerCallback]
	private void ServerUpdateDetonationTime()
	{
		if (!global::Mirror.NetworkServer.active || !global::Mirror.NetworkServer.active || !Info.InProgress)
		{
			return;
		}
		if (!_blastDoorsShut && TimeUntilDetonation < 2f)
		{
			_blastDoorsShut = true;
			global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(BlastDoor.Instances, delegate(BlastDoor x)
			{
				x.SetClosed(prev: false, b: true);
			});
		}
		if (_openDoors && !_doorsAlreadyOpen && TimeUntilDetonation < (float)CurScenario.TimeToDetonate)
		{
			_doorsAlreadyOpen = true;
			global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.OpenerEventType.WarheadStart);
		}
		if (!_alreadyDetonated && !(TimeUntilDetonation > 0f))
		{
			Detonate();
		}
	}

	private void MirrorProcessed()
	{
	}

	private void UserCode_RpcShake(bool achieve)
	{
	}

	protected static void InvokeUserCode_RpcShake(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
	{
		if (!global::Mirror.NetworkClient.active)
		{
			global::UnityEngine.Debug.LogError("RPC RpcShake called on server.");
		}
		else
		{
			((AlphaWarheadController)obj).UserCode_RpcShake(global::Mirror.NetworkReaderExtensions.ReadBoolean(reader));
		}
	}

	static AlphaWarheadController()
	{
		global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(AlphaWarheadController), "RpcShake", InvokeUserCode_RpcShake);
	}

	public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
	{
		bool result = base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WritePickupSyncInfo(Info);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, CooldownEndTime);
			return true;
		}
		global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1L) != 0L)
		{
			writer.WritePickupSyncInfo(Info);
			result = true;
		}
		if ((base.syncVarDirtyBits & 2L) != 0L)
		{
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, CooldownEndTime);
			result = true;
		}
		return result;
	}

	public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			AlphaWarheadSyncInfo info = Info;
			NetworkInfo = reader.ReadPickupSyncInfo();
			double cooldownEndTime = CooldownEndTime;
			NetworkCooldownEndTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
			return;
		}
		long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
		if ((num & 1L) != 0L)
		{
			AlphaWarheadSyncInfo info2 = Info;
			NetworkInfo = reader.ReadPickupSyncInfo();
		}
		if ((num & 2L) != 0L)
		{
			double cooldownEndTime2 = CooldownEndTime;
			NetworkCooldownEndTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}
	}
}
