using UnityEngine;
using TMPro;
using UserSettings.GUIElements;

namespace UserSettings.AudioSettings
{
    public class MenuThemeDropdown : MonoBehaviour
    {
        private void Awake()
        {
            TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown == null)
            {
                Debug.LogError("[MenuThemeDropdown] TMP_Dropdown component not found.");
                return;
            }

            dropdown.ClearOptions();

            string[] themeNames = MainMenuSoundtrackController.GetThemeNames;
            if (themeNames != null && themeNames.Length > 0)
            {
                foreach (string themeName in themeNames)
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData(themeName));
                }

                dropdown.RefreshShownValue();
            }
            else
            {
                Debug.LogWarning("[MenuThemeDropdown] No theme names available from MainMenuSoundtrackController.");
            }

            UserSettingsDropdown settingsDropdown = GetComponent<UserSettingsDropdown>();
            if (settingsDropdown == null)
            {
                Debug.LogError("[MenuThemeDropdown] UserSettingsDropdown component not found.");
                return;
            }

            settingsDropdown.ChangeTargetEnum(
                MainMenuSoundtrackController.ThemePrefsKey,  
                false                                        
            );
        }
    }
}