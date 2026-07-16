using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System;
using System.Collections.Generic;

namespace UserSettings.VideoSettings
{
    public static class LightingVideoSettings
    {
        private struct RoomLightInfo
        {
            public RoomLight Light;
            public HDAdditionalLightData Data;
            public bool CastsShadows;
        }

        private static readonly RoomLightInfo[] Lights = new RoomLightInfo[4096]; 
        private static int _lightsCount;
        private static bool _renderLights;
        private static bool _shadowsEnabled;
        private static ShadowResolution _shadowResolution;

        private const bool DefaultRenderLights = true;
        private const bool DefaultShadowsEnabled = true;
        private const int DefaultShadowResolution = 2;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            UserSetting<bool>.SetDefaultValue<LightingVideoSetting>(/*RenderLights*/ (LightingVideoSetting)2, DefaultRenderLights);
            UserSetting<bool>.AddListener<LightingVideoSetting>((LightingVideoSetting)2, val => {
                _renderLights = val;
                ForceUpdateAll();
            });

            UserSetting<bool>.SetDefaultValue<LightingVideoSetting>(/*ShadowsEnabled*/ 0, DefaultShadowsEnabled);
            UserSetting<bool>.AddListener<LightingVideoSetting>(0, val => {
                _shadowsEnabled = val;
                ForceUpdateAll();
            });

            UserSetting<int>.SetDefaultValue<LightingVideoSetting>(/*ShadowRes*/ (LightingVideoSetting)1, DefaultShadowResolution);
            UserSetting<int>.AddListener<LightingVideoSetting>((LightingVideoSetting)1, val => {
                _shadowResolution = (ShadowResolution)val;
                ForceUpdateAll();
            });

            MapGeneration.SeedSynchronizer.OnMapGenerated += OnMapGenerated;
        }

        private static void OnMapGenerated()
        {
            _lightsCount = 0;

            foreach (var controller in RoomLightController.Instances)
            {
                controller.LightsInRoom.ForEach(RegisterLight);
            }

            _renderLights = UserSetting<bool>.Get<LightingVideoSetting>((LightingVideoSetting)2);
            _shadowsEnabled = UserSetting<bool>.Get<LightingVideoSetting>(0);
            _shadowResolution = (ShadowResolution)UserSetting<int>.Get<LightingVideoSetting>((LightingVideoSetting)1);

            ForceUpdateAll();
        }

        private static void RegisterLight(RoomLight rl)
        {
            if (rl == null || !rl.HasLight) return;

            Light lightSource = rl.LightSource;
            if (lightSource.TryGetComponent<HDAdditionalLightData>(out var hdData))
            {
                Lights[_lightsCount++] = new RoomLightInfo
                {
                    Light = rl,
                    Data = hdData,
                    CastsShadows = lightSource.shadows != LightShadows.None
                };
            }
        }

        private static void ForceUpdateAll()
        {
            CustomCulling.CullingManager.AllLightsDisabled = !_renderLights;

            for (int i = 0; i < _lightsCount; i++)
            {
                UpdateInstance(Lights[i]);
            }
        }

        private static void UpdateInstance(RoomLightInfo info)
        {
            if (info.Light == null || info.Data == null) return;

            if (_shadowsEnabled && info.CastsShadows)
            {
                info.Data.EnableShadows(true);
                info.Data.SetShadowResolutionOverride(true);
                info.Data.SetShadowResolution((int)_shadowResolution);
            }
            else
            {
                info.Data.EnableShadows(false);
            }
        }
    }
}