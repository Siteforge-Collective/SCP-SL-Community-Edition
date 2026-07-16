using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UserSettings.GUIElements
{
    public abstract class UserSettingsUIBase<TBehaviour, TStoredValue> : MonoBehaviour
        where TBehaviour : UIBehaviour
    {
        [SerializeField] private bool _setupOnAwake = true;

        private bool _cachedTargetSet;
        private TBehaviour _cachedTargetUi;

        public LinkableEnum TargetEnum { get; private set; }
        public TStoredValue DefaultValue { get; private set; }
        public bool OverrideDefaultValue { get; private set; }
        public bool IsSetup { get; private set; }

        public TBehaviour TargetUI
        {
            get
            {
                if (!_cachedTargetSet)
                {
                    _cachedTargetUi = GetComponent<TBehaviour>();
                    _cachedTargetSet = true;
                }
                return _cachedTargetUi;
            }
        }

        protected abstract UnityEvent<TStoredValue> OnValueChangedEvent { get; }

        protected TStoredValue StoredValue
        {
            get
            {
                string key = SettingsKeyGenerator.TypeValueToKey(TargetEnum.TypeHash, TargetEnum.Value);
                return UserSetting<TStoredValue>.Load(key, DefaultValue);
            }
            set
            {
                string key = SettingsKeyGenerator.TypeValueToKey(TargetEnum.TypeHash, TargetEnum.Value);
                UserSetting<TStoredValue>.Save(key, value);
            }
        }

        private void SaveUserInput(TStoredValue val)
        {
            StoredValue = val;
        }

        protected virtual void Awake()
        {
            if (_setupOnAwake)
            {
                Setup();
            }
        }

        protected virtual void OnDestroy()
        {
            Unlink();
        }

        protected abstract void SetValueAndTriggerEvent(TStoredValue val);
        protected abstract void SetValueWithoutNotify(TStoredValue val);

        public void Setup()
        {
            if (IsSetup || TargetEnum.TypeHash == 0) return;

            IsSetup = true;

            TStoredValue currentValue = StoredValue;

            SetValueWithoutNotify(currentValue);

            UnityEvent<TStoredValue> onValueChangedEvent = OnValueChangedEvent;
            if (onValueChangedEvent != null)
            {
                onValueChangedEvent.AddListener(SaveUserInput);
            }
        }

        public void Unlink()
        {
            if (!IsSetup) return;

            IsSetup = false;

            UnityEvent<TStoredValue> onValueChangedEvent = OnValueChangedEvent;
            if (onValueChangedEvent != null)
            {
                onValueChangedEvent.RemoveListener(SaveUserInput);
            }
        }

        public void ChangeTargetEnum(LinkableEnum newEnum, bool autoSetup = true)
        {
            Unlink();

            TargetEnum = newEnum; 

            DefaultValue = UserSetting<TStoredValue>.GetDefaultValue(newEnum.TypeHash, newEnum.Value);
            OverrideDefaultValue = true;

            if (autoSetup)
            {
                Setup();
            }
        }
    }
}