using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/TiltShift")]
    public sealed class TiltShift : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum TiltShiftMethod
        {
            Horizontal = 0,
            Radial = 1
        }

        public enum QualityType
        {
            Performance = 0,
            Appearance = 1
        }

        private enum Pass
        {
            FragHorizontal = 0,
            FragHorizontalHQ = 1,
            FragRadial = 2,
            FragRadialHQ = 3,
            FragBlend = 4,
            FragDebug = 5
        }

        [Serializable]
        public sealed class TiltShiftQualityParameter : VolumeParameter<QualityType> { }

        [Serializable]
        public sealed class TiltShifMethodParameter : VolumeParameter<TiltShiftMethod> { }

        public static bool Debug;

        private const string ShaderName = "Hidden/Shader/TiltShift";

        private readonly int _paramsId;
        private readonly int _offsetId;
        private readonly int _angleId;
        private readonly int _blurredTexId;

        public ClampedFloatParameter Amount;
        public TiltShifMethodParameter Mode;

        [Tooltip("Choose to use more texture samples, for a smoother blur when using a high blur amout")]
        public TiltShiftQualityParameter Quality;

        public ClampedFloatParameter AreaSize;
        public ClampedFloatParameter AreaFalloff;
        public ClampedFloatParameter Offset;
        public ClampedFloatParameter Angle;

        private Material _material;
        private RTHandle screenCopy;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public TiltShift()
        {
            _paramsId = Shader.PropertyToID("_Params");
            _offsetId = Shader.PropertyToID("_Offset");
            _angleId = Shader.PropertyToID("_Angle");
            _blurredTexId = Shader.PropertyToID("_BlurredTex");

            Amount = new ClampedFloatParameter(0f, 0f, 1f);
            Mode = new TiltShifMethodParameter();
            Quality = new TiltShiftQualityParameter();
            AreaSize = new ClampedFloatParameter(1f, 0f, 1f);
            AreaFalloff = new ClampedFloatParameter(1f, 0f, 1f);
            Offset = new ClampedFloatParameter(0f, -1f, 1f);
            Angle = new ClampedFloatParameter(0f, 0f, 360f);
        }

        public bool IsActive()
        {
            return _material != null && Amount.value > 0f;
        }

        public override void Setup()
        {
            if (Shader.Find(ShaderName) != null)
            {
                _material = new Material(Shader.Find(ShaderName));
            }
            else
            {
                UnityEngine.Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume TiltShift is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            int passIndex = 0;
            if (Mode.value == TiltShiftMethod.Horizontal)
                passIndex = (Quality.value == QualityType.Appearance) ? (int)Pass.FragHorizontalHQ : (int)Pass.FragHorizontal;
            else
                passIndex = (Quality.value == QualityType.Appearance) ? (int)Pass.FragRadialHQ : (int)Pass.FragRadial;

            Vector4 shaderParams = new Vector4(Amount.value, AreaSize.value, AreaFalloff.value, 0f);
            _material.SetVector(_paramsId, shaderParams);
            _material.SetFloat(_offsetId, Offset.value);
            _material.SetFloat(_angleId, Angle.value);

            if (Debug)
            {
                PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, (int)Pass.FragDebug);
            }
            else
            {
                PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, passIndex);
            }
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
            if (screenCopy != null) screenCopy.Release();
        }
    }
}