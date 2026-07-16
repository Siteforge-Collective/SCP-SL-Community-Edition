using System;
using System.Runtime.InteropServices;
using CustomPlayerEffects;
using Footprinting;
using InventorySystem.Searching;
using Mirror;
using Mirror.RemoteCalls;
using RoundRestarting;
using UnityEngine;

namespace InventorySystem.Items.Pickups
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class ItemPickupBase : NetworkBehaviour
	{
		private const float MinimalPickupTime = 0.245f;

		private const float WeightToTime = 0.175f;

		[SyncVar(hook = nameof(InfoReceivedHook))]
		public PickupSyncInfo Info;

        public SyncList<byte> PhysicsModuleSyncData = new SyncList<byte>();

        public Footprint PreviousOwner;

		private Transform _transform;

		private bool _transformCacheSet;

		private bool _wasServer;

        protected virtual PickupPhysicsModule DefaultPhysicsModule => new PickupStandardPhysics(this);

        public PickupPhysicsModule PhysicsModule { get; protected set; }

        public Vector3 Position
        {
            get
            {
                return CachedTransform.position;
            }
            set
            {
                CachedTransform.position = value;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return CachedTransform.rotation;
            }
            set
            {
                CachedTransform.rotation = value;
            }
        }

        private Transform CachedTransform
        {
            get
            {
                if (!_transformCacheSet)
                {
                    _transformCacheSet = true;
                    _transform = base.transform;
                }
                return _transform;
            }
        }

        public static event Action<ItemPickupBase> OnPickupAdded;

        public static event Action<ItemPickupBase> OnPickupDestroyed;

        public event Action OnInfoChanged;

        public event Action<PickupSyncInfo, PickupSyncInfo> OnInfoChangedPrevNew;

        private void InfoReceivedHook(PickupSyncInfo oldInfo, PickupSyncInfo newInfo)
        {
            this.OnInfoChanged?.Invoke();
            this.OnInfoChangedPrevNew?.Invoke(oldInfo, newInfo);
        }

        public virtual float SearchTimeForPlayer(ReferenceHub hub)
        {
            float num = 0.245f + 0.175f * Info.WeightKg;
            StatusEffectBase[] allEffects = hub.playerEffectsController.AllEffects;
            foreach (StatusEffectBase statusEffectBase in allEffects)
            {
                if (statusEffectBase.IsEnabled && statusEffectBase is ISearchTimeModifier searchTimeModifier)
                {
                    num = searchTimeModifier.ProcessSearchTime(num);
                }
            }
            if (hub.inventory.CurInstance is ISearchTimeModifier searchTimeModifier2 && searchTimeModifier2 != null)
            {
                num = searchTimeModifier2.ProcessSearchTime(num);
            }
            return num;
        }


        [Server]
        public void DestroySelf()
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'System.Void InventorySystem.Items.Pickups.ItemPickupBase::DestroySelf()' called when server was not active");
            }
            else
            {
                NetworkServer.Destroy(base.gameObject);
            }
        }

        [ClientRpc]
		internal virtual void SendPhysicsModuleRpc(ArraySegment<byte> arrSeg)
		{
            using NetworkReaderPooled rpcData = NetworkReaderPool.Get(arrSeg);
            PhysicsModule?.ClientProcessRpc(rpcData);
        }

        protected virtual void Awake()
        {
            PhysicsModule = DefaultPhysicsModule;
        }


        protected virtual void Start()
        {
            ItemPickupBase.OnPickupAdded?.Invoke(this);
            if (NetworkServer.active)
            {
                _wasServer = true;
                RoundRestart.OnRestartTriggered += DestroySelf;
            }
        }

        protected virtual void OnDestroy()
        {
            PhysicsModule?.DestroyModule();
            ItemPickupBase.OnPickupDestroyed?.Invoke(this);
            if (_wasServer)
            {
                RoundRestart.OnRestartTriggered -= DestroySelf;
            }
        }

        protected ItemPickupBase()
        {
            InitSyncObject(PhysicsModuleSyncData);
        }
    }
}
