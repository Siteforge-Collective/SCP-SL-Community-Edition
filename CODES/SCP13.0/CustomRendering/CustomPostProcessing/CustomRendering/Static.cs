using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Static")]
    public sealed class Static : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Static";

        private readonly int _fadeId;
        private readonly int _fadeAdditiveId;
        private readonly int _fadeDistortionId;
        private readonly int _staticTexId;

        [Tooltip("Controls the standard fade intensity.")]
        public ClampedFloatParameter Fade;

        [Tooltip("Controls the additive noise intensity.")]
        public ClampedFloatParameter FadeAdditive;

        [Tooltip("Controls the distortion intensity.")]
        public ClampedFloatParameter FadeDistortion;

        [Tooltip("The noise texture used for the static effect.")]
        public TextureParameter StaticTexture;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public Static()
        {
            _fadeId = Shader.PropertyToID("_Fade");
            _fadeAdditiveId = Shader.PropertyToID("_FadeAdditive");
            _fadeDistortionId = Shader.PropertyToID("_FadeDistortion");
            _staticTexId = Shader.PropertyToID("_StaticTex");

            Fade = new ClampedFloatParameter(0f, 0f, 1f);
            FadeAdditive = new ClampedFloatParameter(0f, 0f, 1f);
            FadeDistortion = new ClampedFloatParameter(0f, 0f, 1f);
            StaticTexture = new TextureParameter(null);
        }

        public bool IsActive()
        {
            return _material != null
                   && StaticTexture.value != null
                   && (Fade.value > 0f || FadeAdditive.value > 0f || FadeDistortion.value > 0f);
        }

        public override void Setup()
        {
            if (Shader.Find(ShaderName) != null)
            {
                _material = new Material(Shader.Find(ShaderName));
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Static is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_fadeId, Fade.value);
            _material.SetFloat(_fadeAdditiveId, FadeAdditive.value);
            _material.SetFloat(_fadeDistortionId, FadeDistortion.value);
            _material.SetTexture(_staticTexId, StaticTexture.value);

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}