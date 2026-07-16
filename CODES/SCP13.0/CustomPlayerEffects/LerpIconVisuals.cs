using UnityEngine;
using UnityEngine.UI;

namespace CustomPlayerEffects
{
    public class LerpIconVisuals : LerpVisualsBase
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private GameObject _iconParent;

        protected override void OnWeightChanged(float weight)
        {
            if (_icon != null)
            {
                _icon.fillAmount = weight;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (_iconParent != null)
            {
                _iconParent.SetActive(true);
            }
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            if (_iconParent != null)
            {
                _iconParent.SetActive(false);
            }
        }

        public LerpIconVisuals()
        {
            UpdateOnRoleChange = true;
            EnableSpeed = 1f;
            DisableSpeed = 1f;
        }
    }
}