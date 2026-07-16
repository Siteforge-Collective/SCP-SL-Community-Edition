using UnityEngine.Events;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    public class UserSettingsToggle : UserSettingsUIBase<Toggle, bool>
    {
        protected override UnityEvent<bool> OnValueChangedEvent => TargetUI.onValueChanged;

        protected override void SetValueAndTriggerEvent(bool val)
        {
            TargetUI.isOn = val;
        }

        protected override void SetValueWithoutNotify(bool val)
        {
            TargetUI.SetIsOnWithoutNotify(val);
        }
    }
}