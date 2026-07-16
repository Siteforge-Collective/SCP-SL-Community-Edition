public class WindowsUpdateWarning : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject warning;

	public global::UnityEngine.GameObject menu;

	private void Start()
	{
		warning.SetActive(UpdateRequired());
		menu.SetActive(global::UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3 || !warning.activeSelf);
	}

	public static bool UpdateRequired()
	{
		try
		{
			return global::UnityEngine.SystemInfo.operatingSystemFamily == global::UnityEngine.OperatingSystemFamily.Windows && global::NorthwoodLib.OperatingSystem.Version.Major < 10 && !global::System.IO.File.Exists(global::System.Environment.GetFolderPath(global::System.Environment.SpecialFolder.System) + global::System.IO.Path.DirectorySeparatorChar + "API-MS-WIN-CRT-MATH-L1-1-0.dll") && !CheckDll("API-MS-WIN-CRT-MATH-L1-1-0.dll");
		}
		catch (global::System.Exception exception)
		{
			global::UnityEngine.Debug.LogException(exception);
			return true;
		}
	}

	private static bool CheckDll(string name)
	{
		global::System.IntPtr intPtr = LoadLibrary(name);
		if (intPtr == global::System.IntPtr.Zero)
		{
			throw new global::System.ComponentModel.Win32Exception();
		}
		if (!FreeLibrary(intPtr))
		{
			throw new global::System.ComponentModel.Win32Exception();
		}
		return true;
	}

	[global::System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = global::System.Runtime.InteropServices.CharSet.Unicode, EntryPoint = "LoadLibraryW", SetLastError = true)]
	private static extern global::System.IntPtr LoadLibrary(string name);

	[global::System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
	private static extern bool FreeLibrary(global::System.IntPtr library);
}
