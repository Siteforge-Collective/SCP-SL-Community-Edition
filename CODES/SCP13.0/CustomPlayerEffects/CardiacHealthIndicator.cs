using PlayerStatsSystem;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    public class CardiacHealthIndicator : SubEffectBase
    {
        [SerializeField]
        private AnimationCurve _healthToWeight;

        [SerializeField]
        private Volume _ppv;

        [SerializeField]
        private float _speedMultiplier;

        private HealthStat _hp;

        private float _targetWeight;
        public override bool IsActive => MainEffect != null && MainEffect.IsEnabled;

        public void SetTargetWeight(float value, bool forceWeight = false)
        {
            _targetWeight = value;

            if (forceWeight)
            {
                _ppv.weight = value;
            }
        }

        public override void DisableEffect()
        {
            base.DisableEffect();
            _targetWeight = 0f;
            _ppv.weight = 0f;
        }

        internal override void UpdateEffect()
        {
            base.UpdateEffect();

            if (IsActive && (IsLocalPlayer || MainEffect.IsSpectated))
            {
                float healthPercentage = _hp.CurValue / _hp.MaxValue;
                float target = _healthToWeight.Evaluate(healthPercentage);
                SetTargetWeight(target);
            }
        }

        internal override void Init(StatusEffectBase mainEffect)
        {
            base.Init(mainEffect);

            ReferenceHub hub = MainEffect.Hub;
            if (hub.playerStats.TryGetModule<HealthStat>(out HealthStat healthStat))
            {
                _hp = healthStat;
            }
        }

        private void Update()
        {
            if (!IsActive)
            {
                if (_ppv != null && _ppv.weight > 0f)
                {
                    _targetWeight = 0f;
                }
                else
                {
                    return;
                }
            }

            float currentWeight = _ppv.weight;
            float difference = _targetWeight - currentWeight;
            float maxChange = _speedMultiplier * Time.deltaTime;

            if (Mathf.Abs(difference) <= maxChange)
            {
                _ppv.weight = _targetWeight;
            }
            else
            {
                _ppv.weight += Mathf.Sign(difference) * maxChange;
            }
        }
    }
}