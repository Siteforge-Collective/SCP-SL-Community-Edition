using UnityEngine;

namespace CustomPlayerEffects
{
    public class Concussed : StatusEffectBase, IHealablePlayerEffect
    {
        [Tooltip("How much the blur effect will increase between 0 and 1 per 90 degrees camera movement.")]
        public float intensityIncreasePer90Degree = 0.4f;

        [Tooltip("The time based blur factor for decreasing the intensity when not turning the camera")]
        public float intensityDecreaseTimeFactor = 0.175f;

        private Quaternion _prevRot;

        private float _weight;

        private DiminishingLerpVisuals _postProcessBehavior;

        protected override void OnAwake()
        {
            base.OnAwake();
            _postProcessBehavior = GetComponent<DiminishingLerpVisuals>();
        }

        protected override void Enabled()
        {
            base.Enabled();
            _prevRot = transform.rotation;
            _weight = 0f;
            if (_postProcessBehavior != null)
            {
                _postProcessBehavior.Intensity = 0f;
            }
        }

        protected override void OnEffectUpdate()
        {
            if (!IsLocalPlayer && !IsSpectated)
            {
                return;
            }

            Quaternion currentRot = transform.rotation;
            float angleDelta = Quaternion.Angle(_prevRot, currentRot);

            if (angleDelta > 0f)
            {
                float intensityIncrease = (angleDelta / 90f) * intensityIncreasePer90Degree;
                _weight = Mathf.Clamp01(_weight + intensityIncrease);
            }
            else
            {
                _weight -= intensityDecreaseTimeFactor * Time.deltaTime;
                _weight = Mathf.Max(0f, _weight);
            }

            _prevRot = currentRot;

            float multiplier = RainbowTaste.CurrentMultiplier(Hub);
            float finalWeight = _weight * multiplier;

            if (_postProcessBehavior != null)
            {
                _postProcessBehavior.Intensity = finalWeight;
            }
        }

        public bool IsHealable(ItemType it)
        {
            return it == ItemType.Painkillers || it == ItemType.SCP500 || it == ItemType.Adrenaline;
        }
    }
}