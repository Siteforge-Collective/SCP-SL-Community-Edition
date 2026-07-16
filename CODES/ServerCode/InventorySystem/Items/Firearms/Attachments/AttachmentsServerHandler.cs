namespace InventorySystem.Items.Firearms.Attachments
{
	public static class AttachmentsServerHandler
	{
		public static readonly global::System.Collections.Generic.Dictionary<ReferenceHub, global::System.Collections.Generic.Dictionary<ItemType, uint>> PlayerPreferences = new global::System.Collections.Generic.Dictionary<ReferenceHub, global::System.Collections.Generic.Dictionary<ItemType, uint>>();

		[global::UnityEngine.RuntimeInitializeOnLoadMethod]
		private static void Init()
		{
			global::InventorySystem.Inventory.OnServerStarted += RegisterNetworkHandlers;
			global::UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ResetOnSceneChange;
			global::InventorySystem.InventoryItemProvider.OnItemProvided = (global::System.Action<ReferenceHub, global::InventorySystem.Items.ItemBase>)global::System.Delegate.Combine(global::InventorySystem.InventoryItemProvider.OnItemProvided, new global::System.Action<ReferenceHub, global::InventorySystem.Items.ItemBase>(SetupProvidedWeapon));
		}

		private static void RegisterNetworkHandlers()
		{
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreference>(ServerReceivePreference);
			global::Mirror.NetworkServer.ReplaceHandler<global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequest>(ServerReceiveChangeRequest);
		}

		private static void ServerReceiveChangeRequest(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.Attachments.AttachmentsChangeRequest msg)
		{
			if (!global::Mirror.NetworkServer.active || !ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub) || !(hub.inventory.CurInstance is global::InventorySystem.Items.Firearms.Firearm firearm) || hub.inventory.CurItem.SerialNumber != msg.WeaponSerial)
			{
				return;
			}
			bool flag = hub.roleManager.CurrentRole is global::PlayerRoles.Spectating.SpectatorRole;
			if (!flag)
			{
				foreach (global::InventorySystem.Items.Firearms.Attachments.WorkstationController allWorkstation in global::InventorySystem.Items.Firearms.Attachments.WorkstationController.AllWorkstations)
				{
					if (!(allWorkstation == null) && allWorkstation.Status == 3 && allWorkstation.IsInRange(hub))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				firearm.ApplyAttachmentsCode(msg.AttachmentsCode, reValidate: true);
				if (firearm.Status.Ammo > firearm.AmmoManagerModule.MaxAmmo)
				{
					hub.inventory.ServerAddAmmo(firearm.AmmoType, firearm.Status.Ammo - firearm.AmmoManagerModule.MaxAmmo);
				}
				firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)global::UnityEngine.Mathf.Min(firearm.Status.Ammo, firearm.AmmoManagerModule.MaxAmmo), firearm.Status.Flags, msg.AttachmentsCode);
			}
		}

		private static void ServerReceivePreference(global::Mirror.NetworkConnection conn, global::InventorySystem.Items.Firearms.Attachments.AttachmentsSetupPreference msg)
		{
			if (global::Mirror.NetworkServer.active && ReferenceHub.TryGetHub(conn.identity.gameObject, out var hub))
			{
				if (PlayerPreferences.TryGetValue(hub, out var value) && value != null)
				{
					value[msg.Weapon] = msg.AttachmentsCode;
					return;
				}
				PlayerPreferences[hub] = new global::System.Collections.Generic.Dictionary<ItemType, uint> { [msg.Weapon] = msg.AttachmentsCode };
			}
		}

		private static void SetupProvidedWeapon(ReferenceHub ply, global::InventorySystem.Items.ItemBase item)
		{
			if (global::Mirror.NetworkServer.active && item is global::InventorySystem.Items.Firearms.Firearm firearm)
			{
				if (!PlayerPreferences.TryGetValue(ply, out var value) || !value.TryGetValue(item.ItemTypeId, out var value2))
				{
					value2 = 0u;
				}
				value2 = firearm.ValidateAttachmentsCode(value2);
				firearm.ApplyAttachmentsCode(value2, reValidate: false);
				if (ply.inventory.UserInventory.ReserveAmmo.TryGetValue(firearm.AmmoType, out var value3))
				{
					int num = global::UnityEngine.Mathf.Min(value3, firearm.AmmoManagerModule.MaxAmmo);
					ply.inventory.UserInventory.ReserveAmmo[firearm.AmmoType] = (ushort)(value3 - num);
					bool flag = firearm.HasAdvantageFlag(global::InventorySystem.Items.Firearms.Attachments.AttachmentDescriptiveAdvantages.Flashlight);
					firearm.Status = new global::InventorySystem.Items.Firearms.FirearmStatus((byte)num, flag ? (global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted | global::InventorySystem.Items.Firearms.FirearmStatusFlags.FlashlightEnabled) : global::InventorySystem.Items.Firearms.FirearmStatusFlags.MagazineInserted, value2);
				}
			}
		}

		private static void ResetOnSceneChange(global::UnityEngine.SceneManagement.Scene arg1, global::UnityEngine.SceneManagement.Scene arg2)
		{
			PlayerPreferences.Clear();
		}
	}
}
