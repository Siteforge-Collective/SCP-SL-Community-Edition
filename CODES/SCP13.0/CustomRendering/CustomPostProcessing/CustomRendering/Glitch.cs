using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Glitch")]
    public sealed class Glitch : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderPath = "Hidden/Shader/Glitch";

        private readonly int _glitchId;
        private readonly int _noiseId;

        public ClampedFloatParameter Amount;
        public ClampedFloatParameter Noise;

        private Material _material;
        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null && Amount.value > 0f;
        }

        public override void Setup()
        {
            Shader shader = Shader.Find(ShaderPath);
            if (shader != null)
            {
                _material = new Material(shader);
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderPath}'. Post Process Volume Glitch is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            if (Amount != null)
            {
                _material.SetFloat(_glitchId, Amount.value);
            }

            if (Noise != null)
            {
                _material.SetFloat(_noiseId, Noise.value);
            }

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Glitch()
        {
            _glitchId = Shader.PropertyToID("_Glitch");
            _noiseId = Shader.PropertyToID("_Noise");

            Amount = new ClampedFloatParameter(0f, 0f, 1f);
            Noise = new ClampedFloatParameter(0f, 0f, 1f);
        }
    }
}