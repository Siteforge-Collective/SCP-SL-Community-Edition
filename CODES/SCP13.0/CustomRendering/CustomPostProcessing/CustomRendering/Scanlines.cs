using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Scanlines")]
    public sealed class Scanlines : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Scanlines";

        private readonly int _paramsId;

        [Tooltip("Controls the overall visibility of the scanline effect.")]
        public ClampedFloatParameter Intensity;

        [Tooltip("Controls the density/number of lines on screen.")]
        public ClampedFloatParameter Amount;

        [Tooltip("Controls the scroll speed of the lines.")]
        public ClampedFloatParameter Speed;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null && Intensity.value > 0f;
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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Glitch is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            float amountScaled = Amount.value / 1.0f; 
            float speedScaled = Speed.value * 1.0f;  

            Vector4 shaderParams = new Vector4(
                Intensity.value,
                amountScaled,
                speedScaled,
                0f
            );

            _material.SetVector(_paramsId, shaderParams);

            HDUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Scanlines()
        {
            _paramsId = Shader.PropertyToID("_Params");

            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
            Amount = new ClampedFloatParameter(10f, 0f, 20f);
            Speed = new ClampedFloatParameter(0f, 0f, 1f);
        }
    }
}