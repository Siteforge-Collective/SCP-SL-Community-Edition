using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
    public static class HotkeyStorage
    {
        private static List<SavedHotkey> _loadedHotkeys = new List<SavedHotkey>();

        private const string PrefsKey = "CustomHotkeyDefinitions";

        private static readonly List<SavedHotkey> DefaultHotkeys;

        public static event Action OnHotkeysSaved;

        public static List<SavedHotkey> Hotkeys
        {
            get
            {
                if (_loadedHotkeys == null || _loadedHotkeys.Count == 0)
                {
                    LoadFromPlayerPrefs();
                }
                return _loadedHotkeys;
            }
            set => _loadedHotkeys = value ?? new List<SavedHotkey>();
        }

        static HotkeyStorage()
        {
            DefaultHotkeys = new List<SavedHotkey>();
            DefaultHotkeys.Add(new SavedHotkey
            {
                CustomName = null,
                TranslationId = 0,
                AssignedKey = KeyCode.Alpha1, 
                Apperance = HotkeyApperance.NormalIcon,
                SortMode = HotkeySortMode.ItemPoolOrder,
                OverrideOption = HotkeyOverrideOption.NoOverride,
                Elements = new PoolElementData[]
                {
                    new PoolElementData(ItemType.GunE11SR),
                }
            });

            DefaultHotkeys.Add(new SavedHotkey
            {
                CustomName = null,
                TranslationId = 0,
                AssignedKey = KeyCode.Alpha2,
                Apperance = HotkeyApperance.NormalIcon,
                SortMode = HotkeySortMode.ItemPoolOrder,
                OverrideOption = HotkeyOverrideOption.BestMedical,
                Elements = new PoolElementData[]
                {
                    new PoolElementData(ItemType.Medkit),
                    new PoolElementData(ItemType.Painkillers),
                    new PoolElementData(ItemType.Adrenaline),
                    new PoolElementData(ItemType.SCP500)
                }
            });
        }

        public static void LoadDefaults()
        {
            Hotkeys = new List<SavedHotkey>(DefaultHotkeys);
        }

        public static void SaveToPlayerPrefs()
        {
            string json = JsonUtility.ToJson(new SavedHotkeyWrapper { hotkeys = Hotkeys });
            PlayerPrefs.SetString(PrefsKey, json);
            PlayerPrefs.Save();

            OnHotkeysSaved?.Invoke();
        }

        private static void LoadFromPlayerPrefs()
        {
            if (!PlayerPrefsSl.HasKey(PrefsKey, PlayerPrefsSl.DataType.String))
            {
                LoadDefaults();
                return;
            }

            string json = PlayerPrefsSl.Get(PrefsKey, string.Empty);
            if (string.IsNullOrEmpty(json))
            {
                LoadDefaults();
                return;
            }

            try
            {
                var wrapper = JsonUtility.FromJson<SavedHotkeyWrapper>(json);
                Hotkeys = wrapper.hotkeys ?? new List<SavedHotkey>();
            }
            catch
            {
                LoadDefaults();
            }
        }

        public static void Save()
        {
            SaveToPlayerPrefs();
        }

        public static void DeleteAll()
        {
            PlayerPrefs.DeleteKey(PrefsKey);
            Hotkeys.Clear();
            LoadDefaults();
            OnHotkeysSaved?.Invoke();
        }

        [Serializable]
        private class SavedHotkeyWrapper
        {
            public List<SavedHotkey> hotkeys;
        }
    }
}