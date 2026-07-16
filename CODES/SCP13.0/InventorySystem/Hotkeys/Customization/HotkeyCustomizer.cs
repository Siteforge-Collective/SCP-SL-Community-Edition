// Файл: HotkeyCustomizer.cs (полная исправленная и завершённая версия)

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserSettings.GUIElements;

namespace InventorySystem.Hotkeys.Customization
{
    public class HotkeyCustomizer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleField;
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private KeycodeField _keycodeField;
        [SerializeField] private TMP_Dropdown _apperance;
        [SerializeField] private TMP_Dropdown _sortMode;
        [SerializeField] private HotkeyPool _itemPool;
        [SerializeField] private HotkeyPool _orderPool;
        [SerializeField] private TMP_Dropdown _overrideOption;
        [SerializeField] private HotkeysTranslation[] _sortModeTranslations;

        private byte _lastTranslationId;
        private bool _usesCustomName;
        private HotkeyListGenerator _spawnedBy;

        private readonly List<HotkeyOverrideBase> _availableOverrides = new List<HotkeyOverrideBase>();
        private readonly Dictionary<TMP_Dropdown, bool> _prevDropdownStates = new Dictionary<TMP_Dropdown, bool>();

        public bool IsModified { get; private set; }

        private PoolElementData[] CombinedPoolData
        {
            get
            {
                var combined = new List<PoolElementData>(_itemPool.Elements);
                combined.AddRange(_orderPool.Elements);
                return combined.ToArray();
            }
            set
            {
                _itemPool.Elements.Clear();
                _orderPool.Elements.Clear();

                if (value != null)
                {
                    foreach (var data in value)
                    {
                        if (data.Type == PoolElementData.ElementType.SpecificItem || data.Type == PoolElementData.ElementType.Group)
                            _itemPool.Elements.Add(data);
                        else if (data.Type == PoolElementData.ElementType.Order)
                            _orderPool.Elements.Add(data);
                    }
                }

                _itemPool.NotifyModified();
                _orderPool.NotifyModified();
            }
        }

        private void SetTitle(string str)
        {
            if (_titleField == null) return;

            string format = Translations.Get<HotkeysTranslation>(HotkeysTranslation.TitlePrefix); 
            _titleField.text = string.Format(format, string.IsNullOrEmpty(str) ? "Hotkey" : str);
            _titleField.gameObject.SetActive(true);
        }

        private void Awake()
        {
            _keycodeField.OnKeySet += _ => IsModified = true;

            _itemPool.OnModified += () => { IsModified = true; _spawnedBy?.NotifyModified(); };
            _orderPool.OnModified += () => { IsModified = true; _spawnedBy?.NotifyModified(); };

            _apperance.onValueChanged.AddListener(_ => IsModified = true);
            _sortMode.onValueChanged.AddListener(OnSortModeEdited);
            _overrideOption.onValueChanged.AddListener(_ => IsModified = true);

            _nameInputField.onValueChanged.AddListener(OnNameEdited);

            RebuildHintsForDropdown(_sortMode, list =>
            {
                for (int i = 0; i < list.Count && i < _sortModeTranslations.Length; i++)
                {
                    list[i].SetTranslation(_sortModeTranslations[i]);
                }
            });

            _availableOverrides.Add(new HotkeyBestMedicalOverride());
        }

        private void RebuildHintsForDropdown(TMP_Dropdown dropdown, Action<List<UserSettingsEntryDescription>> action)
        {
            if (dropdown == null || action == null) return;

            var descriptions = new List<UserSettingsEntryDescription>();
            foreach (var option in dropdown.options)
            {
                descriptions.Add(new UserSettingsEntryDescription { Text = option.text });
            }
            action(descriptions);
        }

        private void OnNameEdited(string str)
        {
            IsModified = true;
            _usesCustomName = !string.IsNullOrEmpty(str);
            SetTitle(str);
            _spawnedBy?.NotifyModified();
        }

        private void OnSortModeEdited(int mode)
        {
            IsModified = true;
            _spawnedBy?.NotifyModified();

            GameObject orderPoolParent = _orderPool.transform.parent.gameObject;
            orderPoolParent.SetActive(mode == 1);

            if (mode == 0)
            {
                if (transform.parent != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent as RectTransform);
                }

                var foldout = GetComponent<UserSettingsFoldoutGroup>();
                foldout?.RefreshSize();
            }
            else if (mode == 1)
            {
                if (_orderPool.Elements == null || _orderPool.Elements.Count == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        _orderPool.Elements.Add(new PoolElementData(i));
                    }

                    _orderPool.NotifyModified();
                }
            }
        }

        public void RemoveSelf()
        {
            _spawnedBy?.RemoveInstance(this);
            Destroy(gameObject);
        }

        public SavedHotkey ToSavedHotkey()
        {
            return new SavedHotkey
            {
                CustomName = _usesCustomName ? _nameInputField.text : null,
                TranslationId = _lastTranslationId,
                AssignedKey = _keycodeField.CurDisplayedKey,
                Apperance = (HotkeyApperance)_apperance.value,
                SortMode = (HotkeySortMode)_sortMode.value,
                OverrideOption = (HotkeyOverrideOption)_overrideOption.value,
                Elements = CombinedPoolData
            };
        }

        public void Load(SavedHotkey data, HotkeyListGenerator spawnedBy)
        {
            _spawnedBy = spawnedBy;

            _usesCustomName = !string.IsNullOrWhiteSpace(data.CustomName);
            _nameInputField.text = data.CustomName ?? string.Empty;
            _lastTranslationId = data.TranslationId;

            SetTitle(data.DisplayName);

            _keycodeField.SetDisplayedKey(data.AssignedKey);
            _apperance.value = (int)data.Apperance;
            _sortMode.value = (int)data.SortMode;
            _overrideOption.value = (int)data.OverrideOption;

            CombinedPoolData = data.Elements;
            OnSortModeEdited((int)data.SortMode);

            IsModified = false;
        }
    }
}