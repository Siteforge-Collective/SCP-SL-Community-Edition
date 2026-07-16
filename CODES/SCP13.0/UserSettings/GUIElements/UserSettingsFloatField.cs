using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UserSettings.GUIElements
{
    public class UserSettingsFloatField : UserSettingsUIBase<TMP_InputField, float>
    {
        [SerializeField] private float _minInput = float.MinValue;
        [SerializeField] private float _maxInput = float.MaxValue;
        [SerializeField] private string _toStringFormat = "F2";
        [SerializeField] private string _finalFormat = "{0}";
        [SerializeField] private float _valueMultiplier = 1f;

        private readonly UnityEvent<float> _onParsed = new UnityEvent<float>();

        protected override UnityEvent<float> OnValueChangedEvent => _onParsed;

        protected override void Awake()
        {
            base.Awake();

            TargetUI.onEndEdit.AddListener(OnEndEdit);
            TargetUI.contentType = TMP_InputField.ContentType.DecimalNumber;
        }

        private void OnEndEdit(string text)
        {
            if (float.TryParse(text, out float parsed))
            {
                parsed *= _valueMultiplier;
                parsed = Mathf.Clamp(parsed, _minInput, _maxInput);
                _onParsed.Invoke(parsed);
            }
            else
            {
                SetValueWithoutNotify(StoredValue);
            }
        }

        protected override void SetValueAndTriggerEvent(float val)
        {
            TargetUI.text = FormatValue(val);
            _onParsed.Invoke(val);
        }

        protected override void SetValueWithoutNotify(float val)
        {
            TargetUI.SetTextWithoutNotify(FormatValue(val));
        }

        private string FormatValue(float val)
        {
            float displayValue = val / _valueMultiplier;
            string formatted = displayValue.ToString(_toStringFormat);

            if (TargetUI.characterLimit > 0 && formatted.Length > TargetUI.characterLimit)
            {
                formatted = formatted.Remove(TargetUI.characterLimit);
            }

            return string.Format(_finalFormat, formatted);
        }
    }
}