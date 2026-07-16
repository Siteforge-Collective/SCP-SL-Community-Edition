namespace InventorySystem.Items.ThrowableProjectiles
{
	public class ThrowableItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IEquipDequipModifier, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Drawers.IItemAlertDrawer, global::InventorySystem.Drawers.IItemDrawer
	{
		[global::System.Serializable]
		public struct ProjectileSettings
		{
			public float StartVelocity;

			public float UpwardsFactor;

			public float TriggerTime;

			public global::UnityEngine.Vector3 StartTorque;

			public global::UnityEngine.Vector3 RelativePosition;
		}

		public global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile Projectile;

		public global::InventorySystem.Items.ThrowableProjectiles.PhantomProjectile Phantom;

		public global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings WeakThrowSettings;

		public global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings FullThrowSettings;

		public float ThrowingAnimTime;

		public float CancelAnimTime;

		public readonly global::System.Diagnostics.Stopwatch ThrowStopwatch = new global::System.Diagnostics.Stopwatch();

		public readonly global::System.Diagnostics.Stopwatch CancelStopwatch = new global::System.Diagnostics.Stopwatch();

		[global::UnityEngine.SerializeField]
		private float _weight;

		[global::UnityEngine.SerializeField]
		private float _pinPullTime;

		[global::UnityEngine.SerializeField]
		private float _postThrownAnimationTime;

		[global::UnityEngine.SerializeField]
		private bool _repickupable;

		private float _destroyTime;

		private bool _tryFire;

		private bool _alreadyFired;

		private bool _fireWeak;

		private bool _alreadySent;

		private global::UnityEngine.KeyCode _primaryKey;

		private global::UnityEngine.KeyCode _secondaryKey;

		private global::UnityEngine.Vector3 _releaseSpeed;

		private global::CustomPlayerEffects.Scp1853 _scp1853;

		private const float ServerTimeTolerance = 0.8f;

		private const float MaxTraceTime = 0.1f;

		private const float MaxAheadTime = 0.2f;

		private const float HintBlinkRate = 9f;

		private const float HintBlinkStartTime = 1f;

		private const float HintBlinkTotalTime = 0.7f;

		private const ActionName CancelAction = ActionName.Reload;

		private static readonly global::System.Diagnostics.Stopwatch TriggerDelay = new global::System.Diagnostics.Stopwatch();

		public override float Weight => _weight;

		public bool AllowHolster
		{
			get
			{
				if (!ThrowStopwatch.IsRunning)
				{
					if (CancelStopwatch.IsRunning)
					{
						return ReadyToCancel;
					}
					return true;
				}
				return false;
			}
		}

		public bool AllowEquip => true;

		private float CurrentTimeTolerance
		{
			get
			{
				if (!base.IsLocalPlayer)
				{
					return 0.8f;
				}
				return 1f;
			}
		}

		private bool ReadyToThrow => ThrowStopwatch.Elapsed.TotalSeconds >= (double)(CurrentTimeTolerance * ThrowingAnimTime);

		private bool ReadyToCancel => CancelStopwatch.Elapsed.TotalSeconds >= (double)(CurrentTimeTolerance * CancelAnimTime);

		private global::UnityEngine.KeyCode CancelKey => NewInput.GetKey(ActionName.Reload);

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			_primaryKey = NewInput.GetKey(ActionName.Shoot);
			_secondaryKey = NewInput.GetKey(ActionName.Zoom);
			_scp1853 = base.Owner.playerEffectsController.GetEffect<global::CustomPlayerEffects.Scp1853>();
		}

		public override void EquipUpdate()
		{
			if (global::Mirror.NetworkServer.active)
			{
				UpdateServer();
			}
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (!global::Mirror.NetworkServer.active || pickup == null || _alreadyFired)
			{
				return;
			}
			global::UnityEngine.Vector3 velocity = global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GetVelocity(base.Owner);
			if (ThrowStopwatch.Elapsed.TotalSeconds < (double)_pinPullTime)
			{
				if (pickup is global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile && pickup.TryGetComponent<global::UnityEngine.Rigidbody>(out var component))
				{
					component.velocity = velocity;
				}
			}
			else
			{
				ServerThrow(0f, 0f, velocity, global::UnityEngine.Vector3.zero);
				pickup.Info.Locked = true;
				pickup.DestroySelf();
			}
		}

		public override void OnHolstered()
		{
		}

		private void UpdateServer()
		{
			if (_destroyTime != 0f && global::UnityEngine.Time.timeSinceLevelLoad >= _destroyTime)
			{
				base.OwnerInventory.ServerRemoveItem(base.ItemSerial, null);
			}
		}

		private void PropelBody(global::UnityEngine.Rigidbody rb, global::UnityEngine.Vector3 torque, global::UnityEngine.Vector3 relativeVelocity, float forceAmount, float upwardFactor)
		{
			float num = 1f - global::UnityEngine.Mathf.Abs(global::UnityEngine.Vector3.Dot(base.Owner.PlayerCameraReference.forward, global::UnityEngine.Vector3.up));
			global::UnityEngine.Vector3 forward = base.Owner.PlayerCameraReference.forward;
			global::UnityEngine.Vector3 vector = base.Owner.PlayerCameraReference.up * upwardFactor;
			global::UnityEngine.Vector3 vector2 = forward + vector * num;
			rb.centerOfMass = global::UnityEngine.Vector3.zero;
			rb.angularVelocity = torque;
			rb.velocity = relativeVelocity + vector2 * forceAmount;
		}

		private void ServerThrow(float forceAmount, float upwardFactor, global::UnityEngine.Vector3 torque, global::UnityEngine.Vector3 startVel)
		{
			if (!_alreadyFired || base.IsLocalPlayer)
			{
				_destroyTime = global::UnityEngine.Time.timeSinceLevelLoad + _postThrownAnimationTime;
				_alreadyFired = true;
				global::InventorySystem.Items.ThrowableProjectiles.ThrownProjectile thrownProjectile = global::UnityEngine.Object.Instantiate(Projectile, base.Owner.PlayerCameraReference.position, base.Owner.PlayerCameraReference.rotation);
				global::InventorySystem.Items.Pickups.PickupSyncInfo pickupSyncInfo = new global::InventorySystem.Items.Pickups.PickupSyncInfo(ItemTypeId, thrownProjectile.transform.position, thrownProjectile.transform.rotation, Weight, base.ItemSerial);
				pickupSyncInfo.Locked = !_repickupable;
				global::InventorySystem.Items.Pickups.PickupSyncInfo newInfo = (thrownProjectile.NetworkInfo = pickupSyncInfo);
				thrownProjectile.PreviousOwner = new global::Footprinting.Footprint(base.Owner);
				global::Mirror.NetworkServer.Spawn(thrownProjectile.gameObject);
				thrownProjectile.InfoReceived(default(global::InventorySystem.Items.Pickups.PickupSyncInfo), newInfo);
				if (thrownProjectile.TryGetComponent<global::UnityEngine.Rigidbody>(out var component))
				{
					PropelBody(component, torque, startVel, forceAmount, upwardFactor);
				}
				thrownProjectile.ServerActivate();
			}
		}

		public void ServerProcessThrowConfirmation(bool fullForce, global::UnityEngine.Vector3 startPos, global::UnityEngine.Quaternion startRot, global::UnityEngine.Vector3 startVel)
		{
			if (ReadyToThrow)
			{
				global::UnityEngine.Transform playerCameraReference = base.Owner.PlayerCameraReference;
				global::UnityEngine.Vector3 position = playerCameraReference.position;
				global::UnityEngine.Quaternion rotation = playerCameraReference.rotation;
				global::UnityEngine.Bounds bounds = global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GenerateTracerBounds(base.Owner, 0.1f, ignoreTeleports: false);
				bounds.Encapsulate(playerCameraReference.position + global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GetVelocity(base.Owner) * 0.2f);
				playerCameraReference.SetPositionAndRotation(bounds.ClosestPoint(startPos), startRot);
				global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem.ProjectileSettings projectileSettings = (fullForce ? FullThrowSettings : WeakThrowSettings);
				startVel = global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.GetLimitedVelocity(startVel);
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.PlayerThrowProjectile, base.Owner, this, projectileSettings, fullForce))
				{
					ServerThrow(projectileSettings.StartVelocity, projectileSettings.UpwardsFactor, projectileSettings.StartTorque, startVel);
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage(base.ItemSerial, (!fullForce) ? global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowWeak : global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.ConfirmThrowFullForce));
					playerCameraReference.SetPositionAndRotation(position, rotation);
				}
			}
		}

		public void ServerProcessInitiation()
		{
			if (AllowHolster && (!global::CustomPlayerEffects.UsableItemModifierEffectExtensions.TryGetSpeedMultiplier(ItemTypeId, base.Owner, out var multiplier) || !(multiplier <= 0f)))
			{
				ThrowStopwatch.Start();
				CancelStopwatch.Reset();
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage(base.ItemSerial, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.BeginThrow));
			}
		}

		public void ServerProcessCancellation()
		{
			if (ReadyToThrow && !_alreadyFired && !CancelStopwatch.IsRunning && !(CancelAnimTime <= 0f))
			{
				CancelStopwatch.Start();
				ThrowStopwatch.Reset();
				global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.ThrowableItemAudioMessage(base.ItemSerial, global::InventorySystem.Items.ThrowableProjectiles.ThrowableNetworkHandler.RequestType.CancelThrow));
			}
		}
	}
}
