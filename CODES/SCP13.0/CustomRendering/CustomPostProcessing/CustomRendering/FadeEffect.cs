using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Fade Effect")]
    public sealed class FadeEffect : DistanceEffect
    {
        // ISIL 007: Возвращает строку "Hidden/Shader/FadeEffect"
        protected override string ShaderName => "Hidden/Shader/FadeEffect";

        public override bool IsActive()
        {
            if (Material == null)
                return false;

            return DistanceFade.value && FadeIntensity.value > 0f;
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (Material == null)
                return;

            SetDistanceProperties(camera);
            PostProcessUtils.BlitFullScreen(cmd, source, destination, Material, 0);
        }

        public FadeEffect() : base()
        {
        }
    }
}