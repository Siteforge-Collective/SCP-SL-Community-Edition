using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public abstract class Scp914ItemProcessor : MonoBehaviour
    {
        public abstract ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial);

        public abstract ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPosition);

        protected virtual FirearmStatus GetStatusForFirearm(ItemType firearm)
        {
            if (!InventoryItemLoader.TryGetItem<Firearm>(firearm, out var result))
            {
                return default(FirearmStatus);
            }
            byte ammo;
            FirearmStatusFlags flags;
            uint attachments;
            if (result is ParticleDisruptor)
            {
                ammo = 5;
                flags = FirearmStatusFlags.MagazineInserted;
                attachments = result.ValidateAttachmentsCode(0u);
            }
            else
            {
                ammo = 0;
                flags = FirearmStatusFlags.None;
                attachments = AttachmentsUtils.GetRandomAttachmentsCode(firearm);
            }
            return new FirearmStatus(ammo, flags, attachments);
        }
    }
}
