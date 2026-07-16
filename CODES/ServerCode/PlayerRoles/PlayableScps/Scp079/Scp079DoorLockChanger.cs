namespace PlayerRoles.PlayableScps.Scp079
{
	public class Scp079DoorLockChanger : global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility, global::GameObjectPools.IPoolResettable
	{
		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _costOverTime;

		[global::UnityEngine.SerializeField]
		private int _startCost;

		[global::UnityEngine.SerializeField]
		private float _reLockCooldown;

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _lockedDoors = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> _doorsToUnlock = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		private readonly global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.DoorUtils.DoorVariant, double> _toggleTimes = new global::System.Collections.Generic.Dictionary<global::Interactables.Interobjects.DoorUtils.DoorVariant, double>();

		private static string _lockText;

		private static string _unlockText;

		private static string _cooldownText;

		private global::Interactables.Interobjects.DoorUtils.DoorVariant _failedDoor;

		private bool _syncvarsDirty;

		private int _prevLockedCount;

		private const global::Interactables.Interobjects.DoorUtils.DoorLockReason LockReason = global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079;

		public int TotalLocked => _lockedDoors.Count;

		public override ActionName ActivationKey => ActionName.Zoom;

		public override float AuxRegenMultiplier
		{
			get
			{
				if (_lockedDoors.Count <= 0)
				{
					return 1f;
				}
				return 0f;
			}
		}

		public override string AbilityName => string.Format(IsLockedBy079 ? _unlockText : _lockText, GetCostForDoor(TargetAction, LastDoor));

		public override bool IsReady
		{
			get
			{
				if (GetRemainingCooldown(LastDoor) == 0f)
				{
					return base.IsReady;
				}
				return false;
			}
		}

		public override string FailMessage
		{
			get
			{
				if (global::PlayerRoles.PlayableScps.Scp079.Scp079LockdownRoomAbility.IsLockedDown(_failedDoor))
				{
					return _cooldownText;
				}
				float remainingCooldown = GetRemainingCooldown(_failedDoor);
				if (remainingCooldown <= 0f)
				{
					return base.FailMessage;
				}
				int secondsRemaining = global::UnityEngine.Mathf.CeilToInt(remainingCooldown);
				return _cooldownText + "\n" + base.AuxManager.GenerateCustomETA(secondsRemaining);
			}
		}

		protected override global::Interactables.Interobjects.DoorUtils.DoorAction TargetAction
		{
			get
			{
				if (!IsLockedBy079)
				{
					return global::Interactables.Interobjects.DoorUtils.DoorAction.Locked;
				}
				return global::Interactables.Interobjects.DoorUtils.DoorAction.Unlocked;
			}
		}

		private bool IsLockedBy079 => global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast((global::Interactables.Interobjects.DoorUtils.DoorLockReason)LastDoor.ActiveLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079);

		private double CurrentTime => global::Mirror.NetworkTime.time;

		public static event global::System.Action<global::PlayerRoles.PlayableScps.Scp079.Scp079Role, global::Interactables.Interobjects.DoorUtils.DoorVariant> OnServerDoorLocked;

		private float GetRemainingCooldown(global::Interactables.Interobjects.DoorUtils.DoorVariant dv)
		{
			if (_lockedDoors.Contains(dv) || !_toggleTimes.TryGetValue(dv, out var value))
			{
				return 0f;
			}
			double num = value + (double)_reLockCooldown - CurrentTime;
			return global::UnityEngine.Mathf.Max(0f, (float)num);
		}

		protected override int GetCostForDoor(global::Interactables.Interobjects.DoorUtils.DoorAction action, global::Interactables.Interobjects.DoorUtils.DoorVariant door)
		{
			if (action != global::Interactables.Interobjects.DoorUtils.DoorAction.Locked)
			{
				return 0;
			}
			return _startCost;
		}

		protected virtual void OnDestroy()
		{
			ServerUnlockAll();
		}

		protected override void Start()
		{
			base.Start();
			_lockText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.LockDoor);
			_unlockText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.UnlockDoor);
			_cooldownText = Translations.Get(global::PlayerRoles.PlayableScps.Scp079.Scp079HudTranslation.DoorLockCooldown);
			base.LostSignalHandler.OnStatusChanged += delegate
			{
				if (global::Mirror.NetworkServer.active && base.LostSignalHandler.Lost)
				{
					ServerUnlockAll();
				}
			};
		}

		protected override void Update()
		{
			base.Update();
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			global::PlayerRoles.PlayableScps.Scp079.Cameras.Scp079Camera currentCamera = base.CurrentCamSync.CurrentCamera;
			float num = base.AuxManager.CurrentAux;
			bool flag = false;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant lockedDoor in _lockedDoors)
			{
				float time = (float)(CurrentTime - _toggleTimes[lockedDoor]);
				num -= _costOverTime.Evaluate(time) * global::UnityEngine.Time.deltaTime;
				if (!global::PlayerRoles.PlayableScps.Scp079.Scp079DoorAbility.ValidateAction(global::Interactables.Interobjects.DoorUtils.DoorAction.Locked, lockedDoor, currentCamera))
				{
					_doorsToUnlock.Add(lockedDoor);
					flag = true;
				}
			}
			base.AuxManager.CurrentAux = num;
			if (num <= 0f)
			{
				ServerUnlockAll();
			}
			else if (flag)
			{
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(_doorsToUnlock, delegate(global::Interactables.Interobjects.DoorUtils.DoorVariant x)
				{
					SetDoorLock(x, lockState: false);
				});
				_doorsToUnlock.Clear();
			}
			if (_syncvarsDirty)
			{
				ServerSendRpc(toAll: false);
				_syncvarsDirty = false;
			}
		}

		public void ServerUnlockAll()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> doorsToRemove = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant lockedDoor in _lockedDoors)
			{
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079UnlockDoor, base.Owner, lockedDoor))
				{
					lockedDoor.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079, newState: false);
					_toggleTimes[lockedDoor] = CurrentTime;
					doorsToRemove.Add(lockedDoor);
				}
			}
			_syncvarsDirty = true;
			_lockedDoors.RemoveWhere((global::Interactables.Interobjects.DoorUtils.DoorVariant x) => doorsToRemove.Contains(x));
		}

		public bool SetDoorLock(global::Interactables.Interobjects.DoorUtils.DoorVariant door, bool lockState, bool skipChecks = false)
		{
			door.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079, lockState);
			_syncvarsDirty = true;
			if (lockState)
			{
				if (!_lockedDoors.Add(door) && !skipChecks)
				{
					return false;
				}
			}
			else if (!_lockedDoors.Remove(door) && !skipChecks)
			{
				return false;
			}
			_toggleTimes[door] = CurrentTime;
			return true;
		}

		public override void OnFailMessageAssigned()
		{
			base.OnFailMessageAssigned();
			_failedDoor = LastDoor;
		}

		public override void ClientWriteCmd(global::Mirror.NetworkWriter writer)
		{
			base.ClientWriteCmd(writer);
			global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, LastDoor.netId);
		}

		public override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (!global::Mirror.NetworkIdentity.spawned.TryGetValue(global::Mirror.NetworkReaderExtensions.ReadUInt32(reader), out var value) || !value.TryGetComponent<global::Interactables.Interobjects.DoorUtils.DoorVariant>(out LastDoor) || !IsReady)
			{
				return;
			}
			if (TargetAction == global::Interactables.Interobjects.DoorUtils.DoorAction.Locked)
			{
				if (!(GetRemainingCooldown(LastDoor) > 0f) && !global::PlayerRoles.PlayableScps.Scp079.Scp079LockdownRoomAbility.IsLockedDown(LastDoor) && !base.LostSignalHandler.Lost && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079LockDoor, base.Owner, LastDoor))
				{
					SetDoorLock(LastDoor, lockState: true);
					base.RewardManager.MarkRooms(LastDoor.Rooms);
					global::PlayerRoles.PlayableScps.Scp079.Scp079DoorLockChanger.OnServerDoorLocked?.Invoke(base.ScpRole, LastDoor);
					base.AuxManager.CurrentAux -= GetCostForDoor(global::Interactables.Interobjects.DoorUtils.DoorAction.Locked, LastDoor);
				}
			}
			else if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp079UnlockDoor, base.Owner, LastDoor))
			{
				SetDoorLock(LastDoor, lockState: false);
			}
		}

		public override void ServerWriteRpc(global::Mirror.NetworkWriter writer)
		{
			base.ServerWriteRpc(writer);
			foreach (global::System.Collections.Generic.KeyValuePair<global::Interactables.Interobjects.DoorUtils.DoorVariant, double> toggleTime in _toggleTimes)
			{
				if (_lockedDoors.Contains(toggleTime.Key))
				{
					global::Mirror.NetworkWriterExtensions.WriteDouble(writer, toggleTime.Value);
					global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, toggleTime.Key.netId);
				}
				else if (toggleTime.Value > CurrentTime - (double)_reLockCooldown)
				{
					global::Mirror.NetworkWriterExtensions.WriteDouble(writer, toggleTime.Value * -1.0);
					global::Mirror.NetworkWriterExtensions.WriteUInt32(writer, toggleTime.Key.netId);
				}
			}
		}

		public override void ClientProcessRpc(global::Mirror.NetworkReader reader)
		{
			int num = 0;
			_lockedDoors.Clear();
			_toggleTimes.Clear();
			while (reader.Position < reader.Length)
			{
				double num2 = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				if (!global::Mirror.NetworkIdentity.spawned.TryGetValue(global::Mirror.NetworkReaderExtensions.ReadUInt32(reader), out var value) || !value.TryGetComponent<global::Interactables.Interobjects.DoorUtils.DoorVariant>(out var component))
				{
					break;
				}
				if (num2 >= 0.0)
				{
					if (_lockedDoors.Add(component))
					{
						num++;
					}
					_toggleTimes[component] = num2;
				}
				else
				{
					_toggleTimes[component] = num2 * -1.0;
				}
			}
			if (_prevLockedCount != num)
			{
				base.ClientProcessRpc(reader);
				_prevLockedCount = num;
			}
		}

		public override void ResetObject()
		{
			base.ResetObject();
			ServerUnlockAll();
			_lockedDoors.Clear();
			_toggleTimes.Clear();
			_prevLockedCount = 0;
		}
	}
}
