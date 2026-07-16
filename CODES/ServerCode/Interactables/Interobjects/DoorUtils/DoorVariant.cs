namespace Interactables.Interobjects.DoorUtils
{
	public abstract class DoorVariant : global::Mirror.NetworkBehaviour, global::Interactables.IServerInteractable, global::Interactables.IInteractable
	{
		[global::System.Flags]
		private enum CollisionsDisablingReasons : byte
		{
			DoorClosing = 1,
			Scp106 = 2
		}

		public static readonly global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant> AllDoors = new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>();

		public static readonly global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>> DoorsByRoom = new global::System.Collections.Generic.Dictionary<global::MapGeneration.RoomIdentifier, global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>>();

		[global::Mirror.SyncVar]
		public bool TargetState;

		[global::Mirror.SyncVar]
		public ushort ActiveLocks;

		[global::Mirror.SyncVar]
		public byte DoorId;

		public bool CanSeeThrough;

		public global::Interactables.Interobjects.DoorUtils.DoorPermissions RequiredPermissions;

		private bool _prevState;

		private ushort _prevLock;

		private byte _existenceCooldown = byte.MaxValue;

		private ReferenceHub _triggerPlayer;

		private global::Interactables.Interobjects.DoorUtils.DoorVariant.CollisionsDisablingReasons _collidersStatus;

		private global::UnityEngine.BoxCollider[] _colliders;

		private bool _collidersActivationPending;

		private const int WorldDirCount = 4;

		private static int _serverDoorIdClock;

		private static readonly global::MapGeneration.RoomIdentifier[] RoomsNonAlloc = new global::MapGeneration.RoomIdentifier[4];

		private static readonly global::UnityEngine.Vector3[] WorldDirections = new global::UnityEngine.Vector3[4]
		{
			global::UnityEngine.Vector3.forward,
			global::UnityEngine.Vector3.back,
			global::UnityEngine.Vector3.left,
			global::UnityEngine.Vector3.right
		};

		public global::Interactables.Verification.IVerificationRule VerificationRule => global::Interactables.Verification.StandardDistanceVerification.Default;

		public global::MapGeneration.RoomIdentifier[] Rooms { get; private set; }

		public bool NetworkTargetState
		{
			get
			{
				return TargetState;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref TargetState))
				{
					bool targetState = TargetState;
					SetSyncVar(value, ref TargetState, 1uL);
				}
			}
		}

		public ushort NetworkActiveLocks
		{
			get
			{
				return ActiveLocks;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref ActiveLocks))
				{
					ushort activeLocks = ActiveLocks;
					SetSyncVar(value, ref ActiveLocks, 2uL);
				}
			}
		}

		public byte NetworkDoorId
		{
			get
			{
				return DoorId;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref DoorId))
				{
					byte doorId = DoorId;
					SetSyncVar(value, ref DoorId, 4uL);
				}
			}
		}

		[global::Mirror.Server]
		public void ServerInteract(ReferenceHub ply, byte colliderId)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.DoorUtils.DoorVariant::ServerInteract(ReferenceHub,System.Byte)' called when server was not active");
				return;
			}
			if (ActiveLocks > 0 && !ply.serverRoles.BypassMode)
			{
				global::Interactables.Interobjects.DoorUtils.DoorLockMode mode = global::Interactables.Interobjects.DoorUtils.DoorLockUtils.GetMode((global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks);
				if ((!mode.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanClose) || !mode.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanOpen)) && (!mode.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockMode.ScpOverride) || !global::PlayerRoles.PlayerRolesUtils.IsSCP(ply)) && (mode == global::Interactables.Interobjects.DoorUtils.DoorLockMode.FullLock || (TargetState && !mode.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanClose)) || (!TargetState && !mode.HasFlagFast(global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanOpen))))
				{
					LockBypassDenied(ply, colliderId);
					return;
				}
			}
			if (!AllowInteracting(ply, colliderId))
			{
				return;
			}
			bool flag = global::PlayerRoles.PlayerRolesUtils.GetRoleId(ply) == global::PlayerRoles.RoleTypeId.Scp079 || RequiredPermissions.CheckPermissions(ply.inventory.CurInstance, ply);
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerInteractDoor, ply, this, flag))
			{
				if (flag)
				{
					NetworkTargetState = !TargetState;
					_triggerPlayer = ply;
				}
				else
				{
					PermissionsDenied(ply, colliderId);
					global::Interactables.Interobjects.DoorUtils.DoorEvents.TriggerAction(this, global::Interactables.Interobjects.DoorUtils.DoorAction.AccessDenied, ply);
				}
			}
		}

		[global::Mirror.Server]
		public void ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason reason, bool newState)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.DoorUtils.DoorVariant::ServerChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason,System.Boolean)' called when server was not active");
				return;
			}
			global::Interactables.Interobjects.DoorUtils.DoorLockReason activeLocks = (global::Interactables.Interobjects.DoorUtils.DoorLockReason)ActiveLocks;
			activeLocks = ((!newState) ? ((global::Interactables.Interobjects.DoorUtils.DoorLockReason)((uint)activeLocks & (uint)(ushort)(~(int)reason))) : (activeLocks | reason));
			if ((uint)ActiveLocks != (uint)activeLocks)
			{
				if (newState)
				{
					if (ActiveLocks == 0)
					{
						global::Interactables.Interobjects.DoorUtils.DoorEvents.TriggerAction(this, global::Interactables.Interobjects.DoorUtils.DoorAction.Locked, null);
					}
				}
				else if (ActiveLocks != 0)
				{
					global::Interactables.Interobjects.DoorUtils.DoorEvents.TriggerAction(this, global::Interactables.Interobjects.DoorUtils.DoorAction.Unlocked, null);
				}
			}
			NetworkActiveLocks = (ushort)activeLocks;
		}

		public abstract void LockBypassDenied(ReferenceHub ply, byte colliderId);

		public abstract bool AnticheatPassageApproved();

		public abstract void PermissionsDenied(ReferenceHub ply, byte colliderId);

		public abstract bool AllowInteracting(ReferenceHub ply, byte colliderId);

		public abstract float GetExactState();

		public abstract bool IsConsideredOpen();

		protected virtual void LockChanged(ushort prevValue)
		{
		}

		internal virtual void TargetStateChanged()
		{
		}

		protected virtual void Start()
		{
			if (global::Mirror.NetworkServer.active)
			{
				_colliders = GetComponentsInChildren<global::UnityEngine.BoxCollider>();
				_serverDoorIdClock++;
				if (_serverDoorIdClock > 255)
				{
					_serverDoorIdClock = 1;
				}
				NetworkDoorId = (byte)_serverDoorIdClock;
			}
			AllDoors.Add(this);
			if (global::MapGeneration.SeedSynchronizer.MapGenerated)
			{
				RegisterRooms();
			}
		}

		protected virtual void OnDestroy()
		{
			AllDoors.Remove(this);
			if (Rooms == null)
			{
				return;
			}
			global::MapGeneration.RoomIdentifier[] rooms = Rooms;
			foreach (global::MapGeneration.RoomIdentifier key in rooms)
			{
				if (DoorsByRoom.TryGetValue(key, out var value))
				{
					value.Remove(this);
				}
			}
		}

		protected virtual void Update()
		{
			if (_existenceCooldown == 0)
			{
				if (_prevLock != ActiveLocks)
				{
					LockChanged(_prevLock);
					_prevLock = ActiveLocks;
				}
				if (_prevState != TargetState)
				{
					TargetStateChanged();
					global::Interactables.Interobjects.DoorUtils.DoorEvents.TriggerAction(this, (!TargetState) ? global::Interactables.Interobjects.DoorUtils.DoorAction.Closed : global::Interactables.Interobjects.DoorUtils.DoorAction.Opened, _triggerPlayer);
					_triggerPlayer = null;
					if (global::Mirror.NetworkServer.active)
					{
						if (TargetState)
						{
							_collidersStatus &= ~global::Interactables.Interobjects.DoorUtils.DoorVariant.CollisionsDisablingReasons.DoorClosing;
						}
						else
						{
							_collidersStatus |= global::Interactables.Interobjects.DoorUtils.DoorVariant.CollisionsDisablingReasons.DoorClosing;
							_collidersActivationPending = true;
						}
						SetColliders();
					}
					_prevState = TargetState;
				}
			}
			else
			{
				_existenceCooldown--;
			}
			if (_collidersActivationPending && !AnticheatPassageApproved())
			{
				_collidersActivationPending = false;
				_collidersStatus &= ~global::Interactables.Interobjects.DoorUtils.DoorVariant.CollisionsDisablingReasons.DoorClosing;
				SetColliders();
			}
		}

		private void SetColliders()
		{
			global::UnityEngine.BoxCollider[] colliders = _colliders;
			for (int i = 0; i < colliders.Length; i++)
			{
				colliders[i].isTrigger = _collidersStatus != (global::Interactables.Interobjects.DoorUtils.DoorVariant.CollisionsDisablingReasons)0;
			}
		}

		private void RegisterRooms()
		{
			global::UnityEngine.Vector3 position = base.transform.position;
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				global::UnityEngine.Vector3Int key = global::MapGeneration.RoomIdUtils.PositionToCoords(position + WorldDirections[i]);
				if (global::MapGeneration.RoomIdentifier.RoomsByCoordinates.TryGetValue(key, out var value) && DoorsByRoom.GetOrAdd(value, () => new global::System.Collections.Generic.HashSet<global::Interactables.Interobjects.DoorUtils.DoorVariant>()).Add(this))
				{
					RoomsNonAlloc[num] = value;
					num++;
				}
			}
			Rooms = new global::MapGeneration.RoomIdentifier[num];
			global::System.Array.Copy(RoomsNonAlloc, Rooms, num);
		}

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::MapGeneration.SeedSynchronizer.OnMapGenerated += delegate
			{
				global::Utils.NonAllocLINQ.HashsetExtensions.ForEach(AllDoors, delegate(global::Interactables.Interobjects.DoorUtils.DoorVariant x)
				{
					x.RegisterRooms();
				});
			};
			global::MapGeneration.RoomIdentifier.OnRemoved += delegate(global::MapGeneration.RoomIdentifier x)
			{
				DoorsByRoom.Remove(x);
			};
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, TargetState);
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, ActiveLocks);
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, DoorId);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, TargetState);
				result = true;
			}
			if ((base.syncVarDirtyBits & 2L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteUInt16(writer, ActiveLocks);
				result = true;
			}
			if ((base.syncVarDirtyBits & 4L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteByte(writer, DoorId);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				bool targetState = TargetState;
				NetworkTargetState = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				ushort activeLocks = ActiveLocks;
				NetworkActiveLocks = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
				byte doorId = DoorId;
				NetworkDoorId = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				bool targetState2 = TargetState;
				NetworkTargetState = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
			if ((num & 2L) != 0L)
			{
				ushort activeLocks2 = ActiveLocks;
				NetworkActiveLocks = global::Mirror.NetworkReaderExtensions.ReadUInt16(reader);
			}
			if ((num & 4L) != 0L)
			{
				byte doorId2 = DoorId;
				NetworkDoorId = global::Mirror.NetworkReaderExtensions.ReadByte(reader);
			}
		}
	}
}
