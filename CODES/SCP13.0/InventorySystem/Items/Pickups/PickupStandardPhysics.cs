using System;
using System.Runtime.CompilerServices;
using Interactables.Interobjects;
using MapGeneration;
using Mirror;
using RelativePositioning;
using UnityEngine;

namespace InventorySystem.Items.Pickups
{
	public class PickupStandardPhysics : PickupPhysicsModule
	{
		public enum FreezingMode
		{
			Default = 0,
			FreezeWhenSleeping = 1,
			NeverFreeze = 2
		}

		private readonly FreezingMode _freezingMode;

		private readonly ItemPickupBase _pickup;

		private ElevatorChamber _trackedChamber;

		private double _serverNextUpdateTime;

		private RelativePosition _lastReceivedRelPos;

		private Quaternion _lastReceivedRelRot;

		private bool _serverPrevSleeping;

		private bool _serverEverDecelerated;

		private float _serverPrevVelSqr;

		private bool _inElevator;

		private bool _isFrozen;

		private byte _serverOrderClock;
        private bool _syncDataDirty;

        private int? _lastReceivedUpdate;

		private float _freezeProgress;

		private const float UpdateCooldown = 0.25f;

		private const float FreezeSpeed = 1.2f;

		private const float FreezePow = 5f;

		private const float UnfrozenLerp = 10f;

		private const float MinWeight = 0.001f;

		private const float UnfreezeVelocity = 2.5f;

		private const float UnfreezeVelocitySqr = 6.25f;

		private const float FrozenTeleportDisSqr = 25f / 64f;

		public Rigidbody Rb { get; private set; }

        protected override ItemPickupBase Pickup => _pickup;

        private Vector3 LastWorldPos => _lastReceivedRelPos.Position;

        private Quaternion LastWorldRot => WaypointBase.GetWorldRotation(_lastReceivedRelPos.WaypointId, _lastReceivedRelRot);

        private bool ClientFrozen
        {
            get
            {
                return _isFrozen;
            }
            set
            {
                _isFrozen = value;
                Rb.isKinematic = value;
                _freezeProgress = 0f;
            }
        }

        private bool ServerSendFreeze
        {
            get
            {
                if (!_serverEverDecelerated)
                {
                    return false;
                }
                return _freezingMode switch
                {
                    FreezingMode.Default => Rb.velocity.sqrMagnitude < 6.25f,
                    FreezingMode.FreezeWhenSleeping => Rb.IsSleeping(),
                    FreezingMode.NeverFreeze => false,
                    _ => throw new InvalidOperationException("Unhandled freezing mode for a pickup: " + _freezingMode),
                };
            }
        }

        public event Action OnParentSetByElevator;

        public PickupStandardPhysics(ItemPickupBase targetPickup, FreezingMode freezingMode = FreezingMode.Default)
        {
            _pickup = targetPickup;
            _freezingMode = freezingMode;
            Rb = Pickup.GetComponent<Rigidbody>();
            Rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            if (NetworkServer.active)
            {
                ServerSetSyncData(ServerWriteRigidbody);
            }
            StaticUnityMethods.OnUpdate += OnUpdate;
            ElevatorChamber.OnElevatorMoved += OnElevatorMoved;
            Pickup.OnInfoChanged += UpdateWeight;
            Pickup.PhysicsModuleSyncData.Callback += OnSyncDataChanged;
        }

        private void UpdateWeight()
        {
            Rb.mass = Mathf.Max(0.001f, Pickup.Info.WeightKg);
        }

        private void OnSyncDataChanged(SyncList<byte>.Operation op, int itemIndex, byte oldItem, byte newItem)
        {
            if (NetworkClient.active && op == SyncList<byte>.Operation.OP_ADD)
            {
                _syncDataDirty = true;
            }
        }

        private void OnUpdate()
        {
            if (NetworkServer.active)
            {
                UpdateServer();
            }
            else
            {
                UpdateClient();
            }
        }

        private void UpdateServer()
        {
            bool flag = Rb.IsSleeping() && _freezingMode != FreezingMode.NeverFreeze;
            if (flag)
            {
                if (_serverPrevSleeping)
                {
                    return;
                }
                _serverEverDecelerated = true;
                ServerSetSyncData(ServerWriteRigidbody);
            }
            else
            {
                float sqrMagnitude = Rb.velocity.sqrMagnitude;
                if (sqrMagnitude < _serverPrevVelSqr)
                {
                    _serverEverDecelerated = true;
                }
                _serverPrevVelSqr = sqrMagnitude;
                if (!_serverPrevSleeping && _serverNextUpdateTime > NetworkTime.time)
                {
                    return;
                }
                ServerSendRpc(ServerWriteRigidbody);
                _serverNextUpdateTime = NetworkTime.time + 0.25;
            }
            _serverPrevSleeping = flag;
        }

