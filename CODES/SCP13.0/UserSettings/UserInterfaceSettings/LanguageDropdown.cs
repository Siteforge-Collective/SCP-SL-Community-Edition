using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UserSettings.UserInterfaceSettings
{
    public class LanguageDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Text _onChangeWarning;

        private TMP_Dropdown _dropdown;
        private readonly List<(string Name, bool IsValid)> _loadedOptions = new List<(string, bool)>();

        private const string InvalidCharFormat = "<color=red>{0}</color>";

        private static readonly string LanguageKey = SettingsKeyGenerator.TypeValueToKey(
            SettingsKeyGenerator.GetStableTypeHash(typeof(UISetting)),
            (ushort)UISetting.Language);

        private async void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            if (_dropdown == null)
            {
                Debug.LogError("LanguageDropdown: TMP_Dropdown component not found.");
                return;
            }

            _dropdown.ClearOptions();

            await Task.Run(() => TranslationBrowser.GetTranslationList());

            var translations = TranslationBrowser.Translations;
            _loadedOptions.AddRange(translations);

            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (var (name, isValid) in _loadedOptions)
            {
                string displayName = isValid ? name : string.Format(InvalidCharFormat, name);
                options.Add(new TMP_Dropdown.OptionData(displayName));
            }

            _dropdown.AddOptions(options);

            int savedLanguage = UserSetting<int>.Load(LanguageKey, 0);
            _dropdown.value = Mathf.Clamp(savedLanguage, 0, _loadedOptions.Count - 1);
            _dropdown.RefreshShownValue();

            _dropdown.onValueChanged.AddListener(OnSelected);
        }

        private void OnSelected(int index)
        {
            if (index < 0 || index >= _loadedOptions.Count) return;

            bool isValid = _loadedOptions[index].IsValid;

            if (!isValid)
            {
                if (_onChangeWarning != null)
                {
                    _onChangeWarning.gameObject.SetActive(true);
                }
                return;
            }

            if (_onChangeWarning != null)
            {
                _onChangeWarning.gameObject.SetActive(false);
            }

            UserSetting<int>.Save(LanguageKey, index);
            TranslationReader.Refresh();
        }
    }
}