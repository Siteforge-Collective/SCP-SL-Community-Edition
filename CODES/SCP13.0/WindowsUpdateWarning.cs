using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowsUpdateWarning : MonoBehaviour
{
	public GameObject warning;

	public GameObject menu;

	private void Start()
	{
	}

	public static bool UpdateRequired()
	{
		return false;
	}

	private static bool CheckDll(string name)
	{
		return false;
	}

	[PreserveSig]
	private static extern IntPtr LoadLibrary(string name);

	[PreserveSig]
	private static extern bool FreeLibrary(IntPtr library);
}
