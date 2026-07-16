using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToggleableMenus
{
    public static class ToggleableMenuController
    {
        private static IRegisterableMenu _currentMenu;
        private static bool _wasAnyEnabled;

        public static readonly HashSet<IRegisterableMenu> RegisteredMenus = new HashSet<IRegisterableMenu>();

        public static bool AnyEnabled
        {
            get
            {
                if (IsNull(_currentMenu)) return false;
                return _currentMenu.IsEnabled;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            StaticUnityMethods.OnUpdate += Update;
        }

        public static bool IsNull(IRegisterableMenu menu)
        {
            if (menu == null) return true;
            if (menu is UnityEngine.Object obj && obj == null) return true;
            return false;
        }

        private static void Update()
        {
            if (AnyEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    HideCurrent();
                }
            }

            foreach (var menu in RegisteredMenus)
            {
                if (IsNull(menu)) continue;

                if (menu is ToggleableMenuBase baseMenu)
                {
                    if (menu is IHoldableMenu holdable && holdable.IsHoldable)
                    {
                        HandleHoldableMenu(baseMenu);
                    }
                    else
                    {
                        HandleBaseMenu(baseMenu);
                    }
                }
            }

            _wasAnyEnabled = AnyEnabled;
        }

        private static void HandleBaseMenu(ToggleableMenuBase menu)
        {
            if (Input.GetKeyDown(NewInput.GetKey(menu.MenuActionKey)))
            {
                if (menu.CanToggle)
                {
                    ToggleMenu(menu);
                }
            }
        }
        private static void HandleHoldableMenu(ToggleableMenuBase menu)
        {
            if (!menu.CanToggle) return;
            bool pressed = Input.GetKey(NewInput.GetKey(menu.MenuActionKey));
            if (pressed != menu.IsEnabled)
            {
                if (pressed)
                {
                    ForceCurrent(menu);
                }
                else if (_currentMenu == menu)
                {
                    HideCurrent();
                }
            }
        }

        public static void ToggleMenu(IRegisterableMenu menu)
        {
            if (IsNull(menu)) return;

            if (_currentMenu == menu)
            {
                HideCurrent();
            }
            else
            {
                ForceCurrent(menu);
            }
        }

        public static void ForceCurrent(IRegisterableMenu menu)
        {
            HideCurrent();

            if (IsNull(menu)) return;

            _currentMenu = menu;
            _currentMenu.IsEnabled = true;
        }

        public static void HideCurrent()
        {
            if (!IsNull(_currentMenu))
            {
                _currentMenu.IsEnabled = false;
            }
            _currentMenu = null;
        }
    }
}