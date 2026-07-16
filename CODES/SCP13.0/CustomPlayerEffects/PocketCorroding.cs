using AudioPooling;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using PluginAPI.Events;
using RelativePositioning;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class PocketCorroding : TickingEffectBase, IFootstepEffect, IMovementSpeedModifier, IStaminaModifier
    {
        private const float ActivationHeight = -1998.5f;

        private const float DeactivationHeight = -1800f;

        [SerializeField]
        private float _startingDamage = 1f;

        [SerializeField]
        private AudioClip[] _footstepSounds;

        [SerializeField]
        private float _originalLoudness;

        private float _damagePerTick = 1f;

        public override bool AllowEnabling => true;

        public bool MovementModifierActive => base.IsEnabled;

        public bool StaminaModifierActive => base.IsEnabled;

        public float StaminaUsageMultiplier => 1f;

        public float MovementSpeedMultiplier => 1f;

        public float StaminaRegenMultiplier => 1f;

        public bool SprintingDisabled => true;

        public float MovementSpeedLimit => float.MaxValue;

        public RelativePosition CapturePosition { get; private set; }

        protected override void OnTick()
        {
            if (NetworkServer.active)
            {
                if (base.Hub.roleManager.CurrentRole is IFpcRole fpcRole && fpcRole.FpcModule.Position.y > -1800f)
                {
                    ServerDisable();
                    return;
                }
                base.Hub.playerStats.DealDamage(new UniversalDamageHandler(_damagePerTick, DeathTranslations.PocketDecay));
                _damagePerTick += 0.1f;
            }
        }

        protected override void Enabled()
        {
            if (NetworkServer.active)
            {
                _damagePerTick = _startingDamage;
                if (base.Hub.roleManager.CurrentRole is IFpcRole fpcRole && EventManager.ExecuteEvent(new PlayerEnterPocketDimensionEvent(base.Hub)))
                {
                    CapturePosition = new RelativePosition(fpcRole.FpcModule.Position);
                    fpcRole.FpcModule.ServerOverridePosition(Vector3.up * -1998.5f, Vector3.zero);
                    base.Hub.playerEffectsController.EnableEffect<Sinkhole>();
                }
            }
        }

        protected override void Disabled()
        {
            base.Disabled();
            base.Hub.playerEffectsController.DisableEffect<Sinkhole>();
        }

        public float ProcessFootstepOverrides(float dis)
        {
            AudioSourcePoolManager.PlaySound(_footstepSounds.RandomItem(), base.transform, dis);
            return _originalLoudness;
        }
    }
}
