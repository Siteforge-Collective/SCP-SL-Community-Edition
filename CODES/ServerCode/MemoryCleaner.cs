public static class MemoryCleaner
{
	[global::UnityEngine.RuntimeInitializeOnLoadMethod]
	private static void OnLoad()
	{
		global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += CleanupMemory;
	}

	private static void CleanupMemory(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		global::UnityEngine.Resources.UnloadUnusedAssets();
		global::System.Threading.Thread thread = new global::System.Threading.Thread((global::System.Threading.ThreadStart)delegate
		{
			global::System.Runtime.GCSettings.LargeObjectHeapCompactionMode = global::System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
			global::System.GC.Collect();
			global::System.GC.WaitForPendingFinalizers();
			global::System.GC.Collect();
		});
		thread.IsBackground = true;
		thread.Start();
	}
}
