using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerRoles.Spectating
{
    public class FullSizeSpectatableListElement : SpectatableListElementBase
    {
        [SerializeField]
        private Image _mainBackground;

        [SerializeField]
        private TextMeshProUGUI _nicknameText;

        [SerializeField]
        private GameObject _currentIndicator;

        protected virtual void Update()
        {
            var target = this._target;
            if (target == null) return;

            var mainRole = target.MainRole;
            if (mainRole == null) return;

            ReferenceHub lastOwner = mainRole.LastOwner;

            if (_nicknameText != null && lastOwner != null)
            {
                _nicknameText.text = lastOwner.nicknameSync.DisplayName;
            }

            bool isCurrent = base.IsCurrent;

            if (_currentIndicator != null)
            {
                _currentIndicator.SetActive(isCurrent);
            }

            if (_mainBackground != null)
            {
                _mainBackground.color = isCurrent
                    ? SpectatableListColors.BgSelected
                    : SpectatableListColors.BgRegular;
            }
        }
    }
}