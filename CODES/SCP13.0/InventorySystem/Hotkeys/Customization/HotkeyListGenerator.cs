using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Hotkeys.Customization
{
    public class HotkeyListGenerator : MonoBehaviour
    {
        [SerializeField] private HotkeyCustomizer _customizerTemplate;
        [SerializeField] private Transform _prevSibling;

        private bool _anyModified;

        private readonly List<HotkeyCustomizer> _customizerInstances = new List<HotkeyCustomizer>();

        public static event Action<int> OnModified;

        private void Awake()
        {
            _customizerInstances.Clear();

            foreach (var savedHotkey in HotkeyStorage.Hotkeys)
            {
                SpawnInstance(savedHotkey);
            }

            _anyModified = false;
        }

        private void SpawnInstance(SavedHotkey sv)
        {
            if (_customizerTemplate == null) return;

            HotkeyCustomizer instance = Instantiate(_customizerTemplate, _prevSibling.parent);

            if (_prevSibling != null)
            {
                instance.transform.SetSiblingIndex(_prevSibling.GetSiblingIndex() + 1);
            }
            else
            {
                instance.transform.SetAsLastSibling();
            }

            instance.Load(sv, this);
            _customizerInstances.Add(instance);
        }

        private void OnDisable()
        {
            SaveAllChanges();
        }

        public void RemoveInstance(HotkeyCustomizer instance)
        {
            if (instance == null || !_customizerInstances.Contains(instance)) return;

            _customizerInstances.Remove(instance);
            Destroy(instance.gameObject);

            _anyModified = true;
            SaveAllChanges();
        }

        public void AddNew()
        {
            SavedHotkey newHotkey = new SavedHotkey
            {
                CustomName = null,
                TranslationId = 0,
                AssignedKey = KeyCode.None,
                Apperance = HotkeyApperance.NormalIcon,
                SortMode = HotkeySortMode.ItemPoolOrder,
                OverrideOption = HotkeyOverrideOption.NoOverride,
                Elements = Array.Empty<PoolElementData>()
            };

            SpawnInstance(newHotkey);

            _anyModified = true;
        }

        public void ResetToDefaults()
        {
            foreach (var customizer in _customizerInstances.ToArray())
            {
                if (customizer != null)
                {
                    Destroy(customizer.gameObject);
                }
            }

            _customizerInstances.Clear();

            PlayerPrefsSl.DeleteKey("CustomHotkeys", PlayerPrefsSl.DataType.Int);
            HotkeyStorage.LoadDefaults();

            foreach (var defaultHotkey in HotkeyStorage.Hotkeys)
            {
                SpawnInstance(defaultHotkey);
            }

            _anyModified = false;
            OnModified?.Invoke(0);
        }

        private void SaveAllChanges()
        {
            if (!_anyModified) return;

            List<SavedHotkey> currentHotkeys = new List<SavedHotkey>();
            foreach (var customizer in _customizerInstances)
            {
                if (customizer != null)
                {
                    currentHotkeys.Add(customizer.ToSavedHotkey());
                }
            }

            HotkeyStorage.Hotkeys = currentHotkeys;
            HotkeyStorage.SaveToPlayerPrefs();

            _anyModified = false;

            OnModified?.Invoke(0);
        }

        public void NotifyModified()
        {
            _anyModified = true;
        }
    }
}