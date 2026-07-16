using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    public abstract class DistanceEffect : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private readonly int _distanceParamsId = Shader.PropertyToID("_DistanceParams");

        public BoolParameter DistanceFade;
        public ClampedFloatParameter FadeIntensity;
        public FloatParameter EndDistance;
        public FloatParameter StartDistance;
        public ClampedFloatParameter CoverSkybox;

        protected Material Material;

        public override CustomPostProcessInjectionPoint injectionPoint => (CustomPostProcessInjectionPoint)2;

        protected abstract string ShaderName { get; }

        public bool DistanceFadeIsActive()
        {
            if (DistanceFade != null && DistanceFade.value)
            {
                if (FadeIntensity != null && FadeIntensity.value > 0f)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool IsActive()
        {
            return Material != null;
        }

        public override void Setup()
        {
            if (Shader.Find(ShaderName) != null)
            {
                Material = new Material(Shader.Find(ShaderName));
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume CameraShake is unable to load.");
            }
        }

        public void SetDistanceProperties(HDCamera camera)
        {
            if (Material == null) return;

            if (DistanceFade != null && DistanceFade.value && FadeIntensity != null && FadeIntensity.value > 0f)
            {
                Vector4 distanceParams = new Vector4(
                    StartDistance.value,
                    FadeIntensity.value,
                    EndDistance.value,
                    CoverSkybox.value
                );

                Material.SetVector(_distanceParamsId, distanceParams);
            }
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(Material);
        }

        protected DistanceEffect()
        {
            DistanceFade = new BoolParameter(false);
            FadeIntensity = new ClampedFloatParameter(1f, 0f, 1f);
            EndDistance = new FloatParameter(1000f);               
            StartDistance = new FloatParameter(0f);
            CoverSkybox = new ClampedFloatParameter(1f, 0f, 1f);
        }
    }
}