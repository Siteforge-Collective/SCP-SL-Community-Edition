namespace InventorySystem.Items.Firearms.Modules
{
	public class DisruptorAction : global::InventorySystem.Items.Firearms.Modules.IActionModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase, global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule
	{
		private const float StatusUpdateTime = 0.4f;

		private const float PostShotCooldown = 1.5f;

		private const float AdsCooldown = 0.1f;

		private const float DestroyTime = 3.1f;

		private const float ShotAnimTime = 2.2667f;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly bool _isAmmoManager;

		private global::InventorySystem.Items.Firearms.FirearmStatus _predictedStatus;

		private float _lastShotTime;

		private bool _allowLoadSound;

		public const int MaxShots = 5;

		public readonly float ShotDelay;

		private float TimeSinceLastShot => CurTime - _lastShotTime;

		private float CurTime => global::UnityEngine.Time.timeSinceLevelLoad;

		public global::InventorySystem.Items.Firearms.FirearmStatus PredictedStatus
		{
			get
			{
				if (TimeSinceLastShot >= ShotDelay + 0.4f || !_firearm.IsLocalPlayer)
				{
					_predictedStatus = _firearm.Status;
				}
				return _predictedStatus;
			}
			private set
			{
				_predictedStatus = value;
			}
		}

		private bool IsReloading => _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(0).tagHash == global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Reload;

		private bool ModulesReady
		{
			get
			{
				if (_firearm.AmmoManagerModule.Standby && _firearm.EquipperModule.Standby)
				{
					return _firearm.AdsModule.Standby;
				}
				return false;
			}
		}

		private float ActualCooldown
		{
			get
			{
				if (!_firearm.IsLocalPlayer)
				{
					return 1.5f;
				}
				return 1.5f + ShotDelay;
			}
		}

		public bool Standby
		{
			get
			{
				if (!_isAmmoManager)
				{
					if (!_firearm.IsLocalPlayer || !ShotTriggered)
					{
						return TimeSinceLastShot > ActualCooldown;
					}
					return false;
				}
				return !IsReloading;
			}
		}

		public float CyclicRate { get; private set; }

		public bool IsTriggerHeld { get; private set; }

		public byte MaxAmmo => 5;

		public bool ShotTriggered { get; private set; }

		public bool ClientCanReload => false;

		public bool ClientCanUnload => false;

		public DisruptorAction(global::InventorySystem.Items.Firearms.Firearm selfRef, float reloadTime, float chargeupTime, bool isAmmoManager)
		{
			selfRef.OnShotCalled += delegate
			{
				_allowLoadSound = true;
			};
			selfRef.OnHolsteredCalled += delegate
			{
				ShotTriggered = false;
			};
			_firearm = selfRef;
			ShotDelay = chargeupTime;
			CyclicRate = 1f / (reloadTime + chargeupTime);
			_isAmmoManager = isAmmoManager;
			_allowLoadSound = true;
		}

		public global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse DoClientsideAction(bool isTriggerPressed)
		{
			return global::InventorySystem.Items.Firearms.Modules.ActionModuleResponse.Idle;
		}

		public bool ServerAuthorizeShot()
		{
			if (!_firearm.IsLocalPlayer && TimeSinceLastShot < 1.5f)
			{
				return false;
			}
			if (_firearm.Status.Ammo <= 0)
			{
				_firearm.OwnerInventory.ServerRemoveItem(_firearm.ItemSerial, null);
				return false;
			}
			if (!ModulesReady)
			{
				_firearm.Owner.gameConsoleTransmission.SendToClient(_firearm.OwnerInventory.connectionToClient, $"Shot rejected, ammoManager={_firearm.AmmoManagerModule.Standby}, equipperModule={_firearm.EquipperModule.Standby}, adsModule={_firearm.AdsModule.Standby}", "gray");
				return false;
			}
			_firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)(_firearm.Status.Ammo - 1), _firearm.Status.Flags, _firearm.Status.Attachments);
			_firearm.ServerSendAudioMessage(0);
			if (!_firearm.IsLocalPlayer)
			{
				_lastShotTime = CurTime;
			}
			_firearm.ServerSideAnimator.Play(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Fire, 0, ShotDelay / 2.2667f);
			return true;
		}

		public bool ServerTryReload()
		{
			if (!_allowLoadSound)
			{
				return false;
			}
			_firearm.ServerSendAudioMessage(1);
			_allowLoadSound = false;
			return false;
		}

		public bool ServerAuthorizeDryFire()
		{
			return false;
		}

		public bool ServerTryUnload()
		{
			return false;
		}

		public void ClientReload()
		{
		}

		public void ClientUnload()
		{
		}
	}
}
