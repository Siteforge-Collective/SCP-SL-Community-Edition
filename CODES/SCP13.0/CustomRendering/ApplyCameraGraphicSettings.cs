using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UserSettings;
using static UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData;

namespace CustomRendering
{
    public class ApplyCameraGraphicSettings : MonoBehaviour
    {
        private enum AntiAliasingType
        {
            Disabled = 0,
            FXAA = 1,
            SMAA = 2
        }
        [Header("Settings")]
        [SerializeField]
        private int _defaultAAType;

        [SerializeField]
        private int _defaultAAQuality;

        [SerializeField] 
        private HDAdditionalCameraData _hdCamera;
        private void Start()
        {
            if (_hdCamera == null)
            {
                _hdCamera = GetComponent<HDAdditionalCameraData>();
                if (_hdCamera == null)
                {
                    Debug.LogError($"[ApplyCameraGraphicSettings] _hdCamera не назначен на {gameObject.name}!", this);
                    return;
                }
            }

            UserSetting<int>.AddListener(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingType, UpdateAAType);
            UserSetting<int>.AddListener(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingQuality, UpdateAAQuality);

            UpdateAAType(UserSetting<int>.Get(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingType));
            UpdateAAQuality(UserSetting<int>.Get(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingQuality));
        }

        private void OnDestroy()
        {
            UserSetting<int>.RemoveListener(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingType, UpdateAAType);
            UserSetting<int>.RemoveListener(UserSettings.VideoSettings.PerformanceVideoSetting.AntiAliasingQuality, UpdateAAQuality);
        }

        private void UpdateAAType(int type)
        {
            if (_hdCamera == null) return;
            _hdCamera.antialiasing = (type == 1) ? AntialiasingMode.FastApproximateAntialiasing : AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        }

        private void UpdateAAQuality(int quality)
        {
            _hdCamera.SMAAQuality = (SMAAQualityLevel)quality;
        }
    }
}
