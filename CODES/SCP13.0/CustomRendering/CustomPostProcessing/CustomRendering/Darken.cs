using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Darken")]
    public sealed class Darken : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Darken";

        private readonly int _intensityId; 
        public ClampedFloatParameter Intensity; 

        private Material _material; 
        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public Darken()
        {
            _intensityId = Shader.PropertyToID("_DarknessIntensity");
            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
        }

        public bool IsActive()
        {
            return _material != null && Intensity.value > 0f;
        }

        public override void Setup()
        {
            if (_material == null)
            {
                Shader shader = Shader.Find(ShaderName);
                if (shader != null)
                {
                    _material = new Material(shader);
                }
                else
                {
                    Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Glitch is unable to load.");
                }
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_intensityId, Intensity.value);
            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}