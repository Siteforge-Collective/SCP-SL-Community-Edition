using System;
using System.Runtime.InteropServices;
using PlayerRoles;
using UnityEngine;

public static class TaskbarFlasher
{
	private struct FlashWindowInfo
	{
		public uint Size;

		public IntPtr Handle;

		public uint Flags;

		public uint Count;

		public uint Timeout;
	}

	private const string User32 = "User32";

	private static IntPtr _windowHandle;

	private static IntPtr WindowHandle => (IntPtr)0;

	[RuntimeInitializeOnLoadMethod]
	private static void RegisterEvent()
	{
	}

	private static void OnRoleChanged(ReferenceHub hub, PlayerRoleBase oldRole, PlayerRoleBase newRole)
	{
	}

	private static void SetFlash(bool enabled)
	{
	}

	[PreserveSig]
	private static extern IntPtr GetWindow();

	[PreserveSig]
	private static extern IntPtr FindWindow(string windowClass, string windowName);

	[PreserveSig]
	private static extern bool FlashWindow(ref FlashWindowInfo info);
}
