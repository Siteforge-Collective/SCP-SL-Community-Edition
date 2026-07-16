using System;
using InventorySystem.Items;
using InventorySystem.Items.Firearms.Ammo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.GUI
{
    [Serializable]
    public class AmmoElement : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RawImage _iconImg;
        [SerializeField] private TextMeshProUGUI _nameTxt;
        [SerializeField] private RectTransform _myTransform;
        [SerializeField] private Graphic[] _paintableParts;
        [SerializeField] private TextMeshProUGUI _amountIndicator;

        [Header("Buttons UI")]
        [SerializeField] private TextMeshProUGUI _lowText;
        [SerializeField] private TextMeshProUGUI _medText;
        [SerializeField] private TextMeshProUGUI _highText;

        [Header("Hover Settings")]
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;

        private int _lowAmount;
        private int _medAmount;
        private int _highAmount;
        private ItemBase _targetItem;

        public void Setup(ItemType type, Color classColor)
        {
            if (InventoryItemLoader.AvailableItems.TryGetValue(type, out _targetItem))
            {
                _iconImg.texture = _targetItem.Icon;
                _nameTxt.text = type.ToString(); 

                foreach (Graphic graphic in _paintableParts)
                {
                    graphic.color = classColor;
                }
            }
            else
            {
                throw new InvalidOperationException($"Item {type} is not defined. Cannot create an ammo element for it.");
            }
        }

        public void UpdateAmount(int amount)
        {
            if (amount <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            _amountIndicator.text = amount.ToString();

            if (_targetItem != null)
            {
                if (_targetItem is AmmoItem ammoItem)
                {
                    var pickup = ammoItem.PickupDropModel as AmmoPickup;
                    if (pickup != null)
                    {
                        _lowAmount = 15;
                        _medAmount = 30;
                        _highAmount = amount;
                    }
                }
                else
                {
                    _highAmount = amount;
                    _medAmount = Mathf.Max(1, amount / 2);
                    _lowAmount = 1;
                }
            }

            PrepButton(_lowText, _lowAmount);
            UpdateDropButtonUI(_medText, _medAmount);
            UpdateDropButtonUI(_highText, _highAmount);
        }

        private void UpdateDropButtonUI(TextMeshProUGUI textElement, int amount)
        {
            textElement.transform.parent.gameObject.SetActive(amount > 0);
            if (amount > 0)
            {
                textElement.text = $"{amount}x";
            }
        }

        public void PrepButton(TextMeshProUGUI t, int amount)
        {
            t.transform.parent.gameObject.SetActive(amount > 0);
            if (amount > 0)
            {
                t.text = $"{amount}x";
            }
        }

        public void UseButton(int type)
        {
            int amountToDrop = 0;
            switch (type)
            {
                case 0: amountToDrop = _lowAmount; break;
                case 1: amountToDrop = _medAmount; break;
                case 2: amountToDrop = _highAmount; break;
            }

            if (amountToDrop > 0)
            {
                ReferenceHub localHub = ReferenceHub.LocalHub;
                if (localHub != null && localHub.inventory != null)
                {
                    localHub.inventory.CmdDropAmmo((byte)_targetItem.ItemTypeId, (ushort)amountToDrop);
                }
            }
        }

        public bool IsHovering()
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_myTransform, Input.mousePosition, null, out localPoint))
            {
                return localPoint.x > _minX && localPoint.x < _maxX &&
                       localPoint.y > _minY && localPoint.y < _maxY;
            }
            return false;
        }
    }
}