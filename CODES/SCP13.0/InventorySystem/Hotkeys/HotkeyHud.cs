using System;
using System.Collections.Generic;
using System.Diagnostics;
using InventorySystem.Hotkeys.Customization;
using InventorySystem.Items;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace InventorySystem.Hotkeys
{
    public class HotkeyHud : MonoBehaviour
    {
        private struct HotkeyToDisplay
        {
            public ItemBase Item;
            public HotkeyApperance Apperance;
            public KeyCode Key;
            public bool IsNew;
        }

        [SerializeField] private CanvasGroup _fadeGroup;
        [SerializeField] private List<HotkeyIconBase> _iconTemplates;
        [SerializeField] private RectTransform _iconParent;
        [SerializeField] private RectTransform _ellipsisIndicator;

        [SerializeField] private float _spacing = 10f;
        [SerializeField] private float _maxWidth = 800f;

        private readonly List<HotkeyToDisplay> _hotkeysToDisplay = new List<HotkeyToDisplay>();
        private readonly List<HotkeyIconBase> _iconInstances = new List<HotkeyIconBase>();
        private readonly Stopwatch _fadeoutStopwatch = new Stopwatch();

        private bool[] _prevEmpty;
        private Color _lastColor = Color.white;

        private const float FadeoutTime = 5.5f;
        private const float FadeoutMaxSpeed = 1.2f;
        private const float FadeoutMinSpeed = 0.05f;
        private const float FadeinSpeed = 2f;

        private void Start()
        {
            PlayerRoleManager.OnRoleChanged += UpdateColors;
            HotkeyInterpreter.OnHotkeysRevalidated += UpdateHotkeys;
            HotkeyController.OnHotkeyTriggered += OnHotkeyTriggered;

            if (HotkeyStorage.Hotkeys != null)
            {
                _prevEmpty = new bool[HotkeyStorage.Hotkeys.Count];
                for (int i = 0; i < _prevEmpty.Length; i++) _prevEmpty[i] = true;
            }

            UpdateHotkeys();
            if (ReferenceHub.LocalHub != null && ReferenceHub.LocalHub.roleManager != null)
            {
                UpdateColors(ReferenceHub.LocalHub, null, ReferenceHub.LocalHub.roleManager.CurrentRole);
            }
        }

        private void OnDestroy()
        {
            PlayerRoleManager.OnRoleChanged -= UpdateColors;
            HotkeyInterpreter.OnHotkeysRevalidated -= UpdateHotkeys;
            HotkeyController.OnHotkeyTriggered -= OnHotkeyTriggered;
        }

        private void Update()
        {
            bool shouldBeVisible = _fadeoutStopwatch.IsRunning && _fadeoutStopwatch.Elapsed.TotalSeconds < FadeoutTime;
            float targetAlpha = shouldBeVisible ? 1f : 0f;

            float speed = shouldBeVisible ? FadeinSpeed : Mathf.Lerp(FadeoutMinSpeed, FadeoutMaxSpeed, 1f - _fadeGroup.alpha);
            _fadeGroup.alpha = Mathf.MoveTowards(_fadeGroup.alpha, targetAlpha, Time.deltaTime * speed);

            foreach (var icon in _iconInstances)
            {
                if (icon.gameObject.activeSelf)
                {
                    icon.UpdateAnimations();
                }
            }
        }

        private void OnHotkeyTriggered(int index, ItemBase item)
        {
            _fadeoutStopwatch.Restart();

            foreach (var icon in _iconInstances)
            {
                if (icon.gameObject.activeSelf && icon.Key == HotkeyStorage.Hotkeys[index].AssignedKey)
                {
                    icon.PlayHighlightAnimation();
                    break;
                }
            }
        }

        private void UpdateHotkeys()
        {
            _hotkeysToDisplay.Clear(); 

            var hotkeys = HotkeyStorage.Hotkeys;
            if (hotkeys == null) return;

            for (int i = 0; i < hotkeys.Count; i++)
            {
                bool hasItem = HotkeyInterpreter.TryGetItem(i, out ItemBase item);

                if (hasItem && item != null)
                {
                    HotkeyApperance apperance = hotkeys[i].Apperance;

                    _hotkeysToDisplay.Add(new HotkeyToDisplay
                    {
                        Item = item,
                        Key = hotkeys[i].AssignedKey,
                        Apperance = apperance,
                        IsNew = _prevEmpty[i]
                    });

                    _prevEmpty[i] = false;
                }
                else
                {
                    _prevEmpty[i] = true;
                }
            }

            Draw();
        }

        private void Draw()
        {
            foreach (var icon in _iconInstances)
            {
                icon.gameObject.SetActive(false);
            }

            if (_ellipsisIndicator != null)
                _ellipsisIndicator.gameObject.SetActive(false);

            float currentX = 0f;
            bool overflow = false;

            foreach (var displayData in _hotkeysToDisplay)
            {
                HotkeyIconBase icon = SpawnIcon(displayData.Item, displayData.Apperance);

                if (icon == null) continue;

                RectTransform rect = icon.transform as RectTransform;
                if (rect == null) continue;

                float width = rect.rect.width;

                if (currentX + width > _maxWidth)
                {
                    overflow = true;
                    break;
                }

                icon.gameObject.SetActive(true);
                icon.Key = displayData.Key;
                icon.SetItem(displayData.Item);
                icon.SetColors(_lastColor);

                if (displayData.IsNew)
                {
                    icon.PlayHighlightAnimation();
                }

                rect.anchoredPosition = new Vector2(currentX + (width * rect.pivot.x), 0f);
                currentX += width + _spacing;
            }

            if (overflow && _ellipsisIndicator != null)
            {
                _ellipsisIndicator.gameObject.SetActive(true);
                _ellipsisIndicator.anchoredPosition = new Vector2(currentX + (_ellipsisIndicator.rect.width * _ellipsisIndicator.pivot.x), 0f);
            }
        }

        private HotkeyIconBase SpawnIcon(ItemBase item, HotkeyApperance apperance)
        {
            HotkeyIconBase template = null;
            foreach (var t in _iconTemplates)
            {
                if (t.CheckCompatibility(item, apperance))
                {
                    template = t;
                    break;
                }
            }

            if (template == null) return null;

            foreach (var instance in _iconInstances)
            {
                if (!instance.gameObject.activeSelf && instance.GetType() == template.GetType())
                {
                    if (instance.CheckCompatibility(item, apperance))
                    {
                        return instance;
                    }
                }
            }

            HotkeyIconBase newIcon = Instantiate(template, _iconParent);
            _iconInstances.Add(newIcon);
            return newIcon;
        }

        private void UpdateColors(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (hub != ReferenceHub.LocalHub) return;

            if (newRole != null)
            {
                _lastColor = newRole.RoleColor;
            }
            else
            {
                _lastColor = Color.white;
            }

            foreach (var icon in _iconInstances)
            {
                if (icon.gameObject.activeSelf)
                {
                    icon.SetColors(_lastColor);
                }
            }
        }
    }
}