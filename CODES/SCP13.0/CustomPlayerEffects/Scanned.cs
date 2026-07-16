using UnityEngine;
using PlayerRoles.Spectating;

namespace CustomPlayerEffects
{
    public class Scanned : StatusEffectBase, ISoundtrackMutingEffect
    {
        [SerializeField]
        private AudioSource _soundSource;

        public bool MuteSoundtrack => true;

        protected override void Enabled()
        {
            UpdateSourceMute();
            if (_soundSource != null)
            {
                _soundSource.Play();
            }
        }

        protected override void Disabled()
        {
            if (_soundSource != null)
            {
                _soundSource.mute = true;
            }
        }

        protected override void OnEffectUpdate()
        {
            UpdateSourceMute();
        }

        private void UpdateSourceMute()
        {
            if (_soundSource == null) return;

            bool isLocallySpectated = false;
            if (!Hub.isLocalPlayer)
            {
                isLocallySpectated = SpectatorNetworking.IsLocallySpectated(Hub);
            }

            _soundSource.mute = isLocallySpectated;
        }
    }
}