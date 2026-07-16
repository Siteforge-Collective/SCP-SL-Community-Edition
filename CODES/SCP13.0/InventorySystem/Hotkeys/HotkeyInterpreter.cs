using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using InventorySystem.Items;
using InventorySystem.Hotkeys.Customization;
using UnityEngine;

namespace InventorySystem.Hotkeys
{
    public static class HotkeyInterpreter
    {
        private class CachedResult
        {
            public ItemBase Item;
            public bool IsValid;
        }

        private static readonly List<ItemBase> UserItemsByOrder = new List<ItemBase>();
        private static readonly List<ItemBase> Matches = new List<ItemBase>();
        private static readonly List<int> OrderQueue = new List<int>();
        private static readonly List<KeyCode> KeyDuplicateChecker = new List<KeyCode>();

        private static CachedResult[] _cachedResults;
        private static ReferenceHub _lastLocalPlayer;
        private static bool _allowChecks;
        private static bool _cacheSet;

        public static event Action OnHotkeysRevalidated;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _cachedResults = new CachedResult[64];
            for (int i = 0; i < _cachedResults.Length; i++)
            {
                _cachedResults[i] = new CachedResult();
            }
            _allowChecks = true;
        }

        private static void Update()
        {
            if (ReferenceHub.LocalHub != _lastLocalPlayer)
            {
                _lastLocalPlayer = ReferenceHub.LocalHub;
                _cacheSet = false;
            }

            if (!_cacheSet && _allowChecks)
            {
                for (int i = 0; i < _cachedResults.Length; i++)
                {
                    _cachedResults[i].IsValid = false;
                }
                _cacheSet = true;

                var onRevalidated = OnHotkeysRevalidated;
                if (onRevalidated != null)
                    onRevalidated.Invoke();
            }
        }

        private static bool CheckDuplicate(ItemBase ib, ItemBase other)
        {
            return ib != null && other != null && ib.ItemSerial == other.ItemSerial;
        }

        private static bool TryGetOverride(int hotkeyIndex, out ItemBase overrideItem)
        {
            overrideItem = null;
            return false;
        }

        private static void UpdateCacheForItemQueue(int hotkeyIndex)
        {
            if (hotkeyIndex < 0 || hotkeyIndex >= HotkeyStorage.Hotkeys.Count) return;

            SavedHotkey settings = HotkeyStorage.Hotkeys[hotkeyIndex];
            _cachedResults[hotkeyIndex].Item = null;

            if (_lastLocalPlayer == null || _lastLocalPlayer.inventory == null) return;

            foreach (var element in settings.Elements)
            {
                ItemType specific = element.SpecificItem;

                foreach (var item in _lastLocalPlayer.inventory.UserInventory.Items.Values)
                {
                    if (item.ItemTypeId == specific)
                    {
                        _cachedResults[hotkeyIndex].Item = item;
                        goto EndLoop;
                    }
                }
            }

        EndLoop:
            _cachedResults[hotkeyIndex].IsValid = true;
        }

        private static void UpdateCacheForInventoryOrder(int hotkeyIndex)
        {
            if (hotkeyIndex < 0 || hotkeyIndex >= HotkeyStorage.Hotkeys.Count) return;

            SavedHotkey settings = HotkeyStorage.Hotkeys[hotkeyIndex];
            _cachedResults[hotkeyIndex].Item = null;

            if (_lastLocalPlayer == null || _lastLocalPlayer.inventory == null) return;

            UserItemsByOrder.Clear();
            UserItemsByOrder.AddRange(_lastLocalPlayer.inventory.UserInventory.Items.Values);
            UserItemsByOrder.Sort((a, b) => a.ItemSerial.CompareTo(b.ItemSerial));

            foreach (var item in UserItemsByOrder)
            {
                foreach (var element in settings.Elements)
                {
                    if (item.ItemTypeId == element.SpecificItem)
                    {
                        _cachedResults[hotkeyIndex].Item = item;
                        goto EndLoop;
                    }
                }
            }

        EndLoop:
            _cachedResults[hotkeyIndex].IsValid = true;
        }

        private static void UpdateCacheForHotkey(int hotkeyIndex)
        {
            if (hotkeyIndex < 0 || hotkeyIndex >= _cachedResults.Length) return;

            if (TryGetOverride(hotkeyIndex, out ItemBase overrideItem))
            {
                _cachedResults[hotkeyIndex].Item = overrideItem;
                _cachedResults[hotkeyIndex].IsValid = true;
                return;
            }

            if (hotkeyIndex < HotkeyStorage.Hotkeys.Count)
            {
                var sortMode = HotkeyStorage.Hotkeys[hotkeyIndex].SortMode;
                if (sortMode == HotkeySortMode.InventoryOrder)
                    UpdateCacheForInventoryOrder(hotkeyIndex);
                else
                    UpdateCacheForItemQueue(hotkeyIndex);
            }
        }

        public static bool TryGetItem(int hotkeyIndex, out ItemBase ib)
        {
            ib = null;
            if (_lastLocalPlayer == null || hotkeyIndex < 0 || hotkeyIndex >= _cachedResults.Length)
                return false;

            if (!_cachedResults[hotkeyIndex].IsValid)
            {
                UpdateCacheForHotkey(hotkeyIndex);
            }

            ib = _cachedResults[hotkeyIndex].Item;
            return ib != null;
        }
    }
}