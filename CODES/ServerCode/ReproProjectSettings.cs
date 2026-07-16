public class ReproProjectSettings : global::UnityEngine.ScriptableObject
{
	[global::System.Serializable]
	public struct InputItem
	{
		public ReproProjectAssetType AssetType;

		public string AssetPath;
	}

	public string ProjectName;

	public string ProjectPath;

	public bool OpenProject;

	public int TextureScale;

	public ReproProjectSettings.InputItem[] InputFiles;

	public ReproProjectSettings.InputItem[] ProjectFiles;
}
