using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items.Firearms.BasicMessages;
using InventorySystem.Items.Pickups;
using Mirror;
using NorthwoodLib.Pools;
using PluginAPI.Events;
using UnityEngine;
using Utils.Networking;


namespace InventorySystem
{
	public static class InventoryExtensions
	{
        public static event Action<ReferenceHub, ItemBase, ItemPickupBase> OnItemAdded;

        public static event Action<ReferenceHub, ItemBase, ItemPickupBase> OnItemRemoved;

        public static ItemType GetSelectedItemType(this Inventory inv)
        {
            if (inv.CurItem.SerialNumber <= 0)
            {
                return ItemType.None;
            }
            return inv.CurItem.TypeId;
        }

        public static bool TryGetHubHoldingSerial(ushort serial, out ReferenceHub hub)
        {
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.inventory.CurItem.SerialNumber == serial)
                {
                    hub = allHub;
                    return true;
                }
            }
            hub = null;
            return false;
        }

        public static bool ServerTryGetItemWithSerial(ushort serial, out ItemBase ib)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerTryGetItemWithSerial can only be executed on the server.");
            }
            foreach (ReferenceHub allHub in ReferenceHub.AllHubs)
            {
                if (allHub.inventory.UserInventory.Items.TryGetValue(serial, out ib))
                {
                    return true;
                }
            }
            ib = null;
            return false;
        }

        public static ItemBase ServerAddItem(this Inventory inv, ItemType type, ushort itemSerial = 0, ItemPickupBase pickup = null)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerAddItem can only be executed on the server.");
            }
            if (inv.UserInventory.Items.Count >= 8 && InventoryItemLoader.AvailableItems.TryGetValue(type, out var value) && value.Category != ItemCategory.Ammo)
            {
                return null;
            }
            if (itemSerial == 0)
            {
                itemSerial = ItemSerialGenerator.GenerateNext();
            }
            ItemBase itemBase = inv.CreateItemInstance(new ItemIdentifier(type, itemSerial), inv.isLocalPlayer);
            if (itemBase == null)
            {
                return null;
            }
            inv.UserInventory.Items[itemSerial] = itemBase;
            itemBase.OnAdded(pickup);
            InventoryExtensions.OnItemAdded?.Invoke(inv._hub, itemBase, pickup);
            if (inv.isLocalPlayer && itemBase is IAcquisitionConfirmationTrigger acquisitionConfirmationTrigger)
            {
                acquisitionConfirmationTrigger.ServerConfirmAcqusition();
                acquisitionConfirmationTrigger.AcquisitionAlreadyReceived = true;
            }
            inv.SendItemsNextFrame = true;
            return itemBase;
        }

        public static void ServerRemoveItem(this Inventory inv, ushort itemSerial, ItemPickupBase ipb)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerRemoveItem can only be executed on the server.");
            }
            if (inv.DestroyItemInstance(itemSerial, ipb, out var foundItem))
            {
                if (itemSerial == inv.CurItem.SerialNumber)
                {
                    inv.CurItem = ItemIdentifier.None;
                }
                inv.UserInventory.Items.Remove(itemSerial);
                inv.SendItemsNextFrame = true;
                InventoryExtensions.OnItemRemoved?.Invoke(inv._hub, foundItem, ipb);
            }
        }

        public static ItemPickupBase ServerDropItem(this Inventory inv, ushort itemSerial)
        {
            if (!inv.UserInventory.Items.TryGetValue(itemSerial, out var value))
            {
                return null;
            }
            return value.ServerDropItem();
        }

        public static void ServerDropEverything(this Inventory inv)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerDropEverything can only be executed on the server.");
            }
            HashSet<ItemType> hashSet = HashSetPool<ItemType>.Shared.Rent();
            foreach (KeyValuePair<ItemType, ushort> item in inv.UserInventory.ReserveAmmo)
            {
                if (item.Value > 0)
                {
                    hashSet.Add(item.Key);
                }
            }
            foreach (ItemType item2 in hashSet)
            {
                inv.ServerDropAmmo(item2, ushort.MaxValue);
            }
            while (inv.UserInventory.Items.Count > 0)
            {
                inv.ServerDropItem(inv.UserInventory.Items.ElementAt(0).Key);
            }
            HashSetPool<ItemType>.Shared.Return(hashSet);
        }

        public static List<AmmoPickup> ServerDropAmmo(this Inventory inv, ItemType ammoType, ushort amount, bool checkMinimals = false)
        {
            List<AmmoPickup> list = new List<AmmoPickup>();
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerDropAmmo can only be executed on the server.");
            }
            if (inv.CurInstance is Firearm firearm && firearm.AmmoManagerModule.ServerTryStopReload())
            {
                new RequestMessage(inv.CurItem.SerialNumber, RequestType.ReloadStop).SendToAuthenticated();
            }
            if (inv.UserInventory.ReserveAmmo.TryGetValue(ammoType, out var value) && InventoryItemLoader.AvailableItems.TryGetValue(ammoType, out var value2))
            {
                if (value2.PickupDropModel == null)
                {
                    Debug.LogError("No pickup drop model set. Could not drop the ammo.");
                    return list;
                }
                if (checkMinimals && value2.PickupDropModel is AmmoPickup ammoPickup)
                {
                    int num = Mathf.FloorToInt((float)(int)ammoPickup.SavedAmmo / 2f);
                    if (amount < num && value > num)
                    {
                        amount = (ushort)num;
                    }
                }
                int num2 = Mathf.Min(amount, value);
                if (!EventManager.ExecuteEvent(new PlayerDropAmmoEvent(inv._hub, ammoType, num2)))
                {
                    return list;
                }
                inv.UserInventory.ReserveAmmo[ammoType] = (ushort)(value - num2);
                inv.SendAmmoNextFrame = true;
                while (num2 > 0)
                {
                    if (ServerCreatePickup(psi: new PickupSyncInfo(ammoType, value2.Weight, 0), inv: inv, item: value2) is AmmoPickup ammoPickup2)
                    {
                        list.Add(ammoPickup2);
                        int maxAmmo = ammoPickup2.MaxAmmo;
                        PlayerDroppedAmmoEvent playerDroppedAmmoEvent = new PlayerDroppedAmmoEvent(inv._hub, ammoPickup2, num2, maxAmmo);
                        if (EventManager.ExecuteEvent(playerDroppedAmmoEvent))
                        {
                            num2 = playerDroppedAmmoEvent.Amount;
                            maxAmmo = playerDroppedAmmoEvent.MaxAmount;
                            ushort networkSavedAmmo = (ushort)Mathf.Min(maxAmmo, num2);
                            ammoPickup2.SavedAmmo = networkSavedAmmo;
                            num2 -= ammoPickup2.SavedAmmo;
                        }
                    }
                    else
                    {
                        num2--;
                    }
                }
                return list;
            }
            return list;
        }

        public static ItemPickupBase ServerCreatePickup(this Inventory inv, ItemBase item, PickupSyncInfo psi, bool spawn = true, Action<ItemPickupBase> setupMethod = null)
        {
            Quaternion rotation = ReferenceHub.GetHub(inv.gameObject).PlayerCameraReference.rotation;
            Quaternion rotation2 = item.PickupDropModel.transform.rotation;
            return ServerCreatePickup(item, psi, inv.transform.position, rotation * rotation2, spawn, setupMethod);
        }

        public static ItemPickupBase ServerCreatePickup(ItemBase item, PickupSyncInfo psi, Vector3 position, bool spawn = true, Action<ItemPickupBase> setupMethod = null)
        {
            return ServerCreatePickup(item, psi, position, item.PickupDropModel.transform.rotation, spawn, setupMethod);
        }

        public static ItemPickupBase ServerCreatePickup(ItemBase item, PickupSyncInfo psi, Vector3 position, Quaternion rotation, bool spawn = true, Action<ItemPickupBase> setupMethod = null)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerCreatePickup can only be executed on the server.");
            }
            ItemPickupBase itemPickupBase = UnityEngine.Object.Instantiate(item.PickupDropModel, position, rotation);
            itemPickupBase.Info = psi;
            setupMethod?.Invoke(itemPickupBase);
            if (spawn)
            {
                NetworkServer.Spawn(itemPickupBase.gameObject);
            }
            return itemPickupBase;
        }

        public static ushort GetCurAmmo(this Inventory inv, ItemType ammoType)
        {
            if (!inv.UserInventory.ReserveAmmo.TryGetValue(ammoType, out var value))
            {
                return 0;
            }
            return value;
        }


        public static void ServerSetAmmo(this Inventory inv, ItemType ammoType, int amount)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerSetAmmo can only be executed on the server.");
            }
            amount = Mathf.Clamp(amount, 0, 65535);
            inv.UserInventory.ReserveAmmo[ammoType] = (ushort)amount;
            inv.SendAmmoNextFrame = true;
        }


        public static void ServerAddAmmo(this Inventory inv, ItemType ammoType, int amountToAdd)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerAddAmmo can only be executed on the server.");
            }
            inv.ServerSetAmmo(ammoType, inv.GetCurAmmo(ammoType) + amountToAdd);
        }

        public static bool TryGetInventoryItem(this Inventory inv, ItemType it, out ItemBase ib)
        {
            foreach (KeyValuePair<ushort, ItemBase> item in inv.UserInventory.Items)
            {
                if (item.Value.ItemTypeId == it)
                {
                    ib = item.Value;
                    return true;
                }
            }
            ib = null;
            return false;
        }
    }
}
