using System;
using MapGeneration;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.Spectating;
using UnityEngine;

namespace FacilitySoundtrack
{
    public class SCP079BlackoutSoundtrack : SoundtrackLayerBase
    {
        private const float BlackoutTime = 60f;

        private const float FadeInTime = 10f;

        private const float PlayDelay = 10.25f;

        private const float FadeoutTime = 1f;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private float _fadeInMultipler;

        [SerializeField]
        private float _fadeOutMultipler;

        private float _weight;

        private FacilityZone _blackoutZone;

        private double _blackoutTime;

        public override float Weight => _weight;

        public override bool Additive => false;

        public override void UpdateVolume(float volumeScale)
        {
            _audioSource.volume = volumeScale * _weight;
            if ((!ReferenceHub.LocalHub.isLocalPlayer && (SpectatorTargetTracker.CurrentTarget == null || !SpectatorTargetTracker.CurrentTarget.TargetHub.IsLocallySpectated() || SpectatorTargetTracker.CurrentTarget.TargetHub.IsSCP())) || _blackoutTime + 60.0 - 1.0 < NetworkTime.time || ReferenceHub.LocalHub.IsSCP())
            {
                Fade(_in: false, ref _weight);
                return;
            }
            RoomIdentifier roomIdentifier = RoomIdUtils.RoomAtPosition(ReferenceHub.LocalHub.transform.position);
            if (roomIdentifier == null || roomIdentifier.Zone != _blackoutZone || _blackoutTime + 10.0 > NetworkTime.time)
            {
                Fade(_in: false, ref _weight);
            }
            else
            {
                Fade(_in: true, ref _weight);
            }
        }

        private void Fade(bool _in, ref float weight)
        {
            weight = Mathf.Lerp(_weight, _in ? 1 : 0, (_in ? _fadeInMultipler : _fadeOutMultipler) * Time.deltaTime);
        }

        private void OnClientZoneBlackout(ReferenceHub scp079Hub, FacilityZone zone)
        {
            _audioSource.PlayDelayed(10.25f);
            _blackoutZone = zone;
            _blackoutTime = NetworkTime.time;
        }

        private void Start()
        {
            Scp079BlackoutZoneAbility.OnClientZoneBlackout = (Action<ReferenceHub, FacilityZone>)Delegate.Combine(Scp079BlackoutZoneAbility.OnClientZoneBlackout, new Action<ReferenceHub, FacilityZone>(OnClientZoneBlackout));
        }

        private void OnDestroy()
        {
            Scp079BlackoutZoneAbility.OnClientZoneBlackout = (Action<ReferenceHub, FacilityZone>)Delegate.Remove(Scp079BlackoutZoneAbility.OnClientZoneBlackout, new Action<ReferenceHub, FacilityZone>(OnClientZoneBlackout));
        }
    }
}
