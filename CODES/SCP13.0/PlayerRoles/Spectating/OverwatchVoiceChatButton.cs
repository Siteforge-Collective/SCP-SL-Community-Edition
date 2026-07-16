using TMPro;
using UnityEngine;
using VoiceChat;

namespace PlayerRoles.Spectating
{
    public class OverwatchVoiceChatButton : MonoBehaviour
    {
        private static readonly Color EnabledColor = new Color(1f, 1f, 1f, 1f);
        private static readonly Color DisabledColor = new Color(1f, 1f, 1f, 0.2f);

        [Tooltip("The voice channel that will be toggled with the button.")]
        public VoiceChatChannel VoiceChatChannel;

        [Tooltip("The icon of the button, used to handle its color.")]
        public TextMeshProUGUI Icon;

        internal bool _isToggled = true;

        public bool IsToggled
        {
            get => _isToggled;
            set
            {
                _isToggled = value;
                UpdateColor();
            }
        }

        private void UpdateColor()
        {
            if (Icon != null)
            {
                Icon.color = !_isToggled ? DisabledColor : EnabledColor;
            }
        }

        public void Toggle()
        {
            IsToggled = !_isToggled;
        }
    }
}