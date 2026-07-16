public class ClassPresetChooser : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public class PickerPreset
	{
		public string classID;

		public global::UnityEngine.Texture icon;

		public int health;

		public float wSpeed;

		public float rSpeed;

		public float stamina;

		public global::UnityEngine.Texture[] startingItems;

		public string en_additionalInfo;

		public string pl_additionalInfo;
	}

	public global::UnityEngine.GameObject bottomMenuItem;

	public global::UnityEngine.Transform bottomMenuHolder;

	public ClassPresetChooser.PickerPreset[] presets;

	private string curKey;

	private global::System.Collections.Generic.List<ClassPresetChooser.PickerPreset> curPresets = new global::System.Collections.Generic.List<ClassPresetChooser.PickerPreset>();

	private void Update()
	{
	}
}
