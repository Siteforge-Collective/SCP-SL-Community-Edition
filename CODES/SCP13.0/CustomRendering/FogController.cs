using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    public class FogController : MonoBehaviour
    {
        public static FogController Singleton;

        private static readonly int HashGlobalFogColor = Shader.PropertyToID("_GlobalFogColor");

        private static readonly int HashGlobalFogDistance = Shader.PropertyToID("_GlobalFogDistance");

        private const FogType DefaultFog = FogType.Inside;

        private FogSetting[] _fogSettings;

        private FogType? _fogType;

        public CustomFog FogEffect { get; private set; }

        public FogType? ForcedFog
        {
            get
            {
                return _fogType;
            }
            set
            {
                if (value.HasValue)
                {
                    FogEffect.enabled = true;
                    _fogType = value.Value;
                }
                else
                {
                    FogEffect.enabled = false;
                    _fogType = null;
                }
            }
        }

        public static void EnableFogType(FogType fogType, float seconds = 0f)
        {
            if (Singleton != null)
            {
                FogSetting setting = Singleton.GetFogSetting(fogType);
                if (setting != null)
                {
                    setting.IsEnabled = true;
                    setting.BlendTime = seconds;
                }
            }
        }

        public static void DisableFogType(FogType fogType, float seconds = 0f)
        {
            if (Singleton != null)
            {
                FogSetting setting = Singleton.GetFogSetting(fogType);
                if (setting != null)
                {
                    setting.IsEnabled = false;
                    setting.BlendTime = seconds;
                }
            }
        }

        public FogSetting GetFogSetting(FogType fogType)
        {
            return _fogSettings.FirstOrDefault(s => s.FogType == fogType);
        }

        private void Awake()
        {
            Singleton = this;
            _fogSettings = FindObjectsOfType<FogSetting>();
            Array.Sort(_fogSettings, (x, y) => x.Priority.CompareTo(y.Priority));
        }

        private void Start()
        {
            if (TryGetComponent<CustomPassVolume>(out var volume))
            {
                foreach (var pass in volume.customPasses)
                {
                    if (pass is CustomFog fogPass)
                    {
                        FogEffect = fogPass;
                        break;
                    }
                }
            }
        }

        private void OnDisable()
        {
            Singleton = null;
        }

        private void Update()
        {
            bool dirty = false;
            foreach (var setting in _fogSettings)
            {
                setting.UpdateWeight();
                if (setting._isDirty)
                {
                    dirty = true;
                }
            }

            if (dirty || _fogType.HasValue)
            {
                Color color = Color.black;
                float startDistance = 0f;
                float endDistance = 1000f;
                float totalWeight = 0f;

                if (_fogType.HasValue)
                {
                    FogSetting forcedSetting = GetFogSetting(_fogType.Value);
                    if (forcedSetting != null)
                    {
                        color = forcedSetting.Color;
                        startDistance = forcedSetting.StartDistance;
                        endDistance = forcedSetting.EndDistance;
                    }
                }
                else
                {
                    foreach (var setting in _fogSettings)
                    {
                        if (setting.IsEnabled)
                        {
                            float weight = setting.Weight;
                            color += setting.Color * weight;
                            startDistance += setting.StartDistance * weight;
                            endDistance += setting.EndDistance * weight;
                            totalWeight += weight;
                        }
                    }
                    if (totalWeight > 0f)
                    {
                        color /= totalWeight;
                        startDistance /= totalWeight;
                        endDistance /= totalWeight;
                    }
                    else
                    {
                        FogSetting defaultSetting = GetFogSetting(DefaultFog);
                        if (defaultSetting != null)
                        {
                            color = defaultSetting.Color;
                            startDistance = defaultSetting.StartDistance;
                            endDistance = defaultSetting.EndDistance;
                        }
                    }
                }

                FogEffect.FogColor = color;
                FogEffect.StartDistance = startDistance;
                FogEffect.EndDistance = endDistance;

                Shader.SetGlobalVector(HashGlobalFogColor, color);
                Shader.SetGlobalVector(HashGlobalFogDistance, new Vector4(startDistance, endDistance, 0f, 0f));
            }
        }
    }
}