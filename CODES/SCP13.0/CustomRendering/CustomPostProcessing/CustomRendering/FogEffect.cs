using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Fog Effect")]
    public sealed class FogEffect : DistanceEffect
    {
        private readonly int _fogColorId;

        public ColorParameter FogColor;

        protected override string ShaderName => "Hidden/Shader/FogEffect";

        public override bool IsActive()
        {
            if (Material == null)
                return false;

            return DistanceFade.value && FadeIntensity.value > 0f;
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (Material == null) return;

            SetDistanceProperties(camera);

            Material.SetColor(_fogColorId, FogColor.value);

            PostProcessUtils.BlitFullScreen(cmd, source, destination, Material, 0);
        }

        public FogEffect() : base()
        {
            _fogColorId = Shader.PropertyToID("_CustomFogColor");
            FogColor = new ColorParameter(Color.white, true);
        }
    }
}