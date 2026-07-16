using System.Collections.Generic;
using InventorySystem.Configs;
using NorthwoodLib.Pools;
using UnityEngine;

namespace InventorySystem.Items.Armor
{
    public static class BodyArmorUtils
    {
        public static bool TryGetBodyArmor(this Inventory inv, out BodyArmor bodyArmor)
        {
            return inv.TryGetBodyArmorAndItsSerial(out bodyArmor, out ushort serial);
        }

        public static bool TryGetBodyArmorAndItsSerial(this Inventory inv, out BodyArmor bodyArmor, out ushort serial)
        {
            foreach (KeyValuePair<ushort, ItemBase> item in inv.UserInventory.Items)
            {
                if (item.Value is BodyArmor bodyArmor2)
                {
                    serial = item.Key;
                    bodyArmor = bodyArmor2;
                    return true;
                }
            }
            serial = 0;
            bodyArmor = null;
            return false;
        }

        public static float ProcessDamage(int efficacy, float baseDamage, int bulletPenetrationPercent)
        {
            float num = (float)efficacy / 100f;
            float num2 = (float)bulletPenetrationPercent / 100f;
            return baseDamage * (1f - num * (1f - num2));
        }

        public static void RemoveEverythingExceedingLimits(Inventory inv, BodyArmor armor, bool removeItems = true, bool removeAmmo = true)
        {
            HashSet<ushort> hashSet = HashSetPool<ushort>.Shared.Rent();
            Dictionary<ItemCategory, int> dictionary = new Dictionary<ItemCategory, int>();
            Dictionary<ItemType, ushort> dictionary2 = new Dictionary<ItemType, ushort>();
            foreach (KeyValuePair<ushort, ItemBase> item in inv.UserInventory.Items)
            {
                if (item.Value.Category != ItemCategory.Armor)
                {
                    int num = Mathf.Abs(InventoryLimits.GetCategoryLimit(armor, item.Value.Category));
                    int value = ((!dictionary.TryGetValue(item.Value.Category, out value)) ? 1 : (value + 1));
                    if (value > num)
                    {
                        hashSet.Add(item.Key);
                    }
                    dictionary[item.Value.Category] = value;
                }
            }
            foreach (KeyValuePair<ItemType, ushort> item2 in inv.UserInventory.ReserveAmmo)
            {
                ushort ammoLimit = InventoryLimits.GetAmmoLimit(armor, item2.Key);
                if (item2.Value > ammoLimit)
                {
                    dictionary2.Add(item2.Key, (ushort)(item2.Value - ammoLimit));
                }
            }
            if (removeItems)
            {
                foreach (ushort item3 in hashSet)
                {
                    inv.ServerDropItem(item3);
                }
            }
            if (!removeAmmo)
            {
                return;
            }
            foreach (KeyValuePair<ItemType, ushort> item4 in dictionary2)
            {
                inv.ServerDropAmmo(item4.Key, item4.Value);
            }
        }
    }
}
