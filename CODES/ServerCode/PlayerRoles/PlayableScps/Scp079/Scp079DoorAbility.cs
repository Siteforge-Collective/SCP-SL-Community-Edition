namespace PlayerRoles.PlayableScps.Scp079
{
	public abstract class Scp079DoorAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase
	{
		protected global::Interactables.Interobjects.DoorUtils.DoorVariant LastDoor;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _confirmationSound;

		private static string _deniedText;

		private int _lastCost;

		private bool _lastActionValid;

		private int _failMessageAux;

		private bool _failMessageDenied;

		public override bool IsVisible
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
				{
					return false;
				}
				if (global::PlayerRoles.PlayableScps.Scp079.Overcons.OverconManager.Singleton.HighlightedOvercon is global::PlayerRoles.PlayableScps.Scp079.Overcons.DoorOvercon doorOvercon && doorOvercon != null)
				{
					LastDoor = doorOvercon.Target;
					return true;
				}
				return false;
			}
		}

		public override bool IsReady
		{
			get
			{
				global::Interactables.Interobjects.DoorUtils.DoorAction targetAction = TargetAction;
				_lastActionValid = ValidateAction(targetAction, LastDoor, base.CurrentCamSync.CurrentCamera);
				_lastCost = GetCostForDoor(targetAction, LastDoor);
				if (_lastActionValid)
				{
					return (float)_lastCost <= base.AuxManager.CurrentAux;
				}
				return false;
			}
		}

		public override string FailMessage
		{
			get
			{
				if (!_failMessageDenied)
				{
					if (!(base.AuxManager.CurrentAux < (float)_failMessageAux))
					{
						return null;
					}
					return GetNoAuxMessage(_failMessageAux);
				}
				return _deniedText;
			}
		}

		protected abstract global::Interactables.Interobjects.DoorUtils.DoorAction TargetAction { get; }

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::Interactables.Interobjects.DoorUtils.DoorVariant> OnServerAnyDoorInteraction;

		protected abstract int GetCostForDoor(global::Interactables.Interobjects.DoorUtils.DoorAction action, global::Interactables.Interobjects.DoorUtils.DoorVariant door);

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		protected override void Start()
		{
			base.Start();
			_deniedText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.DoorAccessDenied);
			base.CurrentCamSync.OnCameraChanged += delegate
			{
				_failMessageAux = 0;
				_failMessageDenied = false;
			};
		}

		public override void OnFailMessageAssigned()
		{
			_failMessageDenied = !_lastActionValid;
			_failMessageAux = _lastCost;
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			global::AudioPooling.AudioSourcePoolManager.PlaySound(_confirmationSound, null, 1f, 1f, FalloffType.Exponential, global::AudioPooling.AudioMixerChannelType.DefaultSfx, 0f);
		}

		public static bool ValidateAction(global::Interactables.Interobjects.DoorUtils.DoorAction action, global::Interactables.Interobjects.DoorUtils.DoorVariant door, global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera)
		{
			if (!CheckVisibility(door, currentCamera))
			{
				return false;
			}
			if (global::Mirror.NetworkServer.active && AlphaWarheadController.InProgress)
			{
				return false;
			}
			global::Interactables.Interobjects.DoorUtils.DoorLockMode mode = global::Interactables.Interobjects.DoorUtils.DoorLockUtils.GetMode((global::Interactables.Interobjects.DoorUtils.DoorLockReason)door.ActiveLocks);
			if (door is global::Interactables.Interobjects.DoorUtils.IDamageableDoor damageableDoor && damageableDoor.IsDestroyed)
			{
				return false;
			}
			if (global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(mode, global::Interactables.Interobjects.DoorUtils.DoorLockMode.ScpOverride))
			{
				return true;
			}
			switch (action)
			{
			case global::Interactables.Interobjects.DoorUtils.DoorAction.Opened:
				return global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(mode, global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanOpen);
			case global::Interactables.Interobjects.DoorUtils.DoorAction.Closed:
				return global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(mode, global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanClose);
			case global::Interactables.Interobjects.DoorUtils.DoorAction.Locked:
				if (mode != global::Interactables.Interobjects.DoorUtils.DoorLockMode.FullLock)
				{
					return !(door is global::Interactables.Interobjects.CheckpointDoor);
				}
				return false;
			case global::Interactables.Interobjects.DoorUtils.DoorAction.Unlocked:
				return true;
			default:
				return false;
			}
		}

		public static bool CheckVisibility(global::Interactables.Interobjects.DoorUtils.DoorVariant door, global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera)
		{
			global::MapGeneration.RoomIdentifier[] rooms = door.Rooms;
			for (int i = 0; i < rooms.Length; i++)
			{
				if (!(rooms[i] != currentCamera.Room))
				{
					if (door is global::Interactables.Interobjects.DoorUtils.INonInteractableDoor nonInteractableDoor)
					{
						return !nonInteractableDoor.IgnoreLockdowns;
					}
					return true;
				}
			}
			return false;
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger.OnServerDoorLocked += delegate(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
			{
				global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction?.Invoke(role, dv);
			};
			global::PlayerRoles.PlayableScps.Scp079.Scp079DoorStateChanger.OnServerDoorToggled += delegate(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
			{
				global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction?.Invoke(role, dv);
			};
			global::PlayerRoles.PlayableScps.Scp079.Scp079ElevatorStateChanger.OnServerElevatorDoorClosed += delegate(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, global::Interactables.Interobjects.ElevatorDoor dv)
			{
				global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction?.Invoke(role, dv);
			};
			global::PlayerRoles.PlayableScps.Scp079.Scp079LockdownRoomAbility.OnServerDoorLocked += delegate(global::PlayerRoles.PlayableScps.Scp079.Scp079Role role, global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
			{
				global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.OnServerAnyDoorInteraction?.Invoke(role, dv);
			};
		}
	}
}
