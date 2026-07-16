using TMPro;
using UnityEngine.Events;

namespace UserSettings.GUIElements
{
    public class UserSettingsDropdown : UserSettingsUIBase<TMP_Dropdown, int>
    {
        protected override UnityEvent<int> OnValueChangedEvent => TargetUI.onValueChanged;

        protected override void SetValueAndTriggerEvent(int val)
        {
            TargetUI.value = val;
        }

        protected override void SetValueWithoutNotify(int val)
        {
            TargetUI.SetValueWithoutNotify(val);
        }
    }
}