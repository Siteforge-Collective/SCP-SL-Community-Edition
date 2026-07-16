using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UserSettings;

namespace CustomRendering
{
    public class ApplyPostProcessGraphicSettings : MonoBehaviour
    {
        private VolumeProfile _volumeProfile;

        private const float MinBrightness = -0.7f;
        private const float MaxBrightness = 0f;

        private void Start()
        {
            Volume volume = GetComponent<Volume>();
            if (volume == null)
            {
                Debug.LogWarning("[ApplyPostProcessGraphicSettings] No Volume component found on " + gameObject.name);
                enabled = false;
                return;
            }

            _volumeProfile = volume.profile;
            if (_volumeProfile == null)
            {
                Debug.LogWarning("[ApplyPostProcessGraphicSettings] Volume has no profile on " + gameObject.name);
                enabled = false;
                return;
            }

            // ── Brightness ────────────────────────────────────────────────
            float brightness = UserSetting<float>.Get(UserSettings.VideoSettings.MiscVideoSetting.Brightness);
            UpdateBrightness(brightness);
            UserSetting<float>.AddListener(UserSettings.VideoSettings.MiscVideoSetting.Brightness, UpdateBrightness);

            // ── Bloom ─────────────────────────────────────────────────────
            int bloomQuality = UserSetting<int>.Get(UserSettings.VideoSettings.PerformanceVideoSetting.BloomQuality);
            UpdateBloom(bloomQuality);
            UserSetting<int>.AddListener(UserSettings.VideoSettings.PerformanceVideoSetting.BloomQuality, UpdateBloom);

            // ── Ambient Occlusion ─────────────────────────────────────────
            int aoQuality = UserSetting<int>.Get(UserSettings.VideoSettings.PerformanceVideoSetting.AOQuality);
            UpdateAO(aoQuality);
            UserSetting<int>.AddListener(UserSettings.VideoSettings.PerformanceVideoSetting.AOQuality, UpdateAO);

            // ── Explosion Shake ───────────────────────────────────────────
            bool shakeEnabled = UserSetting<bool>.Get(UserSettings.VideoSettings.MiscVideoSetting.ExplosionShake);
            UpdateShake(shakeEnabled);
            UserSetting<bool>.AddListener(UserSettings.VideoSettings.MiscVideoSetting.ExplosionShake, UpdateShake);
        }

        private void OnDestroy()
        {
            UserSetting<float>.RemoveListener(UserSettings.VideoSettings.MiscVideoSetting.Brightness, UpdateBrightness);
            UserSetting<int>.RemoveListener(UserSettings.VideoSettings.PerformanceVideoSetting.BloomQuality, UpdateBloom);
            UserSetting<int>.RemoveListener(UserSettings.VideoSettings.PerformanceVideoSetting.AOQuality, UpdateAO);
            UserSetting<bool>.RemoveListener(UserSettings.VideoSettings.MiscVideoSetting.ExplosionShake, UpdateShake);
        }

        private void UpdateBrightness(float brightness)
        {
            if (_volumeProfile == null) return;
            if (_volumeProfile.TryGet<LiftGammaGain>(out var lgg))
            {
                float pos = brightness > 0f ? brightness : 0f;
                float neg = brightness < 0f ? -brightness : 0f;
                lgg.gain.value = new Vector4(pos, pos, pos, neg);
            }
        }

        private void UpdateBloom(int qualityLevel)
        {
            if (_volumeProfile == null) return;
            if (_volumeProfile.TryGet<Bloom>(out var bloom))
            {
                bloom.active = qualityLevel != 0;
                bloom.quality.value = qualityLevel - 1;
            }
        }

        private void UpdateAO(int qualityLevel)
        {
            if (_volumeProfile == null) return;
            if (_volumeProfile.TryGet<AmbientOcclusion>(out var ao))
            {
                ao.active = qualityLevel != 0;
                switch (qualityLevel - 1)
                {
                    case 0: ao.quality.value = (int)ScalableSettingLevelParameter.Level.Low; break;
                    case 1: ao.quality.value = (int)ScalableSettingLevelParameter.Level.Medium; break;
                    case 2: ao.quality.value = (int)ScalableSettingLevelParameter.Level.High; break;
                }
            }
        }

        private void UpdateShake(bool enable)
        {
            if (_volumeProfile == null) return;
            if (_volumeProfile.TryGet<CameraShake>(out var shake))
                shake.active = enable;
        }
    }
}