namespace Interactables.Interobjects
{
	public class BreakableDoor : global::Interactables.Interobjects.BasicDoor, global::Interactables.Interobjects.DoorUtils.IDamageableDoor, global::Interactables.Interobjects.DoorUtils.INonInteractableDoor, global::Interactables.Interobjects.DoorUtils.IScp106PassableDoor
	{
		[global::Mirror.SyncVar]
		private bool _destroyed;

		private bool _prevDestroyed;

		[global::UnityEngine.Header("Breakable Door Settings")]
		[global::UnityEngine.SerializeField]
		private float _maxHealth = 80f;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.BrokenDoor _brokenPrefab;

		[global::UnityEngine.SerializeField]
		private global::Interactables.Interobjects.DoorUtils.DoorDamageType _ignoredDamageSources;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.GameObject _objectToReplace;

		[global::UnityEngine.SerializeField]
		private bool _nonInteractable;

		public float RemainingHealth { get; set; }

		public float MaxHealth
		{
			get
			{
				return _maxHealth;
			}
			set
			{
				_maxHealth = value;
			}
		}

		public global::Interactables.Interobjects.DoorUtils.DoorDamageType IgnoredDamageSources
		{
			get
			{
				return _ignoredDamageSources;
			}
			set
			{
				_ignoredDamageSources = value;
			}
		}

		public bool IsDestroyed
		{
			get
			{
				return _destroyed;
			}
			set
			{
				if (value)
				{
					ServerDamage(_maxHealth, global::Interactables.Interobjects.DoorUtils.DoorDamageType.ServerCommand);
				}
				else
				{
					global::UnityEngine.Debug.LogError("You cannot unset the IsDestroyed value.");
				}
			}
		}

		public bool IgnoreLockdowns => _nonInteractable;

		public bool IgnoreRemoteAdmin => _nonInteractable;

		public bool IsScp106Passable => true;

		public bool Network_destroyed
		{
			get
			{
				return _destroyed;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _destroyed))
				{
					bool destroyed = _destroyed;
					SetSyncVar(value, ref _destroyed, 1uL);
				}
			}
		}

		[global::Mirror.Server]
		public bool ServerDamage(float hp, global::Interactables.Interobjects.DoorUtils.DoorDamageType type)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				global::UnityEngine.Debug.LogWarning("[Server] function 'System.Boolean Interactables.Interobjects.BreakableDoor::ServerDamage(System.Single,Interactables.Interobjects.DoorUtils.DoorDamageType)' called when server was not active");
				return default(bool);
			}
			if (_destroyed)
			{
				return false;
			}
			if (global::Interactables.Interobjects.DoorUtils.DamageableDoorUtils.HasFlagFast(_ignoredDamageSources, type))
			{
				return false;
			}
			if (_brokenPrefab == null || _objectToReplace == null)
			{
				return false;
			}
			RemainingHealth -= hp;
			if (RemainingHealth <= 0f)
			{
				Network_destroyed = true;
				global::Interactables.Interobjects.DoorUtils.DoorEvents.TriggerAction(this, global::Interactables.Interobjects.DoorUtils.DoorAction.Destroyed, null);
			}
			return true;
		}

		public override float GetExactState()
		{
			if (!_destroyed)
			{
				return base.GetExactState();
			}
			return 1f;
		}

		public override bool AllowInteracting(ReferenceHub ply, byte colliderId)
		{
			if (!_destroyed)
			{
				return base.AllowInteracting(ply, colliderId);
			}
			return false;
		}

		internal override void TargetStateChanged()
		{
			if (!_destroyed)
			{
				base.TargetStateChanged();
			}
		}

		protected override void Update()
		{
			base.Update();
			if (!_prevDestroyed && _destroyed)
			{
				_prevDestroyed = true;
				ClientDestroyEffects();
			}
		}

		protected override void Start()
		{
			base.Start();
			RemainingHealth = _maxHealth;
		}

		public void ClientDestroyEffects()
		{
			_objectToReplace.SetActive(value: false);
		}

		public float GetHealthPercent()
		{
			return global::UnityEngine.Mathf.Clamp01(RemainingHealth / _maxHealth);
		}

		private void MirrorProcessed()
		{
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _destroyed);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _destroyed);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				bool destroyed = _destroyed;
				Network_destroyed = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				bool destroyed2 = _destroyed;
				Network_destroyed = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
		}
	}
}
