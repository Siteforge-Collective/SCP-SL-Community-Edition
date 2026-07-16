namespace CursorManagement
{
	public interface ICursorOverride
	{
		global::CursorManagement.CursorOverrideMode CursorOverride { get; }

		bool LockMovement { get; }
	}
}
