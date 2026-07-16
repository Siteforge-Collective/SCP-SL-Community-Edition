using CustomPlayerEffects;
using PlayerRoles;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace FacilitySoundtrack
{
    public class Scp106HeartbeatSoundtrack : SoundtrackLayerBase
    {
        private const float MaxHighHeartBeatVolume = 1f;
        private const float StaringDesaturation = -90f;
        private const float NotStaringDesaturation = -40f;
        private const float MinimumDistance = 2.5f;
        private const float MaximumDistance = 30f;
        private const float SurfaceMaximumDistance = 60f;
        private const double SustainTime = 2.5;
        private const float MinimumSCP106Distance = 75f;
        private const float ModelSize = 1.44f;

        [SerializeField] private float _fadeInLerp = 0.5f;
        [SerializeField] private float _fadeOutLerp = 0.2f;
        [SerializeField] private AudioSource[] _audioSources;

        private float _weight;
        private Traumatized _traumatized;
        private ColorAdjustments _saturationVolume;
        private bool _isStaring;
        private bool _oldIsStaring;
        private bool _toggled;
        private double _sustainTimer;

        public override bool Additive => !_isStaring;
        public override float Weight => _weight;

        private void Awake()
        {
            PlayerRoleManager.OnRoleChanged += OnRoleChanged;
        }

        private void OnDestroy()
        {
            PlayerRoleManager.OnRoleChanged -= OnRoleChanged;
        }

        private void Update()
        {
            ReferenceHub localHub = ReferenceHub.LocalHub;
            if (localHub == null || _traumatized == null) return;

            float minDistance = float.MaxValue;
            ReferenceHub closestScp = null;

            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                if (hub.GetRoleId() == RoleTypeId.Scp106)
                {
                    float dist = DistanceTo(hub.gameObject);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        closestScp = hub;
                    }
                }
            }

            _isStaring = closestScp != null && IsLooking(localHub, closestScp);

            if (_isStaring)
            {
                _sustainTimer = SustainTime;
                _toggled = true;
            }
            else if (_sustainTimer > 0)
            {
                _sustainTimer -= Time.deltaTime;
            }
            else
            {
                _toggled = false;
            }

            float targetWeight = 0f;
            if (closestScp != null && minDistance < (_traumatized.IsSurface ? SurfaceMaximumDistance : MaximumDistance))
            {
                float maxDist = _traumatized.IsSurface ? SurfaceMaximumDistance : MaximumDistance;
                targetWeight = Mathf.InverseLerp(maxDist, MinimumDistance, minDistance);
            }

            _weight = Mathf.Lerp(_weight, targetWeight, targetWeight > _weight ? _fadeInLerp * Time.deltaTime : _fadeOutLerp * Time.deltaTime);

            LerpAssets(_isStaring);
        }

        private bool IsLooking(ReferenceHub localHub, ReferenceHub scp106)
        {
            Transform cam = localHub.PlayerCameraReference;
            Vector3 dirToScp = (scp106.transform.position - cam.position).normalized;
            float dot = Vector3.Dot(cam.forward, dirToScp);
            return dot > 0.5f;
        }

        private void LerpAssets(bool isStaring)
        {
            if (_saturationVolume == null) return;

            float targetSaturation = isStaring ? StaringDesaturation : NotStaringDesaturation;
            _saturationVolume.saturation.value = Mathf.Lerp(_saturationVolume.saturation.value, targetSaturation, Time.deltaTime * 2f);
        }

        private void OnRoleChanged(ReferenceHub userHub, PlayerRoleBase prevRole, PlayerRoleBase newRole)
        {
            if (userHub.isLocalPlayer)
            {
                _weight = 0f;
                _traumatized = userHub.playerEffectsController.GetEffect<Traumatized>();

                if (_traumatized != null && _traumatized.PPVolume != null)
                {
                    _traumatized.PPVolume.profile.TryGet(out _saturationVolume);
                }
            }
        }

        private float DistanceTo(GameObject oB)
        {
            if (MainCameraController.CurrentCamera == null) return float.MaxValue;
            return Vector3.Distance(oB.transform.position, MainCameraController.CurrentCamera.transform.position);
        }

        public override void UpdateVolume(float volumeScale)
        {
            foreach (var source in _audioSources)
            {
                if (source != null)
                    source.volume = volumeScale * MaxHighHeartBeatVolume;
            }
        }
    }
}