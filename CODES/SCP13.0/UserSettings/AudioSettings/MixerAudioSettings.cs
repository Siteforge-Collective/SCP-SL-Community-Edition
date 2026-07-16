using UnityEngine;
using UnityEngine.Audio;

namespace UserSettings.AudioSettings
{
    public class MixerAudioSettings : MonoBehaviour
    {
        private class AudioSlider
        {
            private readonly VolumeSliderSetting _setting;
            private readonly string _floatName;

            private AudioMixer _mixer;
            private bool _mixerSet;

            public AudioSlider(VolumeSliderSetting setting, string floatName)
            {
                _setting = setting;
                _floatName = floatName;

                UserSetting<float>.SetDefaultValue(_setting, DefaultSliderVal);
                UserSetting<float>.AddListener(_setting, SetFloat);
            }

            public void SetMixer(AudioMixer mixer)
            {
                _mixer = mixer;
                _mixerSet = true;

                float currentVol = UserSetting<float>.Get(_setting);
                SetFloat(currentVol);
            }

            private void SetFloat(float vol)
            {
                if (!_mixerSet) return;

                float clamped = Mathf.Clamp01(vol);

                float db = (clamped > 0f) ? 20f * Mathf.Log10(clamped) : -80f;

                _mixer.SetFloat(_floatName, db);
            }
        }

        public enum VolumeSliderSetting
        {
            Master = 0,
            VoiceChat = 1,
            SoundEffects = 2,
            MenuMusic = 3,
            MenuUI = 4
        }

        [SerializeField] private AudioMixer _mixer;

        private const float DefaultSliderVal = 0.7f;

        private static readonly AudioSlider[] Sliders = new AudioSlider[5]
            {
                new AudioSlider(VolumeSliderSetting.Master, "MasterVolume"),
                new AudioSlider(VolumeSliderSetting.VoiceChat, "VoiceChatVolume"),
                new AudioSlider(VolumeSliderSetting.SoundEffects, "SoundEffectsVolume"),
                new AudioSlider(VolumeSliderSetting.MenuMusic, "MenuMusicVolume"),
                new AudioSlider(VolumeSliderSetting.MenuUI, "MenuUIVolume")
            };

        private void Start()
        {
            foreach (var slider in Sliders)
            {
                slider.SetMixer(_mixer);
            }
        }
    }
}