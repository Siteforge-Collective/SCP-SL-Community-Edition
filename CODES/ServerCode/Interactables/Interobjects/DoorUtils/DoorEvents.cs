namespace Interactables.Interobjects.DoorUtils
{
	public static class DoorEvents
	{
		public static event global::System.Action<global::Interactables.Interobjects.DoorUtils.DoorVariant, global::Interactables.Interobjects.DoorUtils.DoorAction, ReferenceHub> OnDoorAction;

		public static void TriggerAction(global::Interactables.Interobjects.DoorUtils.DoorVariant variant, global::Interactables.Interobjects.DoorUtils.DoorAction action, ReferenceHub user)
		{
			global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction(variant, action, user);
		}

		static DoorEvents()
		{
			global::Interactables.Interobjects.DoorUtils.DoorEvents.OnDoorAction = delegate
			{
			};
		}
	}
}
