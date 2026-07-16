using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldableButton : Button
{
    public readonly Stopwatch _holdSw = new Stopwatch();

    public bool _eventCalled;

    [SerializeField]
    public Image _loadingCircle;

    [SerializeField]
    public bool _deselectOnComplete;

    public bool IsHeld => IsPressed();

    public bool IsHovering
    {
        get
        {
            if (!IsHighlighted())
            {
                return IsHeld;
            }

            return true;
        }
    }

    public float HeldPercent
    {
        get
        {
            if (HoldTime <= 0f || !IsHeld)
            {
                return 0f;
            }

            return Mathf.Clamp01((float)_holdSw.Elapsed.TotalSeconds / HoldTime);
        }
    }

    [field: SerializeField]
    public virtual float HoldTime { get; set; }

    [field: SerializeField]
    public ButtonClickedEvent OnHeld { get; set; }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _holdSw.Restart();
        _eventCalled = false;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _holdSw.Reset();
    }

    public void Update()
    {
        if (_loadingCircle != null)
        {
            _loadingCircle.fillAmount = HeldPercent;
        }

        if (!_eventCalled && IsHeld && !(HeldPercent < 1f))
        {
            _eventCalled = true;
            if (OnHeld != null)
            {
                OnHeld.Invoke();
            }
        }
    }
}
