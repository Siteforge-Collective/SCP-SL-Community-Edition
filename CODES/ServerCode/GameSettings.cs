public class GameSettings : global::UnityEngine.MonoBehaviour
{
	[global::System.Serializable]
	public struct GameSettingsAudioSlider
	{
		public string Key;

		public global::UnityEngine.UI.Slider SliderReference;

		public global::UnityEngine.UI.Text OptionalText;

		[global::UnityEngine.Range(0f, 1f)]
		public float DefaultValue;

		public void SaveState()
		{
			PlayerPrefsSl.Set(Key, SliderReference.value);
		}

		public void LoadState()
		{
			global::UnityEngine.UI.Slider refer = SliderReference;
			refer.value = PlayerPrefsSl.Get(Key, DefaultValue);
			SliderReference.onValueChanged.AddListener(delegate
			{
				singleton.OnSliderValueChange(refer);
			});
			singleton.OnSliderValueChange(refer);
		}
	}

	public global::UnityEngine.Audio.AudioMixer MasterAudioMixer;

	public static GameSettings singleton;

	public GameSettings.GameSettingsAudioSlider[] AudioSliders;

	private void Awake()
	{
		singleton = this;
		GameSettings.GameSettingsAudioSlider[] audioSliders = AudioSliders;
		foreach (GameSettings.GameSettingsAudioSlider gameSettingsAudioSlider in audioSliders)
		{
			gameSettingsAudioSlider.LoadState();
		}
	}

	private void OnSliderValueChange(global::UnityEngine.UI.Slider activator)
	{
		GameSettings.GameSettingsAudioSlider[] audioSliders = AudioSliders;
		for (int i = 0; i < audioSliders.Length; i++)
		{
			GameSettings.GameSettingsAudioSlider gameSettingsAudioSlider = audioSliders[i];
			if (gameSettingsAudioSlider.SliderReference == activator)
			{
				float value = activator.value;
				if (gameSettingsAudioSlider.OptionalText != null)
				{
					gameSettingsAudioSlider.OptionalText.text = global::UnityEngine.Mathf.RoundToInt(value * 100f) + " %";
				}
				float value2 = ((value == 0f) ? (-144f) : (20f * global::UnityEngine.Mathf.Log10(value)));
				MasterAudioMixer.SetFloat(gameSettingsAudioSlider.Key, value2);
				if (gameSettingsAudioSlider.Key == "SFX")
				{
					MasterAudioMixer.SetFloat("No Ducking", value2);
				}
				gameSettingsAudioSlider.SaveState();
			}
		}
	}
}
