namespace InventorySystem.Items.Firearms
{
	public class AutomaticFirearmAnimatorEvents : global::InventorySystem.Items.Firearms.FirearmAnimatorEventsBase
	{
		private float _curGripStatus;

		private float _gripMoveSpeed;

		private void InsertMagazine()
		{
			if (IsServerController)
			{
				global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = base.TargetFirearm.Status.Flags;
				ushort curAmmo = base.TargetFirearm.OwnerInventory.GetCurAmmo(base.TargetFirearm.AmmoType);
				flags |= global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted;
				byte b = (byte)global::UnityEngine.Mathf.Min(curAmmo, base.TargetFirearm.AmmoManagerModule.MaxAmmo - base.TargetFirearm.Status.Ammo);
				ModifyUserAmmo(-b);
				base.TargetFirearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)(base.TargetFirearm.Status.Ammo + b), flags, base.TargetFirearm.Status.Attachments);
			}
		}

		private void RemoveMagazine()
		{
			if (IsServerController)
			{
				global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = base.TargetFirearm.Status.Flags;
				if (global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted))
				{
					int num = ((base.TargetFirearm.AmmoManagerModule is global::InventorySystem.Items.Firearms.Modules.AutomaticAmmoManager automaticAmmoManager) ? automaticAmmoManager.ChamberedAmount : 0);
					flags &= ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted;
					ModifyUserAmmo(base.TargetFirearm.Status.Ammo - num);
					base.TargetFirearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)num, flags, base.TargetFirearm.Status.Attachments);
				}
			}
		}

		private void RemoveMagazineOpenBolt()
		{
			if (IsServerController)
			{
				ModifyUserAmmo(base.TargetFirearm.Status.Ammo);
				base.TargetFirearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(0, base.TargetFirearm.Status.Flags & ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted, base.TargetFirearm.Status.Attachments);
			}
		}

		private void UseChargingHandle()
		{
			if (IsServerController)
			{
				global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = base.TargetFirearm.Status.Flags;
				flags |= global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered;
				flags |= global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked;
				base.TargetFirearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(base.TargetFirearm.Status.Ammo, flags, base.TargetFirearm.Status.Attachments);
			}
		}

		private void UnloadChamberedBullet()
		{
			if (!IsServerController)
			{
				return;
			}
			global::InventorySystem.Items.Firearms.FirearmStatusFlags flags = base.TargetFirearm.Status.Flags;
			if (base.TargetFirearm.Status.Ammo != 0 && !global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted) && global::InventorySystem.Items.Firearms.BasicMessages.FirearmBasicMessagesHandler.HasFlagFast(flags, global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered))
			{
				if (base.TargetFirearm.AmmoManagerModule is global::InventorySystem.Items.Firearms.Modules.AutomaticAmmoManager automaticAmmoManager)
				{
					ModifyUserAmmo(automaticAmmoManager.ChamberedAmount);
				}
				flags &= ~global::InventorySystem.Items.Firearms.FirearmStatusFlags.Chambered;
				flags |= global::InventorySystem.Items.Firearms.FirearmStatusFlags.Cocked;
				base.TargetFirearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus(0, flags, base.TargetFirearm.Status.Attachments);
			}
		}

		private void MarkAsEquipped()
		{
			if ((!base.TargetFirearm.IsLocalPlayer || !IsServerController) && base.TargetFirearm.EquipperModule is global::InventorySystem.Items.Firearms.Modules.EventBasedEquipper eventBasedEquipper)
			{
				eventBasedEquipper.Equip();
			}
		}

		private void SetGripBlendSpeed(float speed)
		{
		}
	}
}
