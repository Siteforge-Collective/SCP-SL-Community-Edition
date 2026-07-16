namespace InventorySystem.Items.Firearms.Modules
{
	public class ClipLoadedInternalMagAmmoManager : global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private const float MinimalBusyTime = 0.3f;

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly int _reloadTriggerHash;

		private readonly int _unloadTriggerHash;

		private readonly int _defaultAnimHash;

		private readonly int _idleTagHash;

		private readonly global::System.Diagnostics.Stopwatch _busyStopwatch;

		private bool _isBusy;

		private byte _defaultMaxAmmo;

		public byte MaxAmmo
		{
			get
			{
				return (byte)((float)(int)_defaultMaxAmmo + global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.MagazineCapacityModifier));
			}
			private set
			{
				_defaultMaxAmmo = value;
			}
		}

		public bool Standby => !_isBusy;

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

		public ClipLoadedInternalMagAmmoManager(global::InventorySystem.Items.Firearms.Firearm selfRef, byte maxAmmo)
		{
			_firearm = selfRef;
			MaxAmmo = maxAmmo;
			_reloadTriggerHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Reload;
			_unloadTriggerHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Unload;
			_idleTagHash = global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Idle;
			_busyStopwatch = new global::System.Diagnostics.Stopwatch();
			_firearm.OnEquipUpdateCalled += EquipUpdate;
			if (global::Mirror.NetworkServer.active)
			{
				_firearm.OnHolsteredCalled += ServerCancelReload;
				_defaultAnimHash = _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
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
			if (UserAmmo == 0)
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
			if (_isBusy || (_firearm.Status.Ammo == 0 && !global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted)))
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
			if (_isBusy && !(_busyStopwatch.Elapsed.TotalSeconds < 0.30000001192092896) && (_firearm.IsLocalPlayer || _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(0).tagHash == _idleTagHash))
			{
				_isBusy = false;
			}
		}

		private void ServerCancelReload()
		{
			_isBusy = false;
			_busyStopwatch.Stop();
			_firearm.ServerSideAnimator.Play(_defaultAnimHash);
		}
	}
}
