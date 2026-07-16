using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using UnityEngine;

namespace Scp914.Processors
{
    public class Scp2176ItemProcessor : StandardItemProcessor
    {
        private const float NumOfCoins = 12f;

        private const float NumOfFlashlights = 1f;

        private const float FlashlightChance = 0.2f;

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition)
        {
            if (ipb.Info.ItemId != ItemType.SCP2176)
            {
                return null;
            }
            switch (setting)
            {
                case Scp914KnobSetting.Rough:
                    if (ipb is Scp2176Projectile scp2176Projectile)
                    {
                        scp2176Projectile.ServerImmediatelyShatter();
                        return ipb;
                    }
                    break;
                case Scp914KnobSetting.OneToOne:
                    {
                        for (int j = 0; (float)j < 12f; j++)
                        {
                            SpawnItem(ItemType.Coin, newPosition, ipb.transform.rotation);
                        }
                        ipb.DestroySelf();
                        return null;
                    }
                case Scp914KnobSetting.VeryFine:
                    if (!(Random.value < 0.2f))
                    {
                        for (int i = 0; (float)i < 1f; i++)
                        {
                            SpawnItem(ItemType.Flashlight, newPosition, ipb.transform.rotation);
                        }
                        ipb.DestroySelf();
                        return null;
                    }
                    break;
            }
            return base.OnPickupUpgraded(setting, ipb, newPosition);
        }

        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            switch (setting)
            {
                case Scp914KnobSetting.OneToOne:
                    {
                        for (int j = 0; (float)j < 12f; j++)
                        {
                            SpawnItem(ItemType.Coin, hub.transform.position, Quaternion.identity);
                        }
                        break;
                    }
                case Scp914KnobSetting.VeryFine:
                    if (!(Random.value < 0.2f))
                    {
                        for (int i = 0; (float)i < 1f; i++)
                        {
                            SpawnItem(ItemType.Flashlight, hub.transform.position, Quaternion.identity);
                        }
                        hub.inventory.ServerRemoveItem(serial, null);
                        return null;
                    }
                    break;
            }
            return base.OnInventoryItemUpgraded(setting, hub, serial);
        }

        private void SpawnItem(ItemType itemType, Vector3 position, Quaternion rotation)
        {
            if (InventoryItemLoader.AvailableItems.TryGetValue(itemType, out var value))
            {
                PickupSyncInfo psi = new PickupSyncInfo
                {
                    ItemId = itemType,
                    Serial = ItemSerialGenerator.GenerateNext(),
                    WeightKg = value.Weight
                };
                InventoryExtensions.ServerCreatePickup(value, psi, position, rotation);
            }
        }
    }
}
