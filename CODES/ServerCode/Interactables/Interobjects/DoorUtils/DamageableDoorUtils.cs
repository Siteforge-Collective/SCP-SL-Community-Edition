namespace Interactables.Interobjects.DoorUtils
{
	public static class DamageableDoorUtils
	{
		public static bool HasFlagFast(this global::Interactables.Interobjects.DoorUtils.DoorDamageType value, global::Interactables.Interobjects.DoorUtils.DoorDamageType flag)
		{
			return (value & flag) == flag;
		}
	}
}
