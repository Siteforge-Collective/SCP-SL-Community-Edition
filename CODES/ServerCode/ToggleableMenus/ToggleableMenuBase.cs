namespace ToggleableMenus
{
	public abstract class ToggleableMenuBase : global::UnityEngine.MonoBehaviour, global::CursorManagement.ICursorOverride
	{
		private bool _isEnabled;

		public ActionName MenuActionKey;

		public static readonly global::System.Collections.Generic.HashSet<global::ToggleableMenus.ToggleableMenuBase> AllMenus = new global::System.Collections.Generic.HashSet<global::ToggleableMenus.ToggleableMenuBase>();

		public abstract bool CanToggle { get; }

		public virtual global::CursorManagement.CursorOverrideMode CursorOverride
		{
			get
			{
				if (!IsEnabled)
				{
					return global::CursorManagement.CursorOverrideMode.NoOverride;
				}
				return global::CursorManagement.CursorOverrideMode.Free;
			}
		}

		public virtual bool LockMovement => false;

		public virtual bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			set
			{
				if (value != _isEnabled)
				{
					_isEnabled = value;
					OnToggled();
				}
			}
		}

		protected abstract void OnToggled();

		protected virtual void Awake()
		{
			AllMenus.Add(this);
			global::CursorManagement.CursorManager.Register(this);
		}

		protected virtual void OnDestroy()
		{
			AllMenus.Remove(this);
			global::CursorManagement.CursorManager.Unregister(this);
		}
	}
}
