public static class Shutdown
{
	private static bool _quitting;

	public static event global::System.Action OnQuit;

	public static void Quit(bool quit = true)
	{
		if (!_quitting)
		{
			_quitting = true;
			Shutdown.OnQuit?.Invoke();
			ServerShutdown.Shutdown();
			CentralServer.Abort = true;
			InternalShutdown(quit);
		}
	}

	private static async void InternalShutdown(bool quit)
	{
		for (int i = 0; (i < 20 && ServerShutdown.ShutdownState != ServerShutdown.ServerShutdownState.Complete) || i < 6; i++)
		{
			await global::System.Threading.Tasks.Task.Delay(100);
		}
		if (quit)
		{
			global::UnityEngine.Application.Quit();
		}
	}
}
