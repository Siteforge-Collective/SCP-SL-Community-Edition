using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/SpeedLines")]
    public sealed class SpeedLines : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/SpeedLines";

        private readonly int _paramsId;
        private readonly int _noiseTexId;

        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter Intensity;

        [Tooltip("Assign any grayscale texture with a vertically repeating pattern and a falloff from left to right")]
        public TextureParameter NoiseTex;

        [Tooltip("Determines the radial tiling of the noise texture")]
        public ClampedFloatParameter Size;

        public ClampedFloatParameter Falloff;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null &&
                   NoiseTex.value != null &&
                   Intensity.value > 0f;
        }

        public override void Setup()
        {
            Shader shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                _material = CoreUtils.CreateEngineMaterial(shader);
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume SpeedLines is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            float falloffVal = Falloff.value + 1.0f; 
            float intensityVal = Intensity.value * 2.0f;

            _material.SetVector(_paramsId, new Vector4(Size.value, falloffVal, intensityVal, 0f));

            if (NoiseTex.value != null)
            {
                _material.SetTexture(_noiseTexId, NoiseTex.value);
            }

            HDUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public SpeedLines()
        {
            _paramsId = Shader.PropertyToID("_Params");
            _noiseTexId = Shader.PropertyToID("_NoiseTex");

            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
            NoiseTex = new TextureParameter(null);
            Size = new ClampedFloatParameter(0.5f, 0f, 1f); 
            Falloff = new ClampedFloatParameter(0.5f, 0f, 1f);
        }
    }
}