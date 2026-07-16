public class UBER_MaterialPresetCollection : global::UnityEngine.ScriptableObject
{
	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	public string currentPresetName;

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	public UBER_PresetParamSection whatToRestore;

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	public UBER_MaterialPreset[] matPresets;

	[global::UnityEngine.SerializeField]
	[global::UnityEngine.HideInInspector]
	public string[] names;
}
