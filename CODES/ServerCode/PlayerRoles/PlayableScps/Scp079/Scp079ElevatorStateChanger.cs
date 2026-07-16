namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079ElevatorStateChanger : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _confirmationSound;

		[global::UnityEngine.SerializeField]
		private int _cost;

		private global::Interactables.Interobjects.ElevatorDoor _lastElevator;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation _failedReason;

		private string _abilityName;

		public override bool IsVisible
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
				{
					return false;
				}
				if (global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconManager.Singleton.HighlightedOvercon is global::PlayerRoles.PlayableScps.Scp079.Overcons.ElevatorOvercon elevatorOvercon && elevatorOvercon != null)
				{
					_lastElevator = elevatorOvercon.Target;
					return true;
				}
				return false;
			}
		}

		public override bool IsReady
		{
			get
			{
				if (ErrorCode == global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom)
				{
					return _lastElevator.TargetPanel.AssignedChamber.IsReady;
				}
				return false;
			}
		}

		public override string FailMessage
		{
			get
			{
				switch (_failedReason)
				{
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom:
					return null;
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux:
					return GetNoAuxMessage(_cost);
				default:
					return Translations.Get(_failedReason);
				}
			}
		}

		public override ActionName ActivationKey => ActionName.Shoot;

		public override string AbilityName => string.Format(_abilityName, _cost);

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation ErrorCode
		{
			get
			{
				if (base.AuxManager.CurrentAux < (float)_cost)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux;
				}
				if (!ValidateLastElevator)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.ElevatorAccessDenied;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			}
		}

		private bool ValidateLastElevator
		{
			get
			{
				if (_lastElevator == null)
				{
					return false;
				}
				if (!global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors.TryGetValue(_lastElevator.Group, out var value))
				{
					return false;
				}
				if (global::Utils.NonAllocLINQ.ListExtensions.Any(value, (global::Interactables.Interobjects.ElevatorDoor x) => x.ActiveLocks != 0))
				{
					return false;
				}
				return true;
			}
		}

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::Interactables.Interobjects.ElevatorDoor> OnServerElevatorDoorClosed;

		protected override void Start()
		{
			base.Start();
			_abilityName = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.SendElevator);
			base.CurrentCamSync.OnCameraChanged += delegate
			{
				_failedReason = global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			};
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			writer.WriteByte((byte)_lastElevator.Group);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (base.AuxManager.CurrentAux < (float)_cost || base.LostSignalHandler.Lost)
			{
				return;
			}
			global::Interactables.Interobjects.ElevatorManager.ElevatorGroup elevatorGroup = (global::Interactables.Interobjects.ElevatorManager.ElevatorGroup)reader.ReadByte();
			if (!global::Interactables.Interobjects.ElevatorDoor.AllElevatorDoors.TryGetValue(elevatorGroup, out var value) || global::Utils.NonAllocLINQ.ListExtensions.Any(value, (global::Interactables.Interobjects.ElevatorDoor x) => x.ActiveLocks != 0))
			{
				return;
			}
			global::MapGeneration.RoomIdentifier curRoom = base.CurrentCamSync.CurrentCamera.Room;
			if (!global::Utils.NonAllocLINQ.ListExtensions.TryGetFirst(value, (global::Interactables.Interobjects.ElevatorDoor x) => x.Rooms.Contains(curRoom), out var first))
			{
				return;
			}
			bool targetState = first.TargetState;
			int num = first.TargetPanel.AssignedChamber.CurrentLevel + 1;
			if (global::Interactables.Interobjects.ElevatorManager.TrySetDestination(elevatorGroup, num % value.Count))
			{
				base.AuxManager.CurrentAux -= _cost;
				value.ForEach(delegate(global::Interactables.Interobjects.ElevatorDoor x)
				{
					base.RewardManager.MarkRooms(x.Rooms);
				});
				ServerSendRpc(toAll: false);
				if (targetState)
				{
					global::PlayerRoles.PlayableScps.Scp079.Scp079ElevatorStateChanger.OnServerElevatorDoorClosed?.Invoke(base.ScpRole, first);
				}
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_confirmationSound, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.DefaultSfx, 0f);
		}

		public override void OnFailMessageAssigned()
		{
			_failedReason = ErrorCode;
		}
	}
}
