using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/EdgeDetection")]
    public sealed class EdgeDetection : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        public enum EdgeDetectMode
        {
            DepthNormals = 0,
            CrossDepthNormals = 1,
            SobelDepth = 2,
            LuminanceBased = 3
        }

        [Serializable]
        public sealed class EdgeDetectionMode : VolumeParameter<EdgeDetectMode>
        {
            public EdgeDetectionMode(EdgeDetectMode value, bool overrideState = false) : base(value, overrideState) { }
        }

        private const string ShaderName = "Hidden/Shader/EdgeDetection";

        private readonly int _sensitivity;
        private readonly int _backgroundFade;
        private readonly int _edgeSize;
        private readonly int _exponent;
        private readonly int _threshold;
        private readonly int _edgeColor;
        private readonly int _edgeOpacity;
        private readonly int _fadeParams;
        private readonly int _sobelParams;

        [Range(0f, 1f)]
        [Tooltip("Shows only the effect, to allow for finetuning")]
        public BoolParameter Debug = new BoolParameter(false);

        [Tooltip("Choose one of the different edge solvers")]
        public EdgeDetectionMode Mode = new EdgeDetectionMode(EdgeDetectMode.DepthNormals);

        public BoolParameter InvertFadeDistance = new BoolParameter(false);

        [Tooltip("Fades out the effect between the cameras near and far clipping plane")]
        public BoolParameter DistanceFade = new BoolParameter(false);

        public FloatParameter StartFadeDistance = new FloatParameter(0f);
        public FloatParameter EndFadeDistance = new FloatParameter(1000f);

        [Tooltip("Sets how much difference in depth between pixels contribute to drawing an edge")]
        public ClampedFloatParameter SensitivityDepth = new ClampedFloatParameter(1f, 0f, 1f);

        [Tooltip("Sets how much difference in normals between pixels contribute to drawing an edge")]
        public ClampedFloatParameter SensitivityNormals = new ClampedFloatParameter(1f, 0f, 1f);

        [Tooltip("Luminance threshold, pixels above this threshold will contribute to the effect")]
        public ClampedFloatParameter LumThreshold = new ClampedFloatParameter(0.5f, 0f, 1f);

        public ColorParameter EdgeColor = new ColorParameter(Color.black);

        [Tooltip("Edge Exponent")]
        public ClampedFloatParameter EdgeExp = new ClampedFloatParameter(1f, 0f, 10f);

        [Tooltip("Edge Distance")]
        public ClampedIntParameter EdgeSize = new ClampedIntParameter(1, 1, 4);

        [Tooltip("Opacity")]
        public ClampedFloatParameter EdgeOpacity = new ClampedFloatParameter(0f, 0f, 1f);

        [Tooltip("Limit the effect to inward edges only")]
        public BoolParameter SobelThin = new BoolParameter(false);

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public EdgeDetection()
        {
            _sensitivity = Shader.PropertyToID("_Sensitivity");
            _backgroundFade = Shader.PropertyToID("_BackgroundFade");
            _edgeSize = Shader.PropertyToID("_EdgeSize");
            _exponent = Shader.PropertyToID("_Exponent");
            _threshold = Shader.PropertyToID("_Threshold");
            _edgeColor = Shader.PropertyToID("_EdgeColor");
            _edgeOpacity = Shader.PropertyToID("_EdgeOpacity");
            _fadeParams = Shader.PropertyToID("_FadeParams");
            _sobelParams = Shader.PropertyToID("_SobelParams");
        }

        public bool IsActive()
        {
            return _material != null && EdgeOpacity.value > 0f;
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
                UnityEngine.Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume EdgeDetection is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetVector(_sensitivity, new Vector4(SensitivityDepth.value, SensitivityNormals.value, 0f, 0f));

            _material.SetFloat(_backgroundFade, Debug.value ? 1f : 0f);

            _material.SetFloat(_edgeSize, (float)EdgeSize.value);
            _material.SetFloat(_exponent, EdgeExp.value);
            _material.SetFloat(_threshold, LumThreshold.value);
            _material.SetColor(_edgeColor, EdgeColor.value);
            _material.SetFloat(_edgeOpacity, EdgeOpacity.value);

            Vector4 fadeParams = new Vector4(
                StartFadeDistance.value,
                EndFadeDistance.value,
                DistanceFade.value ? 1f : 0f,
                InvertFadeDistance.value ? 1f : 0f
            );
            _material.SetVector(_fadeParams, fadeParams);

            _material.SetVector(_sobelParams, new Vector4(SobelThin.value ? 1f : 0f, 0f, 0f, 0f));

            int pass = (int)Mode.value;
            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, pass);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
            _material = null;
        }
    }
}