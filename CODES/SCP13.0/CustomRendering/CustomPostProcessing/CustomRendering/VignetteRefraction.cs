using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/VignetteRefraction")]
    public sealed class VignetteRefraction : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/VignetteRefraction";

        private readonly int _refractionTexId;
        private readonly int _valueId;
        private readonly int _color1Id;
        private readonly int _color2Id;

        public ColorParameter ColorStart = new ColorParameter(Color.white);
        public ColorParameter ColorEnd = new ColorParameter(Color.white);
        public TextureParameter RefractionTexture = new TextureParameter(null);
        public ClampedFloatParameter RefractionPower = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Intensity = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Frosting = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Fade = new ClampedFloatParameter(0f, 0f, 1f);

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public VignetteRefraction()
        {
            _refractionTexId = Shader.PropertyToID("_RefractionTex");
            _valueId = Shader.PropertyToID("_Values");
            _color1Id = Shader.PropertyToID("_Color1");
            _color2Id = Shader.PropertyToID("_Color2");
        }

        public bool IsActive()
        {
            return _material != null && Intensity.value > 0f;
        }

        public override void Setup()
        {
            var shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                _material = CoreUtils.CreateEngineMaterial(shader);
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume VignetteRefraction is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            Vector4 parameters = new Vector4(
                RefractionPower.value,
                Frosting.value,
                Fade.value,
                0f
            );

            _material.SetVector(_valueId, parameters);
            _material.SetTexture(_refractionTexId, RefractionTexture.value);
            _material.SetColor(_color1Id, ColorStart.value);
            _material.SetColor(_color2Id, ColorEnd.value);
            _material.SetFloat("_Intensity", Intensity.value);

            CoreUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}