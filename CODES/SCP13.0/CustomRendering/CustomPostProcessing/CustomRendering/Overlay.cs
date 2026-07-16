using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Overlay")]
    public sealed class Overlay : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum BlendModeType
        {
            Linear = 0,
            Additive = 1,
            Multiply = 2,
            Screen = 3
        }

        [Serializable]
        public sealed class BlendModeParameter : VolumeParameter<BlendModeType>
        {
            public BlendModeParameter(BlendModeType value, bool overrideState = false) : base(value, overrideState) { }
        }

        private const string ShaderName = "Hidden/Shader/Overlay";

        private readonly int _overlayTexId;
        private readonly int _paramsId;

        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter Intensity;

        [Tooltip("The texture's alpha channel controls its opacity")]
        public TextureParameter OverlayTex;

        [Tooltip("Maintains the image aspect ratio, regardless of the screen width")]
        public BoolParameter autoAspect;

        [Tooltip("Blends the gradient through various Photoshop-like blending modes")]
        public BlendModeParameter BlendMode;

        public ClampedFloatParameter Tiling;

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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Overlay is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
             if (_material == null) return; 
            _material.SetTexture(_overlayTexId, OverlayTex.value);

            float aspect = autoAspect.value ? (float)camera.actualWidth / camera.actualHeight : 1f;

            Vector4 parameters = new Vector4(
                Intensity.value,
                Tiling.value,
                (float)BlendMode.value,
                aspect
            );
            _material.SetVector(_paramsId, parameters);

            CoreUtils.DrawFullScreen(cmd, _material, destination);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Overlay()
        {
            _overlayTexId = Shader.PropertyToID("_OverlayTex");
            _paramsId = Shader.PropertyToID("_Params");

            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
            OverlayTex = new TextureParameter(null);
            autoAspect = new BoolParameter(false);
            BlendMode = new BlendModeParameter(BlendModeType.Linear);
            Tiling = new ClampedFloatParameter(1f, 0f, 10f);
        }
    }
}