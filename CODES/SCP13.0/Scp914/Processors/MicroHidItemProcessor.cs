using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.MicroHID;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public class MicroHidItemProcessor : Scp914ItemProcessor
    {
        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            if (!hub.inventory.UserInventory.Items.TryGetValue(serial, out var value))
            {
                return null;
            }
            ItemType output = GetOutput(setting);
            if (output == ItemType.MicroHID)
            {
                if (value is MicroHIDItem microHIDItem)
                {
                    microHIDItem.Recharge();
                    return microHIDItem;
                }
                return null;
            }
            hub.inventory.ServerRemoveItem(serial, null);
            ItemBase itemBase = hub.inventory.ServerAddItem(output, 0);
            if (itemBase is Firearm firearm)
            {
                firearm.Status = GetStatusForFirearm(output);
            }
            return itemBase;
        }

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition)
        {
            ItemType output = GetOutput(setting);
            if (output == ItemType.MicroHID)
            {
                if (ipb is MicroHIDPickup microHIDPickup)
                {
                    microHIDPickup.NetworkEnergy = 1f;
                }
                ipb.transform.position = newPosition;
                return ipb;
            }
            ipb.DestroySelf();
            if (!InventoryItemLoader.AvailableItems.TryGetValue(output, out var value))
            {
                return null;
            }
            PickupSyncInfo psi = new PickupSyncInfo
            {
                ItemId = output,
                Serial = ItemSerialGenerator.GenerateNext(),
                WeightKg = value.Weight
            };
            ItemPickupBase itemPickupBase = InventoryExtensions.ServerCreatePickup(value, psi, newPosition);
            if (itemPickupBase is FirearmPickup firearmPickup)
            {
                firearmPickup.Status = GetStatusForFirearm(value.ItemTypeId);
            }
            return itemPickupBase;
        }

        private ItemType GetOutput(Scp914KnobSetting setting)
        {
            switch (setting)
            {
                case Scp914KnobSetting.Rough:
                    return ItemType.GunE11SR;
                case Scp914KnobSetting.OneToOne:
                case Scp914KnobSetting.Fine:
                case Scp914KnobSetting.VeryFine:
                    return ItemType.MicroHID;
                default:
                    if (!(Random.value >= 0.5f))
                    {
                        return ItemType.ParticleDisruptor;
                    }
                    return ItemType.Jailbird;
            }
        }
    }
}
