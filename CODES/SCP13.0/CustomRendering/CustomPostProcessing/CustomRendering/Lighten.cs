using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Lighten")]
    public sealed class Lighten : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Lighten";

        private readonly int _opacityId;
        private readonly int _brightnessId;

        public ClampedFloatParameter Brightness;
        public ClampedFloatParameter Opacity;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public Lighten()
        {
            _opacityId = Shader.PropertyToID("_Opacity");
            _brightnessId = Shader.PropertyToID("_Brightness");

            Brightness = new ClampedFloatParameter(0f, 0f, 1f);
            Opacity = new ClampedFloatParameter(1f, 0f, 1f);
        }

        public override void Setup()
        {
            var shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                _material = new Material(shader);
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Glitch is unable to load.");
            }
        }

        public bool IsActive()
        {
            return _material != null && Opacity.value > 0f;
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_opacityId, Opacity.value);
            _material.SetFloat(_brightnessId, Brightness.value);

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}