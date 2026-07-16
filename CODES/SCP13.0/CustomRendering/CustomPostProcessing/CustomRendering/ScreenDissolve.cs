using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/ScreenDissolve")]
    public sealed class ScreenDissolve : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/ScreenDissolve";

        private readonly int _dissolveAmountId;
        private readonly int _blendTexId;
        private readonly int _overlayTexId;

        [Tooltip("Controls the intensity of the effect.")]
        public ClampedFloatParameter DissolveAmount;

        [Tooltip("The noise or transition texture used to calculate the dissolve pattern.")]
        public TextureParameter BlendTexture;

        [Tooltip("The texture that appears as the screen dissolves.")]
        public TextureParameter OverlayTexture;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null &&
                   BlendTexture.value != null &&
                   DissolveAmount.value > 0f;
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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume ScreenDissolve is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_dissolveAmountId, DissolveAmount.value);

            _material.SetTexture(_blendTexId, BlendTexture.value);

            Texture overlay = OverlayTexture.value;
            if (overlay == null)
            {
                overlay = Texture2D.blackTexture;
            }
            _material.SetTexture(_overlayTexId, overlay);

            HDUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public ScreenDissolve()
        {
            _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
            _blendTexId = Shader.PropertyToID("_BlendTex");
            _overlayTexId = Shader.PropertyToID("_OverlayTex");

            DissolveAmount = new ClampedFloatParameter(0f, 0f, 1f);
            BlendTexture = new TextureParameter(null);
            OverlayTexture = new TextureParameter(null);
        }
    }
}