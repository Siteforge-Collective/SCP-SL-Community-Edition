namespace GameCore
{
	[global::System.Serializable]
	public class CommandHint
	{
		public string name;

		public string shortDesc;

		[global::UnityEngine.Multiline]
		public string fullDesc;
	}
}
