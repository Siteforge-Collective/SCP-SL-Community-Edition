namespace Interactables.Interobjects.DoorUtils
{
	[global::System.Flags]
	public enum DoorLockMode : byte
	{
		FullLock = 0,
		CanOpen = 1,
		CanClose = 2,
		ScpOverride = 4
	}
}
