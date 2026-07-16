namespace PlayerRoles.PlayableScps.Scp079.Cameras
{
	public class Scp079CurrentCameraSync : global::PlayerRoles.PlayableScps.Subroutines.ScpStandardSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079Role>, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079FailMessageProvider
	{
		public enum ClientSwitchState
		{
			None = 0,
			SwitchingRoom = 1,
			SwitchingZone = 2
		}

		public const float CostPerMeter = 0.16f;

		public const int CostPerFloor = 10;

		public const int CostPerZone = 20;

		public const int CostPerSkippedZone = 30;

		public static readonly global::MapGeneration.FacilityZone[] ZoneQueue = new global::MapGeneration.FacilityZone[4]
		{
			global::MapGeneration.FacilityZone.Surface,
			global::MapGeneration.FacilityZone.Entrance,
			global::MapGeneration.FacilityZone.HeavyContainment,
			global::MapGeneration.FacilityZone.LightContainment
		};

		private const float FloorHeight = 100f;

		private const int ErrorTranslationId = 2;

		private const float SameRoomSwitchDuration = 0.1f;

		private const float ZoneSwitchDuration = 0.98f;

		private const string DefaultCameraName = "079 CONT CHAMBER";

		private readonly global::System.Diagnostics.Stopwatch _switchStopwatch = new global::System.Diagnostics.Stopwatch();

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera _lastCam;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera _switchTarget;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager _auxManager;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler _lostSignalHandler;

		private bool _camSet;

		private bool _eventAssigned;

		private float _targetSwitchTime;

		private ushort _defaultCamId;

		private ushort _requestedCamId;

		private bool _initialized;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation _errorCode;

		private global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState _clientSwitchRequest;

		public string FailMessage { get; private set; }

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState CurClientSwitchState { get; private set; }

		public global::MapGeneration.FacilityZone CurClientTargetZone { get; private set; }

		public global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera CurrentCamera
		{
			get
			{
				TryGetCurrentCamera(out var cam);
				return cam;
			}
			private set
			{
				value.IsActive = true;
				_camSet = true;
				if (!(value == _lastCam))
				{
					_lastCam = value;
					this.OnCameraChanged?.Invoke();
					if (global::Mirror.NetworkServer.active)
					{
						ServerSendRpc(toAll: true);
					}
				}
			}
		}

		public event global::System.Action OnCameraChanged;

		private void Update()
		{
			if (!_initialized)
			{
				if (!TryGetCurrentCamera(out var _))
				{
					return;
				}
				_initialized = true;
			}
			if (CurClientSwitchState != global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None && base.Role.IsLocalPlayer && !(_switchStopwatch.Elapsed.TotalSeconds < (double)_targetSwitchTime))
			{
				_clientSwitchRequest = global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None;
				_switchStopwatch.Stop();
				ClientSendCmd();
			}
		}

		private bool TryGetDefaultCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			if (_defaultCamId == 0)
			{
				if (global::Utils.NonAllocLINQ.HashsetExtensions.TryGetFirst(global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.AllInstances, (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase x) => x is global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera scp079Camera && x.SyncId != 0 && scp079Camera.Room.Name == global::MapGeneration.RoomName.Hcz079 && scp079Camera.Label.Equals("079 CONT CHAMBER", global::System.StringComparison.InvariantCultureIgnoreCase), out var first))
				{
					cam = first as global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera;
					_defaultCamId = cam.SyncId;
					return true;
				}
				cam = null;
				return false;
			}
			if (global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.TryGetInteractable(_defaultCamId, out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera result))
			{
				cam = result;
				return true;
			}
			cam = null;
			return false;
		}

		private void OnHubAdded(ReferenceHub hub)
		{
			if (global::Mirror.NetworkServer.active)
			{
				ServerSendRpc(hub);
			}
		}

		public int GetSwitchCost(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target)
		{
			global::UnityEngine.Vector3 position = CurrentCamera.Position;
			global::UnityEngine.Vector3 position2 = target.Position;
			global::MapGeneration.FacilityZone zone = CurrentCamera.Room.Zone;
			global::MapGeneration.FacilityZone zone2 = target.Room.Zone;
			bool flag = global::UnityEngine.Mathf.Abs(position.y - position2.y) < 100f;
			bool flag2 = zone == zone2;
			int num = ((!flag) ? 10 : global::UnityEngine.Mathf.CeilToInt(global::UnityEngine.Vector3.Distance(position, position2) * 0.16f));
			if (flag2)
			{
				if (!flag)
				{
					return num;
				}
				if (!CurrentCamera.Room.TryGetMainCoords(out var coords))
				{
					return num;
				}
				if (!target.Room.TryGetMainCoords(out var coords2))
				{
					return num;
				}
				if (!((coords - coords2).magnitude <= 1f))
				{
					return num;
				}
				return 0;
			}
			int num2 = global::UnityEngine.Mathf.Abs(ZoneQueue.IndexOf(zone) - ZoneQueue.IndexOf(zone2)) - 1;
			return num + 20 + 30 * num2;
		}

		public void ClientSwitchTo(global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera target)
		{
			if (!base.Role.IsLocalPlayer)
			{
				throw new global::System.InvalidOperationException("Method ClientSwitchTo can only be called on the local client instance!");
			}
			if (CurClientSwitchState == global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
			{
				bool flag = target.Room.Zone == CurrentCamera.Room.Zone;
				_switchTarget = target;
				_targetSwitchTime = (flag ? 0.1f : 0.98f);
				CurClientSwitchState = (flag ? global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingRoom : global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingZone);
				CurClientTargetZone = target.Room.Zone;
				_clientSwitchRequest = CurClientSwitchState;
				ClientSendCmd();
				_switchStopwatch.Restart();
			}
		}

		public override void SpawnObject()
		{
			base.SpawnObject();
			if (global::Mirror.NetworkServer.active)
			{
				_eventAssigned = true;
				ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(OnHubAdded));
				(base.Role as global::PlayerRoles.PlayableScps.Scp079.Scp079Role).SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079AuxManager>(out _auxManager);
				(base.Role as global::PlayerRoles.PlayableScps.Scp079.Scp079Role).SubroutineModule.TryGetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079LostSignalHandler>(out _lostSignalHandler);
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			if (_eventAssigned)
			{
				ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Remove(ReferenceHub.OnPlayerAdded, new global::System.Action<ReferenceHub>(OnHubAdded));
			}
			_defaultCamId = 0;
			_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			_lastCam = null;
			_camSet = false;
			_initialized = false;
			_eventAssigned = false;
			CurClientSwitchState = global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None;
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			writer.WriteByte((byte)_clientSwitchRequest);
			global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _switchTarget.SyncId);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			_clientSwitchRequest = (global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState)reader.ReadByte();
			_requestedCamId = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			if (_clientSwitchRequest != global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
			{
				ServerSendRpc((ReferenceHub x) => x.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole);
				return;
			}
			if (!global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.TryGetInteractable(_requestedCamId, out _switchTarget))
			{
				_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.InvalidCamera;
				ServerSendRpc(toAll: true);
				return;
			}
			float num = GetSwitchCost(_switchTarget);
			if (num > _auxManager.CurrentAux)
			{
				_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux;
				ServerSendRpc(toAll: true);
				return;
			}
			if (_lostSignalHandler.Lost)
			{
				_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.SignalLost;
				ServerSendRpc(toAll: true);
				return;
			}
			if (_switchTarget != CurrentCamera)
			{
				base.Role.TryGetOwner(out var hub);
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079CameraChanged, hub, _switchTarget))
				{
					_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.SignalLost;
					ServerSendRpc(toAll: true);
					return;
				}
			}
			_auxManager.CurrentAux -= num;
			_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			if (_switchTarget != CurrentCamera)
			{
				CurrentCamera = _switchTarget;
			}
			else
			{
				ServerSendRpc(toAll: true);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			writer.WriteByte((byte)_clientSwitchRequest);
			if (_clientSwitchRequest == global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.None)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, CurrentCamera.SyncId);
				writer.WriteByte((byte)_errorCode);
			}
			else
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, _requestedCamId);
			}
			_errorCode = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			CurClientSwitchState = (global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState)reader.ReadByte();
			ushort num = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera result;
			bool flag = global::PlayerRoles.PlayableScps.Scp079.Scp079InteractableBase.TryGetInteractable(num, out result);
			switch (CurClientSwitchState)
			{
			case global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingRoom:
				return;
			case global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079CurrentCameraSync.ClientSwitchState.SwitchingZone:
				if (flag)
				{
					CurClientTargetZone = result.Room.Zone;
				}
				return;
			}
			_errorCode = (global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation)reader.ReadByte();
			if (_errorCode != global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom)
			{
				if (base.Role.IsLocalPlayer || base.ScpRole.IsSpectated)
				{
					global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079AbilityList.Singleton.TrackedFailMessage = this;
				}
			}
			else if (!global::Mirror.NetworkServer.active)
			{
				if (flag)
				{
					CurrentCamera = result;
					return;
				}
				_camSet = false;
				_defaultCamId = num;
			}
		}

		public bool TryGetCurrentCamera(out global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera cam)
		{
			if (base.Role.Pooled)
			{
				cam = null;
				return false;
			}
			if (_camSet)
			{
				cam = _lastCam;
				return true;
			}
			if (TryGetDefaultCamera(out var cam2))
			{
				cam = cam2;
				CurrentCamera = cam;
				return true;
			}
			cam = null;
			return false;
		}

		public void OnFailMessageAssigned()
		{
			FailMessage = Translations.Get(_errorCode);
		}
	}
}
