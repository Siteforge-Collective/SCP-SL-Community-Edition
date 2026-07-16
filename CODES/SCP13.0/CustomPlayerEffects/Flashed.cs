using CustomRendering;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    public class Flashed : StatusEffectBase
    {
        public const float EnableSpeedMultiplier = 2.5f;

        public const float DisableSpeedMultiplier = 0.9f;

        private Volume _processVolume;

        private Lighten _lightenEffect;

        private Darken _darkenEffect;

        private float _remainingWeight;

        public float Weight
        {
            get => _remainingWeight;
            set
            {
                _remainingWeight = Mathf.Clamp01(value);

                if (IsLocalPlayer || IsSpectated)
                {
                    if (_processVolume != null)
                    {
                        _processVolume.weight = _remainingWeight;
                    }

                    if (_lightenEffect != null)
                    {
                        _lightenEffect.active = _remainingWeight > 0.5f;
                    }

                    if (_darkenEffect != null)
                    {
                        _darkenEffect.active = _remainingWeight <= 0.5f;
                        _darkenEffect.Intensity.value = 1f - (_remainingWeight * 2f);
                    }
                }
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            _processVolume = GetComponent<Volume>();
            if (_processVolume != null && _processVolume.profile != null)
            {
                _processVolume.profile.TryGet<Lighten>(out _lightenEffect);
                _processVolume.profile.TryGet<Darken>(out _darkenEffect);
            }
        }

        protected override void Enabled()
        {
            base.Enabled();

            Weight = 1f;

            if (IsLocalPlayer || IsSpectated)
            {
                if (_processVolume != null)
                {
                    _processVolume.weight = 1f;
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!IsLocalPlayer && !IsSpectated)
            {
                return;
            }

            float delta = Time.deltaTime;

            if (IsEnabled)
            {
                if (Weight < 1f)
                {
                    Weight += delta * EnableSpeedMultiplier;
                }
            }
            else
            {
                if (Weight > 0f)
                {
                    Weight -= delta * DisableSpeedMultiplier;
                }
            }
        }

        protected override void Disabled()
        {
            base.Disabled();

            Weight = 0f;
        }

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);

            float timeLeft = newState * 0.1f;

            if (NetworkServer.active)
            {
                TimeLeft = timeLeft;
            }
        }
    }
}