using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UserSettings.VideoSettings
{
    internal static class FullscreenWindowManager
    {
        private const string LibName = "user32.dll";
        private const int MinimizeCode = 2;
        private const FullScreenMode AutoMinimizeScreenMode = FullScreenMode.MaximizedWindow;

        [DllImport(LibName)]
        private static extern IntPtr GetActiveWindow();

        [DllImport(LibName)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Application.focusChanged += OnFocusChanged;
        }

        private static void OnFocusChanged(bool focusState)
        {
            if (!focusState)
            {
                OnLoseFocus();
            }
        }

        private static void OnLoseFocus()
        {
            int displayMode = UserSetting<int>.Get<DisplayVideoSetting>(DisplayVideoSetting.FullscreenMode);

            if (displayMode == 2)
            {
                IntPtr activeWindow = GetActiveWindow();
                if (activeWindow != IntPtr.Zero)
                {
                    ShowWindow(activeWindow, MinimizeCode);
                }
            }
        }
    }
}