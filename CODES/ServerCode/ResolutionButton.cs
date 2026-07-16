public class ResolutionButton : global::UnityEngine.MonoBehaviour
{
	private int cachedResolutionPreset;

	private int resolutionPreset;

	private global::UnityEngine.UI.Text txt;

	public void Click(int id)
	{
		ResolutionManager.ChangeResolution(id);
	}

	protected void Start()
	{
		txt = GetComponent<global::UnityEngine.UI.Text>();
		if (txt != null)
		{
			txt.text = ResolutionManager.CurrentResolutionString();
		}
		cachedResolutionPreset = ResolutionManager.Preset;
	}

	protected void Update()
	{
		resolutionPreset = ResolutionManager.Preset;
		if (cachedResolutionPreset != resolutionPreset && txt != null)
		{
			txt.text = ResolutionManager.CurrentResolutionString();
		}
		cachedResolutionPreset = resolutionPreset;
	}
}
