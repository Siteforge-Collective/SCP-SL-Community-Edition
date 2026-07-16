using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Usables;
using NorthwoodLib.Pools;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
    public static class HotkeyUtils
    {
        private static readonly Dictionary<int, ItemType[]> ItemOrder = new Dictionary<int, ItemType[]>();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            HotkeyStorage.OnHotkeysSaved += () => ItemOrder.Clear();
        }

        public static ItemType[] GetItemsByQueue(int hotkeyIndex)
        {
            if (ItemOrder.TryGetValue(hotkeyIndex, out ItemType[] cached))
                return cached;

            List<SavedHotkey> hotkeys = HotkeyStorage.Hotkeys;
            if (hotkeyIndex < 0 || hotkeyIndex >= hotkeys.Count)
                return Array.Empty<ItemType>();

            SavedHotkey hotkey = hotkeys[hotkeyIndex];

            List<ItemType> result = ListPool<ItemType>.Shared.Rent();
            GetItemsByQueue(hotkey.Elements, result);

            ItemType[] array = result.ToArray();
            ListPool<ItemType>.Shared.Return(result);

            ItemOrder[hotkeyIndex] = array;
            return array;
        }

        public static ItemType[] GetItemsByQueue(this IEnumerable<PoolElementData> elements)
        {
            List<ItemType> list = ListPool<ItemType>.Shared.Rent();
            GetItemsByQueue(elements, list);

            ItemType[] result = list.ToArray();
            ListPool<ItemType>.Shared.Return(list);
            return result;
        }

        public static void GetItemsByQueue(this IEnumerable<PoolElementData> elements, List<ItemType> targetList)
        {
            if (elements == null || targetList == null) return;

            targetList.Clear();

            foreach (PoolElementData element in elements)
            {
                switch (element.Type)
                {
                    case PoolElementData.ElementType.SpecificItem:
                        AddToList(element.SpecificItem, false);
                        break;
                    case PoolElementData.ElementType.Group:
                        ItemType[] groupItems = HotkeyItemGroupDefinitions.GetItems(element.Group);
                        foreach (ItemType item in groupItems)
                        {
                            AddToList(item, false);
                        }
                        break;
                }
            }

            void AddToList(ItemType item, bool replaceIfExists)
            {
                int existingIndex = targetList.IndexOf(item);

                if (existingIndex != -1)
                {
                    if (replaceIfExists)
                    {
                        targetList[existingIndex] = item;
                    }
                }
                else
                {
                    targetList.Add(item);
                }
            }
        }

        public static int CountMatches(this List<PoolElementData> elements, ItemType[] filters)
        {
            if (elements == null || elements.Count == 0 || filters == null || filters.Length == 0)
                return 0;

            int count = 0;

            foreach (PoolElementData element in elements)
            {
                switch (element.Type)
                {
                    case PoolElementData.ElementType.SpecificItem:
                        if (Array.IndexOf(filters, element.SpecificItem) != -1)
                            count++;
                        break;
                    case PoolElementData.ElementType.Group:
                        ItemType[] groupItems = HotkeyItemGroupDefinitions.GetItems(element.Group);
                        count += groupItems.Count(it => Array.IndexOf(filters, it) != -1);
                        break;
                }
            }

            return count;
        }

        public static bool IsHotkeyable(this ItemBase item)
        {
            if (item == null) return false;

            return item.ItemTypeId != ItemType.Radio &&
                   item.Category != ItemCategory.SCPItem ||
                   item is UsableItem usable && usable.AllowHolster;
        }
    }
}