using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/Arcade")]
    public sealed class Arcade : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/Arcade";

        private readonly int _fadeId;

        public ClampedFloatParameter Fade;

        private Material _material;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public Arcade()
        {
            _fadeId = Shader.PropertyToID("_Fade");
            Fade = new ClampedFloatParameter(0f, 0f, 1f, false);
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
                Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume Arcade is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null)
                return;

            _material.SetFloat(_fadeId, Fade.value);
            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
            _material = null;
        }
    }
}