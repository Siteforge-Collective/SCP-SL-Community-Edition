using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomRendering
{
    [Serializable]
    [VolumeComponentMenu("Post-processing/Custom/BloodHit")]
    public sealed class BloodHit : CustomPostProcessVolumeComponent, IPostProcessComponent
    {
        private const string ShaderName = "Hidden/Shader/BloodHit";

        public ClampedFloatParameter Hit_Left = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Hit_Up = new ClampedFloatParameter(0f, 0f, 1f);   
        public ClampedFloatParameter Hit_Right = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Hit_Down = new ClampedFloatParameter(0f, 0f, 1f); 

        public ClampedFloatParameter Blood_Hit_Left = new ClampedFloatParameter(0f, 0f, 1f); 
        public ClampedFloatParameter Blood_Hit_Up = new ClampedFloatParameter(0f, 0f, 1f);   
        public ClampedFloatParameter Blood_Hit_Right = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Blood_Hit_Down = new ClampedFloatParameter(0f, 0f, 1f); 

        public ClampedFloatParameter Hit_Full = new ClampedFloatParameter(0f, 0f, 1f);        
        public ClampedFloatParameter Blood_Hit_Full_1 = new ClampedFloatParameter(0f, 0f, 1f);
        public ClampedFloatParameter Blood_Hit_Full_2 = new ClampedFloatParameter(0f, 0f, 1f); 
        public ClampedFloatParameter Blood_Hit_Full_3 = new ClampedFloatParameter(0f, 0f, 1f); 

        public ClampedFloatParameter LightReflect = new ClampedFloatParameter(0f, 0f, 1f);    
        public TextureParameter BloodHitTexture = new TextureParameter(null);                

        private Material _material;

        private int _LightReflectId;
        private int _HitsId;
        private int _BloodHitsId;
        private int _FullHitsId;
        private int _BloodTexId;

        public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

        public bool IsActive()
        {
            return _material != null && (
                Hit_Left.value > 0f || Hit_Up.value > 0f || Hit_Right.value > 0f || Hit_Down.value > 0f ||
                Blood_Hit_Left.value > 0f || Blood_Hit_Up.value > 0f || Blood_Hit_Right.value > 0f || Blood_Hit_Down.value > 0f ||
                Hit_Full.value > 0f || Blood_Hit_Full_1.value > 0f || Blood_Hit_Full_2.value > 0f || Blood_Hit_Full_3.value > 0f ||
                LightReflect.value > 0f
            );
        }

        public override void Setup()
        {
            _LightReflectId = Shader.PropertyToID("_LightReflect");
            _HitsId = Shader.PropertyToID("_Hits");
            _BloodHitsId = Shader.PropertyToID("_BloodHits");
            _FullHitsId = Shader.PropertyToID("_FullHits");
            _BloodTexId = Shader.PropertyToID("_BloodTex");

            if (_material == null)
            {
                Shader shader = Shader.Find(ShaderName);
                if (shader != null)
                    _material = new Material(shader);
                else
                    Debug.LogError($"Unable to find shader '{ShaderName}'. Post Process Volume BloodHit is unable to load.");
            }
        }

        public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
        {
            if (_material == null) return;

            _material.SetFloat(_LightReflectId, LightReflect.value);
            _material.SetVector(_HitsId, new Vector4(Hit_Left.value, Hit_Up.value, Hit_Right.value, Hit_Down.value));
            _material.SetVector(_BloodHitsId, new Vector4(Blood_Hit_Left.value, Blood_Hit_Up.value, Blood_Hit_Right.value, Blood_Hit_Down.value));
            _material.SetVector(_FullHitsId, new Vector4(Hit_Full.value, Blood_Hit_Full_1.value, Blood_Hit_Full_2.value, Blood_Hit_Full_3.value));

            if (BloodHitTexture.value != null)
                _material.SetTexture(_BloodTexId, BloodHitTexture.value);

            cmd.Blit(source, destination, _material, 0);
        }

        public override void Cleanup()
        {
            CoreUtils.Destroy(_material);
        }
    }
}