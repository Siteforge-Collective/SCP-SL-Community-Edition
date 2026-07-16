public class NonFacilityCompatibility : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public class SceneDescription
	{
		public enum VoiceChatSupportMode
		{
			Unsupported = 0,
			WithoutIntercom = 1,
			FullySupported = 2
		}

		public string sceneName;

		public NonFacilityCompatibility.SceneDescription.VoiceChatSupportMode voiceChatSupport = NonFacilityCompatibility.SceneDescription.VoiceChatSupportMode.FullySupported;

		public bool enableWorldGeneration = true;

		public bool enableRespawning = true;

		public bool enableStandardGamplayItems = true;

		public bool roundAutostart;

		public global::UnityEngine.Vector3 constantRespawnPoint = global::UnityEngine.Vector3.zero;

		public global::PlayerRoles.RoleTypeId forcedClass = global::PlayerRoles.RoleTypeId.None;
	}

	public NonFacilityCompatibility.SceneDescription[] allScenes;

	public static NonFacilityCompatibility singleton;

	public static NonFacilityCompatibility.SceneDescription currentSceneSettings;

	private void Awake()
	{
		singleton = this;
		global::UnityEngine.SceneManagement.SceneManager.sceneLoaded += RefreshDescription;
	}

	private void OnDestroy()
	{
		global::UnityEngine.SceneManagement.SceneManager.sceneLoaded -= RefreshDescription;
	}

	public static void RefreshDescription(global::UnityEngine.SceneManagement.Scene scene, global::UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		NonFacilityCompatibility.SceneDescription[] array = singleton.allScenes;
		foreach (NonFacilityCompatibility.SceneDescription sceneDescription in array)
		{
			if (sceneDescription.sceneName == scene.name)
			{
				currentSceneSettings = sceneDescription;
			}
		}
	}
}
