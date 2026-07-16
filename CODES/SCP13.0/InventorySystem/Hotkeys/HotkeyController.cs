using System;
using System.Collections.Generic;
using InventorySystem.Hotkeys.Customization;
using InventorySystem.Items;
using Mirror;
using UnityEngine;
using InventorySystem.GUI;

namespace InventorySystem.Hotkeys
{
    public class HotkeyController : MonoBehaviour
    {
        public static event Action<int, ItemBase> OnHotkeyTriggered;
        public static bool AllowActivation
        {
            get
            {
                if (Cursor.visible) return false;
                if (InventoryGuiController.InventoryVisible) return false;
                return InventoryGuiController.CanInventoryBeDisplayed();
            }
        }

        private void Awake()
        {
            HotkeyInterpreter.OnHotkeysRevalidated += OnUpdate;
        }

        private void OnDestroy()
        {
            HotkeyInterpreter.OnHotkeysRevalidated -= OnUpdate;
        }

        private void OnUpdate()
        {
            if (!AllowActivation) return;

            var hotkeys = HotkeyStorage.Hotkeys;
            if (hotkeys == null) return;

            for (int i = 0; i < hotkeys.Count; i++)
            {
                if (Input.GetKeyDown(hotkeys[i].AssignedKey))
                {
                    ProcessHotkey(i, hotkeys[i]);
                }
            }
        }

        private void Update()
        {
            if (!AllowActivation) return;

            var hotkeys = HotkeyStorage.Hotkeys;
            if (hotkeys == null) return;

            for (int i = 0; i < hotkeys.Count; i++)
            {
                if (Input.GetKeyDown(hotkeys[i].AssignedKey))
                {
                    ProcessHotkey(i, hotkeys[i]);
                }
            }
        }

        public void ProcessHotkey(int index, SavedHotkey sv)
        {
            if (HotkeyInterpreter.TryGetItem(index, out ItemBase item))
            {
                ReferenceHub localHub = ReferenceHub.LocalHub;
                if (localHub == null || localHub.inventory == null) return;

                localHub.inventory.CmdSelectItem(item.ItemSerial);
                OnHotkeyTriggered?.Invoke(index, item);
            }
        }
    }
}