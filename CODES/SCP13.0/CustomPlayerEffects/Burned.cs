using PlayerStatsSystem;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Burned : StatusEffectBase, IHealablePlayerEffect, IDamageModifierEffect
    {
        private const float BaseDamageMultiplier = 1.25f;

        private TiltShiftWave _tiltShiftOffsetWave;

        private bool _initialized;

        public float DamageMultiplier => 1f + 0.25f * RainbowTaste.CurrentMultiplier(base.Hub);

        public bool DamageModifierActive => base.IsEnabled;

        public bool IsHealable(ItemType it)
        {
            return it == ItemType.Medkit || it == ItemType.SCP500;
        }

        public float GetDamageModifier(float baseDamage, DamageHandlerBase handler, HitboxType hitboxType)
        {
            return DamageMultiplier;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            if (base.IsLocalPlayer)
            {
                _tiltShiftOffsetWave = GetComponent<TiltShiftWave>();
                _initialized = true;
            }
        }

        protected override void Enabled()
        {
            base.Enabled();

            if (base.IsLocalPlayer && _initialized)
            {
                _tiltShiftOffsetWave.EnableEffect();
            }
        }

        protected override void Disabled()
        {
            base.Disabled();

            if (base.IsLocalPlayer && _initialized)
            {
                _tiltShiftOffsetWave.DisableEffect();
            }
        }
    }
}