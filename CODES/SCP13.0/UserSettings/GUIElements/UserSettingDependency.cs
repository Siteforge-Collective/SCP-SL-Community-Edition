using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UserSettingDependency : MonoBehaviour
    {
        [Serializable]
        private class Dependency
        {
            private enum Condition { EqualsTo = 0, LessThan = 1, GreaterThan = 2 }
            private enum SettingType { Slider = 0, Toggle = 1, Dropdown = 2 }

            [SerializeField] private SettingType _settingType;
            [SerializeField] private Component _targetComponent;
            [SerializeField] private Condition _condition;
            [SerializeField] private bool _invertCondition;

            [Header("Compare Values")]
            [SerializeField] private float _compareValueFloat;
            [SerializeField] private int _compareValueInt;
            [SerializeField] private bool _compareValueBool;

            private bool _setup;
            private Slider _slider;
            private Toggle _toggle;
            private TMP_Dropdown _dropdown;

            public bool ConditionMet
            {
                get
                {
                    if (!_setup) { Setup(); _setup = true; }

                    bool result = Evaluate();
                    return _invertCondition ? !result : result;
                }
            }

            private void Setup()
            {
                if (_targetComponent == null) return;

                switch (_settingType)
                {
                    case SettingType.Slider: _slider = _targetComponent as Slider; break;
                    case SettingType.Toggle: _toggle = _targetComponent as Toggle; break;
                    case SettingType.Dropdown: _dropdown = _targetComponent as TMP_Dropdown; break;
                }
            }

            private bool Evaluate()
            {
                switch (_settingType)
                {
                    case SettingType.Slider:
                        if (_slider == null) return false;
                        return _condition switch
                        {
                            Condition.EqualsTo => Mathf.Approximately(_slider.value, _compareValueFloat),
                            Condition.LessThan => _slider.value < _compareValueFloat,
                            Condition.GreaterThan => _slider.value > _compareValueFloat,
                            _ => false
                        };

                    case SettingType.Toggle:
                        if (_toggle == null) return false;
                        return _toggle.isOn == _compareValueBool;

                    case SettingType.Dropdown:
                        if (_dropdown == null) return false;
                        return _condition switch
                        {
                            Condition.EqualsTo => _dropdown.value == _compareValueInt,
                            Condition.LessThan => _dropdown.value < _compareValueInt,
                            Condition.GreaterThan => _dropdown.value > _compareValueInt,
                            _ => false
                        };
                }
                return false;
            }
        }

        [SerializeField] private Dependency[] _dependencies;
        [SerializeField] private float _fadeSpeed = 10f;
        [SerializeField] private float _minFade = 0.2f;

        private CanvasGroup _fader;

        private bool ShouldBeVisible
        {
            get
            {
                if (_dependencies == null || _dependencies.Length == 0) return true;
                foreach (var dep in _dependencies)
                {
                    if (!dep.ConditionMet) return false;
                }
                return true;
            }
        }

        private void Awake() => _fader = GetComponent<CanvasGroup>();

        private void OnEnable() => UpdateVisibility(true);

        private void Update() => UpdateVisibility(false);

        private void UpdateVisibility(bool instant)
        {
            if (_fader == null) return;

            bool visible = ShouldBeVisible;
            float targetAlpha = visible ? 1f : _minFade;

            if (instant) _fader.alpha = targetAlpha;
            else _fader.alpha = Mathf.MoveTowards(_fader.alpha, targetAlpha, Time.deltaTime * _fadeSpeed);

            _fader.blocksRaycasts = visible;
            _fader.interactable = visible;
        }
    }
}