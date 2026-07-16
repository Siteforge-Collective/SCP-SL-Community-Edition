namespace InventorySystem.Items.Firearms.Ammo
{
	public class AmmoItem : global::InventorySystem.Items.ItemBase, global::InventorySystem.Items.IItemNametag, global::InventorySystem.Items.ICustomSearchCompletorItem
	{
		public int UnitPrice;

		public override float Weight => 0.25f + ((PickupDropModel is global::InventorySystem.Items.Firearms.Ammo.AmmoPickup ammoPickup) ? ((float)(int)ammoPickup.SavedAmmo * 0.01f) : 0f);

		public override global::InventorySystem.Items.ItemDescriptionType DescriptionType => global::InventorySystem.Items.ItemDescriptionType.None;

		public global::InventorySystem.Searching.SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::InventorySystem.Items.ItemBase ib, double disSqrt)
		{
			return new global::InventorySystem.Searching.AmmoSearchCompletor(hub, ipb, ib, disSqrt);
		}

		public override void OnAdded(global::InventorySystem.Items.Pickups.ItemPickupBase pickup)
		{
			if (global::Mirror.NetworkServer.active)
			{
				if (PickupDropModel is global::InventorySystem.Items.Firearms.Ammo.AmmoPickup ammoPickup)
				{
					base.OwnerInventory.ServerAddAmmo(ItemTypeId, ammoPickup.SavedAmmo);
				}
				base.OwnerInventory.ServerRemoveItem(base.ItemSerial, null);
			}
		}
	}
}
