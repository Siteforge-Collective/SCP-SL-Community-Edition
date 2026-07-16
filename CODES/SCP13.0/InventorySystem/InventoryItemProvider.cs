using System;
using System.Collections.Generic;
using System.Linq;
using GameCore;
using Hints;
using InventorySystem.Configs;
using InventorySystem.Items;
using InventorySystem.Items.Armor;
using InventorySystem.Items.Pickups;
using InventorySystem.Searching;
using Mirror;
using PlayerRoles;
using UnityEngine;

namespace InventorySystem
{
    public static class InventoryItemProvider
    {
        public static Action<ReferenceHub, ItemBase> OnItemProvided;

        private static readonly Dictionary<ReferenceHub, List<ItemPickupBase>> PreviousInventoryPickups = new Dictionary<ReferenceHub, List<ItemPickupBase>>();

        private static readonly Queue<ReferenceHub> InventoriesToReplenish = new Queue<ReferenceHub>();

        private static readonly bool KeepItemsAfterEscaping = ConfigFile.ServerConfig.GetBool("keep_items_after_escaping", def: true);

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            PlayerRoleManager.OnRoleChanged += RoleChanged;
            StaticUnityMethods.OnUpdate += Update;
        }

        private static void Update()
        {
            if (NetworkServer.active && InventoriesToReplenish.TryDequeue(out var result) && !(result == null))
            {
                SpawnPreviousInventoryPickups(result);
            }
        }

        public static void ServerGrantLoadout(ReferenceHub target, RoleTypeId roleTypeId, bool resetInventory = true)
        {
            if (!NetworkServer.active)
            {
                throw new InvalidOperationException("Method ServerGrantLoadout can only be executed on the server.");
            }
            Inventory inventory = target.inventory;
            if (resetInventory)
            {
                while (inventory.UserInventory.Items.Count > 0)
                {
                    inventory.ServerRemoveItem(inventory.UserInventory.Items.ElementAt(0).Key, null);
                }
                inventory.UserInventory.ReserveAmmo.Clear();
                inventory.SendAmmoNextFrame = true;
            }
            if (!StartingInventories.DefinedInventories.TryGetValue(roleTypeId, out var value))
            {
                return;
            }
            foreach (KeyValuePair<ItemType, ushort> item in value.Ammo)
            {
                inventory.ServerAddAmmo(item.Key, item.Value);
            }
            for (int i = 0; i < value.Items.Length; i++)
            {
                ItemBase arg = inventory.ServerAddItem(value.Items[i], 0);
                OnItemProvided?.Invoke(target, arg);
            }
        }

        private static void SpawnPreviousInventoryPickups(ReferenceHub hub)
        {
            if (!PreviousInventoryPickups.TryGetValue(hub, out var value))
            {
                return;
            }
            NetworkConnection connectionToClient = hub.connectionToClient;
            hub.transform.position = hub.transform.position;
            bool flag = HintDisplay.SuppressedReceivers.Add(connectionToClient);
            foreach (ItemPickupBase item in value)
            {
                if (!(item == null) && !item.Info.Locked)
                {
                    SearchCompletor searchCompletor = SearchCompletor.FromPickup(hub.searchCoordinator, item, 3.4028234663852886E+38);
                    if (searchCompletor.AllowPickupUponEscape && searchCompletor.ValidateStart())
                    {
                        searchCompletor.Complete();
                    }
                    else
                    {
                        item.transform.position = hub.transform.position;
                    }
                }
            }
            PreviousInventoryPickups.Remove(hub);
            if (flag)
            {
                HintDisplay.SuppressedReceivers.Remove(connectionToClient);
            }
        }

        private static void RoleChanged(ReferenceHub ply, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (!NetworkServer.active || !newRole.ServerSpawnFlags.HasFlag(RoleSpawnFlags.AssignInventory))
            {
                return;
            }
            Inventory inventory = ply.inventory;
            bool flag = KeepItemsAfterEscaping && newRole.ServerSpawnReason == RoleChangeReason.Escaped;
            if (flag)
            {
                List<ItemPickupBase> list = new List<ItemPickupBase>();
                if (inventory.TryGetBodyArmor(out var bodyArmor))
                {
                    bodyArmor.DontRemoveExcessOnDrop = true;
                }
                while (inventory.UserInventory.Items.Count > 0)
                {
                    list.Add(inventory.ServerDropItem(inventory.UserInventory.Items.ElementAt(0).Key));
                }
                PreviousInventoryPickups[ply] = list;
            }
            ServerGrantLoadout(ply, newRole.RoleTypeId, !flag);
            InventoriesToReplenish.Enqueue(ply);
        }
    }
}
