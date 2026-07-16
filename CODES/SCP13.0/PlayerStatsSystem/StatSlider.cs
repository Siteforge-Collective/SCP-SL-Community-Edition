using System;
using PlayerRoles;
using UnityEngine;
using UnityEngine.UI;
using UserSettings;
using UserSettings.GUIElements;
using UserSettings.UserInterfaceSettings;

namespace PlayerStatsSystem
{
    public class StatSlider : MonoBehaviour
    {
        private enum DisplayExactMode
        {
            PreferenceBased = 0,
            AlwaysExact = 1,
            AlwaysPercent = 2,
            ValueHidden = 3
        }

        [SerializeField]
        private string _statTypeName;

        [SerializeField]
        private float _lerpSpeed;

        [SerializeField]
        private float _snapValueSkip;

        [SerializeField]
        private DisplayExactMode _displayExactMode;

        [SerializeField]
        private LinkableEnum _preferenceKey;

        [SerializeField]
        private string _suffix;

        [SerializeField]
        private Text _targetText;

        [SerializeField]
        private Slider _targetSlider;

        [SerializeField]
        private int _roundingAccuracy;

        private float _currentValue;

        private string _originalSuffix;

        private int? _cachedTypeId;

        private const float AbsoluteMoveRatio = 0.04f;

        public string CustomSuffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                _suffix = (string.IsNullOrEmpty(value) ? _originalSuffix : value);
            }
        }

        public void ForceUpdate()
        {
            if (TryGetModule(out var sb))
            {
                _currentValue = sb.CurValue;
            }
            _targetSlider.value = _currentValue;
        }

        public bool TryGetModule(out StatBase sb)
        {
            sb = null;
            if (!ReferenceHub.TryGetLocalHub(out var hub))
            {
                return false;
            }
            if (!(hub.roleManager.CurrentRole is IHealthbarRole healthbarRole))
            {
                return false;
            }
            if (healthbarRole.TargetStats == null)
            {
                return false;
            }
            if (!TryGetTypeId(out var val))
            {
                return false;
            }
            sb = healthbarRole.TargetStats.StatModules[val];
            return true;
        }

        public bool TryGetTypeId(out int val)
        {
            if (_cachedTypeId.HasValue)
            {
                val = _cachedTypeId.Value;
                return true;
            }
            val = -1;
            Type[] definedModules = PlayerStats.DefinedModules;
            for (int i = 0; i < definedModules.Length; i++)
            {
                if (!(definedModules[i].Name != _statTypeName))
                {
                    val = i;
                    _cachedTypeId = i;
                    return true;
                }
            }
            string errorMessage = "Type \"" + _statTypeName + "\" is not defined as a stat module. ";
            errorMessage += "Available modules:";
            definedModules.ForEach(delegate (Type x)
            {
                errorMessage = errorMessage + " \"" + x.Name + "\"";
            });
            errorMessage += ". You can add new modules at PlayerStats.StatModules.";
            Debug.LogError(errorMessage);
            return false;
        }

        private void Awake()
        {
            _originalSuffix = _suffix;
            if (_displayExactMode == DisplayExactMode.PreferenceBased)
            {
                UpdateDisplayMode(UserSetting<bool>.Get(UISetting.HealthbarMode));
                UserSetting<bool>.AddListener(UISetting.HealthbarMode, UpdateDisplayMode);
            }
        }

        private void OnDestroy()
        {
            UserSetting<bool>.RemoveListener(UISetting.HealthbarMode, UpdateDisplayMode);
        }

        private void UpdateDisplayMode(bool isPercent)
        {
            _displayExactMode = ((!isPercent) ? DisplayExactMode.AlwaysExact : DisplayExactMode.AlwaysPercent);
        }

        private void Update()
        {
            if (TryGetModule(out var sb))
            {
                UpdateSlider(sb);
                UpdateText(sb);
            }
        }

        private void UpdateSlider(StatBase stat)
        {
            _targetSlider.minValue = stat.MinValue;
            _targetSlider.maxValue = stat.MaxValue;
            float num = Mathf.Abs(stat.CurValue - _currentValue);
            if (num > _snapValueSkip)
            {
                _currentValue = stat.CurValue;
            }
            else
            {
                float num2 = Mathf.Max(_lerpSpeed * num, (stat.MaxValue - stat.MinValue) * 0.04f);
                _currentValue = Mathf.MoveTowards(_currentValue, stat.CurValue, num2 * Time.deltaTime);
            }
            _targetSlider.value = _currentValue;
        }

        private void UpdateText(StatBase stat)
        {
            if (_displayExactMode != DisplayExactMode.ValueHidden)
            {
                bool flag = _displayExactMode == DisplayExactMode.AlwaysExact;
                float num = (flag ? stat.CurValue : ((float)Mathf.CeilToInt(stat.NormalizedValue * 100f)));
                _targetText.text = Mathf.CeilToInt(num * (float)_roundingAccuracy) / _roundingAccuracy + (flag ? _suffix : "%");
            }
        }
    }
}
