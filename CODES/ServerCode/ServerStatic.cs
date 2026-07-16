public class ServerStatic : global::UnityEngine.MonoBehaviour
{
	public enum NextRoundAction : byte
	{
		DoNothing = 0,
		Restart = 1,
		Shutdown = 2
	}

	public static bool IsDedicated;

	public static bool ProcessIdPassed;

	public bool Simulate;

	internal static bool DisableConfigValidation;

	internal static bool KeepSession;

	internal static bool EnableConsoleHeartbeat;

	internal static bool ServerPortSet;

	internal static YamlConfig RolesConfig;

	internal static YamlConfig SharedGroupsConfig;

	internal static YamlConfig SharedGroupsMembersConfig;

	internal static string RolesConfigPath;

	internal static PermissionsHandler PermissionsHandler;

	private static short _serverTickrate = 60;

	public static global::ServerOutput.IServerOutput ServerOutput;

	public static ServerStatic.NextRoundAction StopNextRound { get; set; } = ServerStatic.NextRoundAction.DoNothing;

	internal static short ServerTickrate
	{
		get
		{
			return _serverTickrate;
		}
		set
		{
			if (value < -1 || value == 0)
			{
				_serverTickrate = 60;
			}
			else
			{
				_serverTickrate = value;
			}
			if (IsDedicated)
			{
				global::UnityEngine.Application.targetFrameRate = _serverTickrate;
				ServerConsole.AddLog("Server tickrate set to: " + _serverTickrate);
			}
		}
	}

	public static ushort ServerPort { get; private set; }

	private void Awake()
	{
		string[] args = StartupArgs.Args;
		DisableConfigValidation = args.Contains<string>("-disableconfigvalidation");
		for (int i = 0; i < args.Length - 1; i++)
		{
			string text = args[i];
			if (!(text == "-appdatapath"))
			{
				if (text == "-configpath")
				{
					FileManager.SetConfigFolder(args[i + 1]);
				}
			}
			else
			{
				FileManager.SetAppFolder(args[i + 1]);
			}
		}
		string[] array = args;
		foreach (string text2 in array)
		{
			switch (text2)
			{
			case "-nographics":
				Simulate = true;
				continue;
			case "-keepsession":
				KeepSession = true;
				continue;
			case "-heartbeat":
				EnableConsoleHeartbeat = true;
				continue;
			}
			if (text2.StartsWith("-key", global::System.StringComparison.Ordinal) && text2.Length > 4 && !ServerPortSet && ServerOutput == null)
			{
				ServerOutput = new global::ServerOutput.FileConsole(text2.Remove(0, 4));
			}
			else if (text2.StartsWith("-console", global::System.StringComparison.Ordinal) && ServerOutput == null)
			{
				if (ushort.TryParse(text2.Remove(0, 8), out var result))
				{
					ServerOutput = new global::ServerOutput.TcpConsole(result);
				}
			}
			else if (text2.StartsWith("-id", global::System.StringComparison.Ordinal) && !ProcessIdPassed)
			{
				ProcessIdPassed = true;
				if (int.TryParse(text2.Remove(0, 3), out var result2))
				{
					ServerConsole.ConsoleProcess = global::System.Diagnostics.Process.GetProcessById(result2);
				}
				if (ServerConsole.ConsoleProcess == null)
				{
					OnConsoleExited(null, null);
				}
				ServerConsole.ConsoleProcess.EnableRaisingEvents = true;
				ServerConsole.ConsoleProcess.Exited += OnConsoleExited;
			}
			else if (text2.StartsWith("-port", global::System.StringComparison.Ordinal) && !ServerPortSet)
			{
				if (!ushort.TryParse(text2.Remove(0, 5), out var result3))
				{
					ServerConsole.AddLog("\"-port\" argument value is not a valid unsigned short integer (0 - 65535). Aborting startup.");
					Shutdown.Quit();
					return;
				}
				ServerPort = result3;
				ServerPortSet = true;
			}
			else if (text2 == "-stdout" && !ServerPortSet && ServerOutput == null)
			{
				ServerOutput = new global::ServerOutput.StandardOutput();
			}
		}
		if (ServerOutput == null)
		{
			Shutdown.Quit();
			return;
		}
		ServerOutput.Start();
		if (Simulate || IsDedicated)
		{
			IsDedicated = true;
			global::UnityEngine.AudioListener.volume = 0f;
			global::UnityEngine.AudioListener.pause = true;
			global::UnityEngine.QualitySettings.pixelLightCount = 0;
			global::UnityEngine.GUI.enabled = false;
			ServerConsole.AddLog("SCP Secret Laboratory process started. Creating match...", global::System.ConsoleColor.Green);
			ServerTickrate = 60;
		}
		if (IsDedicated && !ServerPortSet)
		{
			ServerConsole.AddLog("\"-port\" argument is required for dedicated server. Aborting startup.", global::System.ConsoleColor.DarkRed);
			ServerConsole.AddLog("Make sure you are using latest version of LocalAdmin.", global::System.ConsoleColor.DarkRed);
			Shutdown.Quit();
		}
		else
		{
			global::PluginAPI.Loader.AssemblyLoader.Initialize();
			global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneWasLoaded;
		}
	}

	private static void OnConsoleExited(object sender, global::System.EventArgs e)
	{
		ServerConsole.DisposeStatic();
		IsDedicated = false;
		global::UnityEngine.Debug.Log("OnConsoleExited");
		ServerConsole.ConsoleProcess?.Dispose();
		ServerConsole.ConsoleProcess = null;
		Shutdown.Quit();
	}

	private void OnSceneWasLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		if (IsDedicated && (scene.buildIndex == 3 || scene.buildIndex == 4))
		{
			GetComponent<CustomNetworkManager>().CreateMatch();
		}
	}

	public static PermissionsHandler GetPermissionsHandler()
	{
		return PermissionsHandler;
	}
}
