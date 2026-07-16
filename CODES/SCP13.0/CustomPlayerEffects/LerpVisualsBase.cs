using PlayerRoles;
using PlayerRoles.Spectating;
using UnityEngine;

namespace CustomPlayerEffects
{
    public abstract class LerpVisualsBase : MonoBehaviour
    {
        private bool _enableInstant;
        private bool _disableInstant;

        private float _weight;

        [SerializeField]
        protected bool UpdateOnRoleChange = true;

        [SerializeField]
        protected float EnableSpeed = 1f;

        [SerializeField]
        protected float DisableSpeed = 1f;

        protected StatusEffectBase TargetEffect { get; private set; }

        protected float Weight
        {
            get => _weight;
            set
            {
                value = Mathf.Clamp01(value);

                if (value > 0f && _weight <= 0f)
                {
                    OnActivated();
                }

                if (value <= 0f && _weight > 0f)
                {
                    OnShutdown();
                }

                _weight = value;
                OnWeightChanged(_weight);

                // Disable the component when weight reaches 0 or 1 if instant mode is active
                if (value == 0f || value == 1f)
                {
                    enabled = false;
                }
            }
        }

        protected abstract void OnWeightChanged(float weight);

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnShutdown()
        {
        }

        protected virtual void Update()
        {
            if (TargetEffect == null || !TargetEffect.IsEnabled)
            {
                if (!_disableInstant)
                {
                    Weight -= DisableSpeed * Time.deltaTime;
                }
                else
                {
                    Weight = 0f;
                }
                return;
            }

            if (!_enableInstant)
            {
                Weight += EnableSpeed * Time.deltaTime;
            }
            else
            {
                Weight = 1f;
            }
        }

        protected virtual void Awake()
        {
            TargetEffect = GetComponent<StatusEffectBase>();

            if (TargetEffect != null)
            {
                StatusEffectBase.OnEnabled += UpdateState;
                StatusEffectBase.OnDisabled += UpdateState;
            }

            PlayerRoleManager.OnRoleChanged += UpdateRoleChange;
            SpectatorTargetTracker.OnTargetChanged += UpdateSpectator;

            _enableInstant = EnableSpeed <= 0f;
            _disableInstant = DisableSpeed <= 0f;
        }

        private void OnDestroy()
        {
            if (TargetEffect != null)
            {
                StatusEffectBase.OnEnabled -= UpdateState;
                StatusEffectBase.OnDisabled -= UpdateState;
            }

            PlayerRoleManager.OnRoleChanged -= UpdateRoleChange;
            SpectatorTargetTracker.OnTargetChanged -= UpdateSpectator;
        }

        private void UpdateState(StatusEffectBase triggeringPlayerEffect)
        {
            if (TargetEffect == null) return;

            bool shouldBeActive = TargetEffect.IsLocalPlayer || TargetEffect.IsSpectated;

            if (triggeringPlayerEffect == TargetEffect && shouldBeActive)
            {
                enabled = true;
            }
        }

        private void UpdateSpectator()
        {
            if (TargetEffect == null) return;

            if (TargetEffect.IsSpectated)
            {
                if (TargetEffect.IsEnabled)
                {
                    Weight = TargetEffect.Intensity / 255f;
                }
                else
                {
                    Weight = 0f;
                }
            }
        }

        private void UpdateRoleChange(ReferenceHub hub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (TargetEffect == null) return;

            if (hub == TargetEffect.Hub && UpdateOnRoleChange)
            {
                Weight = 0f;
            }
        }

        public LerpVisualsBase()
        {
            UpdateOnRoleChange = true;
            EnableSpeed = 1f;
            DisableSpeed = 1f;
        }
    }
}