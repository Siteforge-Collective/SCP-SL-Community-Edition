using System;
using System.Collections.Generic;
using Interactables.Verification;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PluginAPI.Events;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace Interactables.Interobjects.DoorUtils
{
    public abstract class DoorVariant : NetworkBehaviour, IServerInteractable, IInteractable
    {
        [Flags]
        private enum CollisionsDisablingReasons : byte
        {
            DoorClosing = 1,
            Scp106 = 2
        }

        public static readonly HashSet<DoorVariant> AllDoors = new HashSet<DoorVariant>();

        public static readonly Dictionary<RoomIdentifier, HashSet<DoorVariant>> DoorsByRoom = new Dictionary<RoomIdentifier, HashSet<DoorVariant>>();

        [SyncVar]
        public bool TargetState;

        [SyncVar]
        public ushort ActiveLocks;

        [SyncVar]
        public byte DoorId;

        public bool CanSeeThrough;

        public DoorPermissions RequiredPermissions = new DoorPermissions();

        private bool _prevState;

        private ushort _prevLock;

        private byte _existenceCooldown = byte.MaxValue;

        private ReferenceHub _triggerPlayer;

        private CollisionsDisablingReasons _collidersStatus;

        private BoxCollider[] _colliders;

        private bool _collidersActivationPending;

        private const int WorldDirCount = 4;

        private static int _serverDoorIdClock;

        private static readonly RoomIdentifier[] RoomsNonAlloc = new RoomIdentifier[4];

        private static readonly Vector3[] WorldDirections = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        public IVerificationRule VerificationRule => StandardDistanceVerification.Default;

        public RoomIdentifier[] Rooms { get; private set; }

        [Server]
        public void ServerInteract(ReferenceHub ply, byte colliderId)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.DoorUtils.DoorVariant::ServerInteract(ReferenceHub,System.Byte)' called when server was not active");
                return;
            }

            if (ActiveLocks > 0 && !ply.serverRoles.BypassMode)
            {
                DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)ActiveLocks);

                bool canInteract = mode.HasFlagFast(DoorLockMode.ScpOverride) && ply.IsSCP() ||
                                  (TargetState ? mode.HasFlagFast(DoorLockMode.CanClose) : mode.HasFlagFast(DoorLockMode.CanOpen));

                if (!canInteract || mode == DoorLockMode.FullLock)
                {
                    if (EventManager.ExecuteEvent(new PlayerInteractDoorEvent(ply, this, false)))
                    {
                        LockBypassDenied(ply, colliderId);
                    }
                    return;
                }
            }

            if (!AllowInteracting(ply, colliderId))
                return;

            bool hasPermission = ply.GetRoleId() == RoleTypeId.Scp079 ||
                                 RequiredPermissions.CheckPermissions(ply.inventory.CurInstance, ply);

            if (EventManager.ExecuteEvent(new PlayerInteractDoorEvent(ply, this, hasPermission)))
            {
                if (hasPermission)
                {
                    TargetState = !TargetState;
                    _triggerPlayer = ply;
                }
                else
                {
                    PermissionsDenied(ply, colliderId);
                    DoorEvents.TriggerAction(this, DoorAction.AccessDenied, ply);
                }
            }
        }

        [Server]
        public void ServerChangeLock(DoorLockReason reason, bool newState)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void Interactables.Interobjects.DoorUtils.DoorVariant::ServerChangeLock(Interactables.Interobjects.DoorUtils.DoorLockReason,System.Boolean)' called when server was not active");
                return;
            }

            DoorLockReason current = (DoorLockReason)ActiveLocks;
            DoorLockReason updated = newState ? (current | reason) : (current & ~reason);

            if (ActiveLocks != (ushort)updated)
            {
                if (newState && ActiveLocks == 0)
                {
                    DoorEvents.TriggerAction(this, DoorAction.Locked, null);
                }
                else if (!newState && (ushort)updated == 0)
                {
                    DoorEvents.TriggerAction(this, DoorAction.Unlocked, null);
                }

                ActiveLocks = (ushort)updated;
            }
        }

        public abstract void LockBypassDenied(ReferenceHub ply, byte colliderId);

        public abstract bool AnticheatPassageApproved();

        public abstract void PermissionsDenied(ReferenceHub ply, byte colliderId);

        public abstract bool AllowInteracting(ReferenceHub ply, byte colliderId);

        public abstract float GetExactState();

        public abstract bool IsConsideredOpen();

        protected virtual void LockChanged(ushort prevValue) { }

        internal virtual void TargetStateChanged() { }

        protected virtual void Start()
        {
            if (NetworkServer.active)
            {
                _colliders = GetComponentsInChildren<BoxCollider>();
                _serverDoorIdClock++;
                if (_serverDoorIdClock > 255)
                    _serverDoorIdClock = 1;
                DoorId = (byte)_serverDoorIdClock;
            }

            AllDoors.Add(this);

            if (SeedSynchronizer.MapGenerated)
                RegisterRooms();
        }

        protected virtual void OnDestroy()
        {
            AllDoors.Remove(this);

            if (Rooms == null)
                return;

            foreach (RoomIdentifier room in Rooms)
            {
                if (DoorsByRoom.TryGetValue(room, out var set))
                    set.Remove(this);
            }
        }

        protected virtual void Update()
        {
            if (_existenceCooldown > 0)
            {
                _existenceCooldown--;
                return;
            }

            if (_prevLock != ActiveLocks)
            {
                LockChanged(_prevLock);
                _prevLock = ActiveLocks;
            }

            if (_prevState != TargetState)
            {
                TargetStateChanged();
                DoorEvents.TriggerAction(this, TargetState ? DoorAction.Opened : DoorAction.Closed, _triggerPlayer);
                _triggerPlayer = null;

                if (NetworkServer.active)
                {
                    if (TargetState)
                    {
                        _collidersStatus &= ~CollisionsDisablingReasons.DoorClosing;
                    }
                    else
                    {
                        _collidersStatus |= CollisionsDisablingReasons.DoorClosing;
                        _collidersActivationPending = true;
                    }

                    SetColliders();
                }

                _prevState = TargetState;
            }

            if (_collidersActivationPending && !AnticheatPassageApproved())
            {
                _collidersActivationPending = false;
                _collidersStatus &= ~CollisionsDisablingReasons.DoorClosing;
                SetColliders();
            }
        }

        private void SetColliders()
        {
            if (_colliders == null)
                return;

            bool isTrigger = _collidersStatus != 0;
            foreach (BoxCollider collider in _colliders)
                collider.isTrigger = isTrigger;
        }

        private void RegisterRooms()
        {
            Vector3 pos = transform.position;
            int count = 0;

            for (int i = 0; i < WorldDirCount; i++)
            {
                Vector3Int coords = RoomIdUtils.PositionToCoords(pos + WorldDirections[i]);
                if (RoomIdentifier.RoomsByCoordinates.TryGetValue(coords, out RoomIdentifier room) &&
                    DoorsByRoom.GetOrAdd(room, () => new HashSet<DoorVariant>()).Add(this))
                {
                    RoomsNonAlloc[count++] = room;
                }
            }

            Rooms = new RoomIdentifier[count];
            Array.Copy(RoomsNonAlloc, Rooms, count);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SeedSynchronizer.OnMapGenerated += () =>
            {
                AllDoors.ForEach(door => door.RegisterRooms());
            };

            RoomIdentifier.OnRemoved += room =>
            {
                DoorsByRoom.Remove(room);
            };
        }

        protected DoorVariant()
        {
            _existenceCooldown = byte.MaxValue;
        }
    }
}