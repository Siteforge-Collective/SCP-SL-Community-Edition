using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public class DisruptorItemProcessor : StandardItemProcessor
    {
        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            if (setting != Scp914KnobSetting.Fine)
            {
                return base.OnInventoryItemUpgraded(setting, hub, serial);
            }
            hub.inventory.ServerRemoveItem(serial, null);
            ItemBase itemBase = hub.inventory.ServerAddItem(ItemType.ParticleDisruptor, 0);
            ParticleDisruptor particleDisruptor = itemBase as ParticleDisruptor;
            particleDisruptor.Status = RefillAmmo(particleDisruptor.Status);
            return itemBase;
        }

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition)
        {
            ItemPickupBase itemPickupBase = base.OnPickupUpgraded(setting, ipb, newPosition);
            if (itemPickupBase is FirearmPickup firearmPickup && firearmPickup.Info.ItemId == ItemType.ParticleDisruptor)
            {
                firearmPickup.Status = RefillAmmo(firearmPickup.Status);
            }
            return itemPickupBase;
        }

        private FirearmStatus RefillAmmo(FirearmStatus fa)
        {
            return new FirearmStatus(5, FirearmStatusFlags.Cocked | FirearmStatusFlags.MagazineInserted | FirearmStatusFlags.Chambered, fa.Attachments);
        }
    }
}
