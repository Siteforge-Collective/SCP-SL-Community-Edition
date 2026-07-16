using PlayerStatsSystem;
using RemoteAdmin.Interfaces;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class Invisible : StatusEffectBase, ISpectatorDataPlayerEffect, ICustomRADisplay, IHitmarkerPreventer
    {
        [SerializeField]
        private AudioClip _sfxEnable;

        [SerializeField]
        private AudioClip _sfxDisable;

        private bool _wasEverActive;

        public override EffectClassification Classification => EffectClassification.Positive;

        public string DisplayName => "Invisibility";

        public bool CanBeDisplayed => true;

        public bool GetSpectatorText(out string s)
        {
            s = "SCP-268";
            return true;
        }

        public bool TryPreventHitmarker(AttackerDamageHandler attacker)
        {
            return IsEnabled;
        }

        protected override void Enabled()
        {
            base.Enabled();

            PlaySound(true);

            _wasEverActive = true;
        }

        protected override void Disabled()
        {
            base.Disabled();

            if (_wasEverActive)
            {
                PlaySound(false);
            }
        }

        private void PlaySound(bool isEnabled)
        {
            AudioClip clip = isEnabled ? _sfxEnable : _sfxDisable;

            if (clip != null && (IsLocalPlayer || IsSpectated))
            {
                AudioPooling.AudioSourcePoolManager.PlaySound(clip, Hub.transform.position, 0f);
            }
        }
    }
}