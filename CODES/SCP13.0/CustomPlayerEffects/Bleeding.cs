using Mirror;
using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Bleeding : TickingEffectBase, IPulseEffect, IHealablePlayerEffect
    {
        public float minDamage = 2f;

        public float maxDamage = 20f;

        public float multPerTick = 0.5f;

        private float damagePerTick = 20f;

        private VignettePulse _vignettePulse;

        protected override void OnAwake()
        {
            base.OnAwake();
            _vignettePulse = GetComponent<VignettePulse>();
        }

        public void ExecutePulse()
        {
            _vignettePulse?.Pulse();
        }

        protected override void OnTick()
        {
            if (NetworkServer.active)
            {
                float multiplier = RainbowTaste.CurrentMultiplier(Hub);
                float damage = damagePerTick * multiplier;

                UniversalDamageHandler handler = new UniversalDamageHandler(damage, DeathTranslations.Bleeding);
                Hub.playerStats.DealDamage(handler);

                Hub.playerEffectsController.ServerSendPulse<Bleeding>();

                damagePerTick *= multPerTick;
                damagePerTick = Mathf.Clamp(damagePerTick, minDamage, maxDamage);
            }
        }

        protected override void Enabled()
        {
            base.Enabled();
            if (NetworkServer.active)
            {
                damagePerTick = maxDamage;
            }
        }

        public bool IsHealable(ItemType it)
        {
            if (it == ItemType.SCP500)
            {
                damagePerTick = minDamage;
            }
            return it == ItemType.Medkit;
        }
    }
}