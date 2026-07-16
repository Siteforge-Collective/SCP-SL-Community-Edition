namespace LightContainmentZoneDecontamination
{
	public class DecontaminationController : global::Mirror.NetworkBehaviour
	{
		public enum DecontaminationStatus : byte
		{
			None = 0,
			Disabled = 1,
			Forced = 2
		}

		[global::System.Serializable]
		public struct DecontaminationPhase
		{
			public enum PhaseFunction : byte
			{
				None = 0,
				GloballyAudible = 1,
				OpenCheckpoints = 2,
				Final = 3
			}

			public float TimeTrigger;

			public float GameTime;

			public global::UnityEngine.AudioClip AnnouncementLine;

			public global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction Function;
		}

		public static global::LightContainmentZoneDecontamination.DecontaminationController Singleton;

		public float TimeOffset;

		public global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase[] DecontaminationPhases;

		public global::MapGeneration.ImageGenerator LczGenerator;

		public global::UnityEngine.AudioSource AnnouncementAudioSource;

		[global::Mirror.SyncVar]
		public double RoundStartTime;

		[global::Mirror.SyncVar(hook = "OnChangeDisableDecontamination")]
		public global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus DecontaminationOverride;

		private global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction _curFunction;

		private int _nextPhase;

		private bool _stopUpdating;

		private bool _elevatorsDirty;

		private bool _decontaminationBegun;

		private float _justJoinedCooldown;

		private string _elevatorsLockedText = "ELEVATOR SYSTEM <color=#e00>DISABLED</color>";

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _checkpointHczA;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.UI.Text _checkpointHczB;

		private const float LowerBoundLCZ = -200f;

		private const float UpperBoundLCZ = 200f;

		internal static bool AutoDeconBroadcastEnabled;

		internal static string DeconBroadcastDeconMessage;

		internal static ushort DeconBroadcastDeconMessageTime;

		public static double GetServerTime => global::Mirror.NetworkTime.time - Singleton.RoundStartTime + (double)Singleton.TimeOffset;

		private bool IsAnnouncementHearable
		{
			get
			{
				if (!ReferenceHub.TryGetLocalHub(out var hub))
				{
					return false;
				}
				if (_curFunction == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.Final)
				{
					return true;
				}
				if (_curFunction == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.GloballyAudible)
				{
					return true;
				}
				float num = ((hub.roleManager.CurrentRole is global::PlayerRoles.ICameraController cameraController) ? cameraController.CameraPosition.y : hub.transform.position.y);
				if (num > -200f)
				{
					return num < 200f;
				}
				return false;
			}
		}

		public bool IsDecontaminating
		{
			get
			{
				if (global::Mirror.NetworkServer.active)
				{
					return _decontaminationBegun;
				}
				return false;
			}
		}

		public double NetworkRoundStartTime
		{
			get
			{
				return RoundStartTime;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref RoundStartTime))
				{
					double roundStartTime = RoundStartTime;
					SetSyncVar(value, ref RoundStartTime, 1uL);
				}
			}
		}

		public global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus NetworkDecontaminationOverride
		{
			get
			{
				return DecontaminationOverride;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref DecontaminationOverride))
				{
					global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus decontaminationOverride = DecontaminationOverride;
					SetSyncVar(value, ref DecontaminationOverride, 2uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(2uL))
					{
						setSyncVarHookGuard(2uL, value: true);
						OnChangeDisableDecontamination(decontaminationOverride, value);
						setSyncVarHookGuard(2uL, value: false);
					}
				}
			}
		}

		private void Awake()
		{
			Singleton = this;
		}

		public void OnChangeDisableDecontamination(global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus oldValue, global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus newValue)
		{
			if (oldValue != newValue && newValue == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Disabled)
			{
				global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.OpenerEventType.DeconReset);
			}
		}

		public void ForceDecontamination()
		{
			NetworkDecontaminationOverride = global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Forced;
			FinishDecontamination();
		}

		private void Start()
		{
			if (global::Mirror.NetworkServer.active && global::GameCore.ConfigFile.ServerConfig.GetBool("disable_decontamination"))
			{
				NetworkDecontaminationOverride = global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Disabled;
			}
			for (int i = 0; i < DecontaminationPhases.Length; i++)
			{
				DecontaminationPhases[i].TimeTrigger *= 60f;
			}
		}

		private global::System.Collections.Generic.IEnumerator<float> KillPlayers()
		{
			float timer = 1f;
			while (Singleton != null && _decontaminationBegun && DecontaminationOverride != global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.Disabled)
			{
				timer -= global::UnityEngine.Time.deltaTime;
				yield return float.NegativeInfinity;
				if (!(timer <= 0f))
				{
					continue;
				}
				timer = 1f;
				foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
				{
					if (global::PlayerRoles.PlayerRolesUtils.IsAlive(allHub))
					{
						float y = allHub.transform.position.y;
						bool flag = y < 200f && y > -200f;
						global::CustomPlayerEffects.Decontaminating effect = allHub.playerEffectsController.GetEffect<global::CustomPlayerEffects.Decontaminating>();
						if (!effect.IsEnabled && flag)
						{
							allHub.playerEffectsController.EnableEffect<global::CustomPlayerEffects.Decontaminating>();
						}
						else if (effect.IsEnabled && !flag)
						{
							allHub.playerEffectsController.DisableEffect<global::CustomPlayerEffects.Decontaminating>();
						}
					}
				}
			}
		}

		private void FinishDecontamination()
		{
			if (global::Mirror.NetworkServer.active && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.LczDecontaminationStart))
			{
				global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.OpenerEventType.DeconFinish);
				DisableElevators();
				if (AutoDeconBroadcastEnabled && !_decontaminationBegun)
				{
					Broadcast.Singleton.RpcAddElement(DeconBroadcastDeconMessage, DeconBroadcastDeconMessageTime, Broadcast.BroadcastFlags.Normal);
				}
				_decontaminationBegun = true;
				global::MEC.Timing.RunCoroutine(KillPlayers());
			}
		}

		private void DisableElevators()
		{
			bool flag = false;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant allDoor in global::Interactables.Interobjects.DoorUtils.DoorVariant.AllDoors)
			{
				if (allDoor is global::Interactables.Interobjects.ElevatorDoor elevatorDoor && elevatorDoor.Rooms.Length != 0 && elevatorDoor.Rooms[0].Zone == global::MapGeneration.FacilityZone.LightContainment)
				{
					elevatorDoor.NetworkActiveLocks = (ushort)(elevatorDoor.ActiveLocks | 0x10);
					if (!allDoor.TargetState && !global::Interactables.Interobjects.ElevatorManager.TrySetDestination(elevatorDoor.Group, 1))
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				_elevatorsDirty = false;
			}
		}

		private void Update()
		{
			if (_elevatorsDirty)
			{
				DisableElevators();
			}
			if (!_stopUpdating)
			{
				if (global::Mirror.NetworkServer.active)
				{
					ServersideSetup();
				}
				UpdateTime();
			}
		}

		private void ServersideSetup()
		{
			if (DecontaminationOverride == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.None && RoundStartTime == 0.0 && global::GameCore.RoundStart.singleton.Timer == -1)
			{
				NetworkRoundStartTime = global::Mirror.NetworkTime.time;
			}
		}

		private void UpdateTime()
		{
			if (DecontaminationOverride != global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus.None)
			{
				return;
			}
			if (RoundStartTime <= 0.0)
			{
				if (RoundStartTime == -1.0)
				{
					_stopUpdating = true;
				}
				return;
			}
			if (_justJoinedCooldown < 10f)
			{
				_justJoinedCooldown += global::UnityEngine.Time.deltaTime;
			}
			float num = (float)GetServerTime;
			if (num == -1f || !(num > DecontaminationPhases[_nextPhase].TimeTrigger))
			{
				return;
			}
			if (DecontaminationPhases[_nextPhase].AnnouncementLine != null && _justJoinedCooldown >= 10f)
			{
				_curFunction = DecontaminationPhases[_nextPhase].Function;
				UpdateSpeaker(hard: true);
				AnnouncementAudioSource.PlayOneShot(DecontaminationPhases[_nextPhase].AnnouncementLine);
				if (global::Mirror.NetworkServer.active)
				{
					global::System.Collections.Generic.List<global::Subtitles.SubtitlePart> list = new global::System.Collections.Generic.List<global::Subtitles.SubtitlePart>(1);
					switch (_nextPhase)
					{
					case 0:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.DecontaminationStart, (string[])null));
						break;
					case 1:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.DecontaminationMinutes, "10"));
						break;
					case 2:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.DecontaminationMinutes, "5"));
						break;
					case 3:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.Decontamination1Minute, (string[])null));
						break;
					case 4:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.DecontaminationCountdown, (string[])null));
						break;
					case 6:
						list.Add(new global::Subtitles.SubtitlePart(global::Subtitles.SubtitleType.DecontaminationLockdown, (string[])null));
						break;
					}
					foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
					{
						if (IsAudibleForClient(allHub))
						{
							global::PlayerRoles.Spectating.SpectatorNetworking.SendToSpectatorsOf(new global::Subtitles.SubtitleMessage(list.ToArray()), allHub, includeTarget: true);
						}
					}
				}
			}
			if (DecontaminationPhases[_nextPhase].Function == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.Final)
			{
				FinishDecontamination();
			}
			if (global::Mirror.NetworkServer.active && DecontaminationPhases[_nextPhase].Function == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.OpenCheckpoints)
			{
				global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorEventOpenerExtension.OpenerEventType.DeconEvac);
			}
			if (_nextPhase == DecontaminationPhases.Length - 1)
			{
				_stopUpdating = true;
				return;
			}
			global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.LczDecontaminationAnnouncement, _nextPhase);
			_nextPhase++;
		}

		private bool IsAudibleForClient(ReferenceHub hub)
		{
			if (_curFunction == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.Final)
			{
				return true;
			}
			if (_curFunction == global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationPhase.PhaseFunction.GloballyAudible)
			{
				return true;
			}
			global::PlayerRoles.PlayerRoleBase currentRole = hub.roleManager.CurrentRole;
			if (currentRole is global::PlayerRoles.PlayableScps.Scp079.Scp079Role scp079Role)
			{
				return scp079Role.CurrentCamera.Room.Zone == global::MapGeneration.FacilityZone.LightContainment;
			}
			if (currentRole is global::PlayerRoles.FirstPersonControl.IFpcRole fpcRole)
			{
				global::MapGeneration.RoomIdentifier roomIdentifier = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(fpcRole.FpcModule.Position);
				if (roomIdentifier != null)
				{
					return roomIdentifier.Zone == global::MapGeneration.FacilityZone.LightContainment;
				}
				return false;
			}
			return false;
		}

		private void UpdateSpeaker(bool hard)
		{
			float b = (IsAnnouncementHearable ? 1 : 0);
			float t = (hard ? 1f : (global::UnityEngine.Time.deltaTime * 4f));
			AnnouncementAudioSource.volume = global::UnityEngine.Mathf.Lerp(AnnouncementAudioSource.volume, b, t);
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteDouble(writer, RoundStartTime);
				global::Mirror.GeneratedNetworkCode._Write_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(writer, DecontaminationOverride);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteDouble(writer, RoundStartTime);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.GeneratedNetworkCode._Write_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(writer, DecontaminationOverride);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				double roundStartTime = RoundStartTime;
				NetworkRoundStartTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus decontaminationOverride = DecontaminationOverride;
				NetworkDecontaminationOverride = global::Mirror.GeneratedNetworkCode._Read_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(reader);
				if (!SyncVarEqual(decontaminationOverride, ref DecontaminationOverride))
				{
					OnChangeDisableDecontamination(decontaminationOverride, DecontaminationOverride);
				}
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				double roundStartTime2 = RoundStartTime;
				NetworkRoundStartTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
			}
			if ((num & 2L) != 0L)
			{
				global::LightContainmentZoneDecontamination.DecontaminationController.DecontaminationStatus decontaminationOverride2 = DecontaminationOverride;
				NetworkDecontaminationOverride = global::Mirror.GeneratedNetworkCode._Read_LightContainmentZoneDecontamination_002EDecontaminationController_002FDecontaminationStatus(reader);
				if (!SyncVarEqual(decontaminationOverride2, ref DecontaminationOverride))
				{
					OnChangeDisableDecontamination(decontaminationOverride2, DecontaminationOverride);
				}
			}
		}
	}
}
