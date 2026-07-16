using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Items
{
    public static class ItemTranslationReader
    {
        private class NameDescriptionPair
        {
            public string Name;
            public string Description;
        }

        private const char SplitChar = '~';
        private static NameDescriptionPair[] _cachedTranslations;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            TranslationReader.OnTranslationsRefreshed += ReloadTranslations;
        }

        private static void ReloadTranslations()
        {
            int count = InventoryItemLoader.AvailableItems.Count;
            _cachedTranslations = new NameDescriptionPair[255];

            int index = 0;
            while (TranslationReader.TryGet("Items", index, out string rawLine))
            {
                index++;
                if (string.IsNullOrEmpty(rawLine)) continue;

                string[] parts = rawLine.Split(SplitChar);
                if (parts.Length < 2) continue;

                if (int.TryParse(parts[0], out int itemId))
                {
                    EnsureCapacity(itemId);

                    _cachedTranslations[itemId] = new NameDescriptionPair
                    {
                        Name = parts[1],
                        Description = (parts.Length > 2) ? parts[2] : string.Empty
                    };
                }
            }
        }

        private static void EnsureCapacity(int cap)
        {
            if (_cachedTranslations == null)
            {
                _cachedTranslations = new NameDescriptionPair[cap + 1];
                return;
            }

            if (cap >= _cachedTranslations.Length)
            {
                Array.Resize(ref _cachedTranslations, cap + 1);
            }
        }

        private static NameDescriptionPair GetTranslation(ItemType it)
        {
            int index = (int)it;

            if (_cachedTranslations == null)
                ReloadTranslations();

            if (index >= 0 && index < _cachedTranslations.Length && _cachedTranslations[index] != null)
            {
                return _cachedTranslations[index];
            }

            NameDescriptionPair fallback = new NameDescriptionPair
            {
                Name = it.ToString(),
                Description = string.Empty
            };

            EnsureCapacity(index);
            _cachedTranslations[index] = fallback;

            return fallback;
        }

        public static string GetName(this ItemType it)
        {
            return GetTranslation(it).Name;
        }

        public static string GetDescription(this ItemType it)
        {
            return GetTranslation(it).Description;
        }
    }
}