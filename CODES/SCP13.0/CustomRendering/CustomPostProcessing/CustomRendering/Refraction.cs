using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Refraction")]
    public sealed class Refraction : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Refraction";

        private readonly int _amountId;
        private readonly int _refractionTexId;

        [Tooltip("Takes a DUDV map (normal map without a blue channel) to perturb the image")]
        public TextureParameter RefractionTex;

        [Tooltip("In the absense of a DUDV map, the supplied normal map can be converted in the shader")]
        public BoolParameter ConvertNormalMap;

        public ClampedFloatParameter Amount;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null
                && RefractionTex.value != null
                && Amount.value > 0f;
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

            _material.SetFloat(_amountId, Amount.value);

            if (RefractionTex.value != null)
            {
                _material.SetTexture(_refractionTexId, RefractionTex.value);
            }

            HDUtils.DrawFullScreen(cmd, _material, destination, null, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Refraction()
        {
            _amountId = Shader.PropertyToID("_Amount");
            _refractionTexId = Shader.PropertyToID("_RefractionTex");

            RefractionTex = new TextureParameter(null);
            ConvertNormalMap = new BoolParameter(false);

            Amount = new ClampedFloatParameter(0f, 0f, 100f);
        }
    }
}