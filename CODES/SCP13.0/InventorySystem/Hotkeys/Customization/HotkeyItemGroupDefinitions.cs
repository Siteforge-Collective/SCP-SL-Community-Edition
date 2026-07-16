using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Keycards;
using InventorySystem.Items.ThrowableProjectiles;

namespace InventorySystem.Hotkeys.Customization
{
    public static class HotkeyItemGroupDefinitions
    {
        private static readonly Dictionary<HotkeyItemGroup, CachedValue<ItemType[]>> Groups = new Dictionary<HotkeyItemGroup, CachedValue<ItemType[]>>();

        private static readonly Dictionary<HotkeyItemGroup, HotkeysTranslation> GroupToTranslation = new Dictionary<HotkeyItemGroup, HotkeysTranslation>();

        private static readonly CachedValue<ItemType[]> EmptyFallback;

        static HotkeyItemGroupDefinitions()
        {
            Groups[HotkeyItemGroup.AnyScpItem] = new CachedValue<ItemType[]>(() => FromCategory(ItemCategory.Medical));
            Groups[HotkeyItemGroup.AnyKeycard] = new CachedValue<ItemType[]>(AnyKeycard);
            Groups[HotkeyItemGroup.AnyFirearm] = new CachedValue<ItemType[]>(AnyFirearm);
            Groups[HotkeyItemGroup.AnyThrowable] = new CachedValue<ItemType[]>(() => FromCategory(ItemCategory.Grenade));
        }

        public static ItemType[] GetItems(this HotkeyItemGroup group)
        {
            if (Groups.TryGetValue(group, out var cached))
            {
                return cached.Value;
            }
            return EmptyFallback.Value;
        }

        public static string GetTranslation(this HotkeyItemGroup group)
        {
            if (GroupToTranslation.TryGetValue(group, out var translation))
            {
                return Translations.Get<HotkeysTranslation>(translation);
            }
            return string.Empty;
        }

        private static ItemType[] AnyFirearm()
        {
            return FromCategory(ItemCategory.Firearm);
        }

        private static ItemType[] AnyKeycard()
        {
            return InventoryItemLoader.AvailableItems
                .Where(kvp => kvp.Value is KeycardItem)
                .Select(kvp => kvp.Key)
                .OrderByDescending(itemType =>
                {
                    var keycard = InventoryItemLoader.AvailableItems[itemType] as KeycardItem;
                    return keycard != null ? (int)keycard.Permissions : 0;
                })
                .ToArray();
        }

        private static ItemType[] FromCategory(ItemCategory cat)
        {
            return InventoryItemLoader.AvailableItems
                .Where(kvp => kvp.Value.Category == cat && HotkeyUtils.IsHotkeyable(kvp.Value))
                .Select(kvp => kvp.Key)
                .ToArray();
        }

        private static ItemType[] ItemsOfTypeOrdered<TSource, TKey>(Func<TSource, TKey> keySelector)
            where TSource : ItemBase
        {
            return InventoryItemLoader.AvailableItems
                .Where(kvp => kvp.Value is TSource && HotkeyUtils.IsHotkeyable(kvp.Value))
                .OrderBy(kvp => keySelector((TSource)kvp.Value))
                .Select(kvp => kvp.Key)
                .ToArray();
        }
    }
}