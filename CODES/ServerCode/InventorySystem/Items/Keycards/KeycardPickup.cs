namespace InventorySystem.Items.Keycards
{
	public class KeycardPickup : global::InventorySystem.Items.Pickups.CollisionDetectionPickup
	{
		protected override void ProcessCollision(global::UnityEngine.Collision collision)
		{
			base.ProcessCollision(collision);
			if (global::Mirror.NetworkServer.active && collision.collider.TryGetComponent<global::Interactables.Interobjects.DoorUtils.RegularDoorButton>(out var component) && component.Target is global::Interactables.Interobjects.DoorUtils.DoorVariant doorVariant && doorVariant.ActiveLocks == 0 && doorVariant.RequiredPermissions.RequiredPermissions != global::Interactables.Interobjects.DoorUtils.KeycardPermissions.None && global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(Info.ItemId, out var value) && doorVariant.RequiredPermissions.CheckPermissions(value, null) && doorVariant.AllowInteracting(null, component.ColliderId))
			{
				doorVariant.NetworkTargetState = !doorVariant.TargetState;
			}
		}

		private void MirrorProcessed()
		{
		}
	}
}
