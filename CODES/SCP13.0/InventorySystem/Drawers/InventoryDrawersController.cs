using System;
using System.Collections.Generic;
using InventorySystem.Items;
using PlayerRoles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Drawers
{
    public class InventoryDrawersController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField]
        private TextMeshProUGUI _alertText;

        [SerializeField]
        private Slider _progressbarSlider;

        [SerializeField]
        private RectTransform _progressbarSliderRect;

        [SerializeField]
        private Graphic[] _detailsToRecolor;

        [SerializeField]
        private CanvasGroup _thisGroup;

        [SerializeField]
        private CanvasGroup _inventoryGroup;

        [Header("Tracking")]
        private IItemAlertDrawer _alertToTrack;

        private IItemProgressbarDrawer _progressbarToTrack;

        private bool _active;

        private void Start()
        {
            Inventory.OnCurrentItemChanged += ItemChanged;
            PlayerRoleManager.OnRoleChanged += RecolorDetails;

            if (ReferenceHub.TryGetLocalHub(out ReferenceHub hub))
            {
                RecolorDetails(hub, null, hub.roleManager.CurrentRole);
            }
        }

        private void OnDestroy()
        {
            Inventory.OnCurrentItemChanged -= ItemChanged;
            PlayerRoleManager.OnRoleChanged -= RecolorDetails;
        }

        private void RecolorDetails(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (hub == null || !hub.isLocalPlayer || newRole == null) return;

            Color roleColor = newRole.RoleColor;
            foreach (Graphic detail in _detailsToRecolor)
            {
                if (detail != null)
                {
                    detail.color = roleColor;
                }
            }
        }

        private void Update()
        {
            bool isInventoryOpen = _inventoryGroup != null && _inventoryGroup.alpha > 0.01f;

            if (!_active || !isInventoryOpen)
            {
                if (_thisGroup != null && _thisGroup.alpha > 0f)
                    _thisGroup.alpha = 0f;
                return;
            }

            if (_thisGroup != null && _thisGroup.alpha < 1f)
                _thisGroup.alpha = 1f;

            if (_alertToTrack != null && _alertText != null)
            {
                _alertText.text = _alertToTrack.AlertText;
            }

            if (_progressbarToTrack != null && _progressbarSlider != null)
            {
                bool isEnabled = _progressbarToTrack.ProgressbarEnabled;

                if (_progressbarSlider.gameObject.activeSelf != isEnabled)
                    _progressbarSlider.gameObject.SetActive(isEnabled);

                if (isEnabled)
                {
                    float min = _progressbarToTrack.ProgressbarMin;
                    float max = _progressbarToTrack.ProgressbarMax;
                    float val = _progressbarToTrack.ProgressbarValue;

                    float range = max - min;
                    _progressbarSlider.value = (range > 0) ? (val - min) / range : 0f;

                    if (_progressbarSliderRect != null)
                    {
                        Vector2 size = _progressbarSliderRect.sizeDelta;
                        size.x = _progressbarToTrack.ProgressbarWidth;
                        _progressbarSliderRect.sizeDelta = size;
                    }
                }
            }
        }

        private void ItemChanged(ReferenceHub hub, ItemIdentifier prevItem, ItemIdentifier newItem)
        {
            if (hub == null || !hub.isLocalPlayer) return;

            _alertToTrack = null;
            _progressbarToTrack = null;
            _active = false;

            if (_alertText != null) _alertText.text = string.Empty;
            if (_progressbarSlider != null) _progressbarSlider.gameObject.SetActive(false);

            if (hub.inventory?.UserInventory?.Items != null &&
                hub.inventory.UserInventory.Items.TryGetValue(newItem.SerialNumber, out ItemBase item) && item != null)
            {
                _alertToTrack = item as IItemAlertDrawer;
                _progressbarToTrack = item as IItemProgressbarDrawer;

                _active = (_alertToTrack != null || _progressbarToTrack != null);
            }
        }
    }
}