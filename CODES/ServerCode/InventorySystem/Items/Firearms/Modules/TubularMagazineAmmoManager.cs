namespace InventorySystem.Items.Firearms.Modules
{
	public class TubularMagazineAmmoManager : global::InventorySystem.Items.Firearms.Modules.IAmmoManagerModule, global::InventorySystem.Items.Firearms.Modules.IFirearmModuleBase
	{
		private enum CurrentAction
		{
			Idle = 0,
			Reloading = 1,
			Unloading = 2
		}

		private readonly global::InventorySystem.Items.Firearms.Firearm _firearm;

		private readonly byte _numberOfChambers;

		private readonly global::System.Diagnostics.Stopwatch _cooldownStopwatch;

		private readonly float _cooldownTime;

		private readonly int _bulletsToLoadAnimHash;

		private readonly int _animLoopLayer;

		private readonly ushort _serial;

		private readonly global::UnityEngine.KeyCode[] _cancelReloadKeys;

		private byte _defaultMaxAmmo;

		private global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction _currentAction;

		private global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction CurAction
		{
			get
			{
				if (!_firearm.IsSpectated)
				{
					return _currentAction;
				}
				global::InventorySystem.Items.Firearms.BasicMessages.RequestType reloadStateRaw = global::InventorySystem.Items.Firearms.BasicMessages.FirearmClientsideStateDatabase.GetReloadStateRaw(_serial);
				bool num = reloadStateRaw != global::InventorySystem.Items.Firearms.BasicMessages.RequestType.ReloadStop;
				bool flag = _currentAction != global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
				if (num == flag)
				{
					return _currentAction;
				}
				switch (reloadStateRaw)
				{
				case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Reload:
					_currentAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Reloading;
					break;
				case global::InventorySystem.Items.Firearms.BasicMessages.RequestType.Unload:
					_currentAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Unloading;
					break;
				default:
					_currentAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
					break;
				}
				return _currentAction;
			}
			set
			{
				if (!_firearm.IsSpectated)
				{
					_currentAction = value;
				}
			}
		}

		private int ChamberedRounds
		{
			get
			{
				if (!(_firearm.ActionModule is global::InventorySystem.Items.Firearms.Modules.PumpAction pumpAction))
				{
					return _numberOfChambers;
				}
				return pumpAction.ChamberedRounds;
			}
		}

		private bool ClientIsReloading => false;

		public byte MaxAmmo
		{
			get
			{
				return (byte)((float)(int)_defaultMaxAmmo + global::InventorySystem.Items.Firearms.Attachments.AttachmentsUtils.AttachmentsValue(_firearm, global::InventorySystem.Items.Firearms.Attachments.AttachmentParam.MagazineCapacityModifier) + (float)(global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(_firearm.Status.Flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked) ? ChamberedRounds : 0));
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
				if (CurAction == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle && _cooldownStopwatch.Elapsed.TotalSeconds > (double)_cooldownTime)
				{
					if (!_firearm.IsLocalPlayer)
					{
						return _firearm.ServerSideAnimator.GetCurrentAnimatorStateInfo(_animLoopLayer).tagHash != global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Reload;
					}
					return !ClientIsReloading;
				}
				return false;
			}
		}

		public TubularMagazineAmmoManager(global::InventorySystem.Items.Firearms.Firearm selfRef, ushort serial, byte maxAmmo, byte numberOfChambers, float cooldownTime, int reloadAnimatorLayer, string bulletsToLoadParamName, params ActionName[] cancelReloadActions)
		{
			_firearm = selfRef;
			_serial = serial;
			MaxAmmo = maxAmmo;
			_numberOfChambers = numberOfChambers;
			_cooldownTime = cooldownTime;
			_animLoopLayer = reloadAnimatorLayer;
			_bulletsToLoadAnimHash = global::UnityEngine.Animator.StringToHash(bulletsToLoadParamName);
			_cooldownStopwatch = global::System.Diagnostics.Stopwatch.StartNew();
			_cancelReloadKeys = new global::UnityEngine.KeyCode[cancelReloadActions.Length];
			for (int i = 0; i < _cancelReloadKeys.Length; i++)
			{
				_cancelReloadKeys[i] = NewInput.GetKey(cancelReloadActions[i]);
			}
			_firearm.OnHolsteredCalled += Holstered;
			_firearm.OnEquipUpdateCalled += EquipUpdate;
		}

		public bool ServerTryReload()
		{
			return ServerHandleRequest(global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Reloading);
		}

		public bool ServerTryUnload()
		{
			return ServerHandleRequest(global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Unloading);
		}

		private bool ServerHandleRequest(global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction action)
		{
			if (action == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle)
			{
				throw new global::System.InvalidOperationException("Server can only handle shotgun reload/unload requests!");
			}
			if (action == CurAction)
			{
				return false;
			}
			if (CurAction != global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle)
			{
				CurAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
				_cooldownStopwatch.Restart();
				return true;
			}
			if (!_firearm.EquipperModule.Standby || !_firearm.ActionModule.Standby)
			{
				return false;
			}
			if (_cooldownStopwatch.Elapsed.TotalSeconds < (double)_cooldownTime)
			{
				return false;
			}
			int ammo = _firearm.Status.Ammo;
			int curAmmo = _firearm.OwnerInventory.GetCurAmmo(_firearm.AmmoType);
			if (action == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Reloading && (ammo >= MaxAmmo || curAmmo == 0))
			{
				return false;
			}
			if (action == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Unloading && ammo <= 0)
			{
				return false;
			}
			CurAction = action;
			return true;
		}

		private void EquipUpdate()
		{
			_firearm.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Reload, CurAction == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Reloading);
			_firearm.AnimSetBool(global::InventorySystem.Items.Firearms.FirearmAnimatorHashes.Unload, CurAction == global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Unloading);
			if (CurAction != global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle)
			{
				if (_firearm.IsLocalPlayer)
				{
					UpdateLocalPlayerOnly();
				}
				switch (CurAction)
				{
				case global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Reloading:
					UpdateReload();
					break;
				case global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Unloading:
					UpdateUnload();
					break;
				}
			}
		}

		private void UpdateLocalPlayerOnly()
		{
		}

		private void UpdateReload()
		{
			int b = (_firearm.IsLocalPlayer ? _firearm.OwnerInventory.GetCurAmmo(_firearm.AmmoType) : MaxAmmo);
			int num = global::UnityEngine.Mathf.Min(global::UnityEngine.Mathf.Min(MaxAmmo - _firearm.Status.Ammo, b), _numberOfChambers);
			_firearm.AnimSetInt(_bulletsToLoadAnimHash, num);
			if (num <= 0)
			{
				CurAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
			}
		}

		private void UpdateUnload()
		{
			if (_firearm.Status.Ammo <= 0)
			{
				CurAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
			}
		}

		private void Holstered()
		{
			CurAction = global::InventorySystem.Items.Firearms.Modules.TubularMagazineAmmoManager.CurrentAction.Idle;
			_firearm.ServerSideAnimator.Rebind();
		}
	}
}
