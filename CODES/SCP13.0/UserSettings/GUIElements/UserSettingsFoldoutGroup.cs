using UnityEngine;
using UnityEngine.UI;

namespace UserSettings.GUIElements
{
    public class UserSettingsFoldoutGroup : MonoBehaviour
    {
        private static readonly Quaternion CollapsedRot = Quaternion.Euler(0f, 0f, 90f);   // Стрелка вправо
        private static readonly Quaternion ExtendedRot = Quaternion.Euler(0f, 0f, 0f);     // Стрелка вниз

        private const float FullAnimTime = 0.15f;
        private const float ExtendTime = 0.08f;

        [Tooltip("Fader of the collapsable group. Note the height of this group will be added to the start height when the group is extended.")]
        [SerializeField] private CanvasGroup _fadeGroup;

        [Tooltip("Deactivates the game object of the group if folded")]
        [SerializeField] private bool _deactivateOnFold = true;

        [Tooltip("Arrow that will point to the right when the group is collapsed and will point down when the group is extended.")]
        [SerializeField] private RectTransform _arrow;

        [SerializeField] private Toggle _extendToggle;

        private Vector2 _startSize;
        private RectTransform _parentRt;
        private RectTransform _fadeGroupRt;

        private float _animStatus;
        private static UserSettingsFoldoutGroup _lastFoldedOut;

        public float ExtendRate => Mathf.Clamp01(_animStatus / FullAnimTime);

        private void Awake()
        {
            _parentRt = GetComponent<RectTransform>();
            if (_fadeGroup != null)
            {
                _fadeGroupRt = _fadeGroup.GetComponent<RectTransform>();
            }

            _startSize = _parentRt.sizeDelta;

            if (_extendToggle != null)
            {
                _extendToggle.onValueChanged.AddListener(OnToggleChanged);
            }

            bool isExtended = _extendToggle != null && _extendToggle.isOn;
            _animStatus = isExtended ? FullAnimTime : 0f;
            UpdateVisuals(true);
        }

        private void LateUpdate()
        {
            UpdateVisuals(false);
        }

        private void OnDisable()
        {
            if (_lastFoldedOut == this)
            {
                _lastFoldedOut = null;
            }
        }

        private void OnToggleChanged(bool extended)
        {
            if (extended && _lastFoldedOut != null && _lastFoldedOut != this)
            {
                _lastFoldedOut._extendToggle.isOn = false;
            }

            _lastFoldedOut = extended ? this : null;

            _animStatus = extended ? FullAnimTime : 0f;
        }

        private void UpdateVisuals(bool instant)
        {
            bool targetExtended = _extendToggle != null && _extendToggle.isOn;

            if (instant)
            {
                _animStatus = targetExtended ? FullAnimTime : 0f;
            }
            else
            {
                float target = targetExtended ? FullAnimTime : 0f;
                _animStatus = Mathf.MoveTowards(_animStatus, target, Time.deltaTime);
            }

            float t = _animStatus / FullAnimTime;

            if (_arrow != null)
            {
                _arrow.rotation = Quaternion.Slerp(CollapsedRot, ExtendedRot, t);
            }

            if (_fadeGroup != null)
            {
                _fadeGroup.alpha = t;
                if (_deactivateOnFold)
                {
                    _fadeGroup.gameObject.SetActive(t > 0f);
                }
            }

            if (_fadeGroupRt != null)
            {
                float targetHeight = targetExtended ? _fadeGroupRt.sizeDelta.y : 0f;
                float currentHeight = Mathf.Lerp(0f, targetHeight, Mathf.Clamp01(_animStatus / ExtendTime));

                Vector2 size = _parentRt.sizeDelta;
                size.y = _startSize.y + currentHeight;
                _parentRt.sizeDelta = size;
            }
        }

        private void FoldInstantly()
        {
            if (_extendToggle != null)
            {
                _extendToggle.isOn = false;
            }
            UpdateVisuals(true);
        }

        public void RefreshSize()
        {
            if (_fadeGroupRt != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_fadeGroupRt);
            }
            UpdateVisuals(true);
        }
    }
}