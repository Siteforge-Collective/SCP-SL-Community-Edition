using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    public class DiminishingLerpVisuals : LerpVisualsBase
    {
        [SerializeField]
        private AnimationCurve _intensityCurve;

        [SerializeField]
        private float _diminishingTime = 1f;

        private Volume _processVolume;

        private float _weight;

        public float Intensity
        {
            get => _weight;
            set
            {
                _weight = Mathf.Clamp01(value);
                OnWeightChanged(_weight);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _processVolume = GetComponent<Volume>();
        }

        protected override void Update()
        {
            base.Update();

            if (_weight > 0f)
            {
                _weight = Mathf.Max(0f, _weight - Time.deltaTime / _diminishingTime);
                base.Weight = _weight;
                OnWeightChanged(_weight);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (_processVolume != null)
            {
                _processVolume.enabled = true;
            }
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            if (_processVolume != null)
            {
                _processVolume.enabled = false;
            }
        }

        protected override void OnWeightChanged(float weight)
        {
            if (_processVolume != null && _intensityCurve != null)
            {
                float curveValue = _intensityCurve.Evaluate(weight);
                _processVolume.weight = curveValue;
            }
        }

        public DiminishingLerpVisuals()
        {
            _diminishingTime = 1f;
            UpdateOnRoleChange = true;
            EnableSpeed = 1f;
            DisableSpeed = 1f;
        }
    }
}