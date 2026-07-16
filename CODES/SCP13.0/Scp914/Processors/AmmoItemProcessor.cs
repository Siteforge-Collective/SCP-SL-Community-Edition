using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms.Ammo;
using InventorySystem.Items.Pickups;
using UnityEngine;

namespace Scp914.Processors
{
    public class AmmoItemProcessor : Scp914ItemProcessor
    {
        [SerializeField]
        private ItemType _previousAmmo;

        [SerializeField]
        private ItemType _oneToOne;

        [SerializeField]
        private ItemType _nextAmmo;

        public override ItemBase OnInventoryItemUpgraded(Scp914KnobSetting setting, ReferenceHub hub, ushort serial)
        {
            return null;
        }

        public override ItemPickupBase OnPickupUpgraded(Scp914KnobSetting setting, ItemPickupBase ipb, Vector3 newPos)
        {
            ItemType itemType;
            switch (setting)
            {
                case Scp914KnobSetting.Rough:
                case Scp914KnobSetting.Coarse:
                    itemType = _previousAmmo;
                    break;
                case Scp914KnobSetting.OneToOne:
                    itemType = _oneToOne;
                    break;
                case Scp914KnobSetting.Fine:
                case Scp914KnobSetting.VeryFine:
                    itemType = _nextAmmo;
                    break;
                default:
                    return ipb;
            }
            ipb.transform.position = newPos;
            if (!(ipb is AmmoPickup ammoPickup))
            {
                return ipb;
            }
            ExchangeAmmo(ammoPickup.Info.ItemId, itemType, ammoPickup.SavedAmmo, out var exchangedAmmo, out var change);
            if (change == 0)
            {
                ammoPickup.DestroySelf();
            }
            else
            {
                ammoPickup.SavedAmmo = (ushort)change;
            }
            return CreateAmmoPickup(itemType, exchangedAmmo, newPos);
        }

        public static ItemPickupBase CreateAmmoPickup(ItemType type, int bullets, Vector3 pos)
        {
            if (bullets <= 0 || !InventoryItemLoader.AvailableItems.TryGetValue(type, out var value))
            {
                return null;
            }
            PickupSyncInfo psi = new PickupSyncInfo
            {
                ItemId = type,
                Serial = ItemSerialGenerator.GenerateNext(),
                WeightKg = value.Weight
            };
            if (InventoryExtensions.ServerCreatePickup(value, psi, pos) is AmmoPickup ammoPickup)
            {
                ammoPickup.SavedAmmo = (ushort)bullets;
                return ammoPickup;
            }
            return null;
        }

        public static void ExchangeAmmo(ItemType ammoTypeToExchange, ItemType targetAmmoType, int amount, out int exchangedAmmo, out int change)
        {
            if (!TryGetAmmoItem(ammoTypeToExchange, out var ammoItem) || !InventoryItemLoader.AvailableItems.TryGetValue(targetAmmoType, out var value) || !(value is AmmoItem ammoItem2))
            {
                exchangedAmmo = 0;
                change = 0;
                return;
            }
            int unitPrice = ammoItem.UnitPrice;
            int unitPrice2 = ammoItem2.UnitPrice;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            for (int i = 0; i < amount; i++)
            {
                num3 += unitPrice;
                num++;
                if (num3 % unitPrice2 == 0)
                {
                    num2 += num3 / unitPrice2;
                    num = 0;
                    num3 = 0;
                }
            }
            exchangedAmmo = num2;
            change = num;
        }

        private static bool TryGetAmmoItem(ItemType type, out AmmoItem ammoItem)
        {
            if (InventoryItemLoader.AvailableItems.TryGetValue(type, out var value) && value is AmmoItem ammoItem2)
            {
                ammoItem = ammoItem2;
                return true;
            }
            ammoItem = null;
            return false;
        }
    }
}
