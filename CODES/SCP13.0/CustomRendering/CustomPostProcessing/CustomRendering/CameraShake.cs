using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/CameraShake")]
    public sealed class CameraShake : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/CameraShake";

        private readonly int _jitterJumpId;   
        private readonly int _colorShakeId; 

        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter ScanLineJitter; 
        public ClampedFloatParameter VerticalJump;   
        public ClampedFloatParameter HorizontalShake; 
        public ClampedFloatParameter ColorDrift;     

        private Material _material; 
        private float _verticalJumpTime; 

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public CameraShake()
        {
            _jitterJumpId = Shader.PropertyToID("_JitterJump");
            _colorShakeId = Shader.PropertyToID("_ColorShake");

            ScanLineJitter = new ClampedFloatParameter(0f, 0f, 1f);
            VerticalJump = new ClampedFloatParameter(0f, 0f, 1f);
            HorizontalShake = new ClampedFloatParameter(0f, 0f, 1f);
            ColorDrift = new ClampedFloatParameter(0f, 0f, 1f);
        }

        public bool IsActive()
        {
            return _material != null && (
                ScanLineJitter.value > 0f ||
                VerticalJump.value > 0f ||
                HorizontalShake.value > 0f ||
                ColorDrift.value > 0f
            );
        }

        public override void Setup()
        {
            if (_material == null)
            {
                _material = CoreUtils.CreateEngineMaterial(ShaderName);
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _verticalJumpTime += Time.deltaTime * VerticalJump.value * 11.3f;

            Vector4 jitterJump = new Vector4(
                VerticalJump.value,
                ScanLineJitter.value,
                HorizontalShake.value,
                _verticalJumpTime
            );
            _material.SetVector(_jitterJumpId, jitterJump);

            Vector4 colorShake = new Vector4(
                Time.time * 1.2f, 
                ColorDrift.value,
                0f,
                0f
            );

            _material.SetVector(_colorShakeId, colorShake);
            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}