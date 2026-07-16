namespace InventorySystem.Items.MicroHID
{
	public class MicroHIDItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IEquipDequipModifier, global::PlayerRoles.FirstPersonControl.IStaminaModifier, global::InventorySystem.Items.IItemDescription, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.IAcquisitionConfirmationTrigger, global::InventorySystem.Items.IUpgradeTrigger
	{
		public global::UnityEngine.AudioClip PowerUpClip;

		public global::UnityEngine.AudioClip PowerDownClip;

		public global::UnityEngine.AudioClip PrimedClip;

		public global::UnityEngine.AudioClip FireClip;

		public global::UnityEngine.AudioClip FireToPrimedClip;

		public global::UnityEngine.AudioClip FireToPowerDownClip;

		public float RemainingEnergy;

		public global::InventorySystem.Items.MicroHID.HidUserInput UserInput;

		public global::InventorySystem.Items.MicroHID.HidState State;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.AnimationCurve _energyConsumtionCurve;

		public const float SoundMaxDistance = 30f;

		public const float PreFireTime = 1.7f;

		private const float StaminaUsageMultp = 2f;

		private const float MinimalTimeToSwitchState = 0.35f;

		private const float PowerupTime = 5.95f;

		private const float PowerdownTime = 3.1f;

		private const float FireEnergyConsumption = 0.13f;

		private const float EnemyDotProductThreshold = 0.75f;

		private const float FriendlyDotProductThreshold = 0.98f;

		private const float DamagePerSecond = 1150f;

		private const float DamageOmnidirectionalDistance = 0.7f;

		private const float DamageMaxDistance = 6.3f;

		private const float HitsPerSecond = 8f;

		private static global::UnityEngine.LayerMask _mask = 0;

		private float _damageTimer;

		private readonly global::System.Diagnostics.Stopwatch _stopwatch = new global::System.Diagnostics.Stopwatch();

		private bool _itrReady;

		private global::UnityEngine.KeyCode _primaryKey;

		private global::UnityEngine.KeyCode _secondaryKey;

		public override float Weight => 21.5f;

		public bool AcquisitionAlreadyReceived { get; set; }

		public bool AllowEquip => true;

		public bool AllowHolster => State == global::InventorySystem.Items.MicroHID.HidState.Idle;

		public bool StaminaModifierActive => base.IsEquipped;

		public float StaminaUsageMultiplier => 2f;

		public float StaminaRegenMultiplier => 1f;

		public bool SprintingDisabled => false;

		public float Readiness
		{
			get
			{
				switch (State)
				{
				case global::InventorySystem.Items.MicroHID.HidState.Primed:
				case global::InventorySystem.Items.MicroHID.HidState.Firing:
					return 1f;
				case global::InventorySystem.Items.MicroHID.HidState.PoweringUp:
					return global::UnityEngine.Mathf.Clamp01((float)_stopwatch.Elapsed.TotalSeconds / 5.95f);
				default:
					return 0f;
				}
			}
		}

		public static global::UnityEngine.LayerMask WallMask
		{
			get
			{
				if ((int)_mask == 0)
				{
					_mask = global::UnityEngine.LayerMask.GetMask("Default", "Glass", "CCTV", "Door", "Locker");
				}
				return _mask;
			}
		}

		private byte EnergyToByte => (byte)global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Mathf.Clamp01(RemainingEnergy) * 255f);

		public static event global::System.Action<global::InventorySystem.Items.MicroHID.MicroHIDItem> OnStopCharging;

		public void Recharge()
		{
			RemainingEnergy = 1f;
			ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.EnergySync, EnergyToByte);
		}

		public void ServerConfirmAcqusition()
		{
			base.OwnerInventory.connectionToClient.Send(new global::InventorySystem.Items.MicroHID.HidStatusMessage
			{
				MessageType = global::InventorySystem.Items.MicroHID.HidStatusMessageType.EnergySync,
				Serial = base.ItemSerial,
				MessageCode = EnergyToByte
			});
		}

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active && pickup != null && pickup is global::InventorySystem.Items.MicroHID.MicroHIDPickup microHIDPickup)
			{
				RemainingEnergy = microHIDPickup.Energy;
			}
		}

		public void ServerOnUpgraded(global::Scp914.Scp914KnobSetting setting)
		{
			RemainingEnergy = 1f;
			ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.EnergySync, EnergyToByte);
		}

		public override void OnRemoved(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (pickup is global::InventorySystem.Items.MicroHID.MicroHIDPickup microHIDPickup)
				{
					microHIDPickup.NetworkEnergy = RemainingEnergy;
				}
				ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.State, 5);
			}
		}

		public override void OnEquipped()
		{
			UserInput = global::InventorySystem.Items.MicroHID.HidUserInput.None;
			if (global::Mirror.NetworkServer.active)
			{
				ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.EnergySync, EnergyToByte);
				ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.State, 0);
			}
		}

		public override void EquipUpdate()
		{
			if (base.IsLocalPlayer)
			{
				ExecuteClientside();
			}
			if (global::Mirror.NetworkServer.active)
			{
				ExecuteServerside();
			}
		}

		private void ExecuteClientside()
		{
		}

		private void ExecuteServerside()
		{
			global::InventorySystem.Items.MicroHID.HidState state = State;
			byte energyToByte = EnergyToByte;
			float num = 0f;
			switch (State)
			{
			case global::InventorySystem.Items.MicroHID.HidState.Idle:
			case global::InventorySystem.Items.MicroHID.HidState.StopSound:
				if (RemainingEnergy > 0f && UserInput != global::InventorySystem.Items.MicroHID.HidUserInput.None)
				{
					State = global::InventorySystem.Items.MicroHID.HidState.PoweringUp;
					_stopwatch.Restart();
				}
				break;
			case global::InventorySystem.Items.MicroHID.HidState.PoweringUp:
				if ((UserInput == global::InventorySystem.Items.MicroHID.HidUserInput.None && _stopwatch.Elapsed.TotalSeconds >= 0.3499999940395355) || RemainingEnergy <= 0f)
				{
					global::InventorySystem.Items.MicroHID.MicroHIDItem.OnStopCharging?.Invoke(this);
					State = global::InventorySystem.Items.MicroHID.HidState.PoweringDown;
					_stopwatch.Restart();
				}
				else if (Readiness == 1f)
				{
					State = ((UserInput == global::InventorySystem.Items.MicroHID.HidUserInput.Fire) ? global::InventorySystem.Items.MicroHID.HidState.Firing : global::InventorySystem.Items.MicroHID.HidState.Primed);
					_stopwatch.Restart();
				}
				num = _energyConsumtionCurve.Evaluate((float)(_stopwatch.Elapsed.TotalSeconds / 5.949999809265137));
				break;
			case global::InventorySystem.Items.MicroHID.HidState.PoweringDown:
				if (_stopwatch.Elapsed.TotalSeconds >= 3.0999999046325684)
				{
					State = global::InventorySystem.Items.MicroHID.HidState.Idle;
					_stopwatch.Stop();
					_stopwatch.Reset();
				}
				break;
			case global::InventorySystem.Items.MicroHID.HidState.Primed:
				if ((UserInput != global::InventorySystem.Items.MicroHID.HidUserInput.Prime && _stopwatch.Elapsed.TotalSeconds >= 0.3499999940395355) || RemainingEnergy <= 0f)
				{
					if (UserInput == global::InventorySystem.Items.MicroHID.HidUserInput.Fire && RemainingEnergy > 0f)
					{
						State = global::InventorySystem.Items.MicroHID.HidState.Firing;
					}
					else
					{
						global::InventorySystem.Items.MicroHID.MicroHIDItem.OnStopCharging?.Invoke(this);
						State = global::InventorySystem.Items.MicroHID.HidState.PoweringDown;
					}
					_stopwatch.Restart();
				}
				else
				{
					num = _energyConsumtionCurve.Evaluate(1f);
				}
				break;
			case global::InventorySystem.Items.MicroHID.HidState.Firing:
				if (_stopwatch.Elapsed.TotalSeconds > 1.7000000476837158)
				{
					num = 0.13f;
					Fire();
					if (RemainingEnergy == 0f || (UserInput != global::InventorySystem.Items.MicroHID.HidUserInput.Fire && _stopwatch.Elapsed.TotalSeconds >= 2.049999952316284))
					{
						if (RemainingEnergy > 0f && UserInput == global::InventorySystem.Items.MicroHID.HidUserInput.Prime)
						{
							State = global::InventorySystem.Items.MicroHID.HidState.Primed;
						}
						else
						{
							global::InventorySystem.Items.MicroHID.MicroHIDItem.OnStopCharging?.Invoke(this);
							State = global::InventorySystem.Items.MicroHID.HidState.PoweringDown;
						}
						_stopwatch.Restart();
					}
				}
				else
				{
					num = _energyConsumtionCurve.Evaluate(1f);
				}
				break;
			}
			if (state != State)
			{
				ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.State, (byte)State);
			}
			if (num > 0f)
			{
				RemainingEnergy = global::UnityEngine.Mathf.Clamp01(RemainingEnergy - num * global::UnityEngine.Time.deltaTime);
				if (energyToByte != EnergyToByte)
				{
					ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType.EnergySync, EnergyToByte);
				}
			}
		}

		private void ServerSendStatus(global::InventorySystem.Items.MicroHID.HidStatusMessageType msgType, byte code)
		{
			global::Utils.Networking.NetworkUtils.SendToAuthenticated(new global::InventorySystem.Items.MicroHID.HidStatusMessage
			{
				MessageType = msgType,
				Serial = base.ItemSerial,
				MessageCode = code
			});
		}

		private void Fire()
		{
			if (_damageTimer > 0f)
			{
				_damageTimer -= global::UnityEngine.Time.deltaTime;
				return;
			}
			_damageTimer += 0.125f;
			global::System.Collections.Generic.HashSet<uint> hashSet = global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Rent();
			bool flag = false;
			float num = 143.75f;
			foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
			{
				if (allHub == base.Owner || !HitboxIdentity.CheckFriendlyFire(allHub, base.Owner))
				{
					continue;
				}
				global::UnityEngine.Vector3 direction = allHub.transform.position - base.Owner.transform.position;
				float num2 = global::UnityEngine.Vector3.Distance(allHub.transform.position, base.Owner.transform.position);
				if (!(num2 > 6.3f))
				{
					bool flag2 = HitboxIdentity.CheckFriendlyFire(allHub, base.Owner, ignoreConfig: true);
					float num3 = (flag2 ? 0.75f : 0.98f);
					if (((num2 < 0.7f && flag2) || global::UnityEngine.Vector3.Dot(direction.normalized, base.Owner.PlayerCameraReference.forward) >= num3) && !global::UnityEngine.Physics.Raycast(base.Owner.transform.position, direction, num2, WallMask) && hashSet.Add(allHub.networkIdentity.netId))
					{
						bool flag3 = allHub.playerStats.DealDamage(new global::PlayerStatsSystem.MicroHidDamageHandler(this, num));
						flag = flag || flag3;
					}
				}
			}
			if (global::UnityEngine.Physics.Raycast(base.Owner.PlayerCameraReference.position, base.Owner.PlayerCameraReference.forward, out var hitInfo, 6.3f, global::InventorySystem.Items.Firearms.Modules.StandardHitregBase.HitregMask) && hitInfo.collider.TryGetComponent<IDestructible>(out var component) && hashSet.Add(component.NetworkId) && (!(component is HitboxIdentity hitboxIdentity) || hitboxIdentity.TargetHub != base.Owner) && component.Damage(num, new global::PlayerStatsSystem.MicroHidDamageHandler(this, num), hitInfo.point))
			{
				flag = true;
			}
			if (flag)
			{
				Hitmarker.SendHitmarker(base.Owner, 1.5f);
			}
			global::NorthwoodLib.Pools.HashSetPool<uint>.Shared.Return(hashSet);
		}
	}
}
