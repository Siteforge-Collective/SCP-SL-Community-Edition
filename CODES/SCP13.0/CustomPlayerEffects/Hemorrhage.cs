using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Hemorrhage : TickingEffectBase
    {
        public float damagePerTick = 1f;

        public float effectIncrease = 0.05f;

        private bool _isSprinting;

        private DiminishingLerpVisuals _diminishingPlayerEffect;

        protected override void OnAwake()
        {
            base.OnAwake();
            _diminishingPlayerEffect = GetComponent<DiminishingLerpVisuals>();
        }

        protected override void OnTick()
        {
            if (NetworkServer.active && _isSprinting)
            {
                float multiplier = RainbowTaste.CurrentMultiplier(Hub);
                float damage = damagePerTick * multiplier;

                var handler = new UniversalDamageHandler(damage, DeathTranslations.Bleeding);
                Hub.playerStats.DealDamage(handler);
            }
        }

        protected override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            bool currentlySprinting = false;

            if (Hub.roleManager.CurrentRole is IFpcRole fpcRole)
            {
                currentlySprinting = fpcRole.FpcModule.CurrentMovementState == PlayerMovementState.Sprinting;
            }

            _isSprinting = currentlySprinting;

            if ((IsLocalPlayer || IsSpectated) && _diminishingPlayerEffect != null)
            {
                if (_isSprinting)
                {
                    float newWeight = _diminishingPlayerEffect.Intensity + effectIncrease;
                    _diminishingPlayerEffect.Intensity = newWeight;
                }
            }
        }

        public Hemorrhage()
        {
            damagePerTick = 1f;
            effectIncrease = 0.05f;
        }
    }
}