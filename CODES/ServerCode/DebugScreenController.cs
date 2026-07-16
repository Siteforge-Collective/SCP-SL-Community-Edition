public class DebugScreenController : global::UnityEngine.MonoBehaviour
{
	public static int Asserts;

	public static int Errors;

	public static int Exceptions;

	private static bool _logged;

	private void Awake()
	{
		global::System.IO.Directory.SetCurrentDirectory(global::System.IO.Path.GetDirectoryName(global::System.IO.Path.GetFullPath(global::UnityEngine.Application.dataPath)));
		if (!global::System.Environment.GetCommandLineArgs().Contains<string>("-nographics"))
		{
			Shutdown.Quit();
		}
	}

	private void Start()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		global::UnityEngine.Application.logMessageReceivedThreaded += LogMessage;
		Log();
	}

	private static void Log()
	{
		if (!_logged)
		{
			global::UnityEngine.Debug.Log(string.Concat("Time: ", TimeBehaviour.Rfc3339Time(), "\nGPU: ", global::UnityEngine.SystemInfo.graphicsDeviceName, "\nGPU Driver version: ", GpuDriver.DriverVersion, "\nVRAM: ", global::UnityEngine.SystemInfo.graphicsMemorySize, "MB\nShaderLevel: ", global::UnityEngine.SystemInfo.graphicsShaderLevel.ToString().Insert(1, "."), "\nVendor: ", global::UnityEngine.SystemInfo.graphicsDeviceVendor, "\nAPI: ", global::UnityEngine.SystemInfo.graphicsDeviceType, "\nInfo: ", global::UnityEngine.SystemInfo.graphicsDeviceVersion, "\nResolution: ", global::UnityEngine.Screen.width, "x", global::UnityEngine.Screen.height, "\nFPS Limit: ", global::UnityEngine.Application.targetFrameRate, "\nFullscreen: ", ResolutionManager.ScreenMode, "\nCPU: ", global::UnityEngine.SystemInfo.processorType, "\nThreads: ", global::UnityEngine.SystemInfo.processorCount, "\nFrequency: ", global::UnityEngine.SystemInfo.processorFrequency, "MHz\nRAM: ", global::UnityEngine.SystemInfo.systemMemorySize, "MB\nAudio Supported: ", global::UnityEngine.SystemInfo.supportsAudio.ToString(), "\nOS: ", global::NorthwoodLib.OperatingSystem.VersionString, "\nUnity: ", global::UnityEngine.Application.unityVersion, "\nFramework: ", Misc.GetRuntimeVersion(), "\nIL2CPP: ", PlatformInfo.singleton.IsIl2Cpp.ToString(), "\nVersion: ", global::GameCore.Version.VersionString, "\nBuild: ", global::UnityEngine.Application.buildGUID, "\nSystem Language: ", global::System.Globalization.CultureInfo.CurrentCulture.EnglishName, " (", global::System.Globalization.CultureInfo.CurrentCulture.Name, ")\nGame Language: ", PlayerPrefsSl.Get("translation_path", "English (default)"), "\nLaunch arguments: ", global::System.Environment.CommandLine));
			global::UnityEngine.Debug.Log(global::CommandSystem.Commands.Shared.BuildInfoCommand.BuildInfoString);
			if (WindowsUpdateWarning.UpdateRequired())
			{
				global::UnityEngine.Debug.LogError("Important system file that is needed for voicechat is missing, please install this windows update in order to get your voicechat working https://support.microsoft.com/en-us/help/2999226/update-for-universal-c-runtime-in-windows");
			}
		}
	}

	private static void LogMessage(string condition, string stackTrace, global::UnityEngine.LogType type)
	{
		switch (type)
		{
		case global::UnityEngine.LogType.Assert:
			global::System.Threading.Interlocked.Increment(ref Asserts);
			break;
		case global::UnityEngine.LogType.Error:
			global::System.Threading.Interlocked.Increment(ref Errors);
			break;
		case global::UnityEngine.LogType.Exception:
			global::System.Threading.Interlocked.Increment(ref Exceptions);
			break;
		case global::UnityEngine.LogType.Warning:
		case global::UnityEngine.LogType.Log:
			break;
		}
	}

	private void Update()
	{
	}
}
