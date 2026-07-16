using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Radial Blur")]
    public sealed class RadialBlur : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Radial Blur";

        private readonly int _amountId;
        private readonly int _iterationsId;

        [Tooltip("Controls the strength of the blur.")]
        public ClampedFloatParameter Amount;

        [Tooltip("Number of samples taken for the blur effect.")]
        public ClampedIntParameter Iterations;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null && Amount.value > 0f;
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

            float blurValue = Amount.value / 1.0f;
            _material.SetFloat(_amountId, blurValue);

            _material.SetFloat(_iterationsId, (float)Iterations.value);

            HDUtils.DrawFullScreen(cmd, _material, destination);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public RadialBlur()
        {
            _amountId = Shader.PropertyToID("_Amount");
            _iterationsId = Shader.PropertyToID("_Iterations");

            Amount = new ClampedFloatParameter(0f, 0f, 100f);
            Iterations = new ClampedIntParameter(6, 3, 12);
        }
    }
}