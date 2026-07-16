namespace InventorySystem.Items.Pickups
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Rigidbody))]
	public abstract class ItemPickupBase : global::Mirror.NetworkBehaviour
	{
		private const float AutoDeleteHeight = -2500f;

		private const float MinimalWeight = 0.001f;

		private const float MinimalPickupTime = 0.245f;

		private const float WeightToTime = 0.175f;

		[global::Mirror.SyncVar(hook = "InfoReceived")]
		public global::InventorySystem.Items.Pickups.PickupSyncInfo Info = global::InventorySystem.Items.Pickups.PickupSyncInfo.None;

		public global::Footprinting.Footprint PreviousOwner;

		protected global::UnityEngine.Rigidbody Rb;

		protected global::InventorySystem.Items.Pickups.IPickupPhysicsModule PhysicsModule;

		private global::UnityEngine.Vector3 _estimatedVelocity;

		private global::UnityEngine.Transform _transform;

		public global::UnityEngine.Rigidbody RigidBody => Rb;

		public global::InventorySystem.Items.Pickups.PickupSyncInfo NetworkInfo
		{
			get
			{
				return Info;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref Info))
				{
					global::InventorySystem.Items.Pickups.PickupSyncInfo info = Info;
					SetSyncVar(value, ref Info, 1uL);
					if (global::Mirror.NetworkServer.localClientActive && !getSyncVarHookGuard(1uL))
					{
						setSyncVarHookGuard(1uL, value: true);
						InfoReceived(info, value);
						setSyncVarHookGuard(1uL, value: false);
					}
				}
			}
		}

		public static event global::System.Action<global::InventorySystem.Items.Pickups.ItemPickupBase> OnPickupAdded;

		public static event global::System.Action<global::InventorySystem.Items.Pickups.ItemPickupBase> OnPickupDestroyed;

		public virtual void InfoReceived(global::InventorySystem.Items.Pickups.PickupSyncInfo oldInfo, global::InventorySystem.Items.Pickups.PickupSyncInfo newInfo)
		{
			PhysicsModule.UpdateInfo(newInfo);
			Rb.mass = global::UnityEngine.Mathf.Max(0.001f, newInfo.Weight);
		}

		public virtual float SearchTimeForPlayer(ReferenceHub hub)
		{
			float num = 0.245f + 0.175f * Info.Weight;
			global::CustomPlayerEffects.StatusEffectBase[] allEffects = hub.playerEffectsController.AllEffects;
			foreach (global::CustomPlayerEffects.StatusEffectBase statusEffectBase in allEffects)
			{
				if (statusEffectBase.IsEnabled && statusEffectBase is global::InventorySystem.Searching.ISearchTimeModifier searchTimeModifier)
				{
					num = searchTimeModifier.ProcessSearchTime(num);
				}
			}
			if (hub.inventory.CurInstance is global::InventorySystem.Searching.ISearchTimeModifier searchTimeModifier2 && searchTimeModifier2 != null)
			{
				num = searchTimeModifier2.ProcessSearchTime(num);
			}
			return num;
		}

		[global::Mirror.ServerCallback]
		public void RefreshPositionAndRotation()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				return;
			}
			if (_transform.position.y < -2500f)
			{
				DestroySelf();
				return;
			}
			global::InventorySystem.Items.Pickups.PickupSyncInfo info = Info;
			info.ServerSetPositionAndRotation(_transform.position, _transform.rotation);
			if (info.Position != Info.Position || info.Rotation != Info.Rotation)
			{
				NetworkInfo = info;
			}
		}

		[global::Mirror.Server]
		public void DestroySelf()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Void InventorySystem.Items.Pickups.ItemPickupBase::DestroySelf()' called when server was not active");
			}
			else
			{
				global::Mirror.NetworkServer.Destroy(base.gameObject);
			}
		}

		protected virtual void Awake()
		{
			Rb = GetComponent<global::UnityEngine.Rigidbody>();
			PhysicsModule = new global::InventorySystem.Items.Pickups.PhysicsPredictionPickup(this, Rb);
			Rb.ResetCenterOfMass();
			_transform = base.transform;
		}

		protected virtual void Start()
		{
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupAdded?.Invoke(this);
		}

		protected virtual void OnDestroy()
		{
			PhysicsModule?.DestroyModule();
			global::InventorySystem.Items.Pickups.ItemPickupBase.OnPickupDestroyed?.Invoke(this);
		}

		protected virtual void FixedUpdate()
		{
			PhysicsModule?.UpdatePhysics();
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				writer.WritePickupSyncInfo(Info);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				writer.WritePickupSyncInfo(Info);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				global::InventorySystem.Items.Pickups.PickupSyncInfo info = Info;
				NetworkInfo = reader.ReadPickupSyncInfo();
				if (!SyncVarEqual(info, ref Info))
				{
					InfoReceived(info, Info);
				}
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				global::InventorySystem.Items.Pickups.PickupSyncInfo info2 = Info;
				NetworkInfo = reader.ReadPickupSyncInfo();
				if (!SyncVarEqual(info2, ref Info))
				{
					InfoReceived(info2, Info);
				}
			}
		}
	}
}
