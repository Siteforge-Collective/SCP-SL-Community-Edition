using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Ripples")]
    public sealed class Ripples : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum RipplesMode
        {
            Radial = 0,
            OmniDirectional = 1
        }

        [Serializable]
        public sealed class RipplesModeParam : VolumeParameter<RipplesMode>
        {
            public RipplesModeParam(RipplesMode value, bool overrideState = false) : base(value, overrideState) { }
        }

        private const string ShaderName = "Hidden/Shader/Ripples";

        private readonly int _strengthId;
        private readonly int _distanceId;
        private readonly int _speedId;
        private readonly int _sizeId;

        public RipplesModeParam Mode;
        public ClampedFloatParameter Strength;
        public ClampedFloatParameter Distance;
        public ClampedFloatParameter Speed;
        public ClampedFloatParameter Width;
        public ClampedFloatParameter Height;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null && Strength.value > 0f;
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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Ripples is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_strengthId, Strength.value * 100f);
            _material.SetFloat(_distanceId, Distance.value * 100f);
            _material.SetFloat(_speedId, Speed.value);

            _material.SetVector(_sizeId, new Vector4(Width.value, Height.value, 0, 0));

            int pass = (int)Mode.value;
            HDUtils.DrawFullScreen(cmd, _material, destination, null, pass);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }

        public Ripples()
        {
            _strengthId = Shader.PropertyToID("_Strength");
            _distanceId = Shader.PropertyToID("_Distance");
            _speedId = Shader.PropertyToID("_Speed");
            _sizeId = Shader.PropertyToID("_Size");

            Mode = new RipplesModeParam(RipplesMode.Radial);

            Strength = new ClampedFloatParameter(0f, 0f, 10f);
            Distance = new ClampedFloatParameter(1f, 0f, 10f);
            Speed = new ClampedFloatParameter(0.5f, 0f, 10f);
            Width = new ClampedFloatParameter(0.2f, 0f, 5f);
            Height = new ClampedFloatParameter(0.1f, 0f, 5f);
        }
    }
}