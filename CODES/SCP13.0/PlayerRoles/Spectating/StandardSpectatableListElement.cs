using PlayerStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace PlayerRoles.Spectating
{
    public class StandardSpectatableListElement : FullSizeSpectatableListElement
    {
        [SerializeField]
        private Image _healthCircle;

        [SerializeField]
        private Image _avatarBackground;

        [SerializeField]
        private RawImage _avatarIcon;

        [SerializeField]
        private TextMeshProUGUI _roleText;

        [SerializeField]
        private Image _healthIcon;

        [SerializeField]
        private TextMeshProUGUI _healthText;

        [SerializeField]
        private Image _shieldIcon;

        [SerializeField]
        private TextMeshProUGUI _shieldText;

        private HumeShieldStat _hsStat;
        private HealthStat _hpStat;
        private AhpStat _ahpStat;

        private Color _prevColor;
        private const float FillAmountLerpSpeed = 15.5f;

        protected override void Update()
        {
            base.Update();
            if (Target == null || Target.MainRole == null) return;

            PlayerRoleBase mainRole = Target.MainRole;
            float lerpT = Time.deltaTime * FillAmountLerpSpeed;

            UpdateStats(lerpT);

            Color roleColor = mainRole.RoleColor;
            if (roleColor != _prevColor)
            {
                UpdateColor(roleColor);
            }

            if (mainRole.Team == Team.SCPs)
            {
                _roleText.text = mainRole.RoleName;
            }
            else
            {
                _roleText.text = RoleTranslations.GetAbbreviatedRoleName(mainRole.RoleTypeId);
            }

            if (mainRole is IAvatarRole avatarRole)
            {
                _avatarIcon.texture = avatarRole.RoleAvatar;
            }
        }

        private void UpdateStats(float lerpT)
        {
            if (_hpStat == null) return;

            float hsValue = (_hsStat != null) ? _hsStat.CurValue : 0f;
            _shieldText.text = Mathf.CeilToInt(hsValue).ToString();

            bool showShield = hsValue > 0;
            _shieldIcon.enabled = showShield;
            _shieldText.enabled = showShield;

            float hpValue = _hpStat.CurValue;
            _healthText.text = Mathf.CeilToInt(hpValue).ToString();

            float targetFill = hpValue / _hpStat.MaxValue;
            _healthCircle.fillAmount = Mathf.Lerp(_healthCircle.fillAmount, targetFill, lerpT);
        }

        private void UpdateColor(Color c)
        {
            _prevColor = c;
            _roleText.color = c;
            _healthCircle.color = c;
            _healthIcon.color = c;
            _healthText.color = c;

            _avatarBackground.color = new Color(
                Mathf.GammaToLinearSpace(c.r),
                Mathf.GammaToLinearSpace(c.g),
                Mathf.GammaToLinearSpace(c.b),
                c.a
            );
        }

        protected override void OnTargetChanged(SpectatableModuleBase prevTarget, SpectatableModuleBase newTarget)
        {
            if (newTarget == null || newTarget.MainRole == null) return;

            _ahpStat = newTarget.GetModule<AhpStat>();
            _hpStat = newTarget.GetModule<HealthStat>();
            _hsStat = newTarget.GetModule<HumeShieldStat>();
            UpdateStats(1f);
        }
    }
}