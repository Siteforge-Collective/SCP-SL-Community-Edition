using AudioPooling;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Sinkhole : StatusEffectBase, IStaminaModifier, IMovementSpeedModifier, IFootstepEffect
    {
        [SerializeField]
        [Range(0f, 100f)]
        private float _slowAmount;

        [SerializeField]
        private AudioClip[] _footstepSounds;

        [SerializeField]
        private float _originalLoudness;

        public bool MovementModifierActive => base.IsEnabled;

        public float MovementSpeedMultiplier => 1f - _slowAmount * 0.01f;

        public float MovementSpeedLimit => float.MaxValue;

        public bool StaminaModifierActive => base.IsEnabled;

        public float StaminaUsageMultiplier => 1f;

        public float StaminaRegenMultiplier => 1f;

        public bool SprintingDisabled => true;

        public float ProcessFootstepOverrides(float dis)
        {
            AudioSourcePoolManager.PlaySound(_footstepSounds.RandomItem(), base.transform, dis);
            return _originalLoudness;
        }
    }
}
