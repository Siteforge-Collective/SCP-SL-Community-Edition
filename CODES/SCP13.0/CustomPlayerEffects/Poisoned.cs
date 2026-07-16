using Mirror;
using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Poisoned : TickingEffectBase, IHealablePlayerEffect, IPulseEffect
    {
        private const float MinDamage = 2f;
        private const float MaxDamage = 20f;
        private const float MultPerTick = 2f;

        private float damagePerTick;

        private VignettePulse _vignettePulse;

        public bool IsHealable(ItemType it) => it == ItemType.SCP500;

        public void ExecutePulse()
        {
            if (_vignettePulse != null)
            {
                _vignettePulse.enabled = true;
            }
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _vignettePulse = GetComponent<VignettePulse>();
        }

        protected override void OnTick()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            float damage = damagePerTick;
            var handler = new UniversalDamageHandler(damage, DeathTranslations.Poisoned);
            Hub.playerStats.DealDamage(handler);

            Hub.playerEffectsController.ServerSendPulse<Poisoned>();

            damagePerTick *= MultPerTick;
            damagePerTick = Mathf.Clamp(damagePerTick, MinDamage, MaxDamage);
        }

        protected override void Enabled()
        {
            base.Enabled();

            if (NetworkServer.active)
            {
                damagePerTick = MinDamage;
            }
        }

        public Poisoned()
        {
            damagePerTick = 2f;
            TimeBetweenTicks = 1f;
        }
    }
}