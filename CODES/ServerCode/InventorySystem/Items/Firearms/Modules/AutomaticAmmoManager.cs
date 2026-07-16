namespace InventorySystem.Items.Firearms.Modules
{
	public class AutomaticAmmoManager : global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private const float MinimalBusyTime = 0.3f;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly int _reloadTriggerHash;

		private readonly int _unloadTriggerHash;

		private readonly int _defaultAnimHash;

		private readonly int _idleTagHash;

		private readonly global::System.Diagnostics.Stopwatch _busyStopwatch;

		private readonly int _reloadAnimsLayer;

		private readonly int _chamberSize;

		private bool _isBusy;

		private byte _defaultMaxAmmo;

		public int ChamberedAmount
		{
			get
			{
				if (!global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered))
				{
					return 0;
				}
				return _chamberSize;
			}
		}

		public byte MaxAmmo
		{
			get
			{
				return (byte)((float)(int)_defaultMaxAmmo + global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.MagazineCapacityModifier) + (float)ChamberedAmount);
			}
			private set
			{
				_defaultMaxAmmo = value;
			}
		}

		public bool Standby
		{
			get
			{
				if (_isBusy)
				{
					return _firearm.IsSpectated;
				}
				return true;
			}
		}

		private ushort UserAmmo
		{
			get
			{
				if (!_firearm.OwnerInventory.UserInventory.ReserveAmmo.TryGetValue(_firearm.AmmoType, out var value))
				{
					return 0;
				}
				return value;
			}
		}

		public AutomaticAmmoManager(global::InventorySystem.Items.Firearms.Firearm selfRef, byte maxAmmo, int reloadAnimsLayer, int chamberSize)
		{
			_firearm = selfRef;
			MaxAmmo = maxAmmo;
			_reloadTriggerHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Reload;
			_unloadTriggerHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Unload;
			_idleTagHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Idle;
			_busyStopwatch = new global::System.Diagnostics.Stopwatch();
			_reloadAnimsLayer = reloadAnimsLayer;
			_chamberSize = chamberSize;
			_firearm.OnEquipUpdateCalled += EquipUpdate;
			_firearm.OnHolsteredCalled += CancelReload;
			if (global::Mirror.NetworkServer.active)
			{
				_defaultAnimHash = _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(_reloadAnimsLayer).fullPathHash;
			}
		}

		public bool ServerTryReload()
		{
			if (_isBusy || _firearm.Status.Ammo >= MaxAmmo)
			{
				return false;
			}
			if (!_firearm.EquipperModule.Standby || !_firearm.ActionModule.Standby)
			{
				return false;
			}
			if (UserAmmo < global::UnityEngine.Mathf.Max(1, _chamberSize))
			{
				return false;
			}
			_isBusy = true;
			_busyStopwatch.Restart();
			_firearm.ServerSideAnimator.SetTrigger(_reloadTriggerHash);
			return true;
		}

		public bool ServerTryUnload()
		{
			if (_isBusy || _firearm.Status.Ammo == 0)
			{
				return false;
			}
			if (!_firearm.EquipperModule.Standby || !_firearm.ActionModule.Standby)
			{
				return false;
			}
			_isBusy = true;
			_busyStopwatch.Restart();
			_firearm.ServerSideAnimator.SetTrigger(_unloadTriggerHash);
			return true;
		}

		private void EquipUpdate()
		{
			if (_isBusy && !(_busyStopwatch.Elapsed.TotalSeconds < 0.30000001192092896) && (_firearm.IsLocalPlayer || _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(_reloadAnimsLayer).tagHash == _idleTagHash))
			{
				_isBusy = false;
			}
		}

		private void CancelReload()
		{
			_isBusy = false;
			_busyStopwatch.Stop();
			if (global::Mirror.NetworkServer.active)
			{
				_firearm.ServerSideAnimator.Play(_defaultAnimHash);
			}
		}
	}
}
