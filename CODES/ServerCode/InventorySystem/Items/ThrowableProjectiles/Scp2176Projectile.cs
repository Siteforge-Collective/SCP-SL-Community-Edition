namespace InventorySystem.Items.ThrowableProjectiles
{
	public class Scp2176Projectile : global::InventorySystem.Items.ThrowableProjectiles.EffectGrenade
	{
		private const float LockdownDuration = 13f;

		private const float LockdownDisableValue = 0.1f;

		private const float PanicDuration = 5f;

		private const float ShatterVelocity = 8.5f;

		private const float ActivateVelocity = 6.5f;

		private const float DropSoundRange = 20f;

		private bool _hasTriggered;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _dropSound;

		[global::Mirror.SyncVar]
		private bool _playedDropSound;

		public bool Network_playedDropSound
		{
			get
			{
				return _playedDropSound;
			}
			[param: global::System.Runtime.InteropServices.In]
			set
			{
				if (!SyncVarEqual(value, ref _playedDropSound))
				{
					bool playedDropSound = _playedDropSound;
					SetSyncVar(value, ref _playedDropSound, 1uL);
				}
			}
		}

		public static event global::System.Action<global::MapGeneration.RoomIdentifier> OnServerShattered;

		protected override void ProcessCollision(global::UnityEngine.Collision collision)
		{
			float sqrMagnitude = collision.relativeVelocity.sqrMagnitude;
			if (!global::Mirror.NetworkServer.active || !(sqrMagnitude >= 42.25f))
			{
				return;
			}
			base.ProcessCollision(collision);
			if (!_hasTriggered)
			{
				ServerActivate();
				if (sqrMagnitude >= 72.25f)
				{
					ServerFuseEnd();
				}
				else if (!_playedDropSound)
				{
					Network_playedDropSound = true;
					RpcMakeSound();
				}
			}
		}

		protected override void ServerFuseEnd()
		{
			_hasTriggered = true;
			if (base.TargetTime <= 0f)
			{
				base.TargetTime = global::UnityEngine.Time.timeSinceLevelLoad;
			}
			base.ServerFuseEnd();
			ServerShatter();
		}

		[global::Mirror.ClientRpc]
		private void RpcMakeSound()
		{
			global::Mirror.PooledNetworkWriter writer = global::Mirror.NetworkWriterPool.GetWriter();
			SendRPCInternal(typeof(global::InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile), "RpcMakeSound", writer, 0, includeOwner: true);
			global::Mirror.NetworkWriterPool.Recycle(writer);
		}

		public override void ServerActivate()
		{
			base.ServerActivate();
			if (base.TargetTime <= 0f)
			{
				global::InventorySystem.Items.Pickups.PickupSyncInfo info = Info;
				info.Locked = true;
				base.NetworkInfo = info;
			}
		}

		public void ServerImmediatelyShatter()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Tried to call ServerImmediatelyShatter from the client!");
			}
			ServerActivate();
			ServerFuseEnd();
		}

		private void ServerShatter()
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Tried to call ServerShatter from the client!");
			}
			global::MapGeneration.RoomIdentifier rid = global::MapGeneration.RoomIdUtils.RoomAtPositionRaycasts(base.transform.position);
			if (rid == null)
			{
				return;
			}
			global::InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile.OnServerShattered?.Invoke(rid);
			global::System.Collections.Generic.IEnumerable<FlickerableLightController> enumerable = global::System.Linq.Enumerable.Where(FlickerableLightController.Instances, (FlickerableLightController x) => x.Room == rid);
			if (rid.Name == global::MapGeneration.RoomName.HczTesla && TryFindTeslaAtRoom(rid, out var gate))
			{
				ServerOverloadTesla(rid, gate, enumerable);
			}
			else
			{
				foreach (FlickerableLightController item in enumerable)
				{
					item.ServerFlickerLights(item.LightsEnabled ? 13f : 0.1f);
				}
			}
			if (global::Interactables.Interobjects.DoorUtils.DoorVariant.DoorsByRoom.TryGetValue(rid, out var value))
			{
				ServerLockdown(value);
			}
		}

		private static bool TryFindTeslaAtRoom(global::MapGeneration.RoomIdentifier rid, out TeslaGate gate)
		{
			gate = global::System.Linq.Enumerable.FirstOrDefault(TeslaGateController.Singleton.TeslaGates, (TeslaGate x) => rid == global::MapGeneration.RoomIdUtils.RoomAtPosition(x.transform.position));
			return gate != null;
		}

		private void ServerOverloadTesla(global::MapGeneration.RoomIdentifier rid, TeslaGate tg, global::System.Collections.Generic.IEnumerable<FlickerableLightController> lightControllers)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Tried to call ServerOverloadTesla from the client!");
			}
			foreach (FlickerableLightController lightController in lightControllers)
			{
				lightController.ServerFlickerLights(lightController.LightsEnabled ? 12f : 0.1f);
			}
			tg.NetworkInactiveTime = ((tg.InactiveTime > 0f) ? 0.1f : 12f);
			tg.RpcInstantBurst();
			tg.ServerSideIdle(shouldIdle: false);
		}

		private void ServerLockdown(global::System.Collections.Generic.IEnumerable<global::Interactables.Interobjects.DoorUtils.DoorVariant> doors)
		{
			bool inProgress = AlphaWarheadController.InProgress;
			foreach (global::Interactables.Interobjects.DoorUtils.DoorVariant door in doors)
			{
				if (door is global::Interactables.Interobjects.DoorUtils.INonInteractableDoor nonInteractableDoor && nonInteractableDoor.IgnoreLockdowns)
				{
					continue;
				}
				global::Interactables.Interobjects.DoorUtils.DoorLockReason activeLocks = (global::Interactables.Interobjects.DoorUtils.DoorLockReason)door.ActiveLocks;
				if (!door.TargetState && (global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(activeLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown079) || global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(activeLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown2176) || global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(activeLocks, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Regular079)))
				{
					global::Interactables.Interobjects.DoorUtils.DoorScheduledUnlocker.UnlockLater(door, 0f, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown2176);
					if (!door.RequiredPermissions.Bypass2176)
					{
						door.NetworkTargetState = true;
					}
					continue;
				}
				global::Interactables.Interobjects.DoorUtils.DoorLockMode mode = global::Interactables.Interobjects.DoorUtils.DoorLockUtils.GetMode((global::Interactables.Interobjects.DoorUtils.DoorLockReason)door.ActiveLocks);
				if (global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(mode, global::Interactables.Interobjects.DoorUtils.DoorLockMode.CanClose) || global::Interactables.Interobjects.DoorUtils.DoorLockUtils.HasFlagFast(mode, global::Interactables.Interobjects.DoorUtils.DoorLockMode.ScpOverride))
				{
					door.ServerChangeLock(global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown2176, newState: true);
					global::Interactables.Interobjects.DoorUtils.DoorScheduledUnlocker.UnlockLater(door, 13f, global::Interactables.Interobjects.DoorUtils.DoorLockReason.Lockdown2176);
					if (!inProgress)
					{
						door.NetworkTargetState = false;
					}
				}
			}
		}

		private void MirrorProcessed()
		{
		}

		private void UserCode_RpcMakeSound()
		{
		}

		protected static void InvokeUserCode_RpcMakeSound(global::Mirror.NetworkBehaviour obj, global::Mirror.NetworkReader reader, global::Mirror.NetworkConnectionToClient senderConnection)
		{
			if (!global::Mirror.NetworkClient.active)
			{
				global::UnityEngine.Debug.LogError("RPC RpcMakeSound called on server.");
			}
			else
			{
				((global::InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile)obj).UserCode_RpcMakeSound();
			}
		}

		static Scp2176Projectile()
		{
			global::Mirror.RemoteCalls.RemoteCallHelper.RegisterRpcDelegate(typeof(global::InventorySystem.Items.ThrowableProjectiles.Scp2176Projectile), "RpcMakeSound", InvokeUserCode_RpcMakeSound);
		}

		public override bool SerializeSyncVars(global::Mirror.NetworkWriter writer, bool forceAll)
		{
			bool result = base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _playedDropSound);
				return true;
			}
			global::Mirror.NetworkWriterExtensions.WriteUInt64(writer, base.syncVarDirtyBits);
			if ((base.syncVarDirtyBits & 1L) != 0L)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(writer, _playedDropSound);
				result = true;
			}
			return result;
		}

		public override void DeserializeSyncVars(global::Mirror.NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				bool playedDropSound = _playedDropSound;
				Network_playedDropSound = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
				return;
			}
			long num = (long)global::Mirror.NetworkReaderExtensions.ReadUInt64(reader);
			if ((num & 1L) != 0L)
			{
				bool playedDropSound2 = _playedDropSound;
				Network_playedDropSound = global::Mirror.NetworkReaderExtensions.ReadBoolean(reader);
			}
		}
	}
}
