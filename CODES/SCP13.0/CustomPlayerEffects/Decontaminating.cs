using Mirror;
using PlayerRoles;
using PlayerStatsSystem;
using Subtitles;
using CustomRendering;

namespace CustomPlayerEffects
{
    public class Decontaminating : TickingEffectBase
    {
        private const string CassieAnnouncementText = "LOST IN DECONTAMINATION SEQUENCE";

        public float FogFadeInSpeed = 2f;

        public float FogFadeOutSpeed = 3f;

        public override bool AllowEnabling => true;

        protected override void OnTick()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            if (Hub.roleManager.CurrentRole is IHealthbarRole healthbarRole)
            {
                float damage = healthbarRole.MaxHealth / 10f;

                var cassie = new DamageHandlerBase.CassieAnnouncement
                {
                    Announcement = CassieAnnouncementText,
                    SubtitleParts = new[]
                    {
                        new SubtitlePart(SubtitleType.LostInDecontamination)
                    }
                };

                var handler = new UniversalDamageHandler(damage, DeathTranslations.Decontamination, cassie);

                Hub.playerStats.DealDamage(handler);
            }
        }

        protected override void Enabled()
        {
            base.Enabled();

            if (IsLocalPlayer || IsSpectated)
            {
                FogController.EnableFogType(FogType.Decontamination, FogFadeInSpeed);
            }
        }

        public override void OnBeginSpectating()
        {
            base.OnBeginSpectating();

            if (IsEnabled)
            {
                FogController.EnableFogType(FogType.Decontamination, 0f);
            }
            else
            {
                FogController.DisableFogType(FogType.Decontamination, 0f);
            }
        }

        public override void OnStopSpectating()
        {
            base.OnStopSpectating();

            FogController.DisableFogType(FogType.Decontamination, 0f);
        }

        protected override void Disabled()
        {
            base.Disabled();

            if (IsLocalPlayer || IsSpectated)
            {
                FogController.DisableFogType(FogType.Decontamination, FogFadeOutSpeed);
            }
        }
    }
}