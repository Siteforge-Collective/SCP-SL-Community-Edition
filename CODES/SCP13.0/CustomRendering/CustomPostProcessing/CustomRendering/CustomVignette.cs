using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/CustomVignette")]
    public sealed class CustomVignette : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/CustomVignette";

        private readonly int _fadeID;      
        private readonly int _intensityId; 
        private readonly int _colorId;    

        public ClampedFloatParameter Size;   
        public ClampedFloatParameter Fade;    
        public ColorParameter Color;        

        private Material _material; 
        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public CustomVignette()
        {
            _fadeID = Shader.PropertyToID("_Fade");
            _intensityId = Shader.PropertyToID("_Intensity");
            _colorId = Shader.PropertyToID("_Color");

            Size = new ClampedFloatParameter(0f, 0f, 1f);
            Fade = new ClampedFloatParameter(0f, 0f, 1f);
            Color = new ColorParameter(UnityEngine.Color.black, true);
        }

        public bool IsActive()
        {
            return _material != null && Fade.value > 0f;
        }

        public override void Setup()
        {
            if (_material == null)
            {
                Shader shader = Shader.Find(ShaderName);
                if (shader != null)
                {
                    _material = new Material(shader);
                }
                else
                {
                    Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume CustomVignette is unable to load.");
                }
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_fadeID, Fade.value);
            _material.SetFloat(_intensityId, Size.value);
            _material.SetColor(_colorId, Color.value);

            PostProcessUtils.BlitFullScreen(cmd, source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}