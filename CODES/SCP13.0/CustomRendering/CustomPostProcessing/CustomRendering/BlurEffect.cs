using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Blur Effect")]
    public sealed class BlurEffect : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        [Serializable]
        public sealed class BlurMethodParameter : VolumeParameter<BlurMethod> { }

        public enum BlurMethod { Gaussian = 0, Box = 1 }

        private static readonly int _BlurOffsetsId = Shader.PropertyToID("_BlurOffsets");
        private static readonly int _BlurredTexId = Shader.PropertyToID("_BlurredTex");

        public BlurMethodParameter Mode = new BlurMethodParameter { value = BlurMethod.Gaussian };
        public BoolParameter HighQuality = new BoolParameter(false);
        public ClampedFloatParameter Amount = new ClampedFloatParameter(0f, 0f, 1f);
        [Range(1, 12)]
        public ClampedIntParameter Iterations = new ClampedIntParameter(1, 1, 12);

        private RTHandle _blur1;
        private RTHandle _blur2;
        private Material _material;

        private const string ShaderName = "Hidden/Shader/BlurEffect";

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive() => _material != null && Amount.value > 0f;

        public override void Setup()
        {
            if (_material == null)
                _material = CoreUtils.CreateEngineMaterial(ShaderName);

            _blur1 = RTHandles.Alloc(Vector2.one, name: "_Blur1", colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat, useDynamicScale: true);
            _blur2 = RTHandles.Alloc(Vector2.one, name: "_Blur2", colorFormat: UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat, useDynamicScale: true);
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            int pass = (Mode.value == BlurMethod.Gaussian) ? 2 : 3;

            for (int i = 0; i < Iterations.value; i++)
            {
                float offset = Amount.value * (i + 1);
                cmd.SetGlobalVector(_BlurOffsetsId, new Vector4(offset, offset, 0, 0));

                HDUtils.BlitCameraTexture(cmd, source, _blur1, _material, pass);
                HDUtils.BlitCameraTexture(cmd, _blur1, source, _material, pass);
            }

            HDUtils.BlitCameraTexture(cmd, source, destination);
        }

        public override void Cleanup()
        {
            RTHandles.Release(_blur1);
            RTHandles.Release(_blur2);
            CoreUtils.Destroy(_material);
        }
    }
}