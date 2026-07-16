namespace MapGeneration.Distributors
{
	public class ItemDistributor : global::MapGeneration.Distributors.SpawnablesDistributorBase
	{
		protected override void PlaceSpawnables()
		{
			while (global::MapGeneration.Distributors.ItemSpawnpoint.RandomInstances.Remove(null))
			{
			}
			while (global::MapGeneration.Distributors.ItemSpawnpoint.AutospawnInstances.Remove(null))
			{
			}
			global::MapGeneration.Distributors.SpawnableItem[] spawnableItems = Settings.SpawnableItems;
			foreach (global::MapGeneration.Distributors.SpawnableItem item in spawnableItems)
			{
				PlaceItem(item);
			}
			foreach (global::MapGeneration.Distributors.ItemSpawnpoint autospawnInstance in global::MapGeneration.Distributors.ItemSpawnpoint.AutospawnInstances)
			{
				global::UnityEngine.Transform t = autospawnInstance.Occupy();
				CreatePickup(autospawnInstance.AutospawnItem, t, autospawnInstance.TriggerDoorName);
			}
		}

		private void PlaceItem(global::MapGeneration.Distributors.SpawnableItem item)
		{
			float num = global::UnityEngine.Random.Range(item.MinimalAmount, item.MaxAmount);
			global::System.Collections.Generic.List<global::MapGeneration.Distributors.ItemSpawnpoint> list = global::NorthwoodLib.Pools.ListPool<global::MapGeneration.Distributors.ItemSpawnpoint>.Shared.Rent();
			foreach (global::MapGeneration.Distributors.ItemSpawnpoint randomInstance in global::MapGeneration.Distributors.ItemSpawnpoint.RandomInstances)
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
				ItemType itemType = item.PossibleSpawns[global::UnityEngine.Random.Range(0, item.PossibleSpawns.Length)];
				if (itemType == ItemType.None)
				{
					continue;
				}
				int index = global::UnityEngine.Random.Range(0, list.Count);
				global::UnityEngine.Transform transform = list[index].Occupy();
				if (global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.ItemSpawned, itemType, transform.transform.position))
				{
					CreatePickup(itemType, transform, list[index].TriggerDoorName);
					if (!list[index].CanSpawn(itemType))
					{
						list.RemoveAt(index);
					}
				}
			}
			global::NorthwoodLib.Pools.ListPool<global::MapGeneration.Distributors.ItemSpawnpoint>.Shared.Return(list);
		}

		private void CreatePickup(ItemType id, global::UnityEngine.Transform t, string triggerDoor)
		{
			if (global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(id, out var value))
			{
				global::InventorySystem.Items.Pickups.ItemPickupBase itemPickupBase = global::UnityEngine.Object.Instantiate(value.PickupDropModel, t.position, t.rotation);
				itemPickupBase.Info.ItemId = id;
				itemPickupBase.Info.Weight = value.Weight;
				itemPickupBase.transform.SetParent(t);
				(itemPickupBase as global::InventorySystem.Items.Pickups.IPickupDistributorTrigger)?.OnDistributed();
				if (string.IsNullOrEmpty(triggerDoor) || !global::Interactables.Interobjects.DoorUtils.DoorNametagExtension.NamedDoors.TryGetValue(triggerDoor, out var value2))
				{
					SpawnPickup(itemPickupBase);
				}
				else
				{
					RegisterUnspawnedObject(value2.TargetDoor, itemPickupBase.gameObject);
				}
			}
		}

		public static void SpawnPickup(global::InventorySystem.Items.Pickups.ItemPickupBase ipb)
		{
			if (!(ipb == null))
			{
				global::Mirror.NetworkServer.Spawn(ipb.gameObject);
				global::InventorySystem.Items.Pickups.PickupSyncInfo pickupSyncInfo = new global::InventorySystem.Items.Pickups.PickupSyncInfo(ipb.Info.ItemId, ipb.transform.position, ipb.transform.rotation, ipb.Info.Weight, 0);
				pickupSyncInfo.Locked = ipb.Info.Locked;
				global::InventorySystem.Items.Pickups.PickupSyncInfo pickupSyncInfo2 = pickupSyncInfo;
				global::MapGeneration.InitiallySpawnedItems.Singleton.AddInitial(pickupSyncInfo2.Serial);
				ipb.NetworkInfo = pickupSyncInfo2;
				ipb.InfoReceived(default(global::InventorySystem.Items.Pickups.PickupSyncInfo), pickupSyncInfo2);
			}
		}

		protected override void SpawnObject(global::UnityEngine.GameObject objectToSpawn)
		{
			if (objectToSpawn != null && objectToSpawn.TryGetComponent<global::InventorySystem.Items.Pickups.ItemPickupBase>(out var component))
			{
				SpawnPickup(component);
			}
		}
	}
}
