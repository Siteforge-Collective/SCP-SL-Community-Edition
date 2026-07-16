using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserSettings.UserInterfaceSettings
{
    public class PasswordInputField : MonoBehaviour
    {
        private TMP_InputField _inputField;

        [SerializeField] private Toggle _toggleVisibility;
        [SerializeField] private TMP_InputField.ContentType _falseType = TMP_InputField.ContentType.Password;
        [SerializeField] private TMP_InputField.ContentType _trueType = TMP_InputField.ContentType.Standard;

        private bool _prevValue;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            if (_inputField == null)
            {
                Debug.LogError("PasswordInputField: TMP_InputField component not found.");
                return;
            }

            if (_toggleVisibility != null)
            {
                _toggleVisibility.onValueChanged.AddListener(OnToggleVisibilityChanged);
            }

            _prevValue = _toggleVisibility != null && _toggleVisibility.isOn;
            UpdateContentType();
        }

        private void OnToggleVisibilityChanged(bool isVisible)
        {
            UpdateContentType();
            _prevValue = isVisible;
        }

        private void Update()
        {
            if (_toggleVisibility == null) return;

            bool currentValue = _toggleVisibility.isOn;
            if (currentValue != _prevValue)
            {
                UpdateContentType();
                _prevValue = currentValue;
            }
        }

        private void UpdateContentType()
        {
            if (_inputField == null) return;

            _inputField.contentType = _toggleVisibility.isOn ? _trueType : _falseType;
            _inputField.ForceLabelUpdate();
        }
    }
}