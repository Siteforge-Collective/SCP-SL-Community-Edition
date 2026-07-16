public class FullscreenToggle : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.GameObject checkmark;

	public bool isOn;

	private void OnEnable()
	{
		isOn = PlayerPrefsSl.Get("SavedFullscreen", defaultValue: true);
		checkmark.SetActive(isOn);
	}

	public void Click()
	{
		isOn = !isOn;
		checkmark.SetActive(isOn);
		PlayerPrefsSl.Set("SavedFullscreen", isOn);
		ResolutionManager.ChangeFullscreen(isOn);
	}
}
