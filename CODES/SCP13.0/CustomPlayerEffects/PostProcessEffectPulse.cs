using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace CustomPlayerEffects
{
    public abstract class PostProcessEffectPulse : MonoBehaviour
    {
        [FormerlySerializedAs("PulseIntensity")]
        [SerializeField]
        private float _pulseIntensity = 1f;

        public float animationSpeedMultiplier = 1f;

        private float _defaultIntensity;

        private float timer;

        private AnimationCurve pulseAnimation;

        public float PulseIntensity
        {
            get => _pulseIntensity;
            set
            {
                _pulseIntensity = Mathf.Max(0f, value);
                UpdateAnimationCurve();
            }
        }

        protected abstract float EffectValue { get; set; }

        protected abstract void SetEffectType(VolumeProfile profile);

        public void Pulse()
        {
            timer = 0f;
            enabled = true;
        }

        private void Awake()
        {
            pulseAnimation = new AnimationCurve();

            Volume volume = GetComponent<Volume>();
            if (volume != null && volume.profile != null)
            {
                SetEffectType(volume.profile);
                _defaultIntensity = EffectValue;
            }

            UpdateAnimationCurve();
        }

        private void UpdateAnimationCurve()
        {
            pulseAnimation.keys = null;

            float peakTime = 0.15f;
            float peakValue = _defaultIntensity + _pulseIntensity;

            pulseAnimation.AddKey(0f, _defaultIntensity);
            pulseAnimation.AddKey(peakTime, peakValue);
            pulseAnimation.AddKey(1f, _defaultIntensity);

            for (int i = 0; i < pulseAnimation.keys.Length; i++)
            {
                pulseAnimation.SmoothTangents(i, 0f);
            }
        }

        private void Update()
        {
            timer += Time.deltaTime * animationSpeedMultiplier;

            if (timer >= 1f)
            {
                EffectValue = _defaultIntensity;
                enabled = false;
                return;
            }

            float evaluated = pulseAnimation.Evaluate(timer);
            EffectValue = evaluated;
        }

        private void Reset()
        {
            timer = 0f;
            enabled = false;
            EffectValue = _defaultIntensity;
        }
    }
}