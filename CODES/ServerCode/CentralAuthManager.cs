public static class CentralAuthManager
{
	private static bool _initialized;

	public static global::GameCore.DistributionPlatform Platform { get; private set; }

	internal static void InitAuth()
	{
		if (!_initialized)
		{
			_initialized = true;
			CentralServer.Init();
			Platform = global::GameCore.DistributionPlatform.Dedicated;
			global::GameCore.Console.AddLog("Running as headless dedicated server. Skipping distribution platform detection.", new global::UnityEngine.Color32(0, byte.MaxValue, 0, byte.MaxValue));
		}
	}
}
