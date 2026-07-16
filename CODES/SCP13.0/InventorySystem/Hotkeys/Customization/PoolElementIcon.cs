using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Items;

namespace InventorySystem.Hotkeys.Customization
{
    public class PoolElementIcon : MonoBehaviour
    {
        private PoolElementData? _data;

        [SerializeField] private GameObject[] _roots;
        [SerializeField] private RawImage _specificItemIcon;
        [SerializeField] private List<RectTransform> _groupRows;
        [SerializeField] private List<RawImage> _groupIcons;
        [SerializeField] private TMP_Text _orderText;
        [SerializeField] private GameObject _addElement;

        public RectTransform RectTransform => transform as RectTransform;

        public RawImage Icon => _specificItemIcon;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public PoolElementData? Data
        {
            get => _data;
            set
            {
                _data = value;
                UpdateAnyIcon();
            }
        }

        public void MarkAsAddButton()
        {
            foreach (var root in _roots)
            {
                if (root != null)
                    root.SetActive(false);
            }

            if (_addElement != null)
                _addElement.SetActive(true);
        }

        private void UpdateAnyIcon()
        {
            foreach (var root in _roots)
            {
                if (root != null)
                    root.SetActive(false);
            }

            if (_addElement != null)
                _addElement.SetActive(false);

            if (!_data.HasValue) return;

            switch (_data.Value.Type)
            {
                case PoolElementData.ElementType.SpecificItem:
                    SetupSpecificIcon(_data.Value.SpecificItem);
                    break;
                case PoolElementData.ElementType.Group:
                    SetupGroupIcon(_data.Value.Group);
                    break;
                case PoolElementData.ElementType.Order:
                    SetupOrderIcon(_data.Value.Order);
                    break;
            }
        }

        private void SetupSpecificIcon(ItemType it)
        {
            if (_roots.Length > 0 && _roots[0] != null)
                _roots[0].SetActive(true);

            if (_specificItemIcon != null && InventoryItemLoader.AvailableItems.TryGetValue(it, out ItemBase itemBase))
            {
                _specificItemIcon.texture = itemBase.Icon;
                _specificItemIcon.enabled = true;
            }
            else if (_specificItemIcon != null)
            {
                _specificItemIcon.enabled = false;
            }
        }

        private void SetupGroupIcon(HotkeyItemGroup group)
        {
            if (_roots.Length > 1 && _roots[1] != null)
                _roots[1].SetActive(true);

            ItemType[] items = HotkeyItemGroupDefinitions.GetItems(group);

            DeactivateAll(_groupRows);
            DeactivateAll(_groupIcons);

            for (int i = 0; i < items.Length && i < 8; i++)
            {
                RectTransform row = GetOrDuplicate(_groupRows, i, true);
                RawImage icon = GetOrDuplicate(_groupIcons, i, false);

                if (row != null) row.gameObject.SetActive(true);
                if (icon != null && InventoryItemLoader.AvailableItems.TryGetValue(items[i], out ItemBase itemBase))
                {
                    icon.texture = itemBase.Icon;
                    icon.enabled = true;
                }
                else if (icon != null)
                {
                    icon.enabled = false;
                }
            }
        }

        private void SetupOrderIcon(int order)
        {
            if (_roots.Length > 2 && _roots[2] != null)
                _roots[2].SetActive(true);

            if (_orderText != null)
            {
                _orderText.text = (order + 1).ToString();
            }
        }

        private T GetOrDuplicate<T>(List<T> targetList, int index, bool includeChildren = false) where T : Component
        {
            while (targetList.Count <= index)
            {
                T duplicate = null;

                if (targetList.Count == 0)
                {
                    targetList.Add(null);
                    continue;
                }

                T original = targetList[targetList.Count - 1];
                if (original == null)
                {
                    targetList.Add(null);
                    continue;
                }

                GameObject originalGo = original.gameObject;
                GameObject duplicateGo = Instantiate(originalGo, originalGo.transform.parent);

                if (includeChildren)
                {
                }

                duplicate = duplicateGo.GetComponent<T>();
                if (duplicate == null)
                {
                    duplicate = duplicateGo.AddComponent<T>();
                }

                targetList.Add(duplicate);
            }

            return targetList[index];
        }

        private void DeactivateAll<T>(List<T> comp) where T : Component
        {
            foreach (var item in comp)
            {
                if (item != null && item.gameObject != null)
                    item.gameObject.SetActive(false);
            }
        }
    }
}