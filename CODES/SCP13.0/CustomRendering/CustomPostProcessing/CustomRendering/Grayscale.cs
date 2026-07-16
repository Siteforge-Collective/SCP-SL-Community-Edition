using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Grayscale")]
    public sealed class Grayscale : DistanceEffect
    {
        private readonly int _intensityId;

        public ClampedFloatParameter Intensity;
        protected override string ShaderName => "Hidden/Shader/Grayscale";

        public override bool IsActive()
        {
            if (Material == null)
                return false;

            return Intensity.value > 0f;
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (Material == null) return;

            SetDistanceProperties(camera);

            Material.SetFloat(_intensityId, Intensity.value);

            bool isActive = DistanceFadeIsActive() && CoverSkybox.value > 0f;

            PostProcessUtils.BlitFullScreen(cmd, source, destination, Material, isActive ? 1 : 0);
        }

        public Grayscale() : base()
        {
            _intensityId = Shader.PropertyToID("_GrayScaleIntensity");
            Intensity = new ClampedFloatParameter(0f, 0f, 1f);
        }
    }
}