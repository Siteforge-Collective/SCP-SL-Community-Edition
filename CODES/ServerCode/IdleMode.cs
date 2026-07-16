public class IdleMode : global::UnityEngine.MonoBehaviour
{
	public static uint IdleModeTime;

	public static uint IdleModePreauthTime;

	private static bool _idleModeEnabled = true;

	private static bool _pauseIdleMode = true;

	private static short _idleModeTickrate = 1;

	private static readonly global::System.Diagnostics.Stopwatch _st = new global::System.Diagnostics.Stopwatch();

	internal static readonly global::System.Diagnostics.Stopwatch PreauthStopwatch = new global::System.Diagnostics.Stopwatch();

	public static bool IdleModeActive { get; private set; }

	public static bool PauseIdleMode
	{
		get
		{
			return _pauseIdleMode;
		}
		set
		{
			if (_pauseIdleMode == value)
			{
				return;
			}
			_pauseIdleMode = value;
			if (value)
			{
				SetIdleMode(state: false);
				_st.Reset();
				PreauthStopwatch.Reset();
				if (_idleModeEnabled)
				{
					ServerConsole.AddLog("Idle mode is now temporarily blocked.");
				}
			}
			else if (_idleModeEnabled)
			{
				ServerConsole.AddLog("Idle mode is now available.");
				_st.Restart();
				PreauthStopwatch.Restart();
			}
		}
	}

	public static bool IdleModeEnabled
	{
		get
		{
			return _idleModeEnabled;
		}
		set
		{
			_idleModeEnabled = value;
			if (!_idleModeEnabled && IdleModeActive)
			{
				SetIdleMode(state: false);
			}
		}
	}

	internal static short IdleModeTickrate
	{
		get
		{
			return _idleModeTickrate;
		}
		set
		{
			_idleModeTickrate = (short)((value < -1 || value == 0) ? 1 : value);
			if (IdleModeActive)
			{
				SetIdleMode(state: true, force: true);
			}
		}
	}

	private void Start()
	{
		global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		ReferenceHub.OnPlayerAdded = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerAdded, (global::System.Action<ReferenceHub>)delegate
		{
			SetIdleMode(state: false);
		});
		ReferenceHub.OnPlayerRemoved = (global::System.Action<ReferenceHub>)global::System.Delegate.Combine(ReferenceHub.OnPlayerRemoved, (global::System.Action<ReferenceHub>)delegate
		{
			if (ReferenceHub.AllHubs.Count <= 1)
			{
				SetIdleMode(state: true);
			}
		});
	}

	private void FixedUpdate()
	{
		if (_st.ElapsedMilliseconds >= IdleModeTime && !_pauseIdleMode && (!PreauthStopwatch.IsRunning || PreauthStopwatch.ElapsedMilliseconds >= IdleModePreauthTime))
		{
			_st.Reset();
			PreauthStopwatch.Reset();
			if (ReferenceHub.AllHubs.Count <= 1)
			{
				SetIdleMode(state: true);
			}
		}
	}

	private static void OnSceneLoaded(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		_st.Reset();
		if (ServerStatic.IsDedicated && !_pauseIdleMode && IdleModeEnabled && scene.name == "Facility")
		{
			_st.Start();
		}
	}

	public static void SetIdleMode(bool state)
	{
		SetIdleMode(state, force: false);
	}

	private static void SetIdleMode(bool state, bool force)
	{
		if (global::Mirror.NetworkServer.active && (!state || !_pauseIdleMode || force) && (state != IdleModeActive || force) && (!state || IdleModeEnabled) && ServerStatic.IsDedicated)
		{
			if (state)
			{
				global::UnityEngine.Application.targetFrameRate = IdleModeTickrate;
				global::UnityEngine.Time.timeScale = 0.01f;
				ServerConsole.AddLog("Server has entered the idle mode.");
				ServerConsole.AddOutputEntry(default(global::ServerOutput.IdleEnterEntry));
			}
			else
			{
				global::UnityEngine.Application.targetFrameRate = ServerStatic.ServerTickrate;
				global::UnityEngine.Time.timeScale = 1f;
				ServerConsole.AddLog("Server has exited the idle mode.");
				ServerConsole.AddOutputEntry(default(global::ServerOutput.IdleExitEntry));
			}
			IdleModeActive = state;
		}
	}
}
