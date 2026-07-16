public class VeryHighPerformance : global::UnityEngine.MonoBehaviour
{
	public static readonly global::UnityEngine.Color NormalColor = new global::UnityEngine.Color(0.01f, 0.01f, 0.01f, 1f);

	public static readonly global::UnityEngine.Color VHColor = new global::UnityEngine.Color(0.22f, 0.22f, 0.22f, 1f);

	public static bool LightsOff { get; private set; }

	public static global::UnityEngine.Color TargetColor
	{
		get
		{
			if (!LightsOff)
			{
				return NormalColor;
			}
			return VHColor;
		}
	}

	private void Start()
	{
		LightsOff = !PlayerPrefsSl.Get(SettingsOption.gfxsets_RenderLight.ToString(), defaultValue: true) || ServerStatic.IsDedicated;
	}
}
