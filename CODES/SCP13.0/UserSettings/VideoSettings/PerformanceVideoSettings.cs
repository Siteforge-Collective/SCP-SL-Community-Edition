using UnityEngine;

namespace UserSettings.VideoSettings
{
    public static class PerformanceVideoSettings
    {
        // 0: Full, 1: Half, 2: Quarter, 3: Eighth
        private static readonly int[] TextureQualityPresets = new int[4] { 3, 2, 1, 0 };

        private static void ApplyTextureResolution(int presetIndex)
        {
            int limit = TextureQualityPresets[Mathf.Clamp(presetIndex, 0, TextureQualityPresets.Length - 1)];
            QualitySettings.masterTextureLimit = limit;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            SetDefaultValues();
            UserSetting<int>.AddListener(PerformanceVideoSetting.TextureResolution, ApplyTextureResolution);
            ApplyAll();
        }

        private static void SetDefaultValues()
        {
            UserSetting<int>.SetDefaultValue(PerformanceVideoSetting.AntiAliasingType, 1);
            UserSetting<int>.SetDefaultValue(PerformanceVideoSetting.AntiAliasingQuality, 1);
            UserSetting<float>.SetDefaultValue(PerformanceVideoSetting.RagdollFreeze, 30f);
            UserSetting<int>.SetDefaultValue(PerformanceVideoSetting.BloomQuality, 1);
            UserSetting<int>.SetDefaultValue(PerformanceVideoSetting.AOQuality, 2);
            UserSetting<int>.SetDefaultValue(PerformanceVideoSetting.TextureResolution, 3);
            UserSetting<bool>.SetDefaultValue(PerformanceVideoSetting.BloodDecalsEnabled, true);
            UserSetting<bool>.SetDefaultValue(PerformanceVideoSetting.BulletDecalsEnabled, true);
        }

        private static void ApplyAll()
        {
            ApplyTextureResolution(UserSetting<int>.Get(PerformanceVideoSetting.TextureResolution));
        }
    }
}