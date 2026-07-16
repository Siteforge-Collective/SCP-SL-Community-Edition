using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Sharpen")]
    public sealed class Sharpen : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Sharpen";

        private readonly int _amountId;
        private readonly int _radiusId;

        [Tooltip("Controls the intensity of the sharpening effect.")]
        public ClampedFloatParameter Amount;

        [Tooltip("The sampling radius used to calculate edge enhancement.")]
        public ClampedFloatParameter Radius;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.BeforePostProcess;

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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Sharpen is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_amountId, Amount.value);
            _material.SetFloat(_radiusId, Radius.value);

            HDUtils.DrawFullScreen(cmd, _material, destination);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Sharpen()
        {
            _amountId = Shader.PropertyToID("_Amount");
            _radiusId = Shader.PropertyToID("_Radius");

            Amount = new ClampedFloatParameter(0f, 0f, 1f);
            Radius = new ClampedFloatParameter(0f, 0f, 1f);
        }
    }
}