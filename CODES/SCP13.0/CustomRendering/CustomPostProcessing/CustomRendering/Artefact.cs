using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Artefact")]
    public sealed class Artefact : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Artefact";

        private readonly int _effectsId;

        public ClampedFloatParameter Fade;

        public ClampedFloatParameter Colorization;

        public ClampedFloatParameter Parasite;

        public ClampedFloatParameter Noise;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public Artefact()
        {
            _effectsId = Shader.PropertyToID("_Effects");

            Fade = new ClampedFloatParameter(0f, 0f, 1f, false);         // [rbx+72]
            Colorization = new ClampedFloatParameter(0.5f, 0f, 1f, false); // [rbx+80]
            Parasite = new ClampedFloatParameter(0.5f, 0f, 1f, false);     // [rbx+88]
            Noise = new ClampedFloatParameter(1f, 0f, 1f, false);        // [rbx+96]
        }

        public bool IsActive()
        {
            if (_material != null)
            {
                return Fade.value > 0f;
            }
            return false;
        }

        public override void Setup()
        {
            var shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                _material = new Material(shader);
            }
            else
            {
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Artefact is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            Vector4 effectsVector = new Vector4(
                Fade.value,        
                Colorization.value, 
                Parasite.value,     
                Noise.value         
            );

            _material.SetVector(_effectsId, effectsVector);
            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
            _material = null;
        }
    }
}