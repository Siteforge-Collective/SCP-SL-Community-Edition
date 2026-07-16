namespace InventorySystem.Items
{
	public interface ICustomSearchCompletorItem
	{
		global::InventorySystem.Searching.SearchCompletor GetCustomSearchCompletor(ReferenceHub hub, global::InventorySystem.Items.Pickups.ItemPickupBase ipb, global::InventorySystem.Items.ItemBase ib, double disSqrt);
	}
}
