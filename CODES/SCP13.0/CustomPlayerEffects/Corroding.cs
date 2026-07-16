using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;

namespace CustomPlayerEffects
{
    public class Corroding : TickingEffectBase, IStaminaModifier
    {
        private const float DamagePerTick = 2.1f;

        private const float StaminaDrainPercentage = 2.5f;

        public ReferenceHub AttackerHub;

        public override bool AllowEnabling => true;

        public bool StaminaModifierActive => base.IsEnabled;

        public float StaminaRegenMultiplier => 0f;

        protected override void OnTick()
        {
            if (NetworkServer.active && !(AttackerHub == null) && !Vitality.CheckPlayer(base.Hub))
            {
                base.Hub.playerStats.DealDamage(new ScpDamageHandler(AttackerHub, 2.1f, DeathTranslations.PocketDecay));
                base.Hub.playerStats.GetModule<StaminaStat>().CurValue -= 0.024999999f;
            }
        }
    }
}
