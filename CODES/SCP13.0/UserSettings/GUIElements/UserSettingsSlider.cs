using UnityEngine.Events;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    public class UserSettingsSlider : UserSettingsUIBase<Slider, float>
    {
        protected override UnityEvent<float> OnValueChangedEvent => TargetUI.onValueChanged;

        protected override void SetValueAndTriggerEvent(float val)
        {
            TargetUI.value = val;
        }

        protected override void SetValueWithoutNotify(float val)
        {
            TargetUI.SetValueWithoutNotify(val);
        }
    }
}