using System;
using System.Runtime.InteropServices;

internal static class LauncherCommunicator
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private delegate void PostAuthDelegate();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	private delegate void SendDelegate(string data, out IntPtr response);

	private static readonly SendDelegate SendMethod;

	internal unsafe static readonly float* AssetVerificationProgress;

	internal unsafe static readonly bool* VerificationFinished;

	private static readonly PostAuthDelegate PostAuth;

	internal static string Send(string data)
	{
		return null;
	}

	internal static void Heartbeat()
	{
	}

	[PreserveSig]
	private static extern IntPtr GetProcAddress(IntPtr handle, string name);

	private static T GetNativeDelegate<T>(IntPtr handle, string name) where T : Delegate
	{
		return null;
	}
}
