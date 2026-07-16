using InventorySystem.Items.Firearms.Attachments;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomPlayerEffects
{
    public class AmnesiaItems : StatusEffectBase, IUsableItemModifierEffect, IWeaponModifierPlayerEffect, IPulseEffect
    {
        private float _activeTime;

        [SerializeField]
        private ItemType[] _blockedUsableItems;

        [SerializeField]
        private float _blockDelay;

        [SerializeField]
        private float _pulseMax;

        [SerializeField]
        private float _vignetteLerp;

        [SerializeField]
        private float _pulseDrop;

        [SerializeField]
        private AnimationCurve _pulseMinOverTime;

        private Vignette _vignette;

        private float _pulseTarget;

        public bool ParamsActive => base.IsEnabled && _activeTime >= _blockDelay;

        
        protected override void OnAwake()
        {
            base.OnAwake();
            Volume volume = GetComponent<Volume>();
            volume.profile.TryGet<Vignette>(out _vignette);
        }

        protected override void Update()
        {
            base.Update();
            if (base.IsEnabled)
            {
                _activeTime += Time.deltaTime;
            }
        }

        protected override void Enabled()
        {
            base.Enabled();
            _activeTime = 0f;
        }
        protected override void OnEffectUpdate()
        {
            if (IsLocalPlayer || IsSpectated)
            {
                _vignette.smoothness.value = Mathf.Lerp(_vignette.smoothness.value, _pulseTarget, _vignetteLerp * Time.deltaTime);
                float min = _pulseMinOverTime.Evaluate(_activeTime);
                _pulseTarget = Mathf.Max(min, _pulseTarget - _pulseDrop * Time.deltaTime);
            }
        }

        public bool TryGetSpeed(ItemType item, out float speed)
        {
            speed = 0f;
            if (NetworkServer.active && _blockedUsableItems.Contains(item) && _activeTime >= _blockDelay)
            {
                ServerSendPulse();
                return true;
            }
            return false;
        }

        public bool TryGetWeaponParam(AttachmentParam param, out float val)
        {
            val = 1f;
            if (NetworkServer.active && param == AttachmentParam.PreventReload && _activeTime >= _blockDelay)
            {
                ServerSendPulse();
                return true;
            }
            return false;
        }

        public void ExecutePulse()
        {
            _pulseTarget = _pulseMax;
        }

        private void ServerSendPulse()
        {
            Hub.playerEffectsController.ServerSendPulse<AmnesiaItems>();
        }
    }
}