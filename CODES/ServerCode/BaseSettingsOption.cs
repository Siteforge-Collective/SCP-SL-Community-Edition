[global::System.Serializable]
public class BaseSettingsOption
{
	public SettingsOption SettingOption;

	[global::UnityEngine.Tooltip("Will disable this option if lighting is disabled.")]
	public bool DependsOnLight;

	[global::UnityEngine.Tooltip("Will disable this option if shadows are disabled.")]
	public bool DependsOnShadows;

	public global::UnityEngine.GameObject GetRootObject => UIelementGameObject.transform.parent.gameObject;

	protected virtual global::UnityEngine.GameObject UIelementGameObject { get; }
}
