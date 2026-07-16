namespace Scp914
{
	public static class Scp914Upgrader
	{
		public static int SolidObjectMask;

		public static global::System.Action<global::InventorySystem.Items.Pickups.ItemPickupBase, global::Scp914.Scp914KnobSetting> OnPickupUpgraded = delegate
		{
		};

		public static global::System.Action<global::InventorySystem.Items.ItemBase, global::Scp914.Scp914KnobSetting> OnInventoryItemUpgraded = delegate
		{
		};

		public static void Upgrade(global::UnityEngine.Collider[] intake, global::UnityEngine.Vector3 moveVector, global::Scp914.Scp914Mode mode, global::Scp914.Scp914KnobSetting setting)
		{
			if (!global::Mirror.NetworkServer.active)
			{
				throw new global::System.InvalidOperationException("Scp914Upgrader.Upgrade is a serverside-only script.");
			}
			global::System.Collections.Generic.HashSet<global::UnityEngine.GameObject> hashSet = global::NorthwoodLib.Pools.HashSetPool<global::UnityEngine.GameObject>.Shared.Rent();
			bool upgradeDropped = (mode & global::Scp914.Scp914Mode.Dropped) == global::Scp914.Scp914Mode.Dropped;
			bool flag = (mode & global::Scp914.Scp914Mode.Inventory) == global::Scp914.Scp914Mode.Inventory;
			bool heldOnly = flag && (mode & global::Scp914.Scp914Mode.Held) == global::Scp914.Scp914Mode.Held;
			for (int i = 0; i < intake.Length; i++)
			{
				global::UnityEngine.GameObject gameObject = intake[i].transform.root.gameObject;
				if (hashSet.Add(gameObject))
				{
					global::InventorySystem.Items.Pickups.ItemPickupBase component;
					if (ReferenceHub.TryGetHub(gameObject, out var hub))
					{
						ProcessPlayer(hub, flag, heldOnly, moveVector, setting);
					}
					else if (gameObject.TryGetComponent<global::InventorySystem.Items.Pickups.ItemPickupBase>(out component))
					{
						ProcessPickup(component, upgradeDropped, moveVector, setting);
					}
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<global::UnityEngine.GameObject>.Shared.Return(hashSet);
		}

		private static void ProcessPlayer(ReferenceHub ply, bool upgradeInventory, bool heldOnly, global::UnityEngine.Vector3 moveVector, global::Scp914.Scp914KnobSetting setting)
		{
			if (global::UnityEngine.Physics.Linecast(ply.transform.position, global::Scp914.Scp914Controller.Singleton.IntakeChamber.position, SolidObjectMask))
			{
				return;
			}
			global::UnityEngine.Vector3 vector = ply.transform.position + moveVector;
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914ProcessPlayer, ply, setting, vector))
			{
				return;
			}
			global::PlayerRoles.FirstPersonControl.FpcExtensionMethods.TryOverridePosition(ply, vector, global::UnityEngine.Vector3.zero);
			if (!upgradeInventory)
			{
				return;
			}
			global::System.Collections.Generic.HashSet<ushort> hashSet = global::NorthwoodLib.Pools.HashSetPool<ushort>.Shared.Rent();
			foreach (global::System.Collections.Generic.KeyValuePair<ushort, global::InventorySystem.Items.ItemBase> item in ply.inventory.UserInventory.Items)
			{
				if (!heldOnly || item.Key == ply.inventory.CurItem.SerialNumber)
				{
					hashSet.Add(item.Key);
				}
			}
			foreach (ushort item2 in hashSet)
			{
				if (ply.inventory.UserInventory.Items.TryGetValue(item2, out var value) && TryGetProcessor(value.ItemTypeId, out var processor) && global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914UpgradeInventory, ply, value, setting))
				{
					OnInventoryItemUpgraded?.Invoke(value, setting);
					global::InventorySystem.Items.ItemBase itemBase = processor.OnInventoryItemUpgraded(setting, ply, item2);
					if (itemBase != null)
					{
						global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914InventoryItemUpgraded, ply, itemBase, setting);
					}
				}
			}
			global::NorthwoodLib.Pools.HashSetPool<ushort>.Shared.Return(hashSet);
			global::InventorySystem.Items.Armor.BodyArmorUtils.RemoveEverythingExceedingLimits(ply.inventory, global::InventorySystem.Items.Armor.BodyArmorUtils.TryGetBodyArmor(ply.inventory, out var bodyArmor) ? bodyArmor : null);
		}

		private static void ProcessPickup(global::InventorySystem.Items.Pickups.ItemPickupBase pickup, bool upgradeDropped, global::UnityEngine.Vector3 moveVector, global::Scp914.Scp914KnobSetting setting)
		{
			if (!(!pickup.Info.Locked && upgradeDropped) || !TryGetProcessor(pickup.Info.ItemId, out var processor))
			{
				return;
			}
			global::UnityEngine.Vector3 vector = pickup.transform.position + moveVector;
			if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914UpgradePickup, pickup, vector, setting))
			{
				OnPickupUpgraded?.Invoke(pickup, setting);
				global::InventorySystem.Items.Pickups.ItemPickupBase itemPickupBase = processor.OnPickupUpgraded(setting, pickup, vector);
				if (itemPickupBase != null)
				{
					global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.Scp914PickupUpgraded, itemPickupBase, vector, setting);
				}
			}
		}

		private static bool TryGetProcessor(ItemType itemType, out global::Scp914.Processors.Scp914ItemProcessor processor)
		{
			if (global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(itemType, out var value) && value.TryGetComponent<global::Scp914.Processors.Scp914ItemProcessor>(out processor))
			{
				return true;
			}
			processor = null;
			return false;
		}
	}
}
