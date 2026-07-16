namespace InventorySystem.Searching
{
	public class Scp244SearchCompletor : global::InventorySystem.Searching.ItemSearchCompletor
	{
		public Scp244SearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase targetPickup, global::InventorySystem.Items.ItemBase targetItem, double maxDistanceSquared)
			: base(hub, targetPickup, targetItem, maxDistanceSquared)
		{
		}

		protected override bool ValidateAny()
		{
			if (base.ValidateAny() && TargetPickup is global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup scp244DeployablePickup)
			{
				return !scp244DeployablePickup.ModelDestroyed;
			}
			return false;
		}

		public override void Complete()
		{
			if (TargetPickup is global::InventorySystem.Items.Usables.Scp244.Scp244DeployablePickup scp244DeployablePickup)
			{
				Hub.inventory.ServerAddItem(TargetPickup.Info.ItemId, TargetPickup.Info.Serial, TargetPickup);
				scp244DeployablePickup.State = global::InventorySystem.Items.Usables.Scp244.Scp244State.PickedUp;
				CheckCategoryLimitHint();
			}
		}
	}
}
