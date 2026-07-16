using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/DoubleVision")]
    public sealed class DoubleVision : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum ModeType
        {
            FullScreen = 0,
            Edges = 1
        }

        [Serializable]
        public sealed class DoubleVisionMode : VolumeParameter<ModeType>
        {
            public DoubleVisionMode(ModeType value, bool overrideState = false) : base(value, overrideState) { }
        }

        private const string ShaderName = "Hidden/Shader/DoubleVision";

        private readonly int _amountId = Shader.PropertyToID("_Amount");

        [Tooltip("Choose to apply the effect over the entire screen or just the edges")]
        public DoubleVisionMode Mode;

        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter Intensity;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            if (_material == null) return false;
            return Intensity != null && Intensity.value > 0f;
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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume DoubleVision is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            float finalAmount = Intensity.value * 0.1f; 
            _material.SetFloat(_amountId, finalAmount);
            int pass = (int)Mode.value;

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, pass);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public DoubleVision()
        {
            Mode = new DoubleVisionMode(ModeType.FullScreen);
            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
        }
    }
}