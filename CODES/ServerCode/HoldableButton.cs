public class HoldableButton : global::UnityEngine.UI.Button
{
	private readonly global::System.Diagnostics.Stopwatch _holdSw = new global::System.Diagnostics.Stopwatch();

	private bool _eventCalled;

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
			return global::UnityEngine.Mathf.Clamp01((float)_holdSw.Elapsed.TotalSeconds / HoldTime);
		}
	}

	[field: global::UnityEngine.SerializeField]
	public float HoldTime { get; private set; }

	[field: global::UnityEngine.SerializeField]
	public global::UnityEngine.UI.Button.ButtonClickedEvent OnHeld { get; private set; }

	public override void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		_holdSw.Restart();
		_eventCalled = false;
	}

	public override void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		_holdSw.Reset();
	}

	private void Update()
	{
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
