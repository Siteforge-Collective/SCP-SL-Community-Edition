using PlayerRoles;
using UnityEngine;

namespace CustomPlayerEffects
{
    public class AmnesiaVision : StatusEffectBase, ISoundtrackMutingEffect
    {
        private float _lastTime;

        [SerializeField]
        private AnimationCurve _whispersOverTime;

        [SerializeField]
        private float _soundtrackLerp;

        [SerializeField]
        private float _growlDetectionRangeSqr;

        [SerializeField]
        private AudioSource _whispersSource;

        [SerializeField]
        private AudioSource _growlSource;

        [SerializeField]
        private AudioSource _disableSource;

        public bool MuteSoundtrack => IsEnabled && _whispersSource?.volume > 0f;

        public float LastActive => Time.timeSinceLevelLoad - _lastTime;

        public override void OnStopSpectating()
        {
            base.OnStopSpectating();
            _disableSource?.Stop();
            _growlSource?.Stop();
        }

        internal override void OnDeath(PlayerRoleBase previousRole)
        {
            base.OnRoleChanged(previousRole, null);
            _disableSource?.Stop();
            _growlSource?.Stop();
        }

        protected override void Disabled()
        {
            base.Disabled();
            _growlSource?.Stop();
            _whispersSource.volume = 0f;
            if (_whispersSource != null && _whispersSource.isPlaying)
            {
                _disableSource?.Play();
            }
        }

        protected override void Update()
        {
            base.Update();
            if (!IsEnabled)
            {
                _whispersSource.volume = 0f;
                return;
            }
            _lastTime = Time.timeSinceLevelLoad;
        }

        protected override void OnEffectUpdate()
        {
            if (!IsLocalPlayer && !IsSpectated) return;

            float targetVolume = _whispersOverTime.Evaluate(Time.timeSinceLevelLoad);
            _whispersSource.volume = Mathf.Lerp(_whispersSource.volume, targetVolume, _soundtrackLerp * Time.deltaTime);
        }

        private void StopSources()
        {
            _disableSource?.Stop();
            _growlSource?.Stop();
        }

        protected override void Enabled()
        {
            base.Enabled();
            _lastTime = Time.timeSinceLevelLoad;

            if (IsLocalPlayer)
            {
                _whispersSource?.Play();

                PlayerRoleBase currentRole = Hub.roleManager.CurrentRole;
                if (currentRole != null)
                {
                    Team myTeam = PlayerRolesUtils.GetTeam(Hub);
                    foreach (ReferenceHub nearbyHub in ReferenceHub.AllHubs)
                    {
                        if (nearbyHub == Hub) continue;

                        PlayerRoleBase nearbyRole = nearbyHub.roleManager.CurrentRole;
                        if (nearbyRole == null) continue;

                        if (HitboxIdentity.IsEnemy(nearbyRole.Team, myTeam))
                        {
                            float sqrDistance = (nearbyHub.transform.position - Hub.transform.position).sqrMagnitude;
                            if (sqrDistance <= _growlDetectionRangeSqr)
                            {
                                _growlSource?.Play();
                                return;
                            }
                        }
                    }
                    _growlSource?.Stop();
                }
            }
        }
    }
}