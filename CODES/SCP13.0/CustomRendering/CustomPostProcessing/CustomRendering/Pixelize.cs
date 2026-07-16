using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Pixelize")]
    public sealed class Pixelize : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Pixelize";

        private readonly int _amountId;

        [Tooltip("Controls the pixelation intensity.")]
        public ClampedFloatParameter Amount;

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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Pixelize is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            float pixelSize = Amount.value / 1.0f;

            _material.SetFloat(_amountId, pixelSize);
            _material.SetTexture("_InputTexture", source);

            HDUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Pixelize()
        {
            _amountId = Shader.PropertyToID("_Resolution");
            Amount = new ClampedFloatParameter(0f, 0f, 100f);
        }
    }
}