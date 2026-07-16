using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    public class UserSettingsTwoButtons : UserSettingsUIBase<Toggle, bool>,
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _trueImage;
        [SerializeField] private Image _falseImage;

        [SerializeField] private RoleAccentColor _inactiveColor;
        [SerializeField] private RoleAccentColor _highlightColor;
        [SerializeField] private RoleAccentColor _activeColor;

        [SerializeField] private float _transitionTime = 0.2f;

        private float _curAnim;
        private bool _isHighlighted;

        protected override UnityEvent<bool> OnValueChangedEvent => TargetUI.onValueChanged;

        private void Update()
        {
            UpdateColors(false);
        }

        private void OnEnable()
        {
            UpdateColors(true);
        }

        private void OnDisable()
        {
            _isHighlighted = false;
            UpdateColors(true);
        }

        private void UpdateColors(bool instant)
        {
            if (_trueImage == null || _falseImage == null) return;

            bool currentValue = StoredValue;

            Color active = _activeColor.Color;
            Color inactive = _inactiveColor.Color;
            Color highlight = _highlightColor.Color;

            float targetAnim = _isHighlighted ? 1f : 0f;
            if (instant)
            {
                _curAnim = targetAnim;
            }
            else
            {
                _curAnim = Mathf.MoveTowards(_curAnim, targetAnim, Time.deltaTime / _transitionTime);
            }

            Color trueColor = Color.Lerp(inactive, currentValue ? active : highlight, _curAnim);
            Color falseColor = Color.Lerp(inactive, !currentValue ? active : highlight, _curAnim);

            _trueImage.color = trueColor;
            _falseImage.color = falseColor;
        }

        protected override void SetValueAndTriggerEvent(bool val)
        {
            TargetUI.isOn = val;
            UpdateColors(true);
        }

        protected override void SetValueWithoutNotify(bool val)
        {
            TargetUI.SetIsOnWithoutNotify(val);
            UpdateColors(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHighlighted = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHighlighted = false;
        }
    }
}