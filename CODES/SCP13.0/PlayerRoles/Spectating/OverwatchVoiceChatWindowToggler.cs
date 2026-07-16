using TMPro;
using ToggleableMenus;
using UnityEngine;
using System;

namespace PlayerRoles.Spectating
{
    public class OverwatchVoiceChatWindowToggler : SimpleToggleableMenu
    {
        [SerializeField]
        private TextMeshProUGUI _toggleHint;

        [SerializeField]
        private TextMeshProUGUI _menuTitle;

        public override bool CanToggle => gameObject.activeInHierarchy;

        protected override void Awake()
        {
            base.Awake();

            string hintFormat = TranslationReader.Get("Overwatch_HUD", 0, "Press {0} to open the voice channel selector.");

            string keyName = this.MenuActionKey.ToString();

            if (_toggleHint != null)
            {
                _toggleHint.text = string.Format(hintFormat, keyName);
            }

            if (_menuTitle != null)
            {
                _menuTitle.text = TranslationReader.Get("Overwatch_HUD", 1, "Voice Channel Selector");
            }
        }

        private void OnDisable()
        {
            if (this.IsEnabled)
            {
                this.IsEnabled = false;
            }
        }
    }
}