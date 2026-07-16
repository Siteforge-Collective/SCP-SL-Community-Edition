using System.Collections.Generic;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items.Pickups;
using Mirror;
using NorthwoodLib.Pools;
using PluginAPI.Events;
using UnityEngine;

namespace MapGeneration.Distributors
{
    public class ItemDistributor : SpawnablesDistributorBase
    {
        protected override void PlaceSpawnables()
        {
            while (ItemSpawnpoint.RandomInstances.Remove(null))
            {
            }
            while (ItemSpawnpoint.AutospawnInstances.Remove(null))
            {
            }
            SpawnableItem[] spawnableItems = Settings.SpawnableItems;
            foreach (SpawnableItem item in spawnableItems)
            {
                PlaceItem(item);
            }
            foreach (ItemSpawnpoint autospawnInstance in ItemSpawnpoint.AutospawnInstances)
            {
                Transform t = autospawnInstance.Occupy();
                CreatePickup(autospawnInstance.AutospawnItem, t, autospawnInstance.TriggerDoorName);
            }
        }

        private void PlaceItem(SpawnableItem item)
        {
            float num = Random.Range(item.MinimalAmount, item.MaxAmount);
            List<ItemSpawnpoint> list = ListPool<ItemSpawnpoint>.Shared.Rent();
            foreach (ItemSpawnpoint randomInstance in ItemSpawnpoint.RandomInstances)
            {
                if (item.RoomNames.Contains(randomInstance.RoomName) && randomInstance.CanSpawn(item.PossibleSpawns))
                {
                    list.Add(randomInstance);
                }
            }
            if (item.MultiplyBySpawnpointsNumber)
            {
                num *= (float)list.Count;
            }
            for (int i = 0; (float)i < num; i++)
            {
                if (list.Count == 0)
                {
                    break;
                }
                ItemType itemType = item.PossibleSpawns[Random.Range(0, item.PossibleSpawns.Length)];
                if (itemType == ItemType.None)
                {
                    continue;
                }
                int index = Random.Range(0, list.Count);
                Transform transform = list[index].Occupy();
                if (EventManager.ExecuteEvent(new ItemSpawnedEvent(itemType, transform.transform.position)))
                {
                    CreatePickup(itemType, transform, list[index].TriggerDoorName);
                    if (!list[index].CanSpawn(itemType))
                    {
                        list.RemoveAt(index);
                    }
                }
            }
            ListPool<ItemSpawnpoint>.Shared.Return(list);
        }

        private void CreatePickup(ItemType id, Transform t, string triggerDoor)
        {
            if (InventoryItemLoader.AvailableItems.TryGetValue(id, out var value))
            {
                ItemPickupBase itemPickupBase = Object.Instantiate(value.PickupDropModel, t.position, t.rotation);
                itemPickupBase.Info.ItemId = id;
                itemPickupBase.Info.WeightKg = value.Weight;
                itemPickupBase.transform.SetParent(t);
                (itemPickupBase as IPickupDistributorTrigger)?.OnDistributed();
                if (string.IsNullOrEmpty(triggerDoor) || !DoorNametagExtension.NamedDoors.TryGetValue(triggerDoor, out var value2))
                {
                    SpawnPickup(itemPickupBase);
                }
                else
                {
                    RegisterUnspawnedObject(value2.TargetDoor, itemPickupBase.gameObject);
                }
            }
        }

        public static void SpawnPickup(ItemPickupBase ipb)
        {
            if (!(ipb == null))
            {
                NetworkServer.Spawn(ipb.gameObject);
                PickupSyncInfo pickupSyncInfo = new PickupSyncInfo(ipb.Info.ItemId, ipb.Info.WeightKg, 0);
                pickupSyncInfo.Locked = ipb.Info.Locked;
                PickupSyncInfo networkInfo = pickupSyncInfo;
                InitiallySpawnedItems.Singleton.AddInitial(networkInfo.Serial);
                ipb.Info = networkInfo;
            }
        }

        protected override void SpawnObject(GameObject objectToSpawn)
        {
            if (objectToSpawn != null && objectToSpawn.TryGetComponent<ItemPickupBase>(out var component))
            {
                SpawnPickup(component);
            }
        }
    }
}
