using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Kuwahara")]
    public sealed class Kuwahara : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum KuwaharaMode
        {
            FullScreen = 0,
            DepthFade = 1
        }

        [Serializable]
        public sealed class KuwaharaModeParam : VolumeParameter<KuwaharaMode>
        {
            public KuwaharaModeParam() : base(KuwaharaMode.FullScreen, false) { }
        }

        private const string ShaderName = "Hidden/Shader/Kuwahara";

        private readonly int _radiusId;

        private readonly int _fadeParamsId;

        [Tooltip("Choose to apply the effect to the entire screen, or fade in/out over a distance")]
        public KuwaharaModeParam Mode;

        public ClampedIntParameter Radius;

        public FloatParameter StartFadeDistance;

        public FloatParameter EndFadeDistance;

        private Material _material;

        private int _currentMode;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            if (_material == null)
                return false;

            return Radius.value > 0;
        }

        public override void Setup()
        {
            Shader shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                _material = new Material(shader);
            }
            else
            {
                Debug.LogError("Unable to find shader 'Hidden/Shader/Kuwahara'. Post Process Volume Kuwahara is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _currentMode = (int)Mode.value;

            if (camera.camera.orthographic)
            {
                _currentMode = 0;
            }

            _material.SetInt(_radiusId, Radius.value);

            if (_currentMode == 1)
            {
                _material.SetVector(_fadeParamsId, new Vector4(StartFadeDistance.value, EndFadeDistance.value, 0, 0));
            }

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, _currentMode);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Kuwahara()
        {
            _radiusId = Shader.PropertyToID("_Radius");
            _fadeParamsId = Shader.PropertyToID("_FadeParams");

            Mode = new KuwaharaModeParam();
            Radius = new ClampedIntParameter(0, 0, 8); 

            StartFadeDistance = new FloatParameter(0f);
            EndFadeDistance = new FloatParameter(0f);
        }
    }
}