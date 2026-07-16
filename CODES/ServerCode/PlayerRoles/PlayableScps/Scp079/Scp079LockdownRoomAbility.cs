namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079LockdownRoomAbility : global::PlayerRoles.PlayableScps.Scp079.Scp079KeyAbilityBase, global::GameObjectPools.IPoolResettable, global::PlayerRoles.PlayableScps.Scp079.GUI.IScp079LevelUpNotifier
	{
		private enum ValidationError
		{
			None = 0,
			Unknown = 1,
			NotEnoughAux = 6,
			TierTooLow = 8,
			Cooldown = 31,
			NoDoors = 32
		}

		[global::UnityEngine.SerializeField]
		private int _minimalTierIndex;

		[global::UnityEngine.SerializeField]
		private float[] _regenerationPerTier;

		[global::UnityEngine.SerializeField]
		private float _lockdownDuration;

		[global::UnityEngine.SerializeField]
		private float _cooldown;

		[global::UnityEngine.SerializeField]
		private int _cost;

		[global::UnityEngine.SerializeField]
		private float _minStateToClose;

		private string _nameFormat;

		private string _failMessage;

		private string _unlockText;

		private double _nextUseTime;

		private bool _hasFailMessage;

		private bool _lockdownInEffect;

		private global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger _doorLockChanger;

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _roomDoors = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _doorsToLockDown = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _alreadyLockedDown = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private global::MapGeneration.RoomIdentifier _lastLockedRoom;

		public override ActionName ActivationKey => ActionName.Scp079Lockdown;

		public override bool IsReady => ErrorCode == global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;

		public override bool IsVisible
		{
			get
			{
				if (!global::PlayerRoles.PlayableScps.Scp079.GUI.Scp079CursorManager.LockCameras)
				{
					return ErrorCode != global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.HigherTierRequired;
				}
				return false;
			}
		}

		public override string AbilityName => string.Format(_nameFormat, _cost);

		public override string FailMessage
		{
			get
			{
				if (!_hasFailMessage)
				{
					return null;
				}
				switch (ErrorCode)
				{
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom:
					return null;
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux:
					return GetNoAuxMessage(_cost);
				case global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LockdownCooldown:
					return _failMessage + "\n" + base.AuxManager.GenerateCustomETA(global::UnityEngine.Mathf.CeilToInt(RemainingCooldown));
				default:
					return _failMessage;
				}
			}
		}

		public override float AuxRegenMultiplier
		{
			get
			{
				if (RemainingLockdownDuration == 0f)
				{
					return 1f;
				}
				int accessTierIndex = base.TierManager.AccessTierIndex;
				int a = _regenerationPerTier.Length - 1;
				return _regenerationPerTier[global::UnityEngine.Mathf.Min(a, accessTierIndex)];
			}
		}

		private global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation ErrorCode
		{
			get
			{
				if (base.TierManager.AccessTierIndex < _minimalTierIndex)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.HigherTierRequired;
				}
				if (!global::Utils.NonAllocLINQ.HashsetExtensions.Any(_roomDoors, (global::Interactables.Interobjects.DoorUtils.DoorVariant x) => ValidateDoor(x)))
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LockdownNoDoorsError;
				}
				if (RemainingCooldown > 0f)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LockdownCooldown;
				}
				if (base.AuxManager.CurrentAuxFloored < _cost)
				{
					return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.NotEnoughAux;
				}
				return global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom;
			}
		}

		private float RemainingCooldown
		{
			get
			{
				return global::UnityEngine.Mathf.Max(0f, (float)(_nextUseTime - global::Mirror.NetworkTime.time));
			}
			set
			{
				_nextUseTime = global::Mirror.NetworkTime.time + (double)value;
			}
		}

		private float RemainingLockdownDuration => global::UnityEngine.Mathf.Max(0f, (float)(_nextUseTime - (double)_cooldown - global::Mirror.NetworkTime.time));

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::MapGeneration.RoomIdentifier> OnServerLockdown;

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::Interactables.Interobjects.DoorUtils.DoorVariant> OnServerDoorLocked;

		private void ServerInitLockdown()
		{
			_lockdownInEffect = true;
			_lastLockedRoom = base.CurrentCamSync.CurrentCamera.Room;
			_doorsToLockDown.UnionWith(_roomDoors);
			global::PlayerRoles.PlayableScps.Scp079.Scp079LockdownRoomAbility.OnServerLockdown?.Invoke(base.ScpRole, _lastLockedRoom);
		}

		private void ServerCancelLockdown()
		{
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079CancelRoomLockdown, base.Owner, _lastLockedRoom))
			{
				return;
			}
			_lockdownInEffect = false;
			RemainingCooldown = _cooldown;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in _alreadyLockedDown)
			{
				_doorLockChanger.SetDoorLock(item, lockState: false, skipChecks: true);
				item.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown079, newState: false);
			}
			_doorsToLockDown.Clear();
			_alreadyLockedDown.Clear();
			ServerSendRpc(toAll: false);
		}

		private bool ValidateDoor(global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = base.CurrentCamSync.CurrentCamera;
			if (global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.ValidateAction(global::Interactables.Interobjects.DoorUtils.DoorAction.Closed, dv, currentCamera))
			{
				return global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.ValidateAction(global::Interactables.Interobjects.DoorUtils.DoorAction.Locked, dv, currentCamera);
			}
			return false;
		}

		protected override void Start()
		{
			base.Start();
			_nameFormat = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Lockdown);
			_unlockText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LockdownAvailable);
			base.CurrentCamSync.OnCameraChanged += delegate
			{
				_hasFailMessage = false;
				_failMessage = null;
				_roomDoors.Clear();
				if (global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(base.CurrentCamSync.CurrentCamera.Room, out var value))
				{
					_roomDoors.UnionWith(value);
				}
			};
			GetSubroutine<global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger>(out _doorLockChanger);
		}

		protected override void Update()
		{
			base.Update();
			if (!_lockdownInEffect || !global::Mirror.NetworkServer.active)
			{
				return;
			}
			if (RemainingLockdownDuration <= 0f)
			{
				ServerCancelLockdown();
				return;
			}
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant item in _doorsToLockDown)
			{
				if (ValidateDoor(item) && !_alreadyLockedDown.Contains(item) && (!item.TargetState || !(item.GetExactState() < _minStateToClose)))
				{
					item.NetworkTargetState = false;
					item.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown079, newState: true);
					_doorLockChanger.SetDoorLock(item, lockState: false, skipChecks: true);
					base.RewardManager.MarkRooms(item.Rooms);
					global::PlayerRoles.PlayableScps.Scp079.Scp079LockdownRoomAbility.OnServerDoorLocked?.Invoke(base.ScpRole, item);
					_alreadyLockedDown.Add(item);
				}
			}
		}

		protected override void Trigger()
		{
			ClientSendCmd();
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (ErrorCode == global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.Zoom && !base.LostSignalHandler.Lost)
			{
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079LockdownRoom, base.Owner, base.CurrentCamSync.CurrentCamera.Room))
				{
					return;
				}
				base.AuxManager.CurrentAux -= _cost;
				RemainingCooldown = _lockdownDuration + _cooldown;
				ServerInitLockdown();
			}
			ServerSendRpc(toAll: false);
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			global::Mirror.NetworkWriterExtensions.WriteDouble(writer, _nextUseTime);
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpc(reader);
			_nextUseTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
		}

		public override void OnFailMessageAssigned()
		{
			base.OnFailMessageAssigned();
			_hasFailMessage = true;
			_failMessage = Translations.Get(ErrorCode);
		}

		public override void ResetObject()
		{
			base.ResetObject();
			if (global::Mirror.NetworkServer.active)
			{
				ServerCancelLockdown();
			}
		}

		public static bool IsLockedDown(global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			return global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)dv.ActiveLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown079);
		}

		public bool WriteLevelUpNotification(global::System.Text.StringBuilder sb, int newLevel)
		{
			if (newLevel != _minimalTierIndex)
			{
				return false;
			}
			sb.AppendFormat(_unlockText, $"[{new ReadableKeyCode(ActivationKey)}]");
			return true;
		}
	}
}
