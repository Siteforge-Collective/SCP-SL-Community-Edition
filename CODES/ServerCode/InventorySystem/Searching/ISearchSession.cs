namespace InventorySystem.Searching
{
	public interface ISearchSession
	{
		global::InventorySystem.Items.Pickups.ItemPickupBase Target { get; set; }

		double InitialTime { get; set; }

		double FinishTime { get; set; }

		double Progress { get; }
	}
}
