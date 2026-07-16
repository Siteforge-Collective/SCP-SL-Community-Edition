using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserSettings.GUIElements
{
    public class UserSettingsEntryDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _potentiallyCurrent;
        private TextLanguageReplacer _tlr;
        private Type _secondaryTranslationType;
        private Enum _secondaryTranslationValue;

        public static UserSettingsEntryDescription CurrentDescription { get; private set; }

        public string Text { get; set; }
        public bool UsesReplacer { get; private set; }

        public void SetTranslation<T>(T translation) where T : Enum
        {
            Text = TranslationReader.Get("SettingsDescriptions", Convert.ToInt32(translation), string.Empty);
            UsesReplacer = false;

            if (!string.IsNullOrEmpty(Text))
            {
                _tlr = null;
                return;
            }

            _secondaryTranslationType = typeof(T);
            _secondaryTranslationValue = translation;
            UsesReplacer = true;
            _tlr = GetComponent<TextLanguageReplacer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _potentiallyCurrent = true;
            CurrentDescription = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _potentiallyCurrent = false;
            Deselect();
        }

        private void Awake()
        {
            if (_tlr == null)
            {
                _tlr = GetComponent<TextLanguageReplacer>();
            }
        }

        private void OnDisable()
        {
            Deselect();
        }

        private void Deselect()
        {
            if (_potentiallyCurrent && CurrentDescription == this)
            {
                CurrentDescription = null;
            }
            _potentiallyCurrent = false;
        }
    }
}