        private void UpdateClient()
        {
            if (_syncDataDirty)
            {
                _syncDataDirty = false;
                base.ClientReadSyncData(ClientApplyRigidbody);
            }

            if (ClientFrozen && !(_freezeProgress > 1f) && SeedSynchronizer.MapGenerated)
            {
                Vector3 position = Rb.position;
                Vector3 lastWorldPos = LastWorldPos;
                if ((position - lastWorldPos).sqrMagnitude > 25f / 64f)
                {
                    _freezeProgress = 0f;
                    Rb.position = lastWorldPos;
                    Rb.rotation = LastWorldRot;
                }
                else
                {
                    _freezeProgress += Time.deltaTime * 1.2f;
                    float t = Mathf.Lerp(10f * Time.deltaTime, 1f, Mathf.Pow(_freezeProgress, 5f));
                    Rb.position = Vector3.Lerp(position, lastWorldPos, t);
                    Rb.rotation = Quaternion.Lerp(Rb.rotation, LastWorldRot, t);
                }
            }
        }


        private void ServerWriteRigidbody(NetworkWriter writer)
        {
            writer.WriteByte(_serverOrderClock++);
            RelativePosition msg = new RelativePosition(Rb.position);
            Quaternion relativeRotation = WaypointBase.GetRelativeRotation(msg.WaypointId, Rb.rotation);
            writer.WriteRelativePosition(msg);
            writer.WriteLowPrecisionQuaternion(new LowPrecisionQuaternion(relativeRotation));
            if (!ServerSendFreeze)
            {
                writer.WriteVector3(Rb.velocity);
                writer.WriteVector3(Rb.angularVelocity);
                if (msg.OutOfRange && base.IsSpawned)
                {
                    Pickup.DestroySelf();
                }
            }
        }

        private void ClientApplyRigidbody(NetworkReader reader)
        {
            int num = reader.ReadByte();
            bool flag = !_lastReceivedUpdate.HasValue;
            if (!flag)
            {
                int num2 = _lastReceivedUpdate.Value - num;
                if (num2 >= 0 && num2 < 127)
                {
                    return;
                }
            }
            _lastReceivedUpdate = num;
            _lastReceivedRelPos = reader.ReadRelativePosition();
            _lastReceivedRelRot = reader.ReadLowPrecisionQuaternion().Value;
            ClientFrozen = reader.Remaining == 0;
            _freezeProgress = 0f;
            if (flag || !ClientFrozen)
            {
                Rb.position = LastWorldPos;
                Rb.rotation = LastWorldRot;
                if (!ClientFrozen)
                {
                    Rb.velocity = reader.ReadVector3();
                    Rb.angularVelocity = reader.ReadVector3();
                }
            }
        }

        private void OnElevatorMoved(Bounds elevatorBounds, ElevatorChamber chamber, Vector3 deltaPos, Quaternion deltaRot)
        {
            bool flag = _inElevator && chamber == _trackedChamber;
            if (!chamber.WorldspaceBounds.Contains(Pickup.Position))
            {
                if (flag)
                {
                    _inElevator = false;
                    Pickup.Position -= deltaPos;
                    Pickup.transform.SetParent(null);
                    this.OnParentSetByElevator?.Invoke();
                }
                return;
            }
            Rb.velocity = deltaRot * Rb.velocity;
            if (!flag)
            {
                Pickup.transform.SetParent(chamber.transform);
                Pickup.Position += deltaPos;
                _trackedChamber = chamber;
                _inElevator = true;
                this.OnParentSetByElevator?.Invoke();
            }
        }


        internal override void ClientProcessRpc(NetworkReader rpcData)
        {
            base.ClientProcessRpc(rpcData);
            if (!NetworkServer.active)
            {
                ClientApplyRigidbody(rpcData);
            }
        }

        public override void DestroyModule()
        {
            base.DestroyModule();
            Pickup.OnInfoChanged -= UpdateWeight;
            Pickup.PhysicsModuleSyncData.Callback -= OnSyncDataChanged;
            StaticUnityMethods.OnUpdate -= OnUpdate;
            ElevatorChamber.OnElevatorMoved -= OnElevatorMoved;
        }
    }
}
