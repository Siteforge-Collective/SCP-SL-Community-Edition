namespace InventorySystem.Items.Jailbird
{
	public class JailbirdItem : global::InventorySystem.Items.Autosync.AutosyncItem, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag, global::PlayerRoles.FirstPersonControl.IMovementInputOverride, global::PlayerRoles.FirstPersonControl.IMovementSpeedModifier, global::InventorySystem.Drawers.IItemAlertDrawer, global::InventorySystem.Drawers.IItemDrawer, global::InventorySystem.Items.IEquipDequipModifier
	{
		private const float DamageLimit = 500f;

		private const float DamageWarning = 400f;

		private const int ChargesLimit = 5;

		private const int ChargesWarning = 4;

		private const ActionName TriggerMelee = ActionName.Shoot;

		private const ActionName TriggerCharge = ActionName.Zoom;

		private const ActionName InspectKey = ActionName.InspectItem;

		private const float ServerChargeTolerance = 0.4f;

		private const float HintDuration = 10f;

		private double _chargeResetTime;

		private bool _chargeLoading;

		private bool _charging;

		private bool _chargeAnyDetected;

		private bool _firstChargeFrame;

		private float _chargeLoadElapsed;

		private bool _attackTriggered;

		private bool _broken;

		private static float _localRemainingHint = 10f;

		private readonly global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown _serverAttackCooldown = new global::PlayerRoles.PlayableScps.Subroutines.TolerantAbilityCooldown();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _clientAttackCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		private readonly global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown _clientDelayCooldown = new global::PlayerRoles.PlayableScps.Subroutines.AbilityCooldown();

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AudioClip _hitClip;

		[global::UnityEngine.SerializeField]
		private float _meleeDelay;

		[global::UnityEngine.SerializeField]
		private float _meleeCooldown;

		[global::UnityEngine.SerializeField]
		private float _chargeDuration;

		[global::UnityEngine.SerializeField]
		private float _chargeReadyTime;

		[global::UnityEngine.SerializeField]
		private float _chargeMovementSpeedMultiplier;

		[global::UnityEngine.SerializeField]
		private float _chargeMovementSpeedLimiter;

		[global::UnityEngine.SerializeField]
		private float _chargeCancelVelocitySqr;

		[global::UnityEngine.SerializeField]
		private float _chargeAutoengageTime;

		[global::UnityEngine.SerializeField]
		private float _chargeDetectionDelay;

		[global::UnityEngine.SerializeField]
		private float _brokenRemoveTime;

		[global::UnityEngine.SerializeField]
		private global::InventorySystem.Items.Jailbird.JailbirdHitreg _hitreg;

		public override float Weight => 1.7f;

		public int TotalChargesPerformed { get; private set; }

		public bool MovementOverrideActive => _charging;

		public global::UnityEngine.Vector3 MovementOverrideDirection => base.Owner.transform.forward;

		public bool MovementModifierActive => _charging;

		public float MovementSpeedMultiplier => _chargeMovementSpeedMultiplier;

		public float MovementSpeedLimit => _chargeMovementSpeedLimiter;

		public bool AllowHolster
		{
			get
			{
				if (!_charging)
				{
					return !_chargeLoading;
				}
				return false;
			}
		}

		public bool AllowEquip => true;

		public static event global::System.Action<ushort, global::InventorySystem.Items.Jailbird.JailbirdMessageType> OnRpcReceived;

		public event global::System.Action<global::InventorySystem.Items.Jailbird.JailbirdMessageType> OnCmdSent;

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnAdded(pickup);
			_hitreg.Setup(this);
			if (global::Mirror.NetworkServer.active)
			{
				if (pickup is global::InventorySystem.Items.Jailbird.JailbirdPickup jailbirdPickup)
				{
					TotalChargesPerformed = jailbirdPickup.TotalCharges;
					_hitreg.TotalMeleeDamageDealt = jailbirdPickup.TotalMelee;
				}
				ServerRecheckUsage();
			}
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			base.OnRemoved(pickup);
			if (global::Mirror.NetworkServer.active && pickup is global::InventorySystem.Items.Jailbird.JailbirdPickup jailbirdPickup)
			{
				if (_broken)
				{
					jailbirdPickup.DestroySelf();
					return;
				}
				jailbirdPickup.TotalCharges = TotalChargesPerformed;
				jailbirdPickup.TotalMelee = _hitreg.TotalMeleeDamageDealt;
			}
		}

		public override void OnHolstered()
		{
			base.OnHolstered();
			_chargeLoading = false;
			_charging = false;
			_attackTriggered = false;
			if (global::Mirror.NetworkServer.active)
			{
				SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.Holstered);
				if (_broken)
				{
					base.OwnerInventory.ServerRemoveItem(base.ItemSerial, null);
				}
			}
		}

		public override void EquipUpdate()
		{
			base.EquipUpdate();
			if (_broken)
			{
				if (global::Mirror.NetworkServer.active)
				{
					_brokenRemoveTime -= global::UnityEngine.Time.deltaTime;
				}
				if (_brokenRemoveTime < 0f)
				{
					base.OwnerInventory.ServerRemoveItem(base.ItemSerial, null);
				}
			}
			else if (_charging)
			{
				UpdateCharging();
			}
		}

		internal override void ClientProcessRpcTemplate(global::Mirror.NetworkReader reader, ushort serial)
		{
			base.ClientProcessRpcTemplate(reader, serial);
			global::InventorySystem.Items.Jailbird.JailbirdMessageType jailbirdMessageType = (global::InventorySystem.Items.Jailbird.JailbirdMessageType)reader.ReadByte();
			global::InventorySystem.Items.Jailbird.JailbirdItem.OnRpcReceived?.Invoke(serial, jailbirdMessageType);
			if (jailbirdMessageType == global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackPerformed && global::Mirror.NetworkReaderExtensions.ReadBoolean(reader) && global::InventorySystem.InventoryExtensions.TryGetHubHoldingSerial(serial, out var hub))
			{
				global::AudioPooling.AudioSourcePoolManager.PlaySound(_hitClip, hub.transform, 15f);
			}
		}

		internal override void ClientProcessRpcLocally(global::Mirror.NetworkReader reader)
		{
			base.ClientProcessRpcLocally(reader);
			switch ((global::InventorySystem.Items.Jailbird.JailbirdMessageType)reader.ReadByte())
			{
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.Broken:
				_broken = true;
				break;
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.ChargeStarted:
				_charging = true;
				_firstChargeFrame = true;
				_chargeLoading = false;
				_chargeAnyDetected = false;
				_chargeResetTime = global::Mirror.NetworkReaderExtensions.ReadDouble(reader);
				break;
			}
		}

		internal override void ServerProcessCmd(global::Mirror.NetworkReader reader)
		{
			base.ServerProcessCmd(reader);
			if (_broken || !base.IsEquipped)
			{
				return;
			}
			global::InventorySystem.Items.Jailbird.JailbirdMessageType jailbirdMessageType = (global::InventorySystem.Items.Jailbird.JailbirdMessageType)reader.ReadByte();
			switch (jailbirdMessageType)
			{
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackTriggered:
				if (!_attackTriggered && _serverAttackCooldown.TolerantIsReady)
				{
					_attackTriggered = true;
					_serverAttackCooldown.Trigger(_meleeCooldown);
					SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackTriggered);
				}
				break;
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackPerformed:
				if (_charging)
				{
					ServerAttack(reader);
				}
				else if (_attackTriggered)
				{
					_attackTriggered = false;
					ServerAttack(reader);
				}
				break;
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.ChargeLoadTriggered:
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.ChargeFailed:
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.Inspect:
				SendRpc(jailbirdMessageType);
				break;
			case global::InventorySystem.Items.Jailbird.JailbirdMessageType.ChargeStarted:
				if (!_charging)
				{
					_charging = true;
					_chargeResetTime = global::Mirror.NetworkTime.time + (double)_chargeDuration;
					TotalChargesPerformed++;
					SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.ChargeStarted, delegate(global::Mirror.NetworkWriter wr)
					{
						global::Mirror.NetworkWriterExtensions.WriteDouble(wr, _chargeResetTime);
					});
				}
				break;
			}
		}

		private void UpdateCharging()
		{
			double num = _chargeResetTime - global::Mirror.NetworkTime.time;
			if (global::Mirror.NetworkServer.active && num < -0.4000000059604645)
			{
				ServerAttack(null);
			}
			else
			{
				if (!base.IsLocalPlayer)
				{
					return;
				}
				if (!_chargeAnyDetected && _hitreg.AnyDetected)
				{
					num = global::UnityEngine.Mathf.Min((float)num, _chargeDetectionDelay);
					_chargeResetTime = global::Mirror.NetworkTime.time + num;
					_chargeAnyDetected = true;
				}
				if (num > 0.0)
				{
					if (_firstChargeFrame)
					{
						_firstChargeFrame = false;
						return;
					}
					if (global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.GetVelocity(base.Owner).SqrMagnitudeIgnoreY() > _chargeCancelVelocitySqr)
					{
						return;
					}
				}
				ClientAttack();
				_charging = false;
				_clientAttackCooldown.Trigger(_meleeCooldown);
			}
		}

		private void ServerAttack(global::Mirror.NetworkReader reader)
		{
			bool anyDamaged = _hitreg.ServerAttack(_charging, reader);
			if (anyDamaged)
			{
				Hitmarker.SendHitmarker(base.Owner, 1f);
			}
			SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackPerformed, delegate(global::Mirror.NetworkWriter wr)
			{
				global::Mirror.NetworkWriterExtensions.WriteBoolean(wr, anyDamaged);
			});
			ServerRecheckUsage();
			if (_charging)
			{
				_charging = false;
				if (_broken && anyDamaged && global::InventorySystem.InventoryItemLoader.TryGetItem<global::InventorySystem.Items.ThrowableProjectiles.ThrowableItem>(ItemType.GrenadeHE, out var result))
				{
					global::UnityEngine.Vector3 position = base.Owner.transform.position;
					global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.Usables.Scp330.CandyPink.CandyExplosionMessage
					{
						Origin = position
					});
					global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade.Explode(new global::Footprinting.Footprint(base.Owner), position, result.Projectile as global::InventorySystem.Items.ThrowableProjectiles.ExplosionGrenade);
				}
			}
		}

		private void ClientAttack()
		{
			if (_hitreg.ClientTryAttack())
			{
				this.OnCmdSent?.Invoke(global::InventorySystem.Items.Jailbird.JailbirdMessageType.AttackPerformed);
			}
		}

		private void ServerRecheckUsage()
		{
			if (!(_hitreg.TotalMeleeDamageDealt < 400f) || TotalChargesPerformed >= 4)
			{
				SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.AlmostDepleted);
				if (!(_hitreg.TotalMeleeDamageDealt < 500f) || TotalChargesPerformed >= 5)
				{
					_broken = true;
					SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType.Broken);
				}
			}
		}

		private void SendRpc(global::InventorySystem.Items.Jailbird.JailbirdMessageType header, global::System.Action<global::Mirror.NetworkWriter> extraData = null)
		{
			global::Mirror.NetworkWriter writer;
			using (new global::InventorySystem.Items.Autosync.AutosyncRpc(this, toAll: true, out writer))
			{
				writer.WriteByte((byte)header);
				extraData?.Invoke(writer);
			}
		}

		private void SendCmd(global::InventorySystem.Items.Jailbird.JailbirdMessageType msg)
		{
			global::Mirror.NetworkWriter writer;
			using (new global::InventorySystem.Items.Autosync.AutosyncCmd(this, out writer))
			{
				writer.WriteByte((byte)msg);
			}
			this.OnCmdSent?.Invoke(msg);
		}
	}
}
