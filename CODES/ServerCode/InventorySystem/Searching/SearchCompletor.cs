namespace InventorySystem.Searching
{
	public abstract class SearchCompletor
	{
		protected readonly ReferenceHub Hub;

		protected readonly global::InventorySystem.Items.Pickups.ItemPickupBase TargetPickup;

		protected readonly global::InventorySystem.Items.ItemBase TargetItem;

		protected readonly double MaxDistanceSqr;

		public static global::InventorySystem.Searching.SearchCompletor FromPickup(global::InventorySystem.Searching.SearchCoordinator coordinator, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, double maxDistanceSquared)
		{
			ReferenceHub hub = coordinator.Hub;
			if (!global::InventorySystem.InventoryItemLoader.AvailableItems.TryGetValue(targetPickup.Info.ItemId, out var value))
			{
				return null;
			}
			if (!(value is global::InventorySystem.Items.ICustomSearchCompletorItem customSearchCompletorItem))
			{
				return new global::InventorySystem.Searching.ItemSearchCompletor(hub, targetPickup, value, maxDistanceSquared);
			}
			return customSearchCompletorItem.GetCustomSearchCompletor(hub, targetPickup, value, maxDistanceSquared);
		}

		protected SearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
		{
			Hub = hub;
			TargetPickup = targetPickup;
			TargetItem = targetItem;
			MaxDistanceSqr = maxDistanceSquared;
		}

		protected bool ValidateDistance()
		{
			return (double)(TargetPickup.transform.position - Hub.transform.position).sqrMagnitude <= MaxDistanceSqr;
		}

		protected virtual bool ValidateAny()
		{
			if (global::PlayerRoles.PlayerRolesUtils.IsHuman(Hub) && !TargetPickup.Info.Locked && !global::InventorySystem.Disarming.DisarmedPlayers.IsDisarmed(Hub.inventory))
			{
				return !Hub.interCoordinator.AnyBlocker(global::InventorySystem.Items.BlockedInteraction.GrabItems);
			}
			return false;
		}

		public virtual bool ValidateStart()
		{
			if (ValidateAny())
			{
				return ValidateDistance();
			}
			return false;
		}

		public virtual bool ValidateUpdate()
		{
			if (TargetPickup != null && ValidateAny())
			{
				return ValidateDistance();
			}
			return false;
		}

		public abstract void Complete();
	}
}
