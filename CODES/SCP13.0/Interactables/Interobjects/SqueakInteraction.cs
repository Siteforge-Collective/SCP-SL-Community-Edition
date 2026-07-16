using PlayerStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using CustomRendering;

namespace Interactables.Interobjects
{
    public class SqueakInteraction : PopupInterobject, IDestructible
    {
        private SqueakSpawner _spawner;
        private TextMeshProUGUI _mouseText;
        private GameObject _mouseGameObject;
        private Volume _tempVolume;

        public uint NetworkId => _spawner.netId;

        public Vector3 CenterOfMass => Vector3.zero;

        public bool Damage(float damage, DamageHandlerBase handler, Vector3 exactHitPos)
        {
            if (handler is AttackerDamageHandler attackerDamageHandler && attackerDamageHandler.Attacker.Hub != null)
            {
                _spawner.TargetHitMouse(attackerDamageHandler.Attacker.Hub.networkIdentity.connectionToClient);
                return true;
            }
            return false;
        }

        private void Awake()
        {
            _spawner = GetComponentInParent<SqueakSpawner>();
            
            if (UserMainInterface.Singleton != null)
            {
                _mouseGameObject = UserMainInterface.Singleton.mouseGameObject;
                _mouseText = _mouseGameObject?.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        protected override void OnClientStateChange()
        {
            if (_tempVolume != null)
            {
                _mouseGameObject.SetActive(true);
                PostProcessingVolumes.DestroyVolume(_tempVolume);
                _tempVolume = null;
                return;
            }

             _mouseGameObject.SetActive(true);

            Ripples ripples = ScriptableObject.CreateInstance<Ripples>();
            ripples.active = true;
            ripples.Strength.Override(0f); 
            ripples.Distance.Override(0f);

            Darken darken = ScriptableObject.CreateInstance<Darken>();
            darken.active = true;
            darken.Intensity.Override(0f);

            if (ReferenceHub.LocalHub != null)
            {
                PopupInterobject.TrackedPosition = ReferenceHub.LocalHub.transform.position;
            }

            VolumeComponent[] effects = { ripples, darken };   
            _tempVolume = PostProcessingVolumes.SafeGetVolume(1, 0f, effects);     
            _tempVolume.weight = 0f;   
        }

        protected override void OnClientUpdate(float enableRatio)
        {
            if (_tempVolume != null)
            {
                 _tempVolume.weight = enableRatio; 
            }

            if (_mouseText != null)
            {
                Color textColor = _mouseText.color;
                textColor.a = enableRatio * 0.5f;   
                _mouseText.color = textColor;
            }
        }
    }
}