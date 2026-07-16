using CustomRendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomPlayerEffects
{
    public class LerpFrostingVisuals : LerpPostProcessVisuals
    {
        [SerializeField]
        [Tooltip("This effect bugs out when Frosting is set to 1.\n\nYou have been warned!")]
        private float _maxFrosting = 0.99f;

        private VignetteRefraction _refraction;

        protected override void Awake()
        {
            base.Awake();

            Volume processVolume = GetComponent<Volume>();
            if (processVolume != null && processVolume.profile != null)
            {
                if (processVolume.profile.TryGet<VignetteRefraction>(out VignetteRefraction refraction))
                {
                    _refraction = refraction;
                }
            }
        }

        protected override void OnWeightChanged(float weight)
        {
            if (_refraction != null && _refraction.Intensity != null)
            {
                float targetFrosting = Mathf.Lerp(0f, _maxFrosting, weight);
                _refraction.Intensity.value = targetFrosting;
            }
        }

        public LerpFrostingVisuals()
        {
            UpdateOnRoleChange = true;
            EnableSpeed = 1f;
            DisableSpeed = 1f;
        }
    }
}