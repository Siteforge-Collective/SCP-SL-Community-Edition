using Mirror;
using PlayerRoles;
using RemoteAdmin.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace CustomPlayerEffects
{
    public class InsufficientLighting : StatusEffectBase, ICustomRADisplay
    {
        private const float NoLightsAmbient = 0.03f;
        private const float LightLerpSpeed = 5f;

        public static float DefaultIntensity => 0f;

        private bool _prevTarget;

        private GradientSky _skyInterior;
        private float _prevIntensity;

        private PlayerRoleBase CurRole => Hub.roleManager.CurrentRole;

        public string DisplayName => "Insufficient Lighting";

        public bool CanBeDisplayed => true;

        protected override void OnAwake()
        {
            base.OnAwake();

            Volume volume = GetComponent<Volume>();
            if (volume != null && volume.profile != null && volume.profile.TryGet<GradientSky>(out GradientSky sky))
            {
                _skyInterior = sky;
                _prevIntensity = sky.bottom.value.r;
            }
        }

        internal override void OnRoleChanged(PlayerRoleBase previousRole, PlayerRoleBase newRole)
        {
            base.OnRoleChanged(previousRole, newRole);
            UpdateServer();
        }

        protected override void Start()
        {
            StaticUnityMethods.OnUpdate += AlwaysUpdate;
        }

        private void OnDestroy()
        {
            StaticUnityMethods.OnUpdate -= AlwaysUpdate;
        }

        private void AlwaysUpdate()
        {
            if (NetworkServer.active)
            {
                UpdateServer();
            }
            else
            {
                UpdateLocal();
            }
        }

        private void UpdateServer()
        {
            bool currentTarget = CurRole is IAmbientLightRole ambientLightRole && ambientLightRole.InsufficientLight;

            if (currentTarget != _prevTarget)
            {
                Intensity = (byte)(currentTarget ? 1 : 0);
                _prevTarget = currentTarget;
            }
        }

        private void UpdateLocal()
        {
            if (!IsLocalPlayer && !IsSpectated)
            {
                return;
            }

            if (_skyInterior == null)
            {
                return;
            }

            float targetIntensity = IsEnabled ? NoLightsAmbient : DefaultIntensity;
            float currentIntensity = _prevIntensity;

            if (Mathf.Abs(currentIntensity - targetIntensity) < 0.001f)
            {
                currentIntensity = targetIntensity;
            }
            else
            {
                currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, LightLerpSpeed * Time.deltaTime);
            }

            Color ambientColor = new Color(currentIntensity, currentIntensity, currentIntensity);
            _skyInterior.bottom.value = ambientColor;
            _skyInterior.middle.value = ambientColor;
            _skyInterior.top.value = ambientColor;

            _prevIntensity = currentIntensity;
        }

        protected override void Enabled()
        {
            base.Enabled();
            _prevIntensity = DefaultIntensity;
            UpdateLocal();
        }

        protected override void Disabled()
        {
            base.Disabled();
            UpdateLocal();
        }
    }
}