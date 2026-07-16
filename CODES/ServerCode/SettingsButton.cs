public class SettingsButton : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject[] Tabs;

	private void Start()
	{
		global::UnityEngine.QualitySettings.SetQualityLevel(4);
		base.gameObject.SetActive(value: false);
	}

	public void ChangeTab(int id)
	{
	}
}
