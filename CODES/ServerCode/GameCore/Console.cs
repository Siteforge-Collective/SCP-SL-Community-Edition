namespace GameCore
{
	public class Console : global::ToggleableMenus.SimpleToggleableMenu
	{
		public enum ConsoleLogType
		{
			DoNotLog = 0,
			Log = 1,
			Warning = 2,
			Error = 3
		}

		public global::GameCore.CommandHint[] hints;

		public readonly global::CommandSystem.GameConsoleCommandHandler ConsoleCommandHandler = global::CommandSystem.GameConsoleCommandHandler.Create();

		internal static bool TranslationDebugMode;

		internal static global::Org.BouncyCastle.Crypto.AsymmetricKeyParameter _publicKey;

		private string _content;

		public static global::GameCore.Console singleton { get; private set; }

		public static bool EnableSCP { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (singleton == null)
			{
				singleton = this;
			}
			else
			{
				global::UnityEngine.Object.DestroyImmediate(base.gameObject);
			}
		}

		private void Start()
		{
			global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
			AddLog("Hi there! Initializing console...", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
			AddLog("Done! Type 'help' to print the list of available commands.", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
			CentralAuthManager.InitAuth();
			if (global::System.Linq.Enumerable.Any(StartupArgs.Args, (string arg) => string.Equals(arg, "-smartclasspicker", global::System.StringComparison.OrdinalIgnoreCase)))
			{
				EnableSCP = true;
				AddLog("Smart Class Picker will be enabled for your server.", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
			}
			if (global::System.Linq.Enumerable.Any(StartupArgs.Args, (string arg) => string.Equals(arg, "-tdm", global::System.StringComparison.OrdinalIgnoreCase)))
			{
				TranslationDebugMode = true;
				AddLog("Translation debug mode has been enabled (startup argument).", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
			}
			else
			{
				TranslationDebugMode = false;
			}
		}

		private void OnSceneLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
		{
			AddLog("Scene Manager: Loaded scene '" + scene.name + "' [" + scene.path + "]", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
			RefreshConsoleScreen();
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
		}

		private void FixedUpdate()
		{
		}

		private void RefreshConsoleScreen()
		{
		}

		public static void AddDebugLog(string debugKey, string message, MessageImportance importance, bool nospace = false)
		{
			if (ConsoleDebugMode.CheckImportance(debugKey, importance, out var color))
			{
				AddLog("[DEBUG_" + debugKey + "] " + message, color, nospace);
			}
		}

		public static void AddLog(string text, global::UnityEngine.Color c, bool nospace = false, global::GameCore.Console.ConsoleLogType type = global::GameCore.Console.ConsoleLogType.Log)
		{
			if (ServerStatic.IsDedicated)
			{
				ServerConsole.AddLog(text, Misc.ClosestConsoleColor(c));
			}
		}

		public static global::UnityEngine.GameObject FindConnectedRoot(global::Mirror.NetworkConnection conn)
		{
			try
			{
				global::UnityEngine.GameObject gameObject = conn.identity.gameObject;
				if (gameObject.CompareTag("Player"))
				{
					return gameObject;
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		internal string TypeCommand(string cmd, CommandSender sender = null)
		{
			if (sender == null)
			{
				sender = ServerConsole.Scs;
			}
			if (cmd.StartsWith(".", global::System.StringComparison.Ordinal) && cmd.Length > 1)
			{
				if (!global::Mirror.NetworkClient.active && !global::Mirror.NetworkServer.active)
				{
					AddLog("You must be connected to a server to use this command.", global::UnityEngine.Color.red);
					return "You must be connected to a server to use remote admin commands!";
				}
				string text = "Sending command to server: " + cmd.Substring(1);
				sender?.Print(text, global::System.ConsoleColor.Green);
				ReferenceHub.LocalHub.gameConsoleTransmission.SendToServer(cmd.Substring(1));
				return text;
			}
			bool flag = cmd.StartsWith("@", global::System.StringComparison.Ordinal);
			if ((cmd.StartsWith("/", global::System.StringComparison.Ordinal) || flag) && cmd.Length > 1)
			{
				string text2 = (flag ? cmd : cmd.Substring(1));
				if (!flag)
				{
					text2 = text2.TrimStart('$');
					if (string.IsNullOrEmpty(text2))
					{
						sender?.Print("Command cant be empty!", global::System.ConsoleColor.Green);
						return "Command cant be empty!";
					}
				}
				if (global::Mirror.NetworkServer.active)
				{
					return global::RemoteAdmin.CommandProcessor.ProcessQuery(text2, sender);
				}
			}
			string[] array = cmd.Trim().Split(global::RemoteAdmin.QueryProcessor.SpaceArray, 512, global::System.StringSplitOptions.RemoveEmptyEntries);
			if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.ConsoleCommand, sender, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1))))
			{
				return null;
			}
			cmd = array[0];
			if (!ConsoleCommandHandler.TryGetCommand(cmd, out var command))
			{
				string text3 = "Command " + cmd + " does not exist!";
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.ConsoleCommandExecuted, sender, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), false, text3))
				{
					return null;
				}
				sender?.Print(text3, global::System.ConsoleColor.DarkYellow, new global::UnityEngine.Color32(byte.MaxValue, 180, 0, byte.MaxValue));
				return text3;
			}
			try
			{
				string response;
				bool flag2 = command.Execute(array.Segment(1), sender, out response);
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.ConsoleCommandExecuted, sender, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), flag2, response))
				{
					return null;
				}
				if (string.IsNullOrWhiteSpace(response))
				{
					return null;
				}
				sender?.Print(response, flag2 ? global::System.ConsoleColor.Green : global::System.ConsoleColor.Red);
				return response;
			}
			catch (global::System.Exception ex)
			{
				string text4 = "Command execution failed! Error: " + Misc.RemoveStacktraceZeroes(ex.ToString());
				if (!global::PluginAPI.Events.EventManager.ExecuteEvent(global::PluginAPI.Enums.ServerEventType.ConsoleCommandExecuted, sender, array[0], global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Skip(array, 1)), false, text4))
				{
					return null;
				}
				sender?.Print(text4, global::System.ConsoleColor.Red);
				return text4;
			}
		}

		public void ProceedButton()
		{
		}

		protected override void OnToggled()
		{
			base.OnToggled();
		}

		private void OnApplicationQuit()
		{
			Shutdown.Quit(quit: false);
		}
	}
}
