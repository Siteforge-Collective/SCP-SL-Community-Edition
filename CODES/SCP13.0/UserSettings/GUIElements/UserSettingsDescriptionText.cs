using TMPro;
using UnityEngine;

namespace UserSettings.GUIElements
{
    public class UserSettingsDescriptionText : MonoBehaviour
    {
        private TMP_Text _text;
        private UserSettingsEntryDescription _prev;

        [SerializeField] private float _transitionSpeed = 6f; // Скорость появления текста (единиц в секунду)

        private UserSettingsEntryDescription Current => UserSettingsEntryDescription.CurrentDescription;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            if (_text != null)
            {
                _text.alpha = 0f;
            }
        }

        private void OnEnable()
        {
            UpdateCurrent();
        }

        private void Update()
        {
            UserSettingsEntryDescription current = Current;

            if (current != _prev)
            {
                DisablePrevious();
                UpdateCurrent();
                _prev = current;
            }

            if (_text != null && current != null)
            {
                _text.alpha = Mathf.MoveTowards(_text.alpha, 1f, Time.deltaTime * _transitionSpeed);
            }
        }

        private void DisablePrevious()
        {
            if (_text != null)
            {
                _text.alpha = 0f;
            }
        }

        private void UpdateCurrent()
        {
            if (_text == null) return;

            UserSettingsEntryDescription current = Current;

            if (current == null)
            {
                _text.text = string.Empty;
                _text.alpha = 0f;
                return;
            }

            _text.text = current.Text;

            if (current.UsesReplacer)
            {
                var replacer = current.GetComponent<TextLanguageReplacer>();
                if (replacer != null)
                {
                    _text.text = replacer.GetCurrentTranslatedText();
                }
            }

            _text.alpha = 0f;
        }
    }
}