using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    public abstract class PostProcessEffectWave : MonoBehaviour
    {
        public float WaveIntensity = 1f;

        public float speedMultiplier = 1f;

        public float disableTolerance = 0.01f;

        private float _defaultIntensity;

        private float _timer;

        private bool _disabling;

        protected abstract float EffectValue { get; set; }

        protected abstract void SetEffectType(VolumeProfile profile);

        public void EnableEffect()
        {
            enabled = true;
            _disabling = false;
        }

        public void DisableEffect()
        {
            if (enabled)
            {
                _disabling = true;
            }
        }

        private void Awake()
        {
            Volume volume = GetComponent<Volume>();
            if (volume != null && volume.profile != null)
            {
                SetEffectType(volume.profile);
                _defaultIntensity = EffectValue;
            }
        }

        private void Update()
        {
            float delta = Time.deltaTime * speedMultiplier;
            _timer += delta;

            float wave = Mathf.Sin(_timer * Mathf.PI * 2f) * WaveIntensity * 0.5f + _defaultIntensity;

            if (_disabling)
            {
                float diff = Mathf.Abs(wave - _defaultIntensity);
                if (diff <= disableTolerance)
                {
                    wave = _defaultIntensity;
                    enabled = false;
                }
            }

            EffectValue = wave;
        }
    }
